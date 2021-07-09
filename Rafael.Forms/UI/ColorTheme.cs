using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafael.Forms.UI
{
	public class ColorThemeBase
	{
		public Color FormBackColor { get; set; }
		public Color TextBoxBackColor { get; set; }
		public Color TextBoxForeColor { get; set; }
		public Color ButtonBackColor { get; set; }
		public Color ButtonForeColor { get; set; }
		public Color LabelForeColor { get; set; }
		// DataGridView Styles
		public Color SheetBackgroundColor { get; set; }
		public Color SheetHeaderBackColor { get; set; }
		public Color SheetHeaderForeColor { get; set; }
		public Color SheetHeaderSelectedBackColor { get; set; }
		public Color SheetHeaderSelectedForeColor { get; set; }
		public Color SheetCellBackColor { get; set; }
		public Color SheetCellForeColor { get; set; }
		public Color SheetCellSelectedBackColor { get; set; }
		public Color SheetCellSelectedForeColor { get; set; }
	}
}
