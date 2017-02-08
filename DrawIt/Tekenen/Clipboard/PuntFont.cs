using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt
{
	public class PuntFont
	{
		public PuntFont(Tekenen.Punt.enPuntStijl puntstijl, Color kleur)
		{
			this.puntstijl = puntstijl;
			this.kleur = kleur;
		}

		#region PuntStijl
		private Tekenen.Punt.enPuntStijl puntstijl;
		public Tekenen.Punt.enPuntStijl PuntStijl
		{
			get { return puntstijl; }
		}
		#endregion
		#region Kleur
		private Color kleur;
		public Color Kleur
		{
			get { return kleur; }
		}
		#endregion
		#region Default
		public static PuntFont Default
		{
			get { return new PuntFont(Tekenen.Punt.enPuntStijl.Plus, Color.Black); }
		}
		#endregion
	}
}
