using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Rafael.DataContainer
{
	public class TextGenerator : GeneratorBase, IDisposable
	{
		public readonly string Path;
		private StreamWriter TextStreamWriter;
		private bool disposedValue;

		public TextGenerator(string path)
		{
			Path = path;
			TextStreamWriter = new StreamWriter(path);
		}
		public override void AppendText(ref string content) 
		{ WriteString(ref content); }
		public override void AppendLine(ref string content) 
		{ content += "\n"; WriteString(ref content); }
		public override void AppendLine() 
		{ string content = "\n"; WriteString(ref content); }
		public override void AppendTitle(string title)
		{ 
			// if (title.Contains("【第") && title.Contains("章】"))
			// { title = title.Replace("【第", "第").Replace("章】", "章"); }
			AppendLine(ref title); 
			AppendLine(); 
		}
		public override void AppendTitle(string title, int index) 
		{
			// 自动加分节前缀
			if (title.Contains("第") && (title.Contains("章") || (title.Contains("节"))))
			{ title = $"第{hanString(index)}章 {title}"; }
			AppendTitle(title);
		}
		public override void AppendArticle(string title, ref string content)
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
		public override void AppendArticle(string title, int index, ref string content)
		{ title = $"第{hanString(index)}章 {title}"; AppendArticle(title, ref content); }
		public override void save()
		{ TextStreamWriter.Flush(); TextStreamWriter.Close(); TextStreamWriter.Dispose(); }

		private void WriteString(ref string content)
		{
			// 处理字符串
			Regex reg;
			if (content.ToLower().Contains("<img src="))
			{
				reg = new Regex("<img src=\"(?<ret>(.+?))\"");
				content = "　　插图：url=>" + reg.Match(content).Groups["ret"].Value + "\n";
				goto L1;
			}
			reg = new Regex("(&|&)#(.+?)(?<ret>([0-9]{1,5}));");
			foreach (Match item in reg.Matches(content))
			{
				try
				{
					var matchedString = item.Groups["ret"].Value;
					int encodedString = int.Parse(matchedString);
					string newString = char.ConvertFromUtf32(encodedString);
					content = content.Replace(item.Value, newString).Replace("&#nbsp;", "　").Replace("&#nbsp;", "　");
				}
				catch { }
			}
			// 写入
			L1: 
			TextStreamWriter.Write(content);
			TextStreamWriter.Flush();
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
