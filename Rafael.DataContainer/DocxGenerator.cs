using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Spire;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Core;
using Spire.Pdf;

namespace Rafael.DataContainer
{
	public class DocxGenerator : IDisposable
	{
		public string Path;
		protected Document Document;
		protected Section Section;
		private bool disposedValue;

		public DocxGenerator(string path)
		{
			Path = path;
			Document = new Document();
			Section = Document.AddSection();
		}

		//[Obsolete("测试项目")]
		public void Test()
		{
			AppendImage(new Uri("https://img.linovelib.com/2/2668/131130/126821.jpg"), false);
			
		}
		// 首行缩进
		public Paragraph SetFirstLineIndent(Paragraph para) 
		{ para.Format.FirstLineIndent = 30f; return para; }
		// 页边距
		public Section SetPageMargin(MarginsF margin)
		{
			Section.PageSetup.Margins = margin;
			return Section;
		}
		// 插入段落
		public Paragraph AppendParagraph() 
		{
			Paragraph para = Section.AddParagraph();
			para = SetFirstLineIndent(para);
			return para;
		}
		public Paragraph AppendLine(string message = "\n")
		{
			var para = (Section.Paragraphs.Count == 0? Section.AddParagraph():Section.Paragraphs[Section.Paragraphs.Count - 1]);
			// 字符转义
			Regex reg = new Regex("(&|&)#(.+?)(?<ret>([0-9]{1,5}));");
			foreach (Match item in reg.Matches(message))
			{
				try
				{
					var matchedString = item.Groups["ret"].Value;
					int encodedString = int.Parse(matchedString);
					string newString = char.ConvertFromUtf32(encodedString);
					message = message.Replace(item.Value, newString).Replace("&#nbsp;", "　").Replace("&#nbsp;", "　");
				}
				catch { }
			}
			para.AppendText(message + "\n");
			return para;
		}
		public void AppendNewPageMark()
		{
			Section.Paragraphs[Section.Paragraphs.Count - 1].AppendBreak(BreakType.PageBreak);
		}
		// 插入标题
		public Paragraph AppendTitle(string title, BuiltinStyle style, bool newpage = false)
		{
			var para = Section.AddParagraph();
			//if (title.Contains("【第") && title.Contains("章】"))
			//{ title = title.Replace("【第", "第").Replace("章】", "章"); }

			// 字符转义
			Regex reg = new Regex("(&|&)#(.+?)(?<ret>([0-9]{1,5}));");
			foreach (Match item in reg.Matches(title))
			{
				try
				{
					var matchedString = item.Groups["ret"].Value;
					int encodedString = int.Parse(matchedString);
					string newString = char.ConvertFromUtf32(encodedString);
					title = title.Replace(item.Value, newString).Replace("&#nbsp;", "　").Replace("&#nbsp;", "　");
				}
				catch { }
			}
			para.AppendText(title);
			para.ApplyStyle(style);
			if (newpage)
			{ para.AppendBreak(BreakType.PageBreak); }
			return para;
		}
		[Obsolete("对于Word文档就不需要用\"第xxx章\"来划分目录了！")]
		public Paragraph AppendTitle(string title, BuiltinStyle style, int index, bool newpage = false)
		{
			// 自动加分节前缀
			//if (title.Contains("第") && (title.Contains("章") || (title.Contains("节"))))
			//{ title = $"第{hanString(index)}章 {title}"; }

			return AppendTitle(title, style, newpage);
		}
		public Paragraph AppendTitle1(string title, bool newpage = false )
		{ return AppendTitle(title, BuiltinStyle.Heading1, newpage); }
		public Paragraph AppendTitle1(string title, int index, bool newpage = false)
		{ return AppendTitle(title, BuiltinStyle.Heading1, index, newpage); }
		public Paragraph AppendTitle2(string title, bool newpage = false)
		{ return AppendTitle(title, BuiltinStyle.Heading2, newpage); }
		public Paragraph AppendTitle2(string title, int index, bool newpage = false)
		{ return AppendTitle(title, BuiltinStyle.Heading1, index, newpage); }
		// 标签起始处
		public void AppendMarkStart(string name)
		{
			int count;
			if (Section.Paragraphs.Count == 0)
			{ count = 0; Section.AddParagraph(); }
			else
			{ count = Section.Paragraphs.Count; }
			Section.Paragraphs[count - 1].AppendBookmarkStart(name); 
		}
		// 标签结束处
		public void AppendMarkEnd(string name)
		{ Section.Paragraphs[Section.Paragraphs.Count - 1].AppendBookmarkEnd(name); }
		// 添加图片
		public void AppendImage(ref System.Drawing.Image image, bool newpage = false)
		{
			// var picture = Section.Paragraphs[Section.Paragraphs.Count - 1].AppendPicture(image);
			var para = Section.AddParagraph();
			var picture = para.AppendPicture(image);
			resize(ref picture);
			if (newpage)
			{ para.AppendBreak(BreakType.PageBreak); }
		}
		public void AppendImage(ref byte[] bytes, bool newpage = false)
		{
			System.Drawing.Image img;
			img = System.Drawing.Image.FromStream(new MemoryStream(bytes), false, false);
			AppendImage(ref img, newpage); 
		}
		[STAThread]
		public void AppendImage(Uri uri, bool newopage = false)
		{
			// System.Drawing.Image img = System.Drawing.Image.FromFile(url);
			BitmapDecoder bitmapDecoder = BitmapDecoder.Create(uri, BitmapCreateOptions.None, BitmapCacheOption.Default);
			System.Drawing.Image img;
			bool done = false;
			bitmapDecoder.DownloadCompleted += delegate (object s, EventArgs e)
			{
				PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Frames.Add(bitmapDecoder.Frames[0]);
				using (MemoryStream stream = new MemoryStream())
				{
					pngBitmapEncoder.Save(stream);
					img = new Bitmap(stream);
					AppendImage(ref img, newopage);
				}
				GC.Collect();
				done = true;
			};
			while (!done)
			{
				System.Windows.Forms.Application.DoEvents();
				System.Threading.Thread.Sleep(20);
			}
		}
		public void AppendImage(string path, bool newpage = false)
		{ var img = System.Drawing.Image.FromFile(path); AppendImage(ref img, newpage); }
		// 保存
		public void Save() 
		{ Document.SaveToFile(Path); }

