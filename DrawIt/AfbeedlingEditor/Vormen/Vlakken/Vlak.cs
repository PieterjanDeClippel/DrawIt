using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public abstract class Vlak : Vorm
	{
		#region Omtrek
		private Pen pen = new Pen(Color.Black);
		public Color LijnKleur
		{
			get { return pen.Color; }
			set { pen.Color = value; }
		}
		public DashStyle LijnStijl
		{
			get { return pen.DashStyle; }
			set { pen.DashStyle = value; }
		}
		public float LijnDikte
		{
			get { return pen.Width; }
			set { pen.Width = value; }
		}
		#endregion
		#region Vulkleur1
		private Color vulKleur1 = Color.White;
		public Color VulKleur1
		{
			get { return vulKleur1; }
			set { vulKleur1 = value; }
		}
		#endregion
		#region Vulkleur2
		private Color vulKleur2 = Color.White;
		public Color VulKleur2
		{
			get { return vulKleur2; }
			set { vulKleur2 = value; }
		}
		#endregion
		#region Soort opvulling
		public enum OpvulSoort
		{
			Solid,
			Hatch,
			LinearGradient
		}
		private OpvulSoort opvulsoort = OpvulSoort.Solid;
		public OpvulSoort OpvulType
		{
			get { return opvulsoort; }
			set { opvulsoort = value; }
		}
		#endregion
		#region VulStijl (HatchBrush)
		private HatchStyle vulstijl = HatchStyle.SolidDiamond;
		public HatchStyle VulStijl
		{
			get { return vulstijl; }
			set { vulstijl = value; }
		}
		#endregion
		#region Loophoek (LinearGradientBrush)
		private int loophoek = 45;
		public int LoopHoek
		{
			get { return loophoek; }
			set { loophoek = value; }
		}
		#endregion

		public Pen GetPen(bool print)
		{
			return new Pen(Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), pen.Color));
		}
		public Brush GetBrush(Graphics gr, float schaal, PointF offset, bool print)
		{
			Color k1, k2;
			if (Geselecteerd & !print)
			{
				k1 = Color.FromArgb(255, 255 - vulKleur1.R, 255 - vulKleur1.G, 255 - vulKleur1.B);
				k2 = Color.FromArgb(255, 255 - vulKleur2.R, 255 - vulKleur2.G, 255 - vulKleur2.B);
				switch (opvulsoort)
				{
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
					default:
						return new SolidBrush(k1);
				}
			}
			else
			{
				k1 = Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), vulKleur1);
				k2 = Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), vulKleur2);
				switch (opvulsoort)
				{
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
					default:
						return new SolidBrush(k1);
				}
			}
		}
	}
}
