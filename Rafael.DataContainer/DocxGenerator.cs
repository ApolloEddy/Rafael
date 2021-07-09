using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Core;
using Spire.Pdf;

namespace Rafael.DataContainer
{
	public class DocxGenerator
	{
		public string Path;
		protected Document Document;
		protected Section Section;
		public DocxGenerator(string path)
		{
			Path = path;
			Document = new Document();
			Section = Document.AddSection();
		}

		// [Obsolete("测试项目")]
		public void Test()
		{
			AppendTitle1("Title1");
			AppendTitle2("Title2");
			AppendParagraph("This is a test exam.");
			AppendTitle2("BILIBILI");
			AppendImage(@"F:\PixivImages\VSBKG\[81181061]白と黒の箱庭_p1.jpg");
			AppendTitle2("伊蕾娜");
			AppendImage(@"F:\PixivImages\伊蕾娜\[82491070]_p2.jpg");
			AppendTitle2("原神");
			AppendImage(@"F:\PixivImages\1.jpg");
			Save();//[85660790]_p1.jpg,
			var margin = Section.PageSetup.Margins;
			var page = Section.PageSetup;
			Console.WriteLine($"margin({margin.Top}, {margin.Left}, {margin.Bottom}, {margin.Right})");
			Console.WriteLine($"page({page.ClientHeight}, {page.ClientWidth})");
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
		public Paragraph AppendParagraph(string message = "\n", bool newpage = false) 
		{
			Paragraph para = Section.AddParagraph();
			para = SetFirstLineIndent(para);
			para.AppendText(message);
			para.AppendBreak(newpage ? BreakType.PageBreak : BreakType.LineBreak);
			return para;
		}
		// 插入标题
		public Paragraph AppendTitle(string title, BuiltinStyle style, bool newpage = false)
		{
			var para = Section.AddParagraph();
			para.AppendText(title);
			para.ApplyStyle(style);
			para.AppendBreak(newpage ? BreakType.PageBreak : BreakType.LineBreak);
			return para;
		}
		public Paragraph AppendTitle1(string title, bool newpage = false )
		{ return AppendTitle(title, BuiltinStyle.Heading1, newpage); }
		public Paragraph AppendTitle2(string title, bool newpage = false)
		{ return AppendTitle(title, BuiltinStyle.Heading2, newpage); }
		// 标签起始处
		public void AppendMarkStart(string name)
		{ Section.Paragraphs[Section.Paragraphs.Count - 1].AppendBookmarkStart(name); }
		// 标签结束处
		public void AppendMarkEnd(string name)
		{ Section.Paragraphs[Section.Paragraphs.Count - 1].AppendBookmarkEnd(name); }
		// 添加图片
		public void AppendImage(Image image, bool newpage = false)
		{
			// var picture = Section.Paragraphs[Section.Paragraphs.Count - 1].AppendPicture(image);
			var para = Section.AddParagraph();
			var picture = para.AppendPicture(image);
			resize(ref picture);
			if (newpage)
			{ para.AppendBreak(BreakType.PageBreak); }
			else
			{ para.AppendBreak(BreakType.LineBreak); }
		}
		public void AppendImage(byte[] bytes, bool newpage = false)
		{ AppendImage(Image.FromStream(new System.IO.MemoryStream(bytes)), newpage); }
		public void AppendImage(string path, bool newpage = false)
		{ AppendImage(Image.FromFile(path), newpage); }
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
	}
}
