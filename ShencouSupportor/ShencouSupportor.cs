using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rafael;
using Aries;
using Rafael.DataContainer;

namespace Rafael.ShencouSupportor
{
    public static class ShencouSupportor
    {
        public static string page(string url)
        {
            var web = new WebProtocol(url);
            web.Timeout = 15000; // 为了应对恶心人的反爬机制
            web.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            web.Headers.Add("Cache-Control", "no-cache");
            web.Host = "www.shencou.com";
            web.Headers.Add("Pragma", "no-cache");
            web.Headers.Add("Upgrade-Insecure-Requests", "1");
            web.Referer = "http://www.shencou.com/";
            web.KeepAlive = true;
            byte[] returnValue = web.contentBytes;

            return Encoding.GetEncoding("gb2312").GetString(returnValue); // 规定编码 : gb2312
        }
        public static LiteNovelInfo createInfo(ref string page)
		{
            var info = new LiteNovelInfo();
            info.source = "神凑小说";
            var tp = new TextParser(page);
            tp.extrim("\n", "\r");
            info.hottext = tp.extractOne("<meta name=\"keywords\" content=\"", "\""); // 热词
            tp.extractOne("<div id=\"centerm\"><div id=\"content\">", "本书公告：", true); // 采集有效数据区域
            info.index = long.Parse(tp.extractOne("<a href=\"http://www.shencou.com/read/[0-9]/", "/index.html\"")); // 正则的妙用23333
            info.bookname = tp.extractOne("/index.html\">", "小说</a>"); // </span>&nbsp;&nbsp;  // 小说书名
            info.tag = tp.extractOne(">&nbsp;&nbsp;&nbsp;&nbsp;出品文库：", "</td>"); // 文库
            info.author = tp.extractOne("小说作者：", "</td>"); // 作者
            info.status = tp.extractOne("写作进度：", "</td>"); // 进度 [“连载中”或“完结”]
            info.summary = tp.extractOne("内容简介：</span><br />&nbsp;&nbsp;&nbsp;&nbsp;", "<span class=\"hottext\">") // 神奇的注释位置
                .Replace("<br />", "\n").Replace("<br/>", "\n").Replace("<br>", "\n");  // 简介

            return info;
		}
        public static void collectBookInfos(string dbpath)
		{
            const string API = "http://www.shencou.com/books/read_{index}.html";
            DateTime now = DateTime.Now;
            string dateParam = $"{now.Year}_{now.Month}_{now.Day}_{now.Hour}{now.Minute}{now.Second}{now.Millisecond}";
            Logger logger = new Logger($"{Environment.CurrentDirectory}\\shencou{dateParam}.log");
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

            for(long i = startIndex; i < 1940; i++) // 好像有限制，静态修改吧  // 好吧目前看起来是没有的
			{
                string url = API.Replace("{index}", i.ToString());
                logger.Log("info", $"Try to load page from {url}");
                Console.Write($"Collecting index[{i}]..."); // :Debugger
                for (int j = 1; j <= 6; j++)
				{
                    try
				    {
                        string html = page(url);
                        // if(!exists(ref html)) 
                        // { logger.Log("warnning", $"Index of {i} does not exist"); Console.WriteLine("Error"); /*:Debugger*/ break; }
                        LiteNovelInfo info = createInfo(ref html);
                        logger.Log("info", $"Successfully parse novel info at [{info.index}]\"{info.bookname}\"");
                        root.LiteNovelInfos.Add(info);
                        Console.WriteLine($"Success :: <bookname:{info.bookname}>"); //:Debugger
                        break;
				    }
				    catch (Exception ex) // 报错机制
				    {
                        if(j < 6) 
                        { logger.Log("warnning", $"{j}th times fail to load page from {url}"); }
                        else 
                        {
                            if (ex.Message.Contains("远程服务器返回错误: (404) 未找到。"))
                            { logger.Log("warnning", $"Index of {i} does not exist"); Console.WriteLine($"Error::Message({ex.Message})"); /*:Debugger*/ break; }
                            logger.Log("error", $"{ex.GetType()}", $"Unable to load page from {url}::Message:({ex.Message})");
                            errorLogger.Log("error", $"{ex.GetType()}", $"Unable to load page from {url}::Message:({ex.Message})");
                            Console.WriteLine("Error!"); // :Debugger
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

        private static bool exists(ref string page)
		{ 
            return 
                (page.Contains("<title>404_神凑小说网</title>") || 
                page.Contains("<title>出现错误！</title>") || 
                (page.Trim() == "")) 
                ? false : true; 
        }
    }
}
