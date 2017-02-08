//#define a
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DrawIt.AfbeedlingEditor
{
	public partial class AfbeeldingEditor : ScrollableControl
	{
		public AfbeeldingEditor()
		{
			InitializeComponent();
			DoubleBuffered = true;
			AutoScroll = true;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if ((ModifierKeys & Keys.Control) == Keys.Control)
			{
				((HandledMouseEventArgs)e).Handled = true;
				if (e.Delta > 0)
					Schaal *= 1 << (e.Delta / 120);
				else
					Schaal /= 1 << (-e.Delta / 120);
			}
			else
				base.OnMouseWheel(e);
		}
		
		public void Init()
		{
			Afbeelding afb = new Afbeelding();
			afb.Dimensie = new Size(512, 512);
			ShapeLayer l = new ShapeLayer();
			afb.Layers.Add(l);
			Ellips el = new Ellips();
			el.Afmeting = new Size(70, 50);
			el.Locatie = new Point(30, 30);
			el.Hoek = 15;
			el.LijnDikte = 2;
			el.LijnKleur = Color.Red;
			el.LijnStijl = DashStyle.Dash;
			l.Vormen.Add(el);
			el.Geselecteerd = true;

			Veelhoek v = new Veelhoek();
			v.Punten.AddRange(new DPoint[] { new DPoint(200, 200), new DPoint(500, 300), new DPoint(400, 350), new DPoint(100, 200) });
			l.Vormen.Add(v);
			v.Geselecteerd = true;
			Afbeelding = afb;
		}

		private Afbeelding afb;
		public Afbeelding Afbeelding
		{
			get { return afb; }
			set
			{
				afb = value;
				CalcAutoScrollMinSize();
				Invalidate();
			}
		}

		private void CalcAutoScrollMinSize()
		{
			if (afb == null)
				AutoScrollMinSize = new Size();
			else
				AutoScrollMinSize = new Size((int)(afb.Dimensie.Width * schaal + 5), (int)(afb.Dimensie.Height * schaal+ 5));
		}

		private double schaal = 1;
		public double Schaal
		{
			get { return schaal; }
			set
			{
				if (value == 0) return;
				schaal = value;
				CalcAutoScrollMinSize();
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (afb != null)
			{
				Bitmap bmp = afb.GetBitmap();
				Rectangle r = new Rectangle(5 + AutoScrollPosition.X, 5 + AutoScrollPosition.Y, (int)(schaal * bmp.Width), (int)(schaal * bmp.Height));
				e.Graphics.DrawImage(bmp, new PointF[] {
					new PointF(r.X, r.Y),
					new PointF(r.X + r.Width, r.Y),
					new PointF(r.X, r.Y + r.Height)
				});
#if a
				foreach (ShapeLayer l in afb.Layers.Where(T => T.GetType() == typeof(ShapeLayer)).Select(T => (ShapeLayer)T))
					foreach (Vorm v in l.Vormen.Where(T => T.Geselecteerd))
						foreach (DPoint p in v.GetPunten())
						{

						}
#endif
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//base.OnPaintBackground(e);
			LinearGradientBrush br = new LinearGradientBrush(new Point(), new Point(Width, Height), Color.FromArgb(197, 207, 223), Color.FromArgb(220, 229, 242));
			e.Graphics.FillRectangle(br, new Rectangle(0, 0, Width, Height));
		}
	}
}
