using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafael.Forms.UI
{
	public sealed class DarkTheme : ColorThemeBase
	{
		public DarkTheme(Form form)
		{
			// init
			FormBackColor = Color.FromArgb(23, 23, 23);
			TextBoxBackColor = Color.FromArgb(58, 58, 58);
			TextBoxForeColor = Color.FromKnownColor(KnownColor.HighlightText);
			ButtonBackColor = Color.FromArgb(50, 50, 50);
			ButtonForeColor = Color.WhiteSmoke;
			LabelForeColor = Color.LightGray;
			SheetBackgroundColor = Color.FromArgb(50, 50, 50);
			SheetHeaderBackColor = Color.FromArgb(38, 38, 38);
			SheetHeaderForeColor = Color.LightGray;
			SheetHeaderSelectedBackColor = SheetHeaderBackColor;
			SheetHeaderSelectedForeColor = SheetHeaderForeColor;
			SheetCellBackColor = Color.FromArgb(43, 43, 43);
			SheetCellForeColor = Color.WhiteSmoke;
			SheetCellSelectedBackColor = Color.FromKnownColor(KnownColor.Highlight);
			SheetCellSelectedForeColor = Color.FromKnownColor(KnownColor.HighlightText);
			
			// adapt
			form.BackColor = FormBackColor;
			foreach ( Control control in form.Controls)
			{
				switch (control.GetType().Name)
				{
					case "TextBox":
						{
							control.BackColor = TextBoxBackColor;
							control.ForeColor = TextBoxForeColor;
							break;
						}
					case "Button":
						{
							control.BackColor = ButtonBackColor;
							control.ForeColor = ButtonForeColor;
							break;
						}
					case "Label":
						{
							control.ForeColor = LabelForeColor;
							break;
						}
					case "DataGridView":
						{
							var dgv = (DataGridView)control;
							dgv.BackgroundColor = SheetBackgroundColor;
							dgv.ColumnHeadersDefaultCellStyle.BackColor = SheetHeaderBackColor;
							dgv.ColumnHeadersDefaultCellStyle.ForeColor = SheetHeaderForeColor;
							dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = SheetHeaderSelectedBackColor;
							dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = SheetHeaderSelectedForeColor;
							dgv.RowHeadersDefaultCellStyle.BackColor = SheetHeaderBackColor;
							dgv.RowHeadersDefaultCellStyle.ForeColor = SheetHeaderForeColor;
							dgv.RowHeadersDefaultCellStyle.SelectionBackColor = SheetHeaderSelectedBackColor;
							dgv.RowHeadersDefaultCellStyle.SelectionForeColor = SheetHeaderSelectedForeColor;
							dgv.DefaultCellStyle.BackColor = SheetCellBackColor;
							dgv.DefaultCellStyle.ForeColor = SheetCellForeColor;
							dgv.DefaultCellStyle.SelectionBackColor = SheetCellSelectedBackColor;
							dgv.DefaultCellStyle.SelectionForeColor = SheetCellSelectedForeColor;
							
							break;
						}
					default:
						{
							control.BackColor = TextBoxBackColor;
							control.ForeColor = TextBoxForeColor;
							break;
						}
				}
			}
		}
	}
}
