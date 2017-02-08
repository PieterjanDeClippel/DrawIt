using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawIt.Tekenen;
using System.Drawing.Drawing2D;

namespace DrawIt
{
	// only used for internal clipboard
	public class VlakFont
	{
		public VlakFont(Pen pen, Vlak.OpvulSoort opvulsoort, Color kleur1, Color kleur2, HatchStyle vulstijl, int loophoek)
		{
			this.pen = pen;
			this.opvulsoort = opvulsoort;
			this.kleur1 = kleur1;
			this.kleur2 = kleur2;
			this.vulstijl = vulstijl;
			this.loophoek = loophoek;
		}

		#region Pen
		private Pen pen;
		public Pen Pen
		{
			get { return pen; }
		}
		#endregion
		#region OpvulSoort
		private Vlak.OpvulSoort opvulsoort;
		public Vlak.OpvulSoort OpvulSoort
		{
			get { return opvulsoort; }
		}
		#endregion
		#region Kleur1
		private Color kleur1;
		public Color Kleur1
		{
			get { return kleur1; }
		}
		#endregion
		#region Kleur2
		private Color kleur2;
		public Color Kleur2
		{
			get { return kleur2; }
		}
		#endregion
		#region OpvulStijl
		private HatchStyle vulstijl;
		public HatchStyle VulStijl
		{
			get { return vulstijl; }
		}
		#endregion
		#region Loophoek
		private int loophoek;
		public int LoopHoek
		{
			get { return loophoek; }
		}
		#endregion
		#region Default
		public static VlakFont Default
		{
			get { return new VlakFont(Pens.Black, Vlak.OpvulSoort.Solid, Color.White, Color.White, HatchStyle.SolidDiamond, 45); }
		}
		#endregion
	}
}
