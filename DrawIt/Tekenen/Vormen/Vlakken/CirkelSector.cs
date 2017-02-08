using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public class CirkelSector : Vlak
	{
		public CirkelSector(Layer default_layer)
			: base(Vorm_type.CirkelSector, default_layer)
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
			PointF M; float straal;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out straal);
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

			if(fill)
				gr.FillPie(GetBrush(gr, tek.Schaal,tek.Offset, false), Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY, start, angle);
			gr.DrawPie(GetPen(false), Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY, start, angle);
		}

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			PointF M; float straal;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out straal);

			Pen pen = (Pen)GetPen(true).Clone();
			pen.Width = pen.Width / 2.54f;

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

			gr.FillPie(GetBrush(gr, schaal, new PointF(), true), Mtek.X - straal, Mtek.Y - straal, 2 * straal, 2 * straal, start, angle);
			gr.DrawPie(pen, Mtek.X - straal, Mtek.Y - straal, 2 * straal, 2 * straal, start, angle);
		}

		public override string ToString()
		{
			return "cirkelsector;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + ColorTranslator.ToOle(VulKleur1) + ";" + ColorTranslator.ToOle(VulKleur2) + ";" + (int)OpvulType + ";" + (int)VulStijl + ";" + LoopHoek + ";" + string.Join(";", punten.Select(T => T.ID.ToString()).ToArray());
		}

		public override RectangleF Bounds(Graphics gr)
		{
			PointF M; float straal;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out straal);
			return new RectangleF(M.X - straal, M.Y - straal, 2 * straal, 2 * straal);
		}
		
        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            if (punten.Count != 2) return;
            PointF M; float straal;
            Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, loc_co, out M, out straal);
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

            gr.FillPie(GetBrush(gr, tek.Schaal, new PointF(), false), Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY, start, angle);
            gr.DrawPie(GetPen(false), Mtek.X - straal * tek.Schaal / 2.54f * gr.DpiX, Mtek.Y - straal * tek.Schaal / 2.54f * gr.DpiY, 2 * straal * tek.Schaal / 2.54f * gr.DpiX, 2 * straal * tek.Schaal / 2.54f * gr.DpiY, start, angle);
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
