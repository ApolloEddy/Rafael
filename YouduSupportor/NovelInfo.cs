using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouduSupportor
{
	public class NovelInfo
	{
		public string bookname { get; set; }
		public long bookindex { get; set; }
		public string author { get; set; }
		public string summary { get; set; }
		public string library { get; set; }
		public string status { get; set; }

		public string toJsonString()
		{ return JsonConvert.SerializeObject(this); }
		public static string toJsonString(NovelInfo[] infos)
		{ return JsonConvert.SerializeObject(infos); }
	}

	public class Information : DbContext
	{
		public Information() 
			: base("name=NorvelInfo") 
		{ }
		public DbSet<NovelInfo> NorvelInfos { get; set; }
	}
}
