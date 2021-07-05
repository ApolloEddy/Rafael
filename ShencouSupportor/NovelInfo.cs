using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShencouSupportor
{

	public class Rootobject
	{
		public List<Novelinfo> NovelInfo = new List<Novelinfo>();
		public string update { get; set; }

		public string toJsonString()
		{ return JsonConvert.SerializeObject(this); }
		public static Rootobject load(string path)
		{
			string jsonString = System.IO.File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Rootobject>(jsonString);
		}
	}

	public struct Novelinfo
	{
		public long index { get; set; }
		public string bookname { get; set; }
		public string hottext { get; set; }
		public string author { get; set; }
		public string summary { get; set; }
		public string library { get; set; }
		public string status { get; set; }

		public string toJsonString()
		{ return JsonConvert.SerializeObject(this); }
	}

}
