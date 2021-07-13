using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rafael;
using YouduSupportor;
using Rafael.DataContainer;
using Rafael.ManhuayunSupportor;
using Rafael.ShencouSupportor;
using Rafael.LinovelibSupportor;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Pdf;

namespace TestUnit
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			// :Debug at 神凑小说
			// 数据收集
			// ShencouSupportor.collectBookInfos($"{Environment.CurrentDirectory}\\shencou.json");

			// :Debug at 漫画云
			// 数据收集
			// ManhuayunSupportor.collectBookInfos($"{Environment.CurrentDirectory}\\manhuayun.json");

			// :Debug at Docx File
			// 数据收集
			// new DocxGenerator(@"C:\Users\Administrator\Desktop\脚本\test.docx").Test();

			// :Debug at 哔哩轻小说
			// 数据收集
			// LinovelibSupportor.collectBookInfos($"{Environment.CurrentDirectory}\\linovelib.json");
			// 使用测试
			string page = LinovelibSupportor.page("https://www.linovelib.com/novel/2668/catalog"); // 
			Catalog catalog = LinovelibSupportor.extractCatalog(ref page);
			LinovelibSupportor.downloadDocx(catalog, Environment.CurrentDirectory + "/侦探已死.docx");

			//var d = new DocxGenerator(Environment.CurrentDirectory + "/test.docx");
			//d.Test();
			//d.Save();

			stopwatch.Stop();
			double timediff = (double)stopwatch.ElapsedMilliseconds / 1000; // 监控运行时长
			Console.WriteLine($"Finish!\tTimes:  {string.Format("{0:F3}", timediff)} seconds");
			Console.ReadKey();
			// ball ball 了，赶快推送上去哇啊啊啊啊啊
		}		
	}
}