		// 图片大小自适应
		private DocPicture resize(ref DocPicture picture)
		{
			float w = picture.Width, h = picture.Height;
			float rate;
			float pw = Section.PageSetup.ClientWidth,
				ph = Section.PageSetup.ClientHeight;
			if ((w > pw) || (h > ph))
			{
				rate = 10f;
				while ((w > pw) || (h > ph))
				{
					w = picture.Width * rate;
					h = picture.Height * rate;
					rate -= 0.01f;
				}
				rate += 0.01f;
			}
			else
			{
				rate = 0.01f;
				while ((w < pw) && (h < ph))
				{
					w = picture.Width * rate;
					h = picture.Height * rate;
					rate += 0.01f;
				}
			rate -= 0.01f;
			}
			
			picture.Width = w;
			picture.Height = h;
			return picture;
		}
		private DocPicture resizeToPage(ref DocPicture picture)
		{
			picture.Width = Section.PageSetup.ClientWidth;
			picture.Height = Section.PageSetup.ClientHeight;
			return picture;
		}
		private static string hanString(int number)
		{ return hanString(number.ToString()); }
		private static string hanString(string number)
		{
			string[] hanArr = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
			string[] unitArr = new string[] { "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千" };
			string result = "";
			int numLen = number.Length;
			for (int i = 0; i < numLen; i++)
			{
				int num = number[i] - 48;
				if (i != numLen - 1 && num != 0)
				{ result += hanArr[num] + unitArr[numLen - 2 - i]; }
				else
				{ result += hanArr[num]; }
			}
			return result;
		}
		[Obsolete("没用的方法")]
		private static Bitmap GetBitmapByBitmapImage(BitmapImage bitmapImage, bool isPng = false)
		{
			Bitmap bitmap;
			MemoryStream outStream = new MemoryStream();
			BitmapEncoder enc = new BmpBitmapEncoder();
			if (isPng)
			{
				enc = new PngBitmapEncoder();
			}
			enc.Frames.Add(BitmapFrame.Create(bitmapImage));
			enc.Save(outStream);
			bitmap = new Bitmap(outStream);
			return bitmap;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~DocxGenerator()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
