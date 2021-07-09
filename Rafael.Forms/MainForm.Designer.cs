
namespace Rafael.Forms
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lb_query = new System.Windows.Forms.Label();
			this.txt_search = new System.Windows.Forms.TextBox();
			this.btn_query = new System.Windows.Forms.Button();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.Bookname = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Library = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Summary = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// lb_query
			// 
			this.lb_query.AutoSize = true;
			this.lb_query.Font = new System.Drawing.Font("宋体", 12.5F);
			this.lb_query.ForeColor = System.Drawing.Color.Silver;
			this.lb_query.Location = new System.Drawing.Point(38, 35);
			this.lb_query.Name = "lb_query";
			this.lb_query.Size = new System.Drawing.Size(59, 17);
			this.lb_query.TabIndex = 0;
			this.lb_query.Text = "查询：";
			this.lb_query.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txt_search
			// 
			this.txt_search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
			this.txt_search.Font = new System.Drawing.Font("宋体", 11F);
			this.txt_search.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.txt_search.Location = new System.Drawing.Point(93, 32);
			this.txt_search.Name = "txt_search";
			this.txt_search.Size = new System.Drawing.Size(699, 24);
			this.txt_search.TabIndex = 1;
			this.txt_search.Text = "A Text Example";
			// 
			// btn_query
			// 
			this.btn_query.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
			this.btn_query.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_query.Font = new System.Drawing.Font("宋体", 10F);
			this.btn_query.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.btn_query.Location = new System.Drawing.Point(798, 32);
			this.btn_query.Name = "btn_query";
			this.btn_query.Size = new System.Drawing.Size(87, 24);
			this.btn_query.TabIndex = 2;
			this.btn_query.Text = "查询";
			this.btn_query.UseVisualStyleBackColor = true;
			this.btn_query.Click += new System.EventHandler(this.btn_query_Click);
			// 
			// dataGridView
			// 
			this.dataGridView.AllowUserToAddRows = false;
			this.dataGridView.AllowUserToDeleteRows = false;
			this.dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
			this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
			dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle1.NullValue = "null";
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Bookname,
            this.Author,
            this.Library,
            this.Status,
            this.Source,
            this.Summary});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
			dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9.3F);
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView.EnableHeadersVisualStyles = false;
			this.dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
			this.dataGridView.Location = new System.Drawing.Point(26, 76);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.ReadOnly = true;
			this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
			dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.LightGray;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView.RowTemplate.Height = 23;
			this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView.Size = new System.Drawing.Size(885, 397);
			this.dataGridView.TabIndex = 3;
			// 
			// Bookname
			// 
			this.Bookname.HeaderText = "书名";
			this.Bookname.MinimumWidth = 130;
			this.Bookname.Name = "Bookname";
			this.Bookname.ReadOnly = true;
			this.Bookname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Bookname.Width = 130;
			// 
			// Author
			// 
			this.Author.HeaderText = "作者";
			this.Author.MinimumWidth = 70;
			this.Author.Name = "Author";
			this.Author.ReadOnly = true;
			this.Author.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Author.Width = 70;
			// 
			// Library
			// 
			this.Library.HeaderText = "文库/分类";
			this.Library.MinimumWidth = 80;
			this.Library.Name = "Library";
			this.Library.ReadOnly = true;
			this.Library.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Library.Width = 80;
			// 
			// Status
			// 
			this.Status.HeaderText = "连载状态";
			this.Status.MinimumWidth = 70;
			this.Status.Name = "Status";
			this.Status.ReadOnly = true;
			this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Status.Width = 70;
			// 
			// Source
			// 
			this.Source.HeaderText = "来源";
			this.Source.MinimumWidth = 90;
			this.Source.Name = "Source";
			this.Source.ReadOnly = true;
			this.Source.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Source.Width = 90;
			// 
			// Summary
			// 
			this.Summary.HeaderText = "简介";
			this.Summary.MinimumWidth = 50;
			this.Summary.Name = "Summary";
			this.Summary.ReadOnly = true;
			this.Summary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Summary.Width = 380;
			// 
			// MainForm
			// 
			this.AcceptButton = this.btn_query;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.ClientSize = new System.Drawing.Size(944, 501);
			this.Controls.Add(this.dataGridView);
			this.Controls.Add(this.btn_query);
			this.Controls.Add(this.txt_search);
			this.Controls.Add(this.lb_query);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Rafael (Beta)";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lb_query;
		private System.Windows.Forms.TextBox txt_search;
		private System.Windows.Forms.Button btn_query;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn Bookname;
		private System.Windows.Forms.DataGridViewTextBoxColumn Author;
		private System.Windows.Forms.DataGridViewTextBoxColumn Library;
		private System.Windows.Forms.DataGridViewTextBoxColumn Status;
		private System.Windows.Forms.DataGridViewTextBoxColumn Source;
		private System.Windows.Forms.DataGridViewTextBoxColumn Summary;
	}
}

