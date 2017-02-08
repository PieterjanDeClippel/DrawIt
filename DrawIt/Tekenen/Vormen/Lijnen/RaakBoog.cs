using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Text;

namespace DrawIt.Tekenen
{
	public class RaakBoog : Lijn
	{
		public RaakBoog(Layer default_layer)
			: base(Vorm_type.RaakBoog, default_layer)
		{
		}

		public override Vorm[] Dep_Vormen
		{
			get
			{
				Punt ap = (Punt)richtlijn.Dep_Vormen.Except(new Punt[] { richtpunt }).First();
                return new Vorm[] { richtpunt, eindpunt, ap, richtlijn };
			}
		}

		#region Richtpunt
		private Punt richtpunt;
		public Punt RichtPunt
		{
			get { return richtpunt; }
			set { richtpunt = value; }
		}
		#endregion
		#region RichtLijn
		private Rechte richtlijn;
		public Rechte RichtLijn
		{
			get { return richtlijn; }
			set { richtlijn = value; }
		}
		#endregion
		#region EindPunt
		private Punt eindpunt;
		public Punt EindPunt
		{
			get { return eindpunt; }
			set { eindpunt = value; }
		}
		#endregion

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			if((richtpunt == null) | (eindpunt == null) | (richtlijn == null)) return;

			// punten eenv. opslaan
			PointF RP = richtpunt.Coordinaat;
			PointF EP = eindpunt.Coordinaat;
			PointF AP = ((Punt)richtlijn.Dep_Vormen.Except(new Punt[] { richtpunt }).First()).Coordinaat;

			// stap 1: loodlijn AP-RP in RP
			float a_l, b_l, c_l;
			Calc_Loodlijn(AP, RP, RP, out a_l, out b_l, out c_l);

			// stap 2: middelloodlijn RP-EP
			float a_ml, b_ml, c_ml;
			Calc_Loodlijn(RP, EP, Midden(RP, EP), out a_ml, out b_ml, out c_ml);

			// stap 3: snijpunt l en ml
			PointF M = Calc_Snijpunt(a_l, b_l, c_l, a_ml, b_ml, c_ml);
			PointF Mtek = new PointF(M.X - window.Left, M.Y - window.Top);
			Mtek = new PointF(Mtek.X * schaal * 10, Mtek.Y * schaal * 10);

			// stap 4: bepalen van ligging EP
			//	- AP invullen in l
			bool teken_AP = a_l * AP.X + b_l * AP.Y + c_l > 0;
			//	- EP invullen in l
			bool teken_EP = a_l * EP.X + b_l * EP.Y + c_l > 0;
			bool meer_dan_180 = !(teken_AP ^ teken_EP);

			// stap 5: hoeken
			float start = (float)(Math.Atan2(RP.Y - M.Y, RP.X - M.X) * 180 / Math.PI);
			float end = (float)(Math.Atan2(EP.Y - M.Y, EP.X - M.X) * 180 / Math.PI);
			float sweep = end - start;
			if(meer_dan_180 & (-180 < sweep) & (sweep < 180))
				sweep += sweep < 0 ? 360 : -360;
			if(!meer_dan_180 & ((sweep < -180) | (180 < sweep)))
				sweep += sweep < 0 ? 360 : -360;

			double R_tek = Ellips.Pyt(M, RP) * schaal * 10;

			Pen pen = (Pen)GetPen(true).Clone();
			pen.Width = pen.Width / 2.54f;
			gr.DrawArc(pen, (float)(Mtek.X - R_tek), (float)(Mtek.Y - R_tek), (float)(2 * R_tek), (float)(2 * R_tek), start, sweep);
			
		}


        public static void Calc_Rechte(PointF p1, PointF p2, out float a, out float b, out float c)
		{
			if(p1.Y == p2.Y)
			{
				a = 0;
				b = 1;
				c = -p1.Y;
			}
			else
			{
				a = 1;
				b = (p2.X - p1.X) / (p1.Y - p2.Y);
				c = -p1.X - p1.Y * b;
			}
		}
        public static PointF Midden(PointF p1, PointF p2)
		{
			return new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
		}
		public static void Calc_Loodlijn(PointF p1, PointF p2, PointF richtpunt, out float a, out float b, out float c)
		{
			// RP -> p1 & AP -> p2
			// m = (p1.X - p2.X) / (p2.Y - p1.Y); // rico ap-rp-loodlijn (door rp)
			// q = richtpunt.Y - m * richtpunt.X;
			float a1, b1, c1;
			Calc_Rechte(p1, p2, out a1, out b1, out c1);

			a = b1;
			b = -a1;
			c = -a * richtpunt.X - b * richtpunt.Y;
		}
		public static void Calc_Loodlijn(float a ,float b, float c, PointF richtpunt, out float a_out, out float b_out, out float c_out)
		{
			a_out = b;
			b_out = -a;
			c_out = -a_out * richtpunt.X - b_out * richtpunt.Y;
		}
        public static PointF Calc_Snijpunt(float a1, float b1, float c1, float a2, float b2, float c2)
		{
			return new PointF((b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1), (a1 * c2 - a2 * c1) / (a2 * b1 - a1 * b2));
		}

