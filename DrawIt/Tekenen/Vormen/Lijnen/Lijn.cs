using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public abstract class Lijn : Vorm
	{
		public Lijn(Vorm_type type, Layer default_layer)
			: base(type, default_layer)
		{
		}

		Pen pen = new Pen(Color.Black);
		public Color LijnKleur
		{
			get { return pen.Color; }
			set
			{
				if(pen.Color == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "LijnKleur", pen.Color, value));
				pen.Color = value;
				OnVeranderd(e);
			}
		}
		public float[] DashPattern
		{
			get
			{
				switch(pen.DashStyle)
				{
					case DashStyle.Solid:
						return new float[] { 1.0f };
					default:
						return pen.DashPattern;
				}
			}
			set
			{
				if(pen.DashPattern == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "DashPattern", pen.DashPattern, value));
				pen.DashPattern = value;
				OnVeranderd(e);
			}
		}
		public DashStyle LijnStijl
		{
			get { return pen.DashStyle; }
			set
			{
				if(pen.DashStyle == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "LijnStijl", pen.DashStyle, value));
				pen.DashStyle = value;
				OnVeranderd(e);
			}
		}
		public float LijnDikte
		{
			get { return pen.Width; }
			set
			{
				if(pen.Width == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "LijnDikte", pen.Width, value));
				pen.Width = value;
				OnVeranderd(e);
			}
		}
		public DashCap DashCap
		{
			get { return pen.DashCap; }
			set
			{
				if (pen.DashCap == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "DashCap", pen.DashCap, value));
				pen.DashCap = value;
				pen.EndCap = (LineCap)value;
				pen.StartCap = (LineCap)value;
				OnVeranderd(e);
			}
		}
		public Pen GetPen(bool print)
		{
			Pen p = (Pen)pen.Clone();
			if(Geselecteerd & !print)
			{
				p.Color = Color.FromArgb(255,Color.FromArgb(int.MaxValue - pen.Color.ToArgb()));
			}
			else
			{
				p.Color = Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), pen.Color);
			}
			return p;
		}
		public override void CopyFont()
		{
			Program.LijnFont = (Pen)pen.Clone();
		}
		public override void PasteFont(out Actie result)
		{
			PropertyBundleChangedActie actie = new PropertyBundleChangedActie(new Vorm[] { this }, "Opmaak plakken");
			if (pen.Color != Program.LijnFont.Color)
				actie.Items.Add(new PropertyChangedActie(this, "LijnKleur", pen.Color, Program.LijnFont.Color));
			if (pen.DashStyle != Program.LijnFont.DashStyle)
				actie.Items.Add(new PropertyChangedActie(this, "LijnStijl", pen.DashStyle, Program.LijnFont.DashStyle));
			if (pen.Width != Program.LijnFont.Width)
				actie.Items.Add(new PropertyChangedActie(this, "LijnDikte", pen.Width, Program.LijnFont.Width));
			//if (pen.DashPattern != Program.LijnFont.DashPattern)
			//	actie.Items.Add(new PropertyChangedActie(this, "DashPattern", pen.DashPattern, Program.LijnFont.DashPattern));

			pen = (Pen)Program.LijnFont.Clone();
			result = actie;
		}
	}
}
