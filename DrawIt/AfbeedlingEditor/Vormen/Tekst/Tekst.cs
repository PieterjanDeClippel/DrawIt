using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class Tekst : Vorm
	{
		#region Tekst
		private string text = "text";
		public string Text
		{
			get { return text; }
			set { text = value; }
		}
		#endregion
		#region Font
		private Font font = new Font(FontFamily.GenericMonospace, 14);
		public Font Font
		{
			get { return font; }
			set { font = value; }
		}
		#endregion
		#region Kleur
		private Color kleur = Color.Black;
		public Color Kleur
		{
			get { return kleur; }
			set { kleur = value; }
		}
		#endregion
		#region Locatie
		private DPoint locatie;
		public DPoint Locatie
		{
			get { return locatie; }
			set { locatie = value; }
		}
		#endregion

		public override void Draw(Graphics gr, bool widepen)
		{
			gr.DrawString(text, font, new SolidBrush(kleur), locatie.Value);
		}

		public override Rectangle Bounds(Graphics gr)
		{
			return new Rectangle();
		}
	}
}
