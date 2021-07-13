using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafael.DataContainer
{
	public abstract class GeneratorBase
	{
		public abstract void AppendText(ref string content);
		public abstract void AppendLine();
		public abstract void AppendLine(ref string content);
		public abstract void AppendTitle(string title);
		public abstract void AppendTitle(string title, int index);
		public abstract void AppendArticle(string title, ref string content);
		public abstract void AppendArticle(string title, int index, ref string content);
		public abstract void save();

		protected static string hanString(int number)
		{ return hanString(number.ToString()); }
		protected static string hanString(string number)
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
	}
}
