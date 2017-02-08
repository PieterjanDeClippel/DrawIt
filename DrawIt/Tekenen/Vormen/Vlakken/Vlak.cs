using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public abstract class Vlak : Vorm
	{
		public Vlak(Vorm_type type, Layer default_layer)
			: base(type, default_layer)
		{
		}

		#region Omtrek
		private Pen pen = new Pen(Color.Black);
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
		#endregion
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
		#region Vulkleur1
		private Color vulKleur1 = Color.White;
		public Color VulKleur1
		{
			get { return vulKleur1; }
			set
			{
				if(vulKleur1 == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "VulKleur1", vulKleur1, value));
				vulKleur1 = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Vulkleur2
		private Color vulKleur2 = Color.White;
		public Color VulKleur2
		{
			get { return vulKleur2; }
			set
			{
				if(vulKleur2 == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "VulKleur2", vulKleur2, value));
				vulKleur2 = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Soort opvulling
		public enum OpvulSoort
		{
			Solid,
			Hatch,
			LinearGradient,
			RadialGradient,
			Geen
		}
		private OpvulSoort opvulsoort = OpvulSoort.Solid;
		public OpvulSoort OpvulType
		{
			get { return opvulsoort; }
			set
			{
				if(opvulsoort == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "OpvulType", opvulsoort, value));
				opvulsoort = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region VulStijl (HatchBrush)
		private HatchStyle vulstijl = HatchStyle.SolidDiamond;
		public HatchStyle VulStijl
		{
			get { return vulstijl; }
			set
			{
				if(vulstijl == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "VulStijl", vulstijl, value));
				vulstijl = value;
				OnVeranderd(e);
			}
		}
		#endregion
		#region Loophoek (LinearGradientBrush)
		private int loophoek = 45;
		public int LoopHoek
		{
			get { return loophoek; }
			set
			{
				if(loophoek == value) return;
				VormVeranderdEventArgs e = new VormVeranderdEventArgs(new PropertyChangedActie(this, "LoopHoek", loophoek, value));
				loophoek = value;
				OnVeranderd(e);
			}
		}
		#endregion
		
		public Pen GetPen(bool print)
		{
			Pen p = (Pen)pen.Clone();
			if(Geselecteerd & !print)
			{
				p.Color = Color.FromArgb(int.MaxValue - pen.Color.ToArgb());
			}
			else
			{
				p.Color = Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), pen.Color);
			}
			return p;
		}
		public Brush GetBrush(Graphics gr, float schaal, PointF offset, bool print)
		{
			Color k1, k2;
			if(Geselecteerd & !print)
			{
				k1 = Color.FromArgb(255, 255 - vulKleur1.R, 255 - vulKleur1.G, 255 - vulKleur1.B);
				k2 = Color.FromArgb(255, 255 - vulKleur2.R, 255 - vulKleur2.G, 255 - vulKleur2.B);
				switch(opvulsoort)
				{
					case OpvulSoort.Geen:
						return Brushes.Transparent;
					case OpvulSoort.Solid:
						return new SolidBrush(k1);
					case OpvulSoort.Hatch:
						return new HatchBrush(vulstijl, k1, k2);
					case OpvulSoort.LinearGradient:
						RectangleF b = Bounds(gr);
						b = new RectangleF(
							b.X * (gr.DpiX / 2.54f) * schaal + offset.X,
							b.Y * (gr.DpiY / 2.54f) * schaal + offset.Y,
							b.Width * (gr.DpiX / 2.54f) * schaal,
							b.Height * (gr.DpiY / 2.54f) * schaal);
                        return new LinearGradientBrush(b, k1, k2, loophoek);
					case OpvulSoort.RadialGradient:
						RectangleF rct = Bounds(gr);
						rct = new RectangleF(
							rct.X * (gr.DpiX / 2.54f) * schaal + offset.X,
							rct.Y * (gr.DpiY / 2.54f) * schaal + offset.Y,
							rct.Width * (gr.DpiX / 2.54f) * schaal,
							rct.Height * (gr.DpiY / 2.54f) * schaal);
						GraphicsPath path = new GraphicsPath();
						switch (Vorm_Type)
						{
							case Vorm_type.Cirkel:
								path.AddEllipse(rct);
								break;
							case Vorm_type.Ellips:
								path.AddEllipse(rct);
								break;
							case Vorm_type.Veelhoek:
								Veelhoek v = (Veelhoek)this;
								path.AddPolygon(v.Punten.Select(T => new PointF(T.Coordinaat.X * gr.DpiX / 2.54f * schaal + offset.X, T.Coordinaat.Y * gr.DpiY / 2.54f * schaal + offset.Y)).ToArray());
								break;
							case Vorm_type.GeslotenKromme:
								GeslotenKromme k = (GeslotenKromme)this;
								path.AddClosedCurve(k.Punten.Select(T => new PointF(T.Coordinaat.X * gr.DpiX / 2.54f * schaal + offset.X, T.Coordinaat.Y * gr.DpiY / 2.54f * schaal + offset.Y)).ToArray());
								break;
							case Vorm_type.CirkelSector:
								path.AddEllipse(rct);
								break;
							case Vorm_type.CirkelSegment:
								path.AddEllipse(rct);
								break;
							default:
								break;
						}
						PathGradientBrush br = new PathGradientBrush(path);
						br.CenterPoint = new PointF((rct.Left + rct.Right) / 2, (rct.Top + rct.Bottom) / 2);
						br.CenterColor = k1;
						br.SurroundColors = new Color[] { k2 };
						return br;
					default:
						return new SolidBrush(k1);
				}
			}
			else
			{
				k1 = Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), vulKleur1);
				k2 = Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), vulKleur2);
				switch(opvulsoort)
				{
					case OpvulSoort.Geen:
						return Brushes.Transparent;
					case OpvulSoort.Solid:
						return new SolidBrush(k1);
					case OpvulSoort.Hatch:
						return new HatchBrush(vulstijl, k1, k2);
					case OpvulSoort.LinearGradient:
						RectangleF b = Bounds(gr);
						b = new RectangleF(
							b.X * (gr.DpiX / 2.54f) * schaal + offset.X,
							b.Y * (gr.DpiY / 2.54f) * schaal + offset.Y,
							b.Width * (gr.DpiX / 2.54f) * schaal,
							b.Height * (gr.DpiY / 2.54f) * schaal);
						// bounds function only needs graphics for Tekst-type
						return new LinearGradientBrush(b, k1, k2, loophoek);
					case OpvulSoort.RadialGradient:
						RectangleF rct = Bounds(gr);
						rct = new RectangleF(
							rct.X * (gr.DpiX / 2.54f) * schaal + offset.X,
							rct.Y * (gr.DpiY / 2.54f) * schaal + offset.Y,
							rct.Width * (gr.DpiX / 2.54f) * schaal,
							rct.Height * (gr.DpiY / 2.54f) * schaal);
						GraphicsPath path = new GraphicsPath();
						switch (Vorm_Type)
						{
							case Vorm_type.Cirkel:
								path.AddEllipse(rct);
								break;
							case Vorm_type.Ellips:
								path.AddEllipse(rct);
								break;
							case Vorm_type.Veelhoek:
								Veelhoek v = (Veelhoek)this;
								path.AddPolygon(v.Punten.Select(T => new PointF(T.Coordinaat.X * gr.DpiX / 2.54f * schaal + offset.X, T.Coordinaat.Y * gr.DpiY / 2.54f * schaal + offset.Y)).ToArray());
								break;
							case Vorm_type.GeslotenKromme:
								GeslotenKromme k = (GeslotenKromme)this;
								path.AddClosedCurve(k.Punten.Select(T => new PointF(T.Coordinaat.X * gr.DpiX / 2.54f * schaal + offset.X, T.Coordinaat.Y * gr.DpiY / 2.54f * schaal + offset.Y)).ToArray());
								break;
							case Vorm_type.CirkelSector:
								path.AddEllipse(rct);
								break;
							case Vorm_type.CirkelSegment:
								path.AddEllipse(rct);
								break;
							default:
								break;
						}
						PathGradientBrush br = new PathGradientBrush(path);
						br.CenterPoint = new PointF((rct.Left + rct.Right) / 2, (rct.Top + rct.Bottom) / 2);
						br.CenterColor = k1;
						br.SurroundColors = new Color[] { k2 };
						return br;
					default:
						return new SolidBrush(k1);
				}
			}
		}

		public override void CopyFont()
		{
			Program.VlakFont = new VlakFont((Pen)pen.Clone(), opvulsoort, vulKleur1, vulKleur2, vulstijl, loophoek);
		}
		public override void PasteFont(out Actie result)
		{
			PropertyBundleChangedActie actie = new PropertyBundleChangedActie(new Vorm[] { this }, "Opmaak plakken");
			if (pen.Color != Program.VlakFont.Pen.Color)
				actie.Items.Add(new PropertyChangedActie(this, "LijnKleur", pen.Color, Program.VlakFont.Pen.Color));
			if (pen.DashStyle != Program.VlakFont.Pen.DashStyle)
				actie.Items.Add(new PropertyChangedActie(this, "LijnStijl", pen.DashStyle, Program.VlakFont.Pen.DashStyle));
			if (pen.Width != Program.VlakFont.Pen.Width)
				actie.Items.Add(new PropertyChangedActie(this, "LijnDikte", pen.Width, Program.VlakFont.Pen.Width));
			if (vulKleur1 != Program.VlakFont.Kleur1)
				actie.Items.Add(new PropertyChangedActie(this, "VulKleur1", vulKleur1, Program.VlakFont.Kleur1));
			if (vulKleur2 != Program.VlakFont.Kleur2)
				actie.Items.Add(new PropertyChangedActie(this, "VulKleur2", vulKleur2, Program.VlakFont.Kleur2));
			if (opvulsoort != Program.VlakFont.OpvulSoort)
				actie.Items.Add(new PropertyChangedActie(this, "OpvulType", opvulsoort, Program.VlakFont.OpvulSoort));
			if (vulstijl != Program.VlakFont.VulStijl)
				actie.Items.Add(new PropertyChangedActie(this, "VulStijl", vulstijl, Program.VlakFont.VulStijl));
			if (loophoek != Program.VlakFont.LoopHoek)
				actie.Items.Add(new PropertyChangedActie(this, "LoopHoek", loophoek, Program.VlakFont.LoopHoek));

			pen = (Pen)Program.VlakFont.Pen.Clone();
			opvulsoort = Program.VlakFont.OpvulSoort;
			vulKleur1 = Program.VlakFont.Kleur1;
			vulKleur2 = Program.VlakFont.Kleur2;
			vulstijl = Program.VlakFont.VulStijl;
			loophoek = Program.VlakFont.LoopHoek;

			result = actie;
		}
	}
}
