using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public class Kromme : Lijn
	{
		public Kromme(Layer default_layer)
			: base(Vorm_type.Kromme, default_layer)
		{
		}

		#region Punten
		private Collectie<Punt> punten = new Collectie<Punt>();
		public Collectie<Punt> Punten
		{
			get { return punten; }
		}
		#endregion

		#region Tension
		private float tension = 0.5f;
		public float Tension
		{
			get { return tension; }
			set
			{
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Tension", tension, value));
				tension = value;
				base.OnVeranderd(e);
			}
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
			IEnumerable<Point> ptn = punten.Select(T => tek.co_pt(T.Coordinaat, gr.DpiX, gr.DpiY));

			Pen pen = (Pen)GetPen(false).Clone();
			if(Geselecteerd)
			{
				Pen selectie = new Pen(Color.Black, pen.Width + 2);
				pen.Width += 1;
				gr.DrawCurve(selectie, ptn.ToArray(), tension);
				gr.DrawCurve(pen, ptn.ToArray(), tension);
			}
			else
			{
				if(widepen)
				{
					pen.Width += 2;
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
				}
				if(ptn.Count() >= 2)
					gr.DrawCurve(pen, ptn.ToArray(), tension);
			}
		}

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			Pen pen = (Pen)GetPen(true).Clone();
			pen.Width = pen.Width / 2.54f;

			IEnumerable<PointF> ptn = punten.Select(T => new PointF((T.X - window.Left) * schaal * 10, (T.Y - window.Top) * schaal * 10));
			gr.DrawCurve(pen, ptn.ToArray());
		}

		public override string ToString()
		{
			return "kromme;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + ColorTranslator.ToOle(LijnKleur) + ";" + LijnDikte + ";" + (int)LijnStijl + ";" + (LijnStijl == System.Drawing.Drawing2D.DashStyle.Custom ? string.Join("/", DashPattern.Select(T => T.ToString()).ToArray()) : "") + ";" + string.Join(";", punten.Select(T => T.ID.ToString()).ToArray());
		}

		public override RectangleF Bounds(Graphics gr)
		{
			float l = punten.Min(T => T.X);
			float r = punten.Max(T => T.X);
			float t = punten.Min(T => T.Y);
			float b = punten.Max(T => T.Y);

			return new RectangleF(l, t, r - l, b - t);
		}

        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            IEnumerable<Point> ptn = punten.Select(T => T.Coordinaat).Combine(loc_co).Select(T => tek.co_pt(T, gr.DpiX, gr.DpiY));            
            if (ptn.Count() >= 2)
                gr.DrawCurve(GetPen(false), ptn.ToArray(), tension);
        }

		public override int AddPunt(Punt p)
		{
			punten.Add(p);
			return punten.Count;
		}

		public override Region GetRegion(Tekening tek)
		{
			Graphics gr = tek.CreateGraphics();
			IEnumerable<Point> ptn = punten.Select(T => T.Coordinaat).Select(T => tek.co_pt(T, gr.DpiX, gr.DpiY));
			GraphicsPath path = new GraphicsPath();
			if (ptn.Count() >= 2)
				path.AddCurve(ptn.ToArray(), tension);
			return new Region(path);
		}
	}
}
