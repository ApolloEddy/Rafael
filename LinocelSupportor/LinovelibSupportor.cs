using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aries;
using Rafael;
using Rafael.DataContainer;

namespace Rafael.LinovelibSupportor
{
    public static class LinovelibSupportor
    {
        public static string page(string url)
        {
            var web = createWebProtocol(url);
            byte[] returnValue = web.contentBytes;

            return Encoding.UTF8.GetString(returnValue); // 规定编码 ：utf-8
        }
        public static LiteNovelInfo createInfo(ref string page)
        {
            var info = new LiteNovelInfo();
            info.source = "哔哩轻小说";
            var tp = new TextParser(page);
            tp.extrim("\n", "\r");
            info.hottext = tp.extractOne("<meta name=\"keywords\" content=\"", "\"").Replace(",哔哩轻小说", ""); // 关键词
            info.index = long.Parse(tp.extractOne("<meta property=\"og:url\" content=\"https://www.linovelib.com/novel/", ".html\""));
            info.bookname = tp.extractOne("<meta property=\"og:title\" content=\"", "\"");
            info.author = tp.extractOne("<meta property=\"og:novel:author\" content=\"", "\"");
            // tp.extractOne("<div class=\"title\">", "<span>开始阅读</span>", true);
            info.tag = tp.extractOne("<meta property=\"og:novel:category\" content=\"", "\"")
                + "||" + tp.extractOne("<meta property=\"og:novel:tags\" content=\"", "\"");
            info.status = tp.extractOne("<meta property=\"og:novel:status\" content=\"", "\"");
            info.summary = tp.extractOne("<div class=\"book-dec Jbook-dec hide\">(.+?)<p>", "</p>(.+?)</div>")
                .Replace("<br>", "\n").Replace("<br />", "\n").Replace("<br/>", "\n");

            return info;
        }
        public static void collectBookInfos(string dbpath)
        {
            const string API = "https://www.linovelib.com/novel/{index}.html";
            DateTime now = DateTime.Now;
            string dateParam = $"{now.Year}_{now.Month}_{now.Day}_{now.Hour}{now.Minute}{now.Second}{now.Millisecond}";
            Logger logger = new Logger($"{Environment.CurrentDirectory}\\linovelib{dateParam}.log");
            Logger errorLogger = new Logger($"{Environment.CurrentDirectory}\\error{dateParam}.log");
            Rootobject root;
            long startIndex;
            logger.Log("info", $"Reading the data from {dbpath}");
            if (File.Exists(dbpath))
            {
                root = Rootobject.load(dbpath);
                if (root.LiteNovelInfos.Count == 0)
                { startIndex = 1; }
                else
                { startIndex = root.LiteNovelInfos.Last().index + 1; }
            }
            else
            { root = new Rootobject(); startIndex = 1; }
            logger.Log("info", $"From index[{startIndex}] collecting the message");

            for (long i = startIndex; i < 2987; i++) // 好像有限制，静态修改吧  // 好吧目前看起来是没有的 // 2987
            {
                string url = API.Replace("{index}", i.ToString());
                logger.Log("info", $"Try to load page from {url}");
                Console.Write($"Collecting index[{i}]..."); // :Debugger
                for (int j = 1; j <= 6; j++)
                {
                    try
                    {
                        string html = page(url);
                        var web = createWebProtocol(url);
                        if (html.Contains("<title>出现错误！哔哩轻小说</title>"))
                        { logger.Log("warnning", $"Index of {i} does not exist"); Console.WriteLine("Error::Message(404 Not Found)"); /*:Debugger*/ break; }
                        //if (!exists(ref web))
                        //{ }
                        LiteNovelInfo info = createInfo(ref html);
                        logger.Log("info", $"Successfully parse novel info at [{info.index}]\"{info.bookname}\"");
                        root.LiteNovelInfos.Add(info);
                        Console.WriteLine($"Success :: <bookname:\"{info.bookname}\">"); //:Debugger
                        break;
                    }
                    catch (Exception ex) // 报错机制
                    {
                        if (ex.Message.Contains("远程服务器返回错误: (404) 未找到。"))
                        { logger.Log("warnning", $"Index of {i} does not exist"); Console.WriteLine($"Error::Message({ex.Message})"); /*:Debugger*/ break; }
                        if (j < 6)
                        { logger.Log("warnning", $"{j}th times fail to load page from {url}"); }
                        else
                        {
                            logger.Log("error", $"{ex.GetType()}", $"Unable to load page from {url}::Message:({ex.Message})");
                            errorLogger.Log("error", $"{ex.GetType()}", $"Unable to load page from {url}::Message:({ex.Message})");
                            Console.WriteLine($"Error::Message({ex.Message})!"); // :Debugger
                            break;
                        }
                    }
                }
                // System.Threading.Thread.Sleep(5000); // 极其害怕被封
            }
            logger.Log("info", "Generating the json data string");
            root.LiteNovelInfos.Distinct();
            root.update = DateTime.Now.ToString();
            string json = root.toJsonString();
            File.WriteAllText(dbpath, json);
            logger.Log("info", $"Successfully save data at {dbpath}");
            errorLogger.Dispose();
            logger.Dispose();
        }
        public static string search(string searchkey)
		{
            const string API = "https://w.linovelib.com/s1/?searchkey={searchkey}&searchtype={searchtype}"; // 电脑端需要注册，手机端不需要
            const string searchType = "All";
            string url = API.Replace("{searchkey}", WebProtocol.EscapeDataString(searchkey)).Replace("{searchtype}", searchType);
            // string postContent = $"searchkey={WebProtocol.EscapeDataString(searchkey)}&searchtype={searchType}";
            var web = createWebProtocol(url);
            web.computerUAsign = false; // 模拟手机端欺骗获取搜索页面
            // web.post(postContent);
            // Console.WriteLine(web.contentDocument); // :Debug

            return web.contentDocument;
        }
        public static Catalog extractCatalog(ref string page)
		{
            const string host = "https://www.linovelib.com/";
            Catalog result = Catalog.Create();
            var tp = new TextParser(page);
            tp.extrim("\r", "\n", "\f");
            tp.extractOne("<ul class=\"chapter-list", "<div class=\"footer\">", true);
            string extractWith;
            if (tp.ToString().Contains("<em class=\"v-line\">"))
            { extractWith = "<em class=\"v-line\">"; }
            else
            { extractWith = "clearfix\">"; }
			foreach (string line in TextParser.extract(tp.ToString(), extractWith, "<div class="))
			{
                var tpLn = new TextParser(line);
                Volume volume = Volume.Create();
                volume.BaseCatalog = result;
                volume.VolumeName = tpLn.extractOne("</em>", "</div>");
                // Console.WriteLine($"Volume: \"{volume.VolumeName}\""); // :Debug
				foreach (string item in TextParser.extract(tpLn.ToString(), "<li", "</li>"))
				{
                    var title = TextParser.extractOne(item, "<a href(.+?)>", "</a>");
                    var link = host + TextParser.extractOne(item, "<a href=\"", "\"");
                    volume.TitleLinkDict.Add(new TitleLink(title, link));
                    // Console.WriteLine($"KEY: \"{title}\"\tValue: \"{link}\""); // :Debug
				}
                result.Volumes.Add(volume);
			}
            return result;
		}
        public static Catalog extractCatalog(LiteNovelInfo novelInfo)
		{
            const string API = "https://www.linovelib.com/novel/{index}/catalog";
            var content = page(API.Replace("{index}", novelInfo.index.ToString()));
            return extractCatalog(ref content);
        }
        public static string extractArticle(string url)
		{
            const string host = "https://www.linovelib.com/";
            StringBuilder content = new StringBuilder();
            bool hasNext = true;
            while (hasNext)
			{
                if (url.Contains("javascript:cid(0)"))
                { throw new UriFormatException("未能识别的URI：\"javascript:cid(0)\""); }
                string page = "";
                for (int i = 0; i < 3; i++)
				{
					try
					{
                        page = LinovelibSupportor.page(url);
                        break;
					}
					catch
					{
                        if (i is 2)
                        { throw; }
					}
				}
                // 提取文章内容
                var temp = TextParser.extractOne(page, "<div class=\"tp\">", "<span id=\"chapter_last\">") + "</p></p></p></p></p>" +
                    TextParser.extractOne(page, "<script>function ts()", "</script>");
                foreach (string para in TextParser.extract(temp, "<p>", "</p>").ToArray())
				{
                    content.Append(para);
                    content.Append("\n");
				}
				foreach (Match imgLink in new Regex("https(.+?)(jpg|jpeg|png|webp|svg|）)").Matches(temp))
				{
                    string linkTemp = imgLink.Value.ToLower();
                    if (!(linkTemp.Contains("jpg") || linkTemp.Contains("jpeg") || linkTemp.Contains("png") || linkTemp.Contains("webp") || linkTemp.Contains("svg")))
                    { continue; }
                    content.Append($"<img src=\"{imgLink.Value}\">");
                    content.Append("\n");
                }

                var tempBlock = TextParser.extractOne(page, "<p class=\"mlfy_page\">", "</p>");
                if (tempBlock.Contains("下一页"))
                { hasNext = true; }
				else 
                { hasNext = false; }
                url = host + TextParser.extractOne(tempBlock, "书签</a><a href=\"", "\"");
			}
            return content.ToString();
		}
        public static LiteNovelInfo[] extractSearchResult(ref string page)
		{
            List<LiteNovelInfo> result = new List<LiteNovelInfo>();
            var tp = new TextParser(page);
            tp.extrim("\r", "\n", "\f");
            tp.extractOne("<div class=\"module-header\">", "<footer class=\"footer\">", true);
            foreach (string block in TextParser.extract(tp.ToString(), "<li class=\"book-li\">", "</a></li>"))
			{
                var tpBlock = new TextParser(block);
                LiteNovelInfo info = new LiteNovelInfo();
                info.bookname = tpBlock.extractOne("class=\"book-cover\" alt=\"", "\"");
                info.author = tp.extractOne("<title>作者</title>(.+?)</svg>", "</span>");
                info.summary = tpBlock.extractOne("<p class=\"book-desc\">", "</p>");
                info.tag = tpBlock.extractOne("<em class=\"tag-small red\">", "</em>") + " " +
                    tpBlock.extractOne("<em class=\"tag-small yellow\">", "</em>");
                info.status = tpBlock.extractOne("<em class=\"tag-small gray\">", "</em></span>");
                info.index = long.Parse(tpBlock.extractOne("<a href=\"/novel/", ".html\""));
                info.source = "哔哩轻小说";
                result.Add(info);
                info.outputToConsole("https://www.linovelib.com/novel/{index}/catalog"); // :Debug
			}

            return result.ToArray();
		}
        public static void download(string url, string path, string title = "")
		{
            // docx ==> true
            // txt  ==> false
            TextGenerator txt = new TextGenerator(path);
            var content = extractArticle(url);
            // string label = "<img" + TextParser.extractOne(content, "<img", ">") + ">";
            // content = content.Replace(label, "");
            txt.AppendArticle(title + (title is ""?"":"\n"), ref content);
            txt.save();
            txt.Dispose();
		}
        public static void downloadText(Catalog info, string path)
		{
            var textFile = new TextGenerator(path);
			foreach (Volume volume in info.Volumes)
			{
                string name = volume.VolumeName;
                if (name is "")
                { name = "第一卷全"; }
                int index = 1;
                textFile.AppendLine(ref name);
				foreach (TitleLink item in volume.TitleLinkDict)
				{
                    string key = item.Title;
                    string content = "";
                    try
					{ content = extractArticle(item.Link); }
					catch(Exception ex)
					{
                        var now = DateTime.Now;
                        string logTime =
                            $"{now.Year}-{string.Format("{0:D2}", now.Month)}-{string.Format("{0:D2}", now.Day)} {string.Format("{0:D2}", now.Hour)}:{string.Format("{0:D2}", now.Minute)}:{string.Format("{0:D2}", now.Second)}.{string.Format("{0:D3}", now.Millisecond)}";
                        string message =
                            $"{logTime},error\t[{ex.GetType().FullName}<url:{item.Link}>]{ex.Message.Split('\n', '\r')[0]}";
                        textFile.AppendLine(ref message);
                        Console.WriteLine($"Error: {message}"); // :Debug
                    }
                    // string label = "<img" + TextParser.extractOne(content, "<img", "\">") + "\">";
                    // content = content.Replace(label, "");
                    textFile.AppendArticle(key, index, ref content);
                    Console.WriteLine($"Successfully download <{name.Replace("\n", "")}> [{key}]"); // :Debug
                    index++;
                }
			}
            textFile.save();
            textFile.Dispose();
		}
        public static void downloadDocx(Catalog info, string path)
		{
            var docFile = new DocxGenerator(path);
			foreach (Volume volume in info.Volumes)
			{
                string name = volume.VolumeName;
                if (name is "")
                { name = "第一卷全"; }
                docFile.AppendTitle1(name, false);
                docFile.AppendMarkStart(name);
                // int index = 1;
				foreach (TitleLink item in volume.TitleLinkDict)
				{
                    string key = item.Title;
                    string content = "";
					try 
                    { content = extractArticle(item.Link); }
					catch(Exception ex)
					{
                        var now = DateTime.Now;
                        string logTime =
                            $"{now.Year}-{string.Format("{0:D2}", now.Month)}-{string.Format("{0:D2}", now.Day)} {string.Format("{0:D2}", now.Hour)}:{string.Format("{0:D2}", now.Minute)}:{string.Format("{0:D2}", now.Second)}.{string.Format("{0:D3}", now.Millisecond)}";
                        string message =
                            $"{logTime},error\t[{ex.GetType().FullName}<url:{item.Link}>]{ex.Message.Split('\n', '\r')[0]}";
                        docFile.AppendParagraph(message, false);
                        Console.WriteLine($"Error: {message}"); // :Debug
                    }
                    docFile.AppendTitle2(key, false);
                    docFile.AppendMarkStart(key);
					foreach (string para in content.Split('\n'))
					{
                        // 下载图片
                        if (para.ToLower().Contains("<img src="))
						{
                            string link = "";
                            byte[] bytes = new byte[0];
                            link = TextParser.extractOne(para.Replace("\\\"", "\""), "<img src=\"", "\"");
                            var tempWeb = createWebProtocol(link);
							try
							{
                                tempWeb.Accept = "image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8";
                                bytes = tempWeb.contentBytes;
                                docFile.AppendImage(ref bytes, true);
                                bytes = null;
							}
							catch (Exception ex)
							{
                                if (bytes.Length != 0)
								{
									try
									{
                                        bytes = null;
                                        GC.Collect(); 
                                        docFile.AppendImage(new Uri(link), true);
                                        goto L2;
									}
									catch
									{ goto L1; }
                                }
                                L1:
                                var now = DateTime.Now;
                                string logTime =
                                    $"{now.Year}-{string.Format("{0:D2}", now.Month)}-{string.Format("{0:D2}", now.Day)} {string.Format("{0:D2}", now.Hour)}:{string.Format("{0:D2}", now.Minute)}:{string.Format("{0:D2}", now.Second)}.{string.Format("{0:D3}", now.Millisecond)}";
                                string message =
                                    $"{logTime},error\t[{ex.GetType().FullName}<url:{link}>]{ex.Message.Split('\n', '\r')[0]}";
                                docFile.AppendParagraph(message, false);
                                Console.WriteLine($"{message}"); // :Debug
                            }
                            L2:
                            continue;                           
						}
                        // 下载文字
                        var paragraph = docFile.AppendParagraph(para, false);
                        docFile.SetFirstLineIndent(paragraph);
					}
                    docFile.AppendMarkEnd(key);
                    docFile.AppendParagraph("", true);
                    Console.WriteLine($"Successfully download <{name.Replace("\n", "")}> [{key}]"); // :Debug
                    // index++;
                }
                docFile.AppendMarkEnd(name);
			}
            docFile.Save();
            docFile.Dispose();
		}

        private static WebProtocol createWebProtocol(string url)
        {
            var web = new WebProtocol(url);
            web.Timeout = 10000; // 防止某一次请求超时
            web.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            web.Headers.Add("Cache-Control", "no-cache");
            web.Headers.Add("Pragma", "no-cache");
            web.Headers.Add("Upgrade-Insecure-Requests", "1");
            web.Headers.Add("sec-fetch-mode", "navigate");
            web.Headers.Add("sec-fetch-dest", "document");
            web.Headers.Add("sec-fetch-site", "none");
            // web.Headers.Add("sec-fetch-user:", "?1");
            web.KeepAlive = true;
            return web;
        }
        private static bool exists(ref WebProtocol web)
        { return (web.getResponse().Headers["location"] is "/err/book") ? false : true; }
    }
}
