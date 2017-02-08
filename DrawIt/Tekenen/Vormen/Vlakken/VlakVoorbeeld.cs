using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawIt.Tekenen;

namespace DrawIt
{
	public class VlakVoorbeeld : Control
	{
		public VlakVoorbeeld()
		{
			this.Resize += VlakVoorbeeld_Resize;
			this.DoubleBuffered = true;
		}

		void VlakVoorbeeld_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics gr = e.Graphics;
			switch(opvultype)
			{
				case Vlak.OpvulSoort.Solid:
					gr.Clear(kleur1);
					break;
				case Vlak.OpvulSoort.Hatch:
					HatchBrush hbr = new HatchBrush(vulstijl, kleur1, kleur2);
					gr.FillRectangle(hbr, 0, 0, Width, Height);
					break;
				case Vlak.OpvulSoort.LinearGradient:
					LinearGradientBrush lbr = new LinearGradientBrush(this.ClientRectangle, kleur1, kleur2, loophoek);
					gr.FillRectangle(lbr, 0, 0, Width, Height);
					break;
				case Vlak.OpvulSoort.RadialGradient:
					GraphicsPath path = new GraphicsPath();
					path.AddRectangle(ClientRectangle);
					PathGradientBrush br = new PathGradientBrush(path);
					br.CenterPoint = new PointF(Width / 2, Height / 2);
					br.CenterColor = kleur1;
					br.SurroundColors = new Color[] { kleur2 };
					gr.FillRectangle(br, 0, 0, Width, Height);
					break;
			}
		}
		

		#region Kleur1
		private Color kleur1;
		public Color Kleur1
		{
			get { return kleur1; }
			set
			{
				kleur1 = value;
				this.Invalidate();
			}
		}
		#endregion
		#region Kleur2
		private Color kleur2;
		public Color Kleur2
		{
			get { return kleur2; }
			set
			{
				kleur2 = value;
				this.Invalidate();
			}
		}
		#endregion
		#region Soort opvulling
		private Vlak.OpvulSoort opvultype;
		public Vlak.OpvulSoort OpvulType
		{
			get { return opvultype; }
			set
			{
				opvultype = value;
				this.Invalidate();
			}
		}
		#endregion
		#region Vulstijl
		private HatchStyle vulstijl;
		public HatchStyle VulStijl
		{
			get { return vulstijl; }
			set
			{
				vulstijl = value;
				this.Invalidate();
			}
		}
		#endregion
		#region Loophoek
		private int loophoek;
		public int Loophoek
		{
			get { return loophoek; }
			set
			{
				loophoek = value;
				this.Invalidate();
			}
		}
		#endregion
	}
}
