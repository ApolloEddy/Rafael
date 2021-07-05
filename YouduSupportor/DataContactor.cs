using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouduSupportor
{
	public class DataContactor
	{
		/// <summary>
		/// 通过书名关键词检索
		/// </summary>
		/// <param name="bookname">书名</param>
		/// <returns><see cref="NovelInfo"/></returns>
		public NovelInfo[] selectBook(string bookname)
		{
			List<NovelInfo> infos = new List<NovelInfo>();
			using (var db = new Information())
			{
				var info = new NovelInfo() { bookname = bookname };
				var query = from N in db.NorvelInfos
							orderby N.bookname
							select N;
				foreach (var item in query)
				{ infos.Add(item); }				
			}
			return infos.ToArray();
		}
		/// <summary>
		/// 通过作者关键词检索
		/// </summary>
		/// <param name="author">作者</param>
		/// <returns><see cref="NovelInfo"/></returns>
		public NovelInfo[] selectAuthor(string author)
		{
			List<NovelInfo> infos = new List<NovelInfo>();
			using (var db = new Information())
			{
				var info = new NovelInfo() { author = author };
				var query = from N in db.NorvelInfos
							orderby N.bookname
							select N;
				foreach (var item in query)
				{ infos.Add(item); }
			}
			return infos.ToArray();
		}
		/// <summary>
		/// 通过索引检索
		/// </summary>
		/// <param name="index">索引数</param>
		/// <returns><see cref="NovelInfo"/></returns>
		public NovelInfo[] selectIndex(long index)
		{
			List<NovelInfo> infos = new List<NovelInfo>();
			using (var db = new Information())
			{
				var info = new NovelInfo() { bookindex = index };
				var query = from N in db.NorvelInfos
							orderby N.bookname
							select N;
				foreach (var item in query)
				{ infos.Add(item); }
			}
			return infos.ToArray();
		}

		public void add(NovelInfo info)
		{
			using (var db = new Information())
			{ db.NorvelInfos.Add(info); db.SaveChanges(); }
		}
	}
}
