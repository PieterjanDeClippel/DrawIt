using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public class Ellips : Vlak
	{
		public Ellips(Layer default_layer)
			: base(Vorm_type.Ellips, default_layer)
		{
		}

		#region Punten
		private Punt f1;
		public Punt F1
		{
			get { return f1; }
			set { f1 = value; }
		}
		private Punt f2;
		public Punt F2
		{
			get { return f2; }
			set { f2 = value; }
		}
		private Punt p;
		public Punt P
		{
			get { return p; }
			set { p = value; }
		}

		public override Vorm[] Dep_Vormen
		{
			get
			{
				return new Punt[] { f1, f2, p };
			}
		}
		#endregion

		public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			PointF loc;
			SizeF afm;
			float hoek;

			CalcEllipsWaarden(f1.Coordinaat, f2.Coordinaat, p.Coordinaat, out loc, out afm, out hoek);

			#region Assenstelsel draaien
			System.Drawing.Drawing2D.Matrix oudematrix = gr.Transform;
			System.Drawing.Drawing2D.Matrix transform = new System.Drawing.Drawing2D.Matrix();
			transform.RotateAt((float)(hoek * 180 / Math.PI), tek.Offset);
			gr.Transform = transform;
			#endregion

			PointF loc2 = tek.co_pt(loc, gr.DpiX, gr.DpiY);

			if(fill)
				gr.DrawFillEllipse(GetPen(false), GetBrush(gr, tek.Schaal, tek.Offset, false), loc2.X, loc2.Y, afm.Width * tek.Schaal / 2.54f * gr.DpiX, afm.Height * tek.Schaal / 2.54f * gr.DpiY);
			else
				gr.DrawEllipse(GetPen(false), loc2.X, loc2.Y, afm.Width * tek.Schaal / 2.54f * gr.DpiX, afm.Height * tek.Schaal / 2.54f * gr.DpiY);

			#region Assenstelsel herstellen
			gr.Transform = oudematrix;
			#endregion
		}

		void CalcEllipsWaarden(PointF F1, PointF F2, PointF P, out PointF loc, out SizeF afm, out float hoek)
		{
			double som = Pyt(F1, P) + Pyt(F2, P);
			double dF = Pyt(F1, F2);

			hoek = (float)Math.Atan2(F2.Y - F1.Y, F2.X - F1.X);
			ChangeAxis(ref F1, hoek);
			ChangeAxis(ref F2, hoek);
			ChangeAxis(ref P, hoek);

			afm = new SizeF((float)som, (float)(2 * Math.Sqrt(Math.Pow(som / 2, 2) - Math.Pow(dF / 2, 2))));
			loc = new PointF(F1.X - (float)(som - dF) / 2, F1.Y - afm.Height / 2);
		}

		public static double Pyt(PointF P1, PointF P2)
		{
			double x1 = P1.X, y1 = P1.Y, x2 = P2.X, y2 = P2.Y;
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
		}
		void ChangeAxis(ref PointF punt, float rotatie)
		{
			// x' = x . cos(a) + y . sin(a)
			// y' = y . cos(a) - x . sin(a)

			punt = new PointF(
				(float)(punt.X * Math.Cos(rotatie) + punt.Y * Math.Sin(rotatie)),
				(float)(punt.Y * Math.Cos(rotatie) - punt.X * Math.Sin(rotatie))
			);
		}

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			Pen pen = (Pen)GetPen(true).Clone();
			pen.Width = pen.Width / 2.54f;

			PointF loc;
			SizeF afm;
			float hoek;

			CalcEllipsWaarden(f1.Coordinaat, f2.Coordinaat, p.Coordinaat, out loc, out afm, out hoek);

			#region Assenstelsel draaien
			System.Drawing.Drawing2D.Matrix oudematrix = gr.Transform;
			System.Drawing.Drawing2D.Matrix transform = gr.Transform.Clone();

			float dx = 10 * window.Left, dy = 10 * window.Top;
			dx = Convert.ToSingle(dx * Math.Cos(hoek) - dy * Math.Sin(hoek));
			dy = Convert.ToSingle(dx * Math.Sin(hoek) + dy * Math.Cos(hoek));
			//transform.Translate(dx, dy);

			transform.RotateAt((float)(hoek * 180 / Math.PI), new PointF(-window.Left * 10, -window.Top * 10));


			gr.Transform = transform;
			#endregion

			loc = new PointF(loc.X - window.Left, loc.Y - window.Top);
			loc = new PointF(loc.X * schaal * 10, loc.Y * schaal * 10);
			afm = new SizeF(afm.Width * schaal * 10, afm.Height * schaal * 10);

			gr.DrawFillEllipse(pen, GetBrush(gr, schaal, new PointF(), true), loc.X, loc.Y, afm.Width, afm.Height);

			#region Assenstelsel herstellen
			gr.Transform = oudematrix;
			#endregion
		}

		public override string ToString()
		{
			return "ellips;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + ColorTranslator.ToOle(VulKleur1) + ";" + ColorTranslator.ToOle(VulKleur2) + ";" + (int)OpvulType + ";" + (int)VulStijl + ";" + LoopHoek + ";" + f1.ID + ";" + f2.ID + ";" + P.ID;
		}

		public override RectangleF Bounds(Graphics gr)
		{
			PointF loc;
			SizeF afm;
			float hoek;

			CalcEllipsWaarden(f1.Coordinaat, f2.Coordinaat, p.Coordinaat, out loc, out afm, out hoek);

			return new RectangleF(
				Convert.ToSingle(loc.X * Math.Cos(-hoek) - afm.Height * Math.Sin(-hoek)),
				Convert.ToSingle(loc.Y * Math.Cos(-hoek)),
				Convert.ToSingle(afm.Height * Math.Sin(-hoek) + afm.Width * Math.Cos(-hoek)),
				Convert.ToSingle(afm.Height * Math.Cos(-hoek) + afm.Width * Math.Sin(-hoek))
			);
		}
		
        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            if ((f1 == null) | (f2 == null)) return;
            PointF loc;
            SizeF afm;
            float hoek;

            CalcEllipsWaarden(f1.Coordinaat, f2.Coordinaat, loc_co, out loc, out afm, out hoek);

            #region Assenstelsel draaien
            System.Drawing.Drawing2D.Matrix oudematrix = gr.Transform;
            System.Drawing.Drawing2D.Matrix transform = new System.Drawing.Drawing2D.Matrix();
            transform.RotateAt((float)(hoek * 180 / Math.PI), tek.Offset);
            gr.Transform = transform;
            #endregion

            PointF loc2 = tek.co_pt(loc, gr.DpiX, gr.DpiY);

            gr.DrawFillEllipse(GetPen(false), GetBrush(gr, tek.Schaal, new PointF(), false), loc2.X, loc2.Y, afm.Width * tek.Schaal / 2.54f * gr.DpiX, afm.Height * tek.Schaal / 2.54f * gr.DpiY);

            #region Assenstelsel herstellen
            gr.Transform = oudematrix;
            #endregion
        }

		public override int AddPunt(Punt p)
		{
			if(f1 == null)
			{
				f1 = p;
				return 1;
			}
			else if(f2 == null)
			{
				f2 = p;
				return 2;
			}
			else if(this.p == null)
			{
				this.p = p;
			}
			return 3;
		}

		public override Region GetRegion(Tekening tek)
		{
			return new Region();
		}
	}
}
