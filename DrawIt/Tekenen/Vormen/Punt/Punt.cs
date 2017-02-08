using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DrawIt.Tekenen
{
    //[StructLayout(LayoutKind.Sequential)]
    public class Punt : Vorm
	{
		public Punt(int id, Layer default_layer)
			: base(Vorm_type.Punt, default_layer)
		{
			this.id = id;
		}
		public Punt(PointF co, Layer default_layer)
			: base(Vorm_type.Punt, default_layer)
		{
			coordinaat = co;
		}
		public Punt(float x, float y, Layer default_layer)
			: base(Vorm_type.Punt, default_layer)
		{
			coordinaat = new PointF(x, y);
		}

		private PointF coordinaat = new PointF();
		public PointF Coordinaat
		{
			get
			{
				if (ref_punt == null) return coordinaat;
				else return ref_punt.Coordinaat;
			}
			set
			{
				if(coordinaat == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Coordinaat", coordinaat, value));
				coordinaat = value;
				OnVeranderd(e);
			}
		}

		public float X
		{
			get
			{
				return Coordinaat.X;
			}
			set
			{
				if(coordinaat.X == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Coordinaat", coordinaat, value));
				coordinaat.X = value;
				OnVeranderd(e);
			}
		}
		public float Y
		{
			get { return Coordinaat.Y; }
			set
			{
				if(coordinaat.Y == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Coordinaat", coordinaat, value));
				coordinaat.Y = value;
				OnVeranderd(e);
			}
		}

		public enum enPuntStijl
		{
			Plus,
			X,
			Rond_open,
			Rond_vol,
			Vierkant_open,
			Vierkant_vol,
			Driehoek_open,
			Driehoek_vol,
			Ruit_open,
			Ruit_vol,
			Onzichtbaar
		}
		private enPuntStijl puntstijl = enPuntStijl.Plus;
		public enPuntStijl PuntStijl
		{
			get
			{
				if (ref_punt == null) return puntstijl;
				else return ref_punt.PuntStijl;
			}
			set
			{
				if(puntstijl == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "PuntStijl", puntstijl, value));
				puntstijl = value;
				OnVeranderd(e);
			}
		}

		private Color kleur = Color.Black;
		public Color Kleur
		{
			get
			{
				if (ref_punt == null) return kleur;
				else return ref_punt.Kleur;
			}
			set
			{
				if(kleur == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Kleur", kleur, value));
				kleur = value;
				OnVeranderd(e);
			}
		}

		private void DrawPunt(Graphics gr, PointF p, Pen pen, Brush br, bool print)
		{
			pen = new Pen(Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), pen.Color), pen.Width);
			float v = print ? 2.54f : 1;
			switch(cursor)
			{
				case SpecialCursor.Grab:
					gr.FillEllipse(Brushes.Black, new RectangleF(p.X - 2 / v, p.Y - 2 / v, 5 / v, 5 / v));
					gr.DrawEllipse(new Pen(Color.Black, 3), new RectangleF(p.X - 5 / v, p.Y - 5 / v, 11 / v, 11 / v));
					break;
				case SpecialCursor.Move:
					float k = 3.5f, b= 1,l=10;
					PointF[] punten = new PointF[]
					{
						new PointF(p.X,p.Y-l),
						new PointF(p.X + k, p.Y-l+k),
						new PointF(p.X+b,p.Y-l+k),
						new PointF(p.X+b,p.Y-b),
						new PointF(p.X+l-k,p.Y-b),
						new PointF(p.X+l-k,p.Y-k),
						new PointF(p.X+l,p.Y),
						new PointF(p.X+l-k,p.Y+k),
						new PointF(p.X+l-k,p.Y+b),
						new PointF(p.X + b,p.Y+b),
						new PointF(p.X+b,p.Y+l-k),
						new PointF(p.X +k,p.Y +l-k),
						new PointF(p.X,p.Y+l),
						new PointF(p.X-k,p.Y+l-k),
						new PointF(p.X-b,p.Y+l-k),
						new PointF(p.X-b,p.Y+b),
						new PointF(p.X-l+k,p.Y+b),
						new PointF(p.X-l+k,p.Y+k),
						new PointF(p.X-l,p.Y),
						new PointF(p.X-l+k,p.Y-k),
						new PointF(p.X-l+k,p.Y-b),
						new PointF(p.X-b,p.Y-b),
						new PointF(p.X-b,p.Y-l+k),
						new PointF(p.X-k,p.Y-l+k),
						new PointF(p.X,p.Y-l)
					};
					gr.FillPolygon(Brushes.Black, punten.Reverse().ToArray());
					Clipboard.Clear();
					//Clipboard.SetText(string.Join(Environment.NewLine, punten.Select(T => T.X + "," + T.Y).ToArray()));
					break;
				case SpecialCursor.None:
					switch(PuntStijl)
					{
						case enPuntStijl.Plus:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 3 / v, p.Y - 3 / v, 6 / v, 6 / v);
							gr.DrawLine(pen, p.X - 3 / v, p.Y, p.X + 3 / v, p.Y);
							gr.DrawLine(pen, p.X, p.Y - 3 / v, p.X, p.Y + 3 / v);
							break;
						case enPuntStijl.X:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 3 / v, p.Y - 3 / v, 7 / v, 7 / v);
							gr.DrawLine(pen, p.X - 3 / v, p.Y - 3 / v, p.X + 3 / v, p.Y + 3 / v);
							gr.DrawLine(pen, p.X - 3 / v, p.Y + 3 / v, p.X + 3 / v, p.Y - 3 / v);
							break;
						case enPuntStijl.Driehoek_open:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 4 / v, p.Y - 4 / v, 9 / v, 8 / v);
							gr.DrawPolygon(pen, new PointF[] {
									new PointF(p.X - 4 / v, p.Y + 3 / v),
									new PointF(p.X + 4 / v, p.Y + 3 / v),
									new PointF(p.X, p.Y - 4 / v)
								});
							break;
						case enPuntStijl.Driehoek_vol:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 4 / v, p.Y - 4 / v, 9 / v, 8 / v);
							gr.FillPolygon(br, new PointF[] {
									new PointF(p.X - 5 / v, p.Y + 4 / v),
									new PointF(p.X + 5 / v, p.Y + 4 / v),
									new PointF(p.X, p.Y - 5 / v)
								});
							break;
						case enPuntStijl.Onzichtbaar:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 1 / v, p.Y - 1 / v, 3 / v, 3 / v);
							gr.FillEllipse(br, new RectangleF(p.X, p.Y, 1 / v, 2 / v));
							break;
						case enPuntStijl.Rond_open:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 3 / v, p.Y - 3 / v, 7 / v, 7 / v);
							gr.DrawEllipse(pen, p.X - 3 / v, p.Y - 3 / v, 7 / v, 7 / v);
							break;
						case enPuntStijl.Rond_vol:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 4 / v, p.Y - 4 / v, 9 / v, 9 / v);
							gr.FillEllipse(br, p.X - 4 / v, p.Y - 4 / v, 9 / v, 9 / v);
							break;
						case enPuntStijl.Vierkant_open:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 3 / v, p.Y - 3 / v, 7 / v, 7 / v);
							gr.DrawRectangle(pen, p.X - 3 / v, p.Y - 3 / v, 7 / v, 7 / v);
							break;
						case enPuntStijl.Vierkant_vol:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 3 / v, p.Y - 3 / v, 7 / v, 7 / v);
							gr.FillRectangle(br, p.X - 4 / v, p.Y - 4 / v, 9 / v, 9 / v);
							break;
						case enPuntStijl.Ruit_open:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 3 / v, p.Y - 3 / v, 7 / v, 7 / v);
							gr.DrawPolygon(pen, new PointF[] { new PointF(p.X - 3 / v, p.Y), new PointF(p.X, p.Y - 3 / v), new PointF(p.X + 3 / v, p.Y), new PointF(p.X, p.Y + 3 / v) });
							break;
						case enPuntStijl.Ruit_vol:
							if(Geselecteerd & !print) gr.FillRectangle(Brushes.Black, p.X - 4 / v, p.Y - 4 / v, 9 / v, 9 / v);
							gr.FillPolygon(br, new PointF[] { new PointF(p.X - 4 / v, p.Y), new PointF(p.X, p.Y - 4 / v), new PointF(p.X + 4 / v, p.Y), new PointF(p.X, p.Y + 4 / v) });
							break;
					}
					break;
			}
		}
		public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			if (ref_punt != null) return;
			Point p = tek.co_pt(Coordinaat, gr.DpiX, gr.DpiY);
			Pen pen = Geselecteerd ? new Pen(Color.FromArgb(int.MaxValue - kleur.ToArgb()), 2) : new Pen(Kleur);
			Brush br = Geselecteerd ? new SolidBrush(Color.FromArgb(int.MaxValue - kleur.ToArgb())) : new SolidBrush(Kleur);
			DrawPunt(gr, p, pen, br, false);
		}
		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			if (ref_punt != null) return;
			//coordinaat op het blad bepalen
			PointF center = new PointF(coordinaat.X - window.Left, coordinaat.Y - window.Top);
			//verschalen
			PointF disp = new PointF(center.X * schaal*10, center.Y * schaal*10);
			//dpi omzetten
			Pen pen = new Pen(Kleur);
			pen.Width = 1 / 25.4f;
			Brush br = new SolidBrush(Kleur);
			DrawPunt(gr, disp, pen, br, true);
		}

		public override string ToString()
		{
			return "punt;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + id + ";" + X.ToGBString() + ";" + Y.ToGBString() + ";" + (int)PuntStijl + ";" + ColorTranslator.ToOle(Kleur);
		}

		public override RectangleF Bounds(Graphics gr)
		{
			return new RectangleF(Coordinaat, new SizeF(0, 0));
		}
		
        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            DrawPunt(gr, tek.co_pt(loc_co, gr.DpiX, gr.DpiY), new Pen(kleur), new SolidBrush(kleur), false);
        }

        public enum SpecialCursor
		{
			None,
			Grab,
			Move
		}

		private SpecialCursor cursor = SpecialCursor.None;
		public SpecialCursor Cursor
		{
			get { return cursor; }
			set { cursor = value; }
		}

		public override Vorm[] Dep_Vormen
		{
			get
			{
				return new Vorm[] { };
			}
		}

		private int id;
		public int ID
		{
			get
			{
				if (ref_punt != null) return ref_punt.ID;
				else return id;
			}
		}
		public void set_id(int id)
		{
			this.id = id;
		}

		public override int AddPunt(Punt p)
		{
			throw new NotImplementedException();
		}

		public override Region GetRegion(Tekening tek)
		{
			return new Region();
		}

		private Punt ref_punt;
		public Punt Ref_Punt
		{
			get { return ref_punt; }
			set { ref_punt = value; }
		}

		public override void CopyFont()
		{
			Program.PuntFont = new PuntFont(puntstijl, kleur);
		}
		public override void PasteFont(out Actie result)
		{
			PropertyBundleChangedActie actie = new PropertyBundleChangedActie(new Vorm[] { this }, "Opmaak plakken");
			if (puntstijl != Program.PuntFont.PuntStijl)
				actie.Items.Add(new PropertyChangedActie(this, "PuntStijl", puntstijl, Program.PuntFont.PuntStijl));
			if (kleur != Program.PuntFont.Kleur)
				actie.Items.Add(new PropertyChangedActie(this, "Kleur", kleur, Program.PuntFont.Kleur));

			puntstijl = Program.PuntFont.PuntStijl;
			kleur = Program.PuntFont.Kleur;
			result = actie;
		}
	}
}
