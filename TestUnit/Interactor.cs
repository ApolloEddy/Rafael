using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aries;
using Rafael;
using Rafael.DataContainer;
using Rafael.LinovelibSupportor;
using Rafael.ManhuayunSupportor;
using Rafael.ShencouSupportor;

namespace TestUnit
{
	public class Interactor
	{
		public string[] Arguments { get; }
		public bool searchMood { get; set; } = false;
		public bool urlMood { get; set; } = true;
		public bool textMethod { get; set; } = true;
		public bool docMethod { get; set; } = false;
		public string parameterValue { get; set; }

		public Interactor(string[] args) 
		{ 
			Arguments = args;
		}

		public void recon()
		{
			if (Arguments.Length == 0)
			{ return; }
			foreach (string arg in Arguments)
			{
				string lowerArg = arg.ToLower();
				if (lowerArg.StartsWith("-s") || lowerArg.StartsWith("-search")) 
				{ searchMood = true; urlMood = false; continue; }
				else if (lowerArg.StartsWith("-u") || lowerArg.StartsWith("-url"))
				{ searchMood = false; urlMood = true; continue; }
				else if (lowerArg.StartsWith("-t") || lowerArg.StartsWith("-text")||lowerArg.StartsWith("-txt"))
				{ textMethod = true; docMethod = false; continue; }
				else if (lowerArg.StartsWith("-d") || lowerArg.StartsWith("-doc")) 
				{ textMethod = false; docMethod = true; continue; }
				else
				{ parameterValue = arg; }
			}
		}

		public void run()
		{
			if ((Arguments.Length == 0) || parameterValue.Length == 0)
			{ return; }
			if (searchMood) 
			{
				var result = LinovelibSupportor.search(parameterValue);
				var infos = LinovelibSupportor.extractSearchResult(ref result);
				foreach (var info in infos)
				{ info.outputToConsole("https://www.linovelib.com/novel/{index}/catalog"); }
				return;
			}

			if (parameterValue.Contains("linovelib.com"))
			{ error($"参数 \"{parameterValue}\" 不是一个合法的url链接"); return; }

			string url = "";
			LiteNovelInfo novelInfo = new LiteNovelInfo();
			Catalog catalog = new Catalog();
			if (parameterValue.Contains("caralog"))
			{
				url = parameterValue;
			}
			else if (parameterValue.Contains("https://www.linovelib.com/novel/") && parameterValue.EndsWith(".html"))
			{
				string page = LinovelibSupportor.page(parameterValue);
				var temp = LinovelibSupportor.createInfo(ref page);
				catalog = LinovelibSupportor.extractCatalog(temp);
				catalog.Novel = temp;
				novelInfo = temp;
				url = parameterValue.Replace(".html", "/catalog");
			}
			else
			{ error($"参数 \"{parameterValue}\" 不是一个合法的url链接"); }

			if (novelInfo.bookname == "")
			{
				string page = LinovelibSupportor.page(parameterValue.Replace("/catalog", ".html"));
				novelInfo = LinovelibSupportor.createInfo(ref page);
				catalog.Novel = novelInfo;
			}

			if (catalog.Volumes.Count == 0)
			{
				string page = LinovelibSupportor.page(url);
				var temp = LinovelibSupportor.createInfo(ref page);
				catalog = LinovelibSupportor.extractCatalog(temp);
			}

			string path = Environment.CurrentDirectory + "/" + catalog.Novel.bookname;

			novelInfo.outputToConsole("https://www.linovelib.com/novel/{index}/catalog");
			if (textMethod)
			{
				LinovelibSupportor.downloadText(catalog, path + "txt");
			}
			else
			{
				LinovelibSupportor.downloadDocx(catalog, path + ".docx");
			}
		}
		public static void error(string message)
		{ 
			Console.ForegroundColor = ConsoleColor.Red; Console.Write("[Error]  ");
			Console.ResetColor(); Console.WriteLine(message);
		}
	}
}
