using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public class Cirkelboog : Lijn
	{
		public Cirkelboog(Layer default_layer)
			: base(Vorm_type.CirkelBoog, default_layer)
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
			if(punten.Count != 3)
				return;
			PointF M; float R;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out R);
			PointF Mtek = tek.co_pt(M, gr.DpiX, gr.DpiY);

			float Rtek = R * tek.Schaal * gr.DpiX / 2.54f;

			double hoek1 = (Math.Atan2(punten[0].Coordinaat.Y - M.Y, punten[0].Coordinaat.X - M.X) / Math.PI * 180);
			double hoek2 = (Math.Atan2(punten[1].Coordinaat.Y - M.Y, punten[1].Coordinaat.X - M.X) / Math.PI * 180);
			double hoek3 = (Math.Atan2(punten[2].Coordinaat.Y - M.Y, punten[2].Coordinaat.X - M.X) / Math.PI * 180);

			float start = (float)hoek1;
			float angle = (float)(hoek3 - hoek1);

			if(!((hoek1 < hoek2) & (hoek2 < hoek3)) & !((hoek3 < hoek2) & (hoek2 < hoek1)))
			{
				if(angle < 0)
					angle += 360;
				else
					angle -= 360;
			}

			Pen pen = (Pen)GetPen(false).Clone();
			if(Geselecteerd)
			{
				Pen selectie = new Pen(Color.Black, pen.Width + 2);
				pen.Width += 1;
				gr.DrawArc(selectie, Mtek.X - Rtek, Mtek.Y - Rtek, 2 * Rtek, 2 * Rtek, start, angle);
				gr.DrawArc(pen, Mtek.X - Rtek, Mtek.Y - Rtek, 2 * Rtek, 2 * Rtek, start, angle);
			}
			else
			{
				if(widepen)
				{
					pen.Width += 2;
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
				}
				gr.DrawArc(pen, Mtek.X - Rtek, Mtek.Y - Rtek, 2 * Rtek, 2 * Rtek, start, angle);
			}
		}


		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			if(punten.Count != 3)
				return;

			Pen pen = (Pen)GetPen(true).Clone();
			pen.Width = pen.Width / 2.54f;

			PointF M; float R;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out R);

			PointF Mtek = new PointF(M.X - window.Left, M.Y - window.Top);
			Mtek = new PointF(Mtek.X * schaal * 10, Mtek.Y * schaal * 10);
			R = R * schaal * 10;

			double hoek1 = (Math.Atan2(punten[0].Coordinaat.Y - M.Y, punten[0].Coordinaat.X - M.X) / Math.PI * 180);
			double hoek2 = (Math.Atan2(punten[1].Coordinaat.Y - M.Y, punten[1].Coordinaat.X - M.X) / Math.PI * 180);
			double hoek3 = (Math.Atan2(punten[2].Coordinaat.Y - M.Y, punten[2].Coordinaat.X - M.X) / Math.PI * 180);

			float start = (float)hoek1;
			float angle = (float)(hoek3 - hoek1);

			if(!((hoek1 < hoek2) & (hoek2 < hoek3)) & !((hoek3 < hoek2) & (hoek2 < hoek1)))
			{
				if(angle < 0)
					angle += 360;
				else
					angle -= 360;
			}
			gr.DrawArc(pen, Mtek.X - R, Mtek.Y - R, 2 * R, 2 * R, start, angle);
		}

		public override string ToString()
		{
			return "boog;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + (LijnStijl == DashStyle.Custom ? string.Join("/", DashPattern.Select(T => T.ToString()).ToArray()) : "") + ";" + string.Join(";", punten.Select(T => T.ID.ToString()).ToArray());
		}

		public override RectangleF Bounds(Graphics gr)
		{
			PointF M; float R;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out R);

			return new RectangleF(M.X - R, M.Y - R, 2 * R, 2 * R);
		}

        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            if (punten.Count != 2)
                return;
            PointF M; float R;
            Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, loc_co, out M, out R);
            PointF Mtek = tek.co_pt(M, gr.DpiX, gr.DpiY);

            float Rtek = R * tek.Schaal * gr.DpiX / 2.54f;

            double hoek1 = (Math.Atan2(punten[0].Coordinaat.Y - M.Y, punten[0].Coordinaat.X - M.X) / Math.PI * 180);
            double hoek2 = (Math.Atan2(punten[1].Coordinaat.Y - M.Y, punten[1].Coordinaat.X - M.X) / Math.PI * 180);
            double hoek3 = (Math.Atan2(loc_co.Y - M.Y, loc_co.X - M.X) / Math.PI * 180);

            float start = (float)hoek1;
            float angle = (float)(hoek3 - hoek1);

            if (!((hoek1 < hoek2) & (hoek2 < hoek3)) & !((hoek3 < hoek2) & (hoek2 < hoek1)))
            {
                if (angle < 0)
                    angle += 360;
                else
                    angle -= 360;
            }
            
            gr.DrawArc(GetPen(false), Mtek.X - Rtek, Mtek.Y - Rtek, 2 * Rtek, 2 * Rtek, start, angle);
        }

		public override int AddPunt(Punt p)
		{
			if (punten.Count < 3)
				punten.Add(p);
			return punten.Count;
		}

		public override Region GetRegion(Tekening tek)
		{
			if (punten.Count != 2)
				return new Region(new Rectangle(0, 0, 0, 0));
			Graphics gr = tek.CreateGraphics();
			PointF M; float R;
			Cirkel.CalcCirkelWaarden(Punten[0].Coordinaat, Punten[1].Coordinaat, Punten[2].Coordinaat, out M, out R);
			PointF Mtek = tek.co_pt(M, gr.DpiX, gr.DpiY);

			float Rtek = R * tek.Schaal * gr.DpiX / 2.54f;

			double hoek1 = (Math.Atan2(punten[0].Coordinaat.Y - M.Y, punten[0].Coordinaat.X - M.X) / Math.PI * 180);
			double hoek2 = (Math.Atan2(punten[1].Coordinaat.Y - M.Y, punten[1].Coordinaat.X - M.X) / Math.PI * 180);
			double hoek3 = (Math.Atan2(Punten[2].Coordinaat.Y - M.Y, Punten[2].Coordinaat.X - M.X) / Math.PI * 180);

			float start = (float)hoek1;
			float angle = (float)(hoek3 - hoek1);

			if (!((hoek1 < hoek2) & (hoek2 < hoek3)) & !((hoek3 < hoek2) & (hoek2 < hoek1)))
			{
				if (angle < 0)
					angle += 360;
				else
					angle -= 360;
			}
			
			GraphicsPath path = new GraphicsPath();
			path.AddArc(Mtek.X - Rtek, Mtek.Y - Rtek, 2 * Rtek, 2 * Rtek, start, angle);
			return new Region(path);
		}
	}
}
