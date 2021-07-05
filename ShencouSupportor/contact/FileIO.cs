using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Aries
{
	public static class FileIO
	{
		public static void save(ref byte[] bytes, string path)
		{
			File.WriteAllBytes(path, bytes);
		}
		public static void save(ref Stream stream, string path)
		{
			using(MemoryStream ms = (MemoryStream)stream)
			{
				byte[] bytes = ms.ToArray();
				File.WriteAllBytes(path, bytes);
				bytes = null;
				ms.Dispose();
			}			
		}
		public static string read(string path)
		{ return File.ReadAllText(path); }
	}
}
