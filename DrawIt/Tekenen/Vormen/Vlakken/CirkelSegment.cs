using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public class CirkelSegment : Vlak
	{
		public CirkelSegment(Layer default_layer)
			: base(Vorm_type.CirkelSegment, default_layer)
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

		public override RectangleF Bounds(Graphics gr)
		{
			PointF M; float R;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out R);

			return new RectangleF(M.X - R, M.Y - R, 2 * R, 2 * R);
		}

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			PointF M; float straal;
			Cirkel.CalcCirkelWaarden(punten[0].Coordinaat, punten[1].Coordinaat, punten[2].Coordinaat, out M, out straal);
			
			PointF Mtek = new PointF(M.X - window.Left, M.Y - window.Top);
			Mtek = new PointF(Mtek.X * schaal * 10, Mtek.Y * schaal * 10);
			straal = straal * schaal * 10;

			double a0 = Math.Atan2(punten[0].Coordinaat.Y - M.Y, punten[0].Coordinaat.X - M.X) / Math.PI * 180;
			double a1 = Math.Atan2(punten[1].Coordinaat.Y - M.Y, punten[1].Coordinaat.X - M.X) / Math.PI * 180;
			double a2 = Math.Atan2(punten[2].Coordinaat.Y - M.Y, punten[2].Coordinaat.X - M.X) / Math.PI * 180;

			float start = (float)a0;
			float angle = (float)(a2 - a0);

			if(!((a0 < a1) & (a1 < a2)) & !((a2 < a1) & (a1 < a0)))
			{
				if(angle < 0)
					angle += 360;
				else
					angle -= 360;
			}

			Brush br = GetBrush(gr, schaal, new PointF(-window.Left, -window.Top), true);
			Pen pen = (Pen)GetPen(true).Clone();
			pen.Width /= 2.54f;

			GraphicsPath path = new GraphicsPath();
			path.AddArc(Mtek.X - straal, Mtek.Y - straal, 2 * straal, 2 * straal, start, angle);
			path.CloseFigure();
			gr.FillPath(br, path);
			gr.DrawPath(pen, path);
		}

		public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			PointF M; float straal;
			Cirkel.CalcCirkelWaarden(punten[0].Coordinaat, punten[1].Coordinaat, punten[2].Coordinaat, out M, out straal);
			PointF Mtek = tek.co_pt(new PointF(M.X, M.Y), gr.DpiX, gr.DpiY);

			double a0 = Math.Atan2(punten[0].Coordinaat.Y - M.Y, punten[0].Coordinaat.X - M.X) / Math.PI * 180;
			double a1 = Math.Atan2(punten[1].Coordinaat.Y - M.Y, punten[1].Coordinaat.X - M.X) / Math.PI * 180;
			double a2 = Math.Atan2(punten[2].Coordinaat.Y - M.Y, punten[2].Coordinaat.X - M.X) / Math.PI * 180;

			float start = (float)a0;
			float angle = (float)(a2 - a0);

			if(!((a0 < a1) & (a1 < a2)) & !((a2 < a1) & (a1 < a0)))
			{
				if(angle < 0)
					angle += 360;
				else
					angle -= 360;
			}

			Brush br = GetBrush(gr, tek.Schaal, tek.Offset, false);
			Pen p = GetPen(false);

			GraphicsPath path = new GraphicsPath();
			path.AddArc(Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY, start, angle);
			path.CloseFigure();
			if (fill) gr.FillPath(br, path);
			gr.DrawPath(p, path);
		}

		public override string ToString()
		{
			return "cirkelsegment;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + ColorTranslator.ToOle(VulKleur1) + ";" + ColorTranslator.ToOle(VulKleur2) + ";" + (int)OpvulType + ";" + (int)VulStijl + ";" + LoopHoek + ";" + string.Join(";", punten.Select(T => T.ID.ToString()).ToArray());
		}
		
        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            if (punten.Count != 2) return;
            PointF M; float straal;
            Cirkel.CalcCirkelWaarden(punten[0].Coordinaat, punten[1].Coordinaat, loc_co, out M, out straal);
            PointF Mtek = tek.co_pt(new PointF(M.X, M.Y), gr.DpiX, gr.DpiY);

            double a0 = Math.Atan2(punten[0].Coordinaat.Y - M.Y, punten[0].Coordinaat.X - M.X) / Math.PI * 180;
            double a1 = Math.Atan2(punten[1].Coordinaat.Y - M.Y, punten[1].Coordinaat.X - M.X) / Math.PI * 180;
            double a2 = Math.Atan2(loc_co.Y - M.Y, loc_co.X - M.X) / Math.PI * 180;

            float start = (float)a0;
            float angle = (float)(a2 - a0);

            if (!((a0 < a1) & (a1 < a2)) & !((a2 < a1) & (a1 < a0)))
            {
                if (angle < 0)
                    angle += 360;
                else
                    angle -= 360;
            }

            Brush br = GetBrush(gr, tek.Schaal, new PointF(), false);
            Pen p = GetPen(false);

            GraphicsPath path = new GraphicsPath();
            path.AddArc(Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY, start, angle);
            path.CloseFigure();
            gr.FillPath(br, path);
            gr.DrawPath(p, path);
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
