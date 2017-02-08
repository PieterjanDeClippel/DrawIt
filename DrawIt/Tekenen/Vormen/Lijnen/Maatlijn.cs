using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawIt.Tekenen
{
	public class Maatlijn : Lijn
	{
		public Maatlijn(Layer default_layer)
			: base(Vorm_type.Maatlijn, default_layer)
		{
		}

		#region Punt1
		private Punt punt1;
		public Punt Punt1
		{
			get { return punt1; }
			set { punt1 = value; }
		}
		#endregion
		#region Punt2
		private Punt punt2;
		public Punt Punt2
		{
			get { return punt2; }
			set { punt2 = value; }
		}
		#endregion
		#region Offset
		private float offset = 30f;
		public float Offset
		{
			get { return offset; }
			set { offset = value; }
		}
		#endregion
		public override Vorm[] Dep_Vormen
		{
			get
			{
				return new Vorm[] { punt1, punt2 };
			}
		}

		public override RectangleF Bounds(Graphics gr)
		{
			//throw new NotImplementedException();
			return new RectangleF();
		}

		public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
		{
		}

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			//throw new NotImplementedException();
		}

		public bool LigtOp(PointF p, Graphics gr, float schaal)
		{
			if ((punt1 == null) | (punt2 == null)) return false;
			PointF p1 = punt1.Coordinaat;
			PointF p2 = punt2.Coordinaat;

			double alfa = Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
			double beta = alfa + Math.PI / 2;

			double dx = offset * schaal / gr.DpiX * 2.54f * Math.Cos(beta);
			double dy = offset * schaal / gr.DpiY * 2.54f * Math.Sin(beta);

			p1 = new PointF((float)(p1.X + dx), (float)(p1.Y + dy));
			p2 = new PointF((float)(p2.X + dx), (float)(p2.Y + dy));

			double m1 = (p2.Y - p1.Y) / (p2.X - p1.X);
			double m2 = (p.Y - p1.Y) / (p.X - p1.X);
			if ((-0.05 < Math.Log10(m2 / m1)) & (Math.Log10(m2 / m1) < 0.05))
				return (((p1.X < p.X) & (p.X < p2.X)) | ((p2.X < p.X) & (p.X < p1.X)));
			else
				return false;
		}
		public Cursor getCursor()
		{
			double alfa = Math.Atan2(punt1.Y - punt2.Y, punt1.X - punt2.X);
			if (alfa < 0) alfa += 2 * Math.PI;

			if (alfa < Math.PI / 8)
				return Cursors.SizeNS;
			else if (alfa < 3 * Math.PI / 8)
				return Cursors.SizeNESW;
			else if (alfa < 5 * Math.PI / 8)
				return Cursors.SizeWE;
			else if (alfa < 7 * Math.PI / 8)
				return Cursors.SizeNWSE;
			else if (alfa < 9 * Math.PI / 8)
				return Cursors.SizeNS;
			else if (alfa < 11 * Math.PI / 8)
				return Cursors.SizeNESW;
			else if (alfa < 13 * Math.PI / 8)
				return Cursors.SizeWE;
			else
				return Cursors.SizeNWSE;
		}

		public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			if ((punt1 == null) | (punt2 == null)) return;
			PointF p1 = tek.co_pt(punt1.Coordinaat, gr.DpiX, gr.DpiY);
			PointF p2 = tek.co_pt(punt2.Coordinaat, gr.DpiX, gr.DpiY);

			double alfa = Math.Atan2(punt1.Coordinaat.Y - punt2.Coordinaat.Y, punt1.Coordinaat.X - punt2.Coordinaat.X);
			double beta = alfa + Math.PI / 2;

			double dx = offset * tek.Schaal * Math.Cos(beta);
			double dy = offset * tek.Schaal * Math.Sin(beta);
			double sx = 5 * Math.Cos(beta);
			double sy = 5 * Math.Sin(beta);

			double t = 3 + 2 * LijnDikte;

			Pen pen = (Pen)GetPen(false).Clone();                   // hoofdlijn
			Pen pen1 = new Pen(Geselecteerd ? Color.White : Color.Black, 1);    // hulplijnen

			double dbl_l = Ellips.Pyt(punt1.Coordinaat, punt2.Coordinaat);
			string l = dbl_l.ToString("0.000");
			int i = ((dbl_l * tek.Schaal / 2.54f * gr.DpiX) < 50) ? -1 : 1;

			if (Geselecteerd)
			{
				Pen selectie = new Pen(Color.Black, pen.Width + 2);
				Pen selectie1 = new Pen(Color.Black, 3);
				pen.Width += 1; pen1.Width += 1;

				gr.DrawLine(selectie1, p1, new PointF((float)(p1.X + dx + sx), (float)(p1.Y + dy + sy)));
				gr.DrawLine(selectie, new PointF((float)(p1.X + dx), (float)(p1.Y + dy)), new PointF((float)(p2.X + dx), (float)(p2.Y + dy)));
				gr.DrawLine(selectie1, p2, new PointF((float)(p2.X + dx + sx), (float)(p2.Y + dy + sy)));

				gr.DrawLine(pen1, p1, new PointF((float)(p1.X + dx + sx), (float)(p1.Y + dy + sy)));
				gr.DrawLine(pen, new PointF((float)(p1.X + dx), (float)(p1.Y + dy)), new PointF((float)(p2.X + dx), (float)(p2.Y + dy)));
				gr.DrawLine(pen1, p2, new PointF((float)(p2.X + dx + sx), (float)(p2.Y + dy + sy)));

				gr.FillPolygon(new SolidBrush(Color.FromArgb(255, pen.Color)), new PointF[] {
					new PointF((float)(p1.X  + dx), (float)(p1.Y + dy)),
					new PointF((float)(p1.X + dx - 20 * Math.Cos(alfa) * i - t * Math.Cos(alfa + Math.PI /2) * i), (float)(p1.Y + dy - 20 * Math.Sin(alfa) * i - t * Math.Sin(alfa + Math.PI /2) * i)),
					new PointF((float)(p1.X + dx - 20 * Math.Cos(alfa) * i - t * Math.Cos(alfa - Math.PI /2) * i), (float)(p1.Y + dy - 20 * Math.Sin(alfa) * i - t * Math.Sin(alfa - Math.PI /2) * i))
				});
				gr.FillPolygon(new SolidBrush(Color.FromArgb(255, pen.Color)), new PointF[] {
					new PointF((float)(p2.X  + dx),(float)( p2.Y + dy)),
					new PointF((float)(p2.X + dx + 20 * Math.Cos(alfa) * i + t * Math.Cos(alfa + Math.PI /2) * i), (float)(p2.Y + dy + 20 * Math.Sin(alfa) * i + t * Math.Sin(alfa + Math.PI /2) * i)),
					new PointF((float)(p2.X + dx + 20 * Math.Cos(alfa) * i + t * Math.Cos(alfa - Math.PI /2) * i), (float)(p2.Y + dy + 20 * Math.Sin(alfa) * i + t * Math.Sin(alfa - Math.PI /2) * i))
				});
				selectie1.Width = 1;
				gr.DrawPolygon(selectie1, new PointF[] {
					new PointF((float)(p1.X  + dx), (float)(p1.Y + dy)),
					new PointF((float)(p1.X + dx - 20 * Math.Cos(alfa) * i - t * Math.Cos(alfa + Math.PI /2) * i), (float)(p1.Y + dy - 20 * Math.Sin(alfa) * i - t * Math.Sin(alfa + Math.PI /2) * i)),
					new PointF((float)(p1.X + dx - 20 * Math.Cos(alfa) * i - t * Math.Cos(alfa - Math.PI /2) * i), (float)(p1.Y + dy - 20 * Math.Sin(alfa) * i - t * Math.Sin(alfa - Math.PI /2) * i))
				});
				gr.DrawPolygon(selectie1, new PointF[] {
					new PointF((float)(p2.X  + dx),(float)( p2.Y + dy)),
					new PointF((float)(p2.X + dx + 20 * Math.Cos(alfa) * i + t * Math.Cos(alfa + Math.PI /2) * i), (float)(p2.Y + dy + 20 * Math.Sin(alfa) * i + t * Math.Sin(alfa + Math.PI /2) * i)),
					new PointF((float)(p2.X + dx + 20 * Math.Cos(alfa) * i + t * Math.Cos(alfa - Math.PI /2) * i), (float)(p2.Y + dy + 20 * Math.Sin(alfa) * i + t * Math.Sin(alfa - Math.PI /2) * i))
				});

				Font f = new Font(FontFamily.GenericMonospace, 12);
				gr.TranslateTransform((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
				gr.RotateTransform((float)(alfa * 180 / Math.PI + 180));
				SizeF size = gr.MeasureString(l, f);
				gr.TranslateTransform(-size.Width / 2, -size.Height - offset * tek.Schaal);

				GraphicsPath path = new GraphicsPath();
				path.AddString(l, f.FontFamily, (int)f.Style, f.SizeInPoints * 1.3f, new PointF(), new StringFormat());
				gr.DrawPath(Pens.Black, path);
				gr.FillPath(new SolidBrush(Color.FromArgb(255, pen.Color)), path);
				gr.ResetTransform();
			}
			else
			{
				Font f = new Font(FontFamily.GenericMonospace, 12);
				if (widepen)
				{
					gr.TranslateTransform((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
					gr.RotateTransform((float)(alfa * 180 / Math.PI + 180));
					SizeF size = gr.MeasureString(l, f);
					gr.TranslateTransform(-size.Width / 2, -size.Height - offset);
					gr.FillRectangle(Brushes.Black, 0, 0, size.Width, size.Height);
					gr.ResetTransform();
				}
				else
				{
					gr.DrawLine(pen1, p1, new PointF((float)(p1.X + dx + sx), (float)(p1.Y + dy + sy)));
					gr.DrawLine(pen, new PointF((float)(p1.X + dx), (float)(p1.Y + dy)), new PointF((float)(p2.X + dx), (float)(p2.Y + dy)));
					gr.DrawLine(pen1, p2, new PointF((float)(p2.X + dx + sx), (float)(p2.Y + dy + sy)));
					gr.FillPolygon(new SolidBrush(Color.FromArgb(255, pen.Color)), new PointF[] {
						new PointF((float)(p1.X  + dx), (float)(p1.Y + dy)),
						new PointF((float)(p1.X + dx - 20 * Math.Cos(alfa) * i - t * Math.Cos(alfa + Math.PI /2) * i), (float)(p1.Y + dy - 20 * Math.Sin(alfa) * i - t * Math.Sin(alfa + Math.PI /2) * i)),
						new PointF((float)(p1.X + dx - 20 * Math.Cos(alfa) * i - t * Math.Cos(alfa - Math.PI /2) * i), (float)(p1.Y + dy - 20 * Math.Sin(alfa) * i - t * Math.Sin(alfa - Math.PI /2) * i))
					});
					gr.FillPolygon(new SolidBrush(Color.FromArgb(255, pen.Color)), new PointF[] {
						new PointF((float)(p2.X  + dx),(float)( p2.Y + dy)),
						new PointF((float)(p2.X + dx + 20 * Math.Cos(alfa) * i + t * Math.Cos(alfa + Math.PI /2) * i), (float)(p2.Y + dy + 20 * Math.Sin(alfa) * i + t * Math.Sin(alfa + Math.PI /2) * i)),
						new PointF((float)(p2.X + dx + 20 * Math.Cos(alfa) * i + t * Math.Cos(alfa - Math.PI /2) * i), (float)(p2.Y + dy + 20 * Math.Sin(alfa) * i + t * Math.Sin(alfa - Math.PI /2) * i))
					});


					gr.TranslateTransform((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
					gr.RotateTransform((float)(alfa * 180 / Math.PI + 180));
					SizeF size = gr.MeasureString(l, f);
					gr.TranslateTransform(-size.Width / 2, -size.Height - offset * tek.Schaal);
					gr.DrawString(l, f, new SolidBrush(LijnKleur), new PointF());
					gr.ResetTransform();
				}
			}
		}

		public override string ToString()
		{
			return "maatlijn;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + punt1.ID + ";" + punt2.ID + ";" + offset;

		}

		public override int AddPunt(Punt p)
		{
			if (punt1 == null)
			{
				punt1 = p;
				return 1;
			}
			else if (punt2 == null)
			{
				punt2 = p;
			}
			return 2;
		}

		public override Region GetRegion(Tekening tek)
		{
			if ((punt1 == null) | (punt2 == null)) return new Region();

			Graphics gr = tek.CreateGraphics();
			PointF p1 = tek.co_pt(punt1.Coordinaat, gr.DpiX, gr.DpiY);
			PointF p2 = tek.co_pt(punt2.Coordinaat, gr.DpiX, gr.DpiY);

			double alfa = Math.Atan2(punt1.Coordinaat.Y - punt2.Coordinaat.Y, punt1.Coordinaat.X - punt2.Coordinaat.X);
			double beta = alfa + Math.PI / 2;

			double dx = offset * Math.Cos(beta);
			double dy = offset * Math.Sin(beta);

			GraphicsPath path = new GraphicsPath();
			path.AddLine((float)(p1.X + dx), (float)(p1.Y + dy), (float)(p2.X + dx), (float)(p2.Y + dy));
			return new Region(path);
		}
	}
}
