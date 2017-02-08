using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawIt
{
	public class LijnVoorbeeld : Control
	{
		public LijnVoorbeeld()
		{
			this.Resize += LijnVoorbeeld_Resize;
			this.DoubleBuffered = true;
		}

		void LijnVoorbeeld_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawLine(pen, Width / 10, Height / 2, 9 * Width / 10, Height / 2);
		}

		private Pen pen = Pens.Black;

		public DashStyle Lijnstijl
		{
			get { return pen.DashStyle; }
			set
			{
				pen = (Pen)pen.Clone();
				pen.DashStyle = value;
				this.Invalidate();
			}
		}
		public Color Kleur
		{
			get { return pen.Color; }
			set
			{
				pen = (Pen)pen.Clone();
				pen.Color = value;
				this.Invalidate();
			}
		}

		public float LijnDikte
		{
			get { return pen.Width; }
			set
			{
				pen = (Pen)pen.Clone();
				pen.Width = value;
				this.Invalidate();
			}
		}

		public DashCap DashCap
		{
			get { return pen.DashCap; }
			set
			{
				pen = (Pen)pen.Clone();
				pen.DashCap = value;
				pen.EndCap = (LineCap)value;
				pen.StartCap = (LineCap)value;
				Invalidate();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float[] DashPattern
		{
			get
			{
				return pen.DashPattern;
			}
			set
			{
				pen.DashPattern = value;
				this.Invalidate();
			}
		}
	}
}
