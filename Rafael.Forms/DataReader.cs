using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rafael.DataContainer;

namespace Rafael.Forms
{
	public class DataReader : IDisposable
	{
		public Rootobject[] ROOTS;
		private bool disposedValue;

		public DataReader(string path)
		{
			// 这里的path接受的是文件夹路径
			string[] HOSTS = { "linovelib", "youdu", "shencou", "manhuayun" };
			List<Rootobject> rt = new List<Rootobject>();
			path = path + @"\{host}.json";
			foreach (string host in HOSTS)
			{
				string dbpath = path.Replace("{host}", host);
				if (File.Exists(dbpath))
				{ rt.Add(Rootobject.load(dbpath)); }
			}
			ROOTS = rt.ToArray();
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
		// ~DataReader()
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
}
