using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Aries;

namespace YouduSupportor
{
    public class YouduSupportor
    {
        public static string page(string url)
        {
            var web = new WebProtocol(url, false);
            web.Timeout = 15000;
            web.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            web.Headers.Add("sec-fetch-dest", "document");
            web.Headers.Add("sec-fetch-mode", "navigate");
            web.Headers.Add("sec-fetch-site", "none");
            web.Headers.Add("sec-fetch-user", "?1");
            web.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"92\", \" Not A;Brand\";v=\"99\", \"Microsoft Edge\";v=\"92\"");
            web.Headers.Add("cache-control", "max-age=0");
            web.Headers.Add("upgrade-insecure-requests", "1");
            web.Referer = "https://www.yoduzw.com/book/12571/";
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.70 Safari/537.36 Edg/92.0.902.30";
            //web.Headers.Add("Hm_lvt_d6c21518da630dd4f86d47c04de176de=1625396539; jieqiVisitId=article_articleviews%3D12571; zh_choose=n; jq_Obj=1; Hm_lpvt_d6c21518da630dd4f86d47c04de176de=1625408699".Replace("=", ":"));

            return web.contentDocument; 
        }
        protected static string genCookies()
		{
            string cookies = string.Empty;
            long Days = 30;
            var Then = new WebProtocol().getTime() + (Days * 24 * 60 * 60 * 1000);

            return cookies;
		}
        public static NovelInfo genInfo(ref string page)
		{
            var info = new NovelInfo();
            var tp = new TextParser(page.Replace("\n", "\\n").Replace("\r","\\n"));
            info.bookindex = long.Parse(tp.extractOne("https://www.yoduzw.com/book/", "/"));
            info.bookname = tp.extractOne("<meta property=\"og:novel:book_name\" content=\"", "\"");
            info.author = tp.extractOne("<meta property=\"og:novel:author\" content=\"", "\"");
            info.library = tp.extractOne("<meta property=\"og:novel:category\" content=\"", "\"").Replace("\\n","\n");
            info.summary = tp.extractOne("<meta property=\"og:description\" content=\"", "\"");
            info.status = tp.extractOne("<meta property=\"og:novel:status\" content=\"", "\"");
            return info;
		}
    }
}
