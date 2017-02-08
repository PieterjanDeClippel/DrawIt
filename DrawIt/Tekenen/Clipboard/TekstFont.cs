using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt
{
	public class TekstFont
	{
		public TekstFont(Font font, Color kleur, ContentAlignment uitlijning, bool meeschalen)
		{
			this.font = font;
			this.kleur = kleur;
			this.uitlijning = uitlijning;
			this.meeschalen = meeschalen;
		}

		#region Font
		private Font font;
		public Font Font
		{
			get { return font; }
		}
		#endregion
		#region Kleur
		private Color kleur;
		public Color Kleur
		{
			get { return kleur; }
		}
		#endregion
		#region Uitlijning
		private ContentAlignment uitlijning;
		public ContentAlignment Uitlijning
		{
			get { return uitlijning; }
		}
		#endregion
		#region Meeschalen
		private bool meeschalen;
		public bool Meeschalen
		{
			get { return meeschalen; }
		}
		#endregion
		#region Default
		public static TekstFont Default
		{
			get { return new TekstFont(new Font(FontFamily.GenericMonospace, 14), Color.Black, ContentAlignment.TopLeft, false); }
		}
		#endregion
	}
}
