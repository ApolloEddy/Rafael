using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aries;
using Rafael.DataContainer;

namespace Rafael.ManhuayunSupportor
{
    public static class ManhuayunSupportor
    {
        public static string page(string url)
		{
            var web = createWebProtocol(url);
            byte[] returnValue = web.contentBytes;
            
            return Encoding.UTF8.GetString(returnValue); // 规定编码 ：utf-8
        }
        public static LiteNovelInfo createInfo (ref string page)
		{
            var info = new LiteNovelInfo();
            info.source = "漫画云";
            var tp = new TextParser(page);
            tp.extrim("\n", "\r");
            info.hottext = tp.extractOne("<meta name=\"keywords\" content=\"", "\"").Replace(",漫画云轻小说文库", ""); // 关键词
            info.index = long.Parse(tp.extractOne("<script src=\"//www.mhuayun.com/api/hits/book/", "\"></script></body>"));
            tp.extractOne("<div class=\"title\">", "<span>开始阅读</span>", true);
            info.bookname = tp.extractOne("<h1 class=\"text\">", "</h1>");
            info.author = tp.extractOne("<dl class=\"author\">(.+?)<dd>", "</dd>");
            info.tag = tp.extractOne("<ul class=\"tags\">(.*)<li>", "</li>");
            info.status = tp.extractOne("<li>", "</li>"); // <ul class=\"tags\">(.+?)
            info.summary = tp.extractOne("<div class=\"j-info-desc desc\"(.+?)<span>", "</span>");

            return info;
        }
        public static void collectBookInfos(string dbpath)
		{
            const string API = "http://www.mhuayun.com/book/{index}";
            DateTime now = DateTime.Now;
            string dateParam = $"{now.Year}_{now.Month}_{now.Day}_{now.Hour}{now.Minute}{now.Second}{now.Millisecond}";
            Logger logger = new Logger($"{Environment.CurrentDirectory}\\manhuayun{dateParam}.log");
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

            for (long i = startIndex; i < 2836; i++) // 好像有限制，静态修改吧  // 好吧目前看起来是没有的 // 2836
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
                        //if (!exists(ref web))
                        //{ logger.Log("warnning", $"Index of {i} does not exist"); Console.WriteLine("Error::Message(404 Not Found)"); /*:Debugger*/ break; }
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

        private static WebProtocol createWebProtocol(string url)
		{
            var web = new WebProtocol(url);
            web.Timeout = 10000; // 防止某一次请求超时
            web.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            web.Headers.Add("Cache-Control", "no-cache");
            web.Host = "www.mhuayun.com";
            web.Headers.Add("Pragma", "no-cache");
            web.Headers.Add("Upgrade-Insecure-Requests", "1");
            web.Referer = "http://www.mhuayun.com/book";
            web.KeepAlive = true;
            return web;
        }
        private static bool exists(ref WebProtocol web)
        { return (web.getResponse().Headers["location"] is "/err/book") ? false : true; }
    }
}
