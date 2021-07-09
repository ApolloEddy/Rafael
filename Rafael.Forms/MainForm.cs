using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rafael;
using Rafael.Forms.UI;
using Rafael.ShencouSupportor;
using Rafael.ManhuayunSupportor;
using Rafael.DataContainer;

namespace Rafael.Forms
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			btn_query.UseVisualStyleBackColor = false;
			new DarkTheme(this);
		}
		private void btn_query_Click(object sender, EventArgs e)
		{
			dataGridView.Rows.Clear();
			List<LiteNovelInfo> results = new List<LiteNovelInfo>();
			using (DataReader data = new DataReader(@"D:\a编程学习\抽象编程工具\UnSortedProjects\Rafael\TestUnit\bin\Debug"))
			{ 
				foreach (Rootobject rt in data.ROOTS)
				{ results.AddRange(rt.search(txt_search.Text)); }
				foreach (LiteNovelInfo result in results)
				{ addCell(result); }
			}
		}

		private void addCell(string bookname, string author, string library, string status, string source, string summary)
		{ dataGridView.SuspendLayout(); dataGridView.Rows.Add(bookname, author, library, status, source, summary); }
		private void addCell(LiteNovelInfo info) 
		{ addCell(info.bookname, info.author, info.tag, info.status, info.source, info.summary); }

	}
}
