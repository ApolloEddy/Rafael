using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafael.DataContainer
{
	public class TextGenerator : IDisposable
	{
		public readonly string Path;
		private FileStream TextStreamWriter;
		private bool disposedValue;

		public TextGenerator(string path)
		{
			Path = path;
			TextStreamWriter = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
		}
		public void AppendText(ref string content) 
		{ WriteString(ref content); }
		public void AppendLine(ref string content) 
		{ content += "\r\n"; WriteString(ref content); }
		public void AppendLine() 
		{ string content = "\r\n"; WriteString(ref content); }
		public void AppendTitle(string title)
		{ AppendLine(ref title); AppendLine(); }
		public void AppendArticle(string title, ref string content)
		{
			AppendTitle(title);
			string[] paragraphs = content.Split('\r', '\n');
			foreach (string paragraph in paragraphs)
			{
				if (paragraph != string.Empty) 
				{
					string line = "　　" + paragraph; // 中文缩进占位符："　"
					AppendLine(ref line);
				}
			}
		}
		public void save()
		{ TextStreamWriter.Flush(false); TextStreamWriter.Close(); }

		private void WriteString(ref string content)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(content);
			TextStreamWriter.Write(bytes, 0, bytes.Length - 1);
			TextStreamWriter.Flush(false);
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
		// ~TextGenerator()
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
