using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawIt.Tekenen
{
	public class Tekst : Vorm
	{
		public Tekst(Layer default_layer)
			: base(Vorm_type.Tekst, default_layer)
		{
		}

		public override Vorm[] Dep_Vormen
		{
			get
			{
				return new Vorm[] { punt };
			}
		}

		#region Punt
		private Punt punt;
		public Punt Punt
		{
			get { return punt; }
			set { punt = value; }
		}
		#endregion
		#region Tekst-uitlijning
		private ContentAlignment uitlijning = ContentAlignment.TopLeft;
		public ContentAlignment Uitlijning
		{
			get { return uitlijning; }
			set
			{
				if(uitlijning == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Uitlijning", uitlijning, value));
				uitlijning = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Tekst
		private string text = "text";
		public string Text
		{
			get { return text; }
			set
			{
				if(text == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Text", text, value));
				text = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Font
		private Font font = new Font(FontFamily.GenericMonospace, 14);
		public Font Font
		{
			get { return font; }
			set
			{
				if(font == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Font", font, value));
				font = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Kleur
		private Color kleur = Color.Black;
		public Color Kleur
		{
			get { return kleur; }
			set
			{
				if(kleur == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Kleur", kleur, value));
				kleur = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Meeschalen
		private bool meeschalen = false;
		public bool Meeschalen
		{
			get { return meeschalen; }
			set
			{
				if(meeschalen == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "Meeschalen", meeschalen, value));
				meeschalen = value;
				OnVeranderd(e);
			}
		}
		#endregion

		public override void Draw(Graphics gr, float schaal, RectangleF window)
		{
			// coordinaat op het blad bepalen
			PointF p = new PointF(punt.Coordinaat.X - window.Left, punt.Coordinaat.Y - window.Top);
			// verschalen
			PointF disp = new PointF(p.X * schaal * 10, p.Y * schaal * 10);
			// dpi omzetten
			float w = gr.MeasureString(text, font).Width, h = gr.MeasureString(text, font).Height;
			if(meeschalen)
			{
				w *= schaal;
				h *= schaal;
			}

			switch(uitlijning)
			{
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.MiddleRight:
					disp = new PointF(disp.X, disp.Y - h / 2);
					break;
				case ContentAlignment.BottomLeft:
				case ContentAlignment.BottomCenter:
				case ContentAlignment.BottomRight:
					disp = new PointF(disp.X, disp.Y - h);
					break;
			}
			switch(uitlijning)
			{
				case ContentAlignment.TopCenter:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.BottomCenter:
					disp = new PointF(disp.X -w / 2, disp.Y);
					break;
				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					disp = new PointF(disp.X - w, disp.Y);
					break;
			}

			Brush br = new SolidBrush(Color.FromArgb((int)(2.55f * Zichtbaarheid), kleur));
			if(meeschalen)
			{
				// muteer font en wijzig enkel size
				Font f2 = new Font(font.Name, font.Size * schaal, font.Style, font.Unit);
				gr.DrawString(text, f2, br, disp);
			}
			else
				gr.DrawString(text, font, br, disp);
		}

		public override void Draw(Tekening tek, Graphics gr, bool widepen, bool fill)
		{
			Point p = tek.co_pt(punt.Coordinaat, gr.DpiX, gr.DpiY);
			float w = gr.MeasureString(text, font).Width, h = gr.MeasureString(text, font).Height;
			if(meeschalen)
			{
				w *= tek.Schaal;
				h *= tek.Schaal;
			}

			switch(uitlijning)
			{
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.MiddleRight:
					p.Offset(0, (int)(-h / 2));
					break;
				case ContentAlignment.BottomLeft:
				case ContentAlignment.BottomCenter:
				case ContentAlignment.BottomRight:
					p.Offset(0, (int)(-h));
					break;
			}
			switch(uitlijning)
			{
				case ContentAlignment.TopCenter:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.BottomCenter:
					p.Offset((int)(-w / 2), 0);
					break;
				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					p.Offset((int)(-w),0);
					break;
			}
			if(widepen)
				gr.FillRectangle(Brushes.Black, p.X, p.Y, w, h);
			else
			{
				Brush br = new SolidBrush(Color.FromArgb((int)(2.55f * Zichtbaarheid), kleur));
				if(meeschalen)
				{
					// muteer font en wijzig enkel size
					Font f2 = new Font(font.Name, font.Size * tek.Schaal, font.Style, font.Unit);
					gr.DrawString(text, f2, br, p);
				}
				else
					gr.DrawString(text, font, br, p);
			}

			if(Geselecteerd)
				gr.DrawRectangle(new Pen(Color.Black) { DashStyle = System.Drawing.Drawing2D.DashStyle.Custom, DashPattern = new float[] { 6, 3 } }, p.X, p.Y, w, h);
		}

		public override string ToString()
		{
			FontConverter c = new FontConverter();
			string f = c.ConvertToString(font).Replace(';','|');
			return "tekst;" + Zichtbaarheid + ";" + Niveau + ";" + Layer.Naam + ";" + Enum.GetName(typeof(ContentAlignment), uitlijning) + ";" + text.Replace(";", "").Replace(Environment.NewLine,"\\r\\n") + ";" + f + ";" + ColorTranslator.ToOle(kleur) + ";" + punt.ID + ";" + (meeschalen ? "1" : "0");
		}

		public override RectangleF Bounds(Graphics gr)
		{
			PointF p = punt.Coordinaat;
			float w = gr.MeasureString(text, font).Width / gr.DpiX / 2.54f, h = gr.MeasureString(text, font).Height / gr.DpiY / 2.54f;
			

			switch(uitlijning)
			{
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.MiddleRight:
					p = new PointF(p.X, p.Y - h / 2);
					break;
				case ContentAlignment.BottomLeft:
				case ContentAlignment.BottomCenter:
				case ContentAlignment.BottomRight:
					p = new PointF(p.X, p.Y - h);
					break;
			}
			switch(uitlijning)
			{
				case ContentAlignment.TopCenter:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.BottomCenter:
					p = new PointF(p.X - w / 2, p.Y);
					break;
				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					p = new PointF(p.X - w, p.Y);
					break;
			}

			return new RectangleF(p, new SizeF(w, h));
		}
		
        public override void Draw(Tekening tek, Graphics gr, PointF loc_co, Vorm[] ref_vormen)
        {
            Point p = tek.co_pt(loc_co, gr.DpiX, gr.DpiY);
            float w = gr.MeasureString(text, font).Width, h = gr.MeasureString(text, font).Height;
            if (meeschalen)
            {
                w *= tek.Schaal;
                h *= tek.Schaal;
            }

            switch (uitlijning)
            {
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    p.Offset(0, (int)(-h / 2));
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    p.Offset(0, (int)(-h));
                    break;
            }
            switch (uitlijning)
            {
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    p.Offset((int)(-w / 2), 0);
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    p.Offset((int)(-w), 0);
                    break;
            }

                Brush br = new SolidBrush(Color.FromArgb((int)(2.55f * Zichtbaarheid), kleur));
                if (meeschalen)
                {
                    // muteer font en wijzig enkel size
                    Font f2 = new Font(font.Name, font.Size * tek.Schaal, font.Style, font.Unit);
                    gr.DrawString(text, f2, br, p);
                }
                else
                    gr.DrawString(text, font, br, p);
        }

		public override int AddPunt(Punt p)
		{
			if (punt == null)
				punt = p;
			return 1;
		}

		public override Region GetRegion(Tekening tek)
		{
			return new Region();
		}

		public override void CopyFont()
		{
			Program.TekstFont = new TekstFont(font, kleur, uitlijning, meeschalen);
		}
		public override void PasteFont(out Actie result)
		{
			PropertyBundleChangedActie actie = new PropertyBundleChangedActie(new Vorm[] { this }, "Opmaak plakken");
			if (uitlijning != Program.TekstFont.Uitlijning)
				actie.Items.Add(new PropertyChangedActie(this, "Uitlijning", uitlijning, Program.TekstFont.Uitlijning));
			if (font != Program.TekstFont.Font)
				actie.Items.Add(new PropertyChangedActie(this, "Font", font, Program.TekstFont.Font));
			if (kleur != Program.TekstFont.Kleur)
				actie.Items.Add(new PropertyChangedActie(this, "Kleur", kleur, Program.TekstFont.Kleur));
			if (meeschalen != Program.TekstFont.Meeschalen)
				actie.Items.Add(new PropertyChangedActie(this, "Meeschalen", meeschalen, Program.TekstFont.Meeschalen));

			uitlijning = Program.TekstFont.Uitlijning;
			font = Program.TekstFont.Font;
			kleur = Program.TekstFont.Kleur;
			meeschalen = Program.TekstFont.Meeschalen;

			result = actie;
		}
	}
}
