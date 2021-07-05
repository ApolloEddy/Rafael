using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShencouSupportor
{
	public class Logger
	{
		public readonly string ContactPath;

		private FileStream file;
		public Logger(string path)
		{
			if (!path.ToLower().EndsWith(".log"))  // 检查文件格式
			{ throw new ArgumentException("错误的文件格式"); }
			if (!File.Exists(path)) // 自动创建文件
			{ file = File.Create(path); }
			else 
			{ file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite); }
			ContactPath = path;
		}

		public void Log(string type, string content)
		{
			var now = DateTime.Now;
			string logTime =
				$"{now.Year}-{string.Format("{0:D2}", now.Month)}-{string.Format("{0:D2}", now.Day)} {string.Format("{0:D2}", now.Hour)}:{string.Format("{0:D2}", now.Minute)}:{string.Format("{0:D2}", now.Second)}.{string.Format("{0:D3}", now.Millisecond)}";
			string message =
				$"{logTime},{type}\t{content.Replace("\n", "\\n").Replace("\r", "\\r")}";
			writeStringLineToFile(ref message);
		}
		public void Log(string type, string logClass, string content)
		{
			var now = DateTime.Now;
			string logTime =
				$"{now.Year}-{string.Format("{0:D2}", now.Month)}-{string.Format("{0:D2}", now.Day)} {string.Format("{0:D2}", now.Hour)}:{string.Format("{0:D2}", now.Minute)}:{string.Format("{0:D2}", now.Second)}.{string.Format("{0:D3}", now.Millisecond)}";
			string message =
				$"{logTime},{type}\t[{logClass}] {content.Replace("\n", "\\n").Replace("\r", "\\r")}";
			writeStringLineToFile(ref message);
		}
		public void Clear() 
		{ file.Seek(0, SeekOrigin.Begin); file.SetLength(0); }
		public void Dispose()
		{ file.Flush(false); file.Close(); file.Dispose(); }
	
		public void writeStringToFile(ref string message)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			file.Write(bytes, 0, bytes.Length - 1);
			file.Flush(false);
			// save();
		}
		public void writeStringLineToFile(ref string message)
		{ string msg = message + "\r\n"; writeStringToFile(ref msg); }
		public void writeStringLineToFile()
		{ string message = "\r\n"; writeStringToFile(ref message); }

		private void save()
		{ file.Close(); file.Dispose(); file = new FileStream(ContactPath, FileMode.Open, FileAccess.ReadWrite); }
	}
}
