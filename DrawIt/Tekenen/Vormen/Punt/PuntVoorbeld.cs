using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawIt.Tekenen;

namespace DrawIt
{
	public class PuntVoorbeeld : Control
	{
		public PuntVoorbeeld()
		{
			this.Resize += PuntVoorbeeld_Resize;
			this.DoubleBuffered = true;
		}

		void PuntVoorbeeld_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		#region PuntStijl
		private Punt.enPuntStijl puntstijl = Punt.enPuntStijl.Plus;
		public Punt.enPuntStijl PuntStijl
		{
			get { return puntstijl; }
			set
			{
				puntstijl = value;
				this.Invalidate();
			}
		}
		#endregion
		#region Kleur
		private Color kleur = Color.Black;
		public Color Kleur
		{
			get { return kleur; }
			set
			{
				kleur = value;
				this.Invalidate();
			}
		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Point p = new Point(Width / 2, Height / 2);
			Pen pen = new Pen(Kleur);
			Brush br = new SolidBrush(Kleur);
			Graphics gr = e.Graphics;
			gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			switch(PuntStijl)
			{
				case Punt.enPuntStijl.Plus:
					gr.DrawLine(pen, p.X - 3, p.Y, p.X + 3, p.Y);
					gr.DrawLine(pen, p.X, p.Y - 3, p.X, p.Y + 3);
					break;
				case Punt.enPuntStijl.X:
					gr.DrawLine(pen, p.X - 3, p.Y - 3, p.X + 3, p.Y + 3);
					gr.DrawLine(pen, p.X - 3, p.Y + 3, p.X + 3, p.Y - 3);
					break;
				case Punt.enPuntStijl.Driehoek_open:
					gr.DrawPolygon(pen, new Point[] {
									new Point(p.X - 4, p.Y + 3),
									new Point(p.X + 4, p.Y + 3),
									new Point(p.X, p.Y - 4)
								});
					break;
				case Punt.enPuntStijl.Driehoek_vol:
					gr.FillPolygon(br, new Point[] {
									new Point(p.X - 5, p.Y + 4),
									new Point(p.X + 5, p.Y + 4),
									new Point(p.X, p.Y - 5)
								});
					break;
				case Punt.enPuntStijl.Onzichtbaar:
					gr.FillEllipse(br, new Rectangle(p.X, p.Y, 1, 2));
					break;
				case Punt.enPuntStijl.Rond_open:
					gr.DrawEllipse(pen, p.X - 3, p.Y - 3, 7, 7);
					break;
				case Punt.enPuntStijl.Rond_vol:
					gr.FillEllipse(br, p.X - 4, p.Y - 4, 9, 9);
					break;
				case Punt.enPuntStijl.Vierkant_open:
					gr.DrawRectangle(pen, p.X - 3, p.Y - 3, 7, 7);
					break;
				case Punt.enPuntStijl.Vierkant_vol:
					gr.FillRectangle(br, p.X - 4, p.Y - 4, 9, 9);
					break;
				case Punt.enPuntStijl.Ruit_open:
					gr.DrawPolygon(pen, new PointF[] { new PointF(p.X - 3, p.Y), new PointF(p.X, p.Y - 3), new PointF(p.X + 3, p.Y), new PointF(p.X, p.Y + 3) });
					break;
				case Punt.enPuntStijl.Ruit_vol:
					gr.FillPolygon(br, new PointF[] { new PointF(p.X - 4, p.Y), new PointF(p.X, p.Y - 4), new PointF(p.X + 4, p.Y), new PointF(p.X, p.Y + 4) });
					break;
			}
		}
	}
}
