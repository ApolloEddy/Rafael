using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			// ShencouSupportor.collectBookInfos($"{Environment.CurrentDirectory}\\shencou.json");

			// :Debug at 漫画云
			// ManhuayunSupportor.collectBookInfos($"{Environment.CurrentDirectory}\\manhuayun.json");

			// :Debug at Docx File
			// new DocxGenerator(@"C:\Users\Administrator\Desktop\脚本\test.docx").Test();

			// :Debug at 哔哩轻小说
			//LinovelibSupportor.collectBookInfos($"{Environment.CurrentDirectory}\\linovelib.json");
			string page = LinovelibSupportor.page("https://www.linovelib.com/novel/1163/catalog");
			Catalog catalog = LinovelibSupportor.extractCatalog(ref page);
			LinovelibSupportor.downloadText(catalog, Environment.CurrentDirectory + "/夏目友人帐.txt");

			// 监控运行时长
			double timediff = stopwatch.ElapsedMilliseconds / 1000;
			Console.WriteLine($"Finish!\tTimes:  {string.Format("{0:F3}", timediff)} seconds");
			Console.ReadKey();
		}
	}
}
