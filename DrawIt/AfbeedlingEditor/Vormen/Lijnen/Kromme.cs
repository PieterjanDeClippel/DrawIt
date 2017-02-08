using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class Kromme : Lijn, IPointShape
	{
		#region Punten
		private Collectie<DPoint> punten = new Collectie<DPoint>();
		public Collectie<DPoint> Punten
		{
			get { return punten; }
		}
		#endregion

		public override void Draw(Graphics gr, bool widepen)
		{
			gr.DrawCurve(GetPen(false), punten.Select(T => T.Value).ToArray());
		}

		public override Rectangle Bounds(Graphics gr)
		{
			int l = punten.Min(T => T.Value.X);
			int r = punten.Max(T => T.Value.X);
			int t = punten.Min(T => T.Value.Y);
			int b = punten.Max(T => T.Value.Y);

			return new Rectangle(l, t, r - l, b - t);
		}

		public void DrawGlyphs(Graphics gr)
		{
			foreach (DPoint p in punten)
			{
				Glyph.Draw(gr, p.Value);
			}
		}
	}
}
