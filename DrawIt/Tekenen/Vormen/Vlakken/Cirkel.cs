using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public class Cirkel : Vlak
	{
		public Cirkel(Layer default_layer)
			: base(Vorm_type.Cirkel, default_layer)
		{
		}

		#region Punten
		private Collectie<Punt> punten = new Collectie<Punt>();
		public Collectie<Punt> Punten
		{
			get { return punten; }
		}

		public override Vorm[] Dep_Vormen
		{
			get
			{
				return punten.ToArray();
			}
		}
		#endregion

		public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			if(Punten.Count == 2)
			{
				PointF pt1 = tek.co_pt(Punten[0].Coordinaat, gr.DpiX, gr.DpiY);
				PointF pt2 = tek.co_pt(Punten[1].Coordinaat, gr.DpiX, gr.DpiY);

				float r = (float)Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));

				if (fill)
					gr.DrawFillEllipse(GetPen(false), GetBrush(gr,tek.Schaal,tek.Offset, false), pt1.X - r, pt1.Y - r, 2 * r, 2 * r);
				else
					gr.DrawEllipse(GetPen(false), pt1.X - r, pt1.Y - r, 2 * r, 2 * r);
			}
			else if(punten.Count > 2)
			{
				PointF M; float straal;
				CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out straal);
				PointF Mtek = tek.co_pt(new PointF(M.X, M.Y), gr.DpiX, gr.DpiY);
				Brush br = GetBrush(gr, tek.Schaal, tek.Offset, false);
				if (fill)
	                gr.DrawFillEllipse(GetPen(false), br, Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY);
				else
					gr.DrawEllipse(GetPen(false), Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY);
			}
		}

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			Pen pen = (Pen)GetPen(true).Clone();
			pen.Width = pen.Width / 2.54f;

			if(Punten.Count == 2)
			{
				IEnumerable<PointF> p = punten.Select(T => new PointF(T.Coordinaat.X - window.Left, T.Coordinaat.Y - window.Top));
				p = p.Select(T => new PointF(T.X * schaal * 10, T.Y * schaal * 10));

				PointF pt1 = p.ElementAt(0);
				PointF pt2 = p.ElementAt(1);

				float r = (float)Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));

				gr.DrawFillEllipse(pen, GetBrush(gr, schaal, new PointF(), true), pt1.X - r, pt1.Y - r, 2 * r, 2 * r);
			}
			else if(punten.Count > 2)
			{
				PointF M; float straal;
				CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out straal);

				PointF Mtek = new PointF(M.X - window.Left, M.Y - window.Top);
				Mtek = new PointF(Mtek.X * schaal * 10, Mtek.Y * schaal * 10);
				straal = straal * schaal * 10;

				gr.DrawFillEllipse(pen, GetBrush(gr, schaal, new PointF(), true), Mtek.X - straal, Mtek.Y - straal, 2 * straal, 2 * straal);
			}
		}
		public static void CalcCirkelWaarden(PointF p1, PointF p2, PointF p3, out PointF M, out float R)
		{
			float x1_2 = Convert.ToSingle(Math.Pow(p1.X, 2));
			float x2_2 = Convert.ToSingle(Math.Pow(p2.X, 2));

			float m12 = (p1.X - p2.X) / (p2.Y - p1.Y);
			float m23 = (p2.X - p3.X) / (p3.Y - p2.Y);

			float teller = -p1.Y / 2 + p3.Y / 2 + m12 * (p1.X + p2.X) / 2 - m23 * (p2.X + p3.X) / 2;
			float noemer = m12 - m23;

			M = new PointF(teller / noemer,
						   (p1.Y + p2.Y) / 2 + (p1.X - p2.X) / (p2.Y - p1.Y) * teller / noemer - (x1_2 - x2_2) / (2 * (p2.Y - p1.Y)));
			R = Convert.ToSingle(Math.Sqrt(Math.Pow(M.X - p1.X, 2) + Math.Pow(M.Y - p1.Y, 2)));
		}

		public override string ToString()
		{
			return "cirkel;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + ColorTranslator.ToOle(VulKleur1) + ";" + ColorTranslator.ToOle(VulKleur2) + ";" + (int)OpvulType + ";" + (int)VulStijl + ";" + LoopHoek + ";" + string.Join(";", punten.Select(T => T.ID.ToString()).ToArray());
		}

		public override RectangleF Bounds(Graphics gr)
		{
			if(Punten.Count == 2)
			{
				PointF pt1 = Punten[0].Coordinaat;
				PointF pt2 = Punten[1].Coordinaat;
				float r = (float)Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));

				return new RectangleF(pt1.X - r, pt1.Y - r, 2 * r, 2 * r);
			}
			else if(punten.Count > 2)
			{
				PointF M; float straal;
				CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out straal);
				return	new RectangleF(M.X - straal, M.Y - straal, 2 * straal, 2 * straal);
			}
			else
			{
				return new RectangleF();
			}
		}
		
        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            if (tek.Actie == enActie.Nieuwe_cirkel2)
            {
                if (punten.Count == 1)
                {
                    PointF pt1 = tek.co_pt(Punten[0].Coordinaat, gr.DpiX, gr.DpiY);
                    PointF pt2 = tek.co_pt(loc_co, gr.DpiX, gr.DpiY);

                    float r = (float)Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));

                    gr.DrawFillEllipse(GetPen(false), GetBrush(gr, tek.Schaal, new PointF(), false), pt1.X - r, pt1.Y - r, 2 * r, 2 * r);
                }
            }
            else if (tek.Actie == enActie.Nieuwe_cirkel3)
            {
                if (punten.Count == 2)
                {
                    PointF M; float straal;
                    CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, loc_co, out M, out straal);
                    PointF Mtek = tek.co_pt(new PointF(M.X, M.Y), gr.DpiX, gr.DpiY);
                    Brush br = GetBrush(gr, tek.Schaal, new PointF(), false);
                    gr.DrawFillEllipse(GetPen(false), br, Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY);
                }
            }
        }

		public override int AddPunt(Punt p)
		{
			if (punten.Count < 3)
				punten.Add(p);
			return punten.Count;
		}

		public override Region GetRegion(Tekening tek)
		{
			return new Region();
		}
	}
}
