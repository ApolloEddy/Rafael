using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aries
{
	/// <summary>
	/// 提供文本处理的方法
	/// </summary>
	public class TextParser 
	{
		// 构造函数
		public TextParser()
		{
			usingString = new StringBuilder("");
			source = "";
		}
		public TextParser(string input)
		{
			source = input;
			usingString = new StringBuilder(input);
		}

		// 字段、属性
		/// <summary>
		/// 实例化时的源字符串
		/// </summary>
		public string source { get; }
		protected StringBuilder usingString;

		// 函数
		/// <summary>
		/// 使用正则表达式从源字符串匹配单个实例
		/// </summary>
		/// <param name="input">源字符串</param>
		/// <param name="pattern">正则表达式模式</param>
		/// <returns>匹配到的字符串实例：<seealso cref="string"/></returns>
		public static string regParseOne(string input, string pattern)
		{
			var reg = new Regex(pattern);
			return reg.Match(input).Value;
		}
		/// <summary>
		/// 使用正则表达式从源字符串匹配单个实例
		/// </summary>
		/// <param name="pattern">正则表达式模式</param>
		/// <param name="upgrade">指定是否覆盖当前实例中的字符串</param>
		/// <returns>匹配到的字符串实例：<seealso cref="string"/></returns>
		public string regParseOne(string pattern, bool upgrade = false)
		{
			var ret = regParseOne(usingString.ToString(),pattern);
			if(upgrade) { usingString = new StringBuilder(ret); }
			return ret;
		}
		/// <summary>
		/// 使用正则表达式从源字符串匹配所有的实例
		/// </summary>
		/// <param name="input">源字符串</param>
		/// <param name="pattern">正则表达式模式</param>
		/// <returns>匹配到的字符串列表：<seealso cref="List{String}"/></returns>
		public static List<string> regParse(string input, string pattern)
		{
			var reg = new Regex(pattern);
			var ret = new List<string>();
			foreach (Match  item in reg.Matches(input))
			{
				ret.Add(item.Value);
			}
			return ret;
		}
		/// <summary>
		/// 使用正则表达式从源字符串匹配所有的实例
		/// <list type="bullet">如果选择覆盖当前实例则会以换行符 '\n' 分割</list>
		/// </summary>
		/// <param name="pattern">正则表达式模式</param>
		/// <param name="upgrade">指定是否覆盖当前实例中的字符串</param>
		/// <returns>匹配到的字符串列表：<seealso cref="List{String}"/></returns>
		public List<string> regParse(string pattern,bool upgrade = false)
		{
			var ret = regParse(usingString.ToString(), pattern);
			if (upgrade) 
			{
				usingString = new StringBuilder();
				foreach (string  item in ret )
				{
					usingString.Append($"{item}\n");
				}
			}
			return ret;
		}
		/// <summary>
		/// 根据前后字符串标识从字符串中提取单个实例
		/// </summary>
		/// <param name="input">源字符串</param>
		/// <param name="start">起始字符串</param>
		/// <param name="end">结束字符串</param>
		/// <returns>匹配到的字符串实例：<seealso cref="string"/></returns>
		public static string extractOne(string input, string start, string end)
		{
			var reg = new Regex($"{start}(?<ret>(.+?)){end}");
			return reg.Match(input).Groups["ret"].Value;
		}
		/// <summary>
		/// 根据前后字符串标识从字符串中提取单个实例
		/// </summary>
		/// <param name="start">起始字符串</param>
		/// <param name="end">结束字符串</param>
		/// <param name="upgrade">指定是否覆盖当前实例中的字符串</param>
		/// <returns>匹配到的字符串实例：<seealso cref="string"/></returns>
		public string extractOne(string start, string end , bool upgrade = false)
		{
			var ret = extractOne(usingString.ToString(), start, end);
			if (upgrade) { usingString = new StringBuilder(ret); }
			return ret;
		}
		/// <summary>
		/// 根据前后字符串标识从字符串中提取所有实例
		/// </summary>
		/// <param name="input">源字符串</param>
		/// <param name="start">起始字符串</param>
		/// <param name="end">结束字符串</param>
		/// <returns>提取到的所有字符串实例：<see cref="List{String}"/></returns>
		public static List<string>	extract(string input, string start, string end)
		{
			var reg = new Regex($"{start}(?<ret>(.+?)){end}");
			var ret = new List<string>();
			string cache;
			foreach (Match item in reg.Matches(input))
			{
				cache = item.Groups["ret"].Value;
				ret.Add(cache);
			}
			return ret;
		}
		/// <summary>
		/// 根据前后字符串标识从字符串中提取所有实例
		/// <list type="bullet">如果选择覆盖当前实例则会以换行符 '\n' 分割</list>
		/// </summary>
		/// <param name="start">起始字符串</param>
		/// <param name="end">结束字符串</param>
		/// <param name="upgrade">指定是否覆盖当前实例中的字符串</param>
		/// <returns>提取到的所有字符串实例：<see cref="List{String}"/></returns>
		public List<string> extract(string start, string end,bool upgrade)
		{
			var ret = extract(usingString.ToString(), start, end);
			if (upgrade)
			{
				usingString = new StringBuilder();
				foreach (string item in ret)
				{
					usingString.Append($"{item}\n");
				}
			}
			return ret;
		}
		/// <summary>
		/// 依次从源字符串中移除指定的字符串
		/// </summary>
		/// <param name="input">源字符串</param>
		/// <param name="values">所有待移除的字符串</param>
		/// <returns>新的字符串实例：<seealso cref="string"/></returns>
		public static string extrim(string input, params string[] values)
		{
			var builder = new StringBuilder(input);
			foreach (string item in values)
			{
				builder.Replace(item, string.Empty);
			}
			return builder.ToString();
		}
		/// <summary>
		/// 依次从源字符串中移除指定的字符串，并自动更新到实例
		/// </summary>
		/// <param name="values">所有待移除的字符串</param>
		/// <returns>新的字符串实例：<seealso cref="string"/></returns>
		public string extrim(params string[] values)
		{
			var ret = extrim(usingString.ToString(), values);
			usingString = new StringBuilder(ret);
			return ret;
		}
		/// <summary>
		/// 去除源字符串的所有换行符('\n')、回车符('\r')、换页符('\f')
		/// </summary>
		/// <param name="input"></param>
		/// <returns>新的字符串实例：<seealso cref="string"/></returns>
		public static string removeNewlineChars(string input)
		{
			var parser = new TextParser(input);
			parser.extrim("\n", "\r", "\f");
			return parser.ToString();
		}
		/// <summary>
		/// 去除源字符串的所有换行符('\n')、回车符('\r')、换页符('\f')，并自动更新到实例
		/// </summary>
		/// <returns>新的字符串实例：<seealso cref="string"/></returns>
		public string removeNewlineChars()
		{
			var ret = removeNewlineChars(usingString.ToString());
			usingString = new StringBuilder(ret);
			return ret;
		}

		/// <summary>
		/// 返回当前实例所表示的字符串
		/// </summary>
		/// <returns>当前实例表示的字符串：<seealso cref="string"/></returns>
		public new string ToString()
		{
			return usingString.ToString();
		}
	}
}