		public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			if((richtpunt == null) | (eindpunt == null) | (richtlijn == null)) return;

			// punten eenv. opslaan
			PointF RP = richtpunt.Coordinaat;
			PointF EP = eindpunt.Coordinaat;
			PointF AP = ((Punt)(richtlijn.Dep_Vormen.Except(new Punt[] { richtpunt }).First())).Coordinaat;
			
			// stap 1: loodlijn AP-RP in RP
			float a_l, b_l, c_l;
			Calc_Loodlijn(AP, RP, RP, out a_l, out b_l, out c_l);

			// stap 2: middelloodlijn RP-EP
			float a_ml, b_ml, c_ml;
			Calc_Loodlijn(RP, EP, Midden(RP, EP), out a_ml, out b_ml, out c_ml);

			// stap 3: snijpunt l en ml
			PointF M = Calc_Snijpunt(a_l, b_l, c_l, a_ml, b_ml, c_ml);
			PointF Mtek = tek.co_pt(M, gr.DpiX, gr.DpiY);

			// stap 4: bepalen van ligging EP
			//	- AP invullen in l
			bool teken_AP = a_l * AP.X + b_l * AP.Y + c_l > 0;
			//	- EP invullen in l
			bool teken_EP = a_l * EP.X + b_l * EP.Y + c_l > 0;
			bool meer_dan_180 = !(teken_AP ^ teken_EP);

			// stap 5: hoeken
			float start = (float)(Math.Atan2(RP.Y - M.Y, RP.X - M.X) * 180 / Math.PI);
			float end = (float)(Math.Atan2(EP.Y - M.Y, EP.X - M.X) * 180 / Math.PI);
			float sweep = end - start;
			if(meer_dan_180 & (-180 < sweep) & (sweep < 180))
				sweep += sweep < 0 ? 360 : -360;
			if(!meer_dan_180 & ((sweep < -180) | (180 < sweep)))
				sweep += sweep < 0 ? 360 : -360;

			double R_tek = Ellips.Pyt(M, RP) * tek.Schaal * gr.DpiX / 2.54f;


