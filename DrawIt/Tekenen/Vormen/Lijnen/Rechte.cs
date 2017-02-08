using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace DrawIt.Tekenen
{
    public class Rechte : Lijn
	{
		public Rechte(int id, Layer default_layer)
			: base(Vorm_type.Rechte, default_layer)
		{
			this.id = id;
		}
		public Rechte(Punt Punt1, Punt Punt2, Layer default_layer)
			: base(Vorm_type.Rechte, default_layer)
		{
			punt1 = Punt1;
			punt2 = Punt2;
		}

		private int id;
		public int ID
		{
			get { return id; }
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

		public override Vorm[] Dep_Vormen
		{
			get
			{
				return new Vorm[] { punt1, punt2 };
			}
		}
        #endregion
        #region Accent
        private bool accent = false;
        public bool Accent
        {
            get { return accent; }
            set
            {
                accent = value;
                OnVeranderd(VormVeranderdEventArgs.Empty);
            }
        }
        #endregion

        public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			if((punt1 == null) | (punt2 == null)) return;
			Point p1 = tek.co_pt(punt1.Coordinaat, gr.DpiX, gr.DpiY);
			Point p2 = tek.co_pt(punt2.Coordinaat, gr.DpiX, gr.DpiY);
			Pen pen = (Pen)GetPen(false).Clone();
			if(Geselecteerd)
			{
				Pen selectie = new Pen(Color.Black, pen.Width + 2);
				pen.Width += 1;
				gr.DrawLine(selectie, p1, p2);
				gr.DrawLine(pen, p1, p2);
			}
			else
			{
				if(widepen)
				{
					pen.Width += 2;
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
				}
                else if(accent)
                {
                    pen.Width += 2;
                }
				gr.DrawLine(pen, p1, p2);
			}
		}
		public override RectangleF Bounds(Graphics gr)
        {
            return new RectangleF(
                Math.Min(punt1.Coordinaat.X, punt2.Coordinaat.X),
                Math.Min(punt1.Coordinaat.Y, punt2.Coordinaat.Y),
                Math.Abs(punt1.Coordinaat.X - punt2.Coordinaat.X),
                Math.Abs(punt1.Coordinaat.Y - punt2.Coordinaat.Y));
        }

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			if((punt1 == null) | (punt2 == null)) return;
			PointF p1 = new PointF(punt1.Coordinaat.X - window.Left,punt1.Coordinaat.Y - window.Top);
			p1 = new PointF(p1.X * schaal * 10, p1.Y * schaal * 10);

			PointF p2 = new PointF(punt2.Coordinaat.X - window.Left, punt2.Coordinaat.Y - window.Top);
			p2 = new PointF(p2.X * schaal * 10, p2.Y * schaal * 10);

			Pen p = (Pen)GetPen(true).Clone();
			p.Width /= 2.54f;

			gr.DrawLine(p, p1, p2);
		}

		public override string ToString()
		{
			return "rechte;" + id + ";" + Zichtbaarheid + ";" + Layer.Naam + ";" + Niveau + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + (LijnStijl == System.Drawing.Drawing2D.DashStyle.Custom ? string.Join("/", DashPattern.Select(T => T.ToString()).ToArray()) : "") + ";" + punt1.ID.ToString() + ";" + punt2.ID.ToString();
		}
		
		public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
			switch(tek.Actie)
			{
				case enActie.Nieuwe_rechte:
					if (punt1 == null) return;
					PointF p1 = tek.co_pt(punt1.Coordinaat, gr.DpiX, gr.DpiY);
					PointF p2 = tek.co_pt(loc_co, gr.DpiX, gr.DpiY);
					gr.DrawLine(GetPen(false), p1, p2);
					break;
				case enActie.Nieuwe_parallelle:
					Rechte r = (Rechte)ref_vormen.First();
					PointF pt1 = tek.co_pt(r.punt1.Coordinaat, gr.DpiX, gr.DpiY);
					PointF pt2 = tek.co_pt(r.punt2.Coordinaat, gr.DpiX, gr.DpiY);

					// Rechte berekenen
					float a, b, c;
					RaakBoog.Calc_Rechte(pt1, pt2, out a, out b, out c);

					// Loodlijn 1 berekenen
					float a_l1, b_l1, c_l1;
					RaakBoog.Calc_Loodlijn(pt1, pt2, pt1, out a_l1, out b_l1, out c_l1);

					// Loodlijn 2 berekenen
					float a_l2, b_l2, c_l2;
					RaakBoog.Calc_Loodlijn(pt1, pt2, pt2, out a_l2, out b_l2, out c_l2);

					// Evenwijdige berekenen
					float a_p, b_p, c_p;
					PointF temp = tek.PointToClient(Control.MousePosition);
					if (ref_vormen.Where(T => T.Vorm_Type == Vorm_type.Punt).Count() != 0)
						temp = tek.co_pt(ref_vormen.Where(T => T.Vorm_Type == Vorm_type.Punt).Select(T => (Punt)T).First().Coordinaat, gr.DpiX, gr.DpiY);
					RaakBoog.Calc_Loodlijn(a_l1, b_l1, c_l1, temp, out a_p, out b_p, out c_p);

					PointF s1 = RaakBoog.Calc_Snijpunt(a_l1, b_l1, c_l1, a_p, b_p, c_p);
					PointF s2 = RaakBoog.Calc_Snijpunt(a_l2, b_l2, c_l2, a_p, b_p, c_p);

					gr.DrawLine(GetPen(false), s1, s2);
					break;
			}
        }

		public override int AddPunt(Punt p)
		{
			if(punt1 == null)
			{
				punt1 = p;
				return 1;
			}
			else if(punt2 == null)
			{
				punt2 = p;
			}
			return 2;
		}

		public void set_id(int v)
		{
			id = v;
		}

		public override Region GetRegion(Tekening tek)
		{
			Graphics gr = tek.CreateGraphics();
			Point p1 = tek.co_pt(punt1.Coordinaat, gr.DpiX, gr.DpiY);
			Point p2 = tek.co_pt(punt2.Coordinaat, gr.DpiX, gr.DpiY);

			GraphicsPath path = new GraphicsPath();
			path.AddLine(p1, p2);
			return new Region(path);
		}
	}
}
