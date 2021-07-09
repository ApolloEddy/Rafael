using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rafael.DataContainer
{
	public class Rootobject : IDisposable
	{
		public List<LiteNovelInfo> LiteNovelInfos = new List<LiteNovelInfo>();
		private bool disposedValue;

		public string update { get; set; }

		public string toJsonString()
		{ return JsonConvert.SerializeObject(this); }
		public static Rootobject load(string path)
		{
			string jsonString = System.IO.File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Rootobject>(jsonString);
		}
		public LiteNovelInfo[] search(string searchkey)
		{
			List<LiteNovelInfo> results = new List<LiteNovelInfo>();
			foreach (LiteNovelInfo info in LiteNovelInfos)
			{
				List<string> keys = new List<string>();
				keys.Add(info.bookname);
				keys.AddRange(info.hottext.Split(','));
				keys.Add(info.author);
				string keytemplate = string.Join("", keys.ToArray());
				if(keytemplate.Contains(searchkey))
				{ results.Add(info); }
			}
			return results.ToArray();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: 释放托管状态(托管对象)
				}

				// TODO: 释放未托管的资源(未托管的对象)并重写终结器
				// TODO: 将大型字段设置为 null
				disposedValue = true;
			}
		}

		// // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
		// ~Rootobject()
		// {
		//     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
	public struct LiteNovelInfo
	{
		public string source { get; set; }
		public long index { get; set; }
		public string bookname { get; set; }
		public string hottext { get; set; }
		public string author { get; set; }
		public string summary { get; set; }
		public string tag { get; set; }
		public string status { get; set; }

		public string toJsonString()
		{ return JsonConvert.SerializeObject(this); }
	}
	public struct Catalog
	{
		public LiteNovelInfo Novel { get; set; }
		public List<Volume> Volumes { get; set; }
		public static Catalog Create()
		{
			Catalog catalog = new Catalog();
			catalog.Volumes = new List<Volume>();
			return catalog;
		}
	}
	public struct Volume
	{
		public Catalog BaseCatalog { get; set; }
		public string VolumeName { get; set; }
		public List<TitleLink> TitleLinkDict;
		public static Volume Create()
		{
			Volume volume = new Volume();
			volume.TitleLinkDict = new List<TitleLink>();
			return volume;
		}
	}
	public struct TitleLink
	{
		public string Title { get; set; }
		public string Link { get; set; }
		public TitleLink(string title, string link) 
		{ Title = title; Link = link; }
	}
}