			Pen pen = (Pen)GetPen(false).Clone();
			if(Geselecteerd)
			{
				Pen selectie = new Pen(Color.Black, pen.Width + 2);
				pen.Width += 1;


				gr.DrawArc(selectie, (float)(Mtek.X - R_tek), (float)(Mtek.Y - R_tek), (float)(2 * R_tek), (float)(2 * R_tek), start, sweep);
				gr.DrawArc(pen, (float)(Mtek.X - R_tek), (float)(Mtek.Y - R_tek), (float)(2 * R_tek), (float)(2 * R_tek), start, sweep);
			}
			else
			{
				if(widepen)
				{
					pen.Width += 2;
					pen.DashStyle = DashStyle.Solid;
				}
				gr.DrawArc(pen, (float)(Mtek.X - R_tek), (float)(Mtek.Y - R_tek), (float)(2 * R_tek), (float)(2 * R_tek), start, sweep);
			}
		}

		public override string ToString()
		{
			return "raak;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + (LijnStijl == System.Drawing.Drawing2D.DashStyle.Custom ? string.Join("/", DashPattern.Select(T => T.ToString()).ToArray()) : "") + ";" + RichtPunt.ID + ";" + eindpunt.ID + ";" + richtlijn.ID;
		}

		public override RectangleF Bounds(Graphics gr)
		{
			// punten eenv. opslaan
			PointF RP = richtpunt.Coordinaat;
			PointF EP = eindpunt.Coordinaat;
			PointF AP = ((Punt)richtlijn.Dep_Vormen.Except(new Punt[] { richtpunt }).First()).Coordinaat;

			// stap 1: loodlijn AP-RP in RP
			float a_l, b_l, c_l;
			Calc_Loodlijn(AP, RP, RP, out a_l, out b_l, out c_l);

			// stap 2: middelloodlijn RP-EP
			float a_ml, b_ml, c_ml;
			Calc_Loodlijn(RP, EP, Midden(RP, EP), out a_ml, out b_ml, out c_ml);

			// stap 3: snijpunt l en ml
			PointF M = Calc_Snijpunt(a_l, b_l, c_l, a_ml, b_ml, c_ml);
			
			float R = (float)Ellips.Pyt(M, RP);
			return new RectangleF(M.X - R, M.Y - R, 2 * R, 2 * R);
		}
		
        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            if ((richtpunt == null) | (richtlijn == null)) return;

            // punten eenv. opslaan
            PointF RP = richtpunt.Coordinaat;
            PointF EP = loc_co;
            PointF AP = ((Punt)(richtlijn.Dep_Vormen.Except(new Punt[] { richtpunt }).First())).Coordinaat;

            // stap 1: loodlijn AP-RP in RP
            float a_l, b_l, c_l;
            Calc_Loodlijn(AP, RP, RP, out a_l, out b_l, out c_l);

            // stap 2: middelloodlijn RP-EP
            float a_ml, b_ml, c_ml;
            Calc_Loodlijn(RP, EP, Midden(RP, EP), out a_ml, out b_ml, out c_ml);

            // stap 3: snijpunt l en ml
            PointF M = Calc_Snijpunt(a_l, b_l, c_l, a_ml, b_ml, c_ml);
            PointF Mtek = tek.co_pt(M, gr.DpiX, gr.DpiY);

            // stap 4: bepalen van ligging EP
            //	- AP invullen in l
            bool teken_AP = a_l * AP.X + b_l * AP.Y + c_l > 0;
            //	- EP invullen in l
            bool teken_EP = a_l * EP.X + b_l * EP.Y + c_l > 0;
            bool meer_dan_180 = !(teken_AP ^ teken_EP);

            // stap 5: hoeken
            float start = (float)(Math.Atan2(RP.Y - M.Y, RP.X - M.X) * 180 / Math.PI);
            float end = (float)(Math.Atan2(EP.Y - M.Y, EP.X - M.X) * 180 / Math.PI);
            float sweep = end - start;
            if (meer_dan_180 & (-180 < sweep) & (sweep < 180))
                sweep += sweep < 0 ? 360 : -360;
            if (!meer_dan_180 & ((sweep < -180) | (180 < sweep)))
                sweep += sweep < 0 ? 360 : -360;

            double R_tek = Ellips.Pyt(M, RP) * tek.Schaal * gr.DpiX / 2.54f;
            try {
                gr.DrawArc(GetPen(false), (float)(Mtek.X - R_tek), (float)(Mtek.Y - R_tek), (float)(2 * R_tek), (float)(2 * R_tek), start, sweep);
            } catch (Exception) { }
        }

		public override int AddPunt(Punt p)
		{
			throw new NotImplementedException();
		}

		public override Region GetRegion(Tekening tek)
		{
			if ((richtpunt == null) | (eindpunt == null) | (richtlijn == null)) return new Region();
			Graphics gr = tek.CreateGraphics();

			// punten eenv. opslaan
			PointF RP = richtpunt.Coordinaat;
			PointF EP = eindpunt.Coordinaat;
			PointF AP = ((Punt)(richtlijn.Dep_Vormen.Except(new Punt[] { richtpunt }).First())).Coordinaat;

			// stap 1: loodlijn AP-RP in RP
			float a_l, b_l, c_l;
			Calc_Loodlijn(AP, RP, RP, out a_l, out b_l, out c_l);

			// stap 2: middelloodlijn RP-EP
			float a_ml, b_ml, c_ml;
			Calc_Loodlijn(RP, EP, Midden(RP, EP), out a_ml, out b_ml, out c_ml);

			// stap 3: snijpunt l en ml
			PointF M = Calc_Snijpunt(a_l, b_l, c_l, a_ml, b_ml, c_ml);
			PointF Mtek = tek.co_pt(M, gr.DpiX, gr.DpiY);

			// stap 4: bepalen van ligging EP
			//	- AP invullen in l
			bool teken_AP = a_l * AP.X + b_l * AP.Y + c_l > 0;
			//	- EP invullen in l
			bool teken_EP = a_l * EP.X + b_l * EP.Y + c_l > 0;
			bool meer_dan_180 = !(teken_AP ^ teken_EP);

			// stap 5: hoeken
			float start = (float)(Math.Atan2(RP.Y - M.Y, RP.X - M.X) * 180 / Math.PI);
			float end = (float)(Math.Atan2(EP.Y - M.Y, EP.X - M.X) * 180 / Math.PI);
			float sweep = end - start;
			if (meer_dan_180 & (-180 < sweep) & (sweep < 180))
				sweep += sweep < 0 ? 360 : -360;
			if (!meer_dan_180 & ((sweep < -180) | (180 < sweep)))
				sweep += sweep < 0 ? 360 : -360;

			double R_tek = Ellips.Pyt(M, RP) * tek.Schaal * gr.DpiX / 2.54f;

			GraphicsPath path = new GraphicsPath();
			path.AddArc((float)(Mtek.X - R_tek), (float)(Mtek.Y - R_tek), (float)(2 * R_tek), (float)(2 * R_tek), start, sweep);
			return new Region(path);
		}
	}
}
