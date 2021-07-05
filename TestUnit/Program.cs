using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouduSupportor;

namespace TestUnit
{
	class Program
	{
		static void Main(string[] args)
		{
			ShencouSupportor.ShencouSupportor
				.collectBookInfos($"{Environment.CurrentDirectory}\\shencou.json");
			Console.ReadKey();
		}
	}
}
