using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class Rechte : Lijn, IPointShape
	{
		#region Punt 1
		private DPoint punt1 = new DPoint();
		public DPoint Punt1
		{
			get { return punt1; }
			set { punt1 = value; }
		}
		#endregion
		#region Punt 2
		private DPoint punt2 = new DPoint();
		public DPoint Punt2
		{
			get { return punt2; }
			set { punt2 = value; }
		}
		#endregion

		public override void Draw(Graphics gr, bool widepen)
		{
			gr.DrawLine(GetPen(false), punt1.Value, punt2.Value);
			DrawGlyphs(gr);
		}

		public override Rectangle Bounds(Graphics gr)
		{
			return new Rectangle(
				Math.Min(punt1.Value.X, punt2.Value.X),
				Math.Min(punt1.Value.Y, punt2.Value.Y),
				Math.Abs(punt1.Value.X - punt2.Value.X),
				Math.Abs(punt1.Value.Y - punt2.Value.Y));
		}

		public void DrawGlyphs(Graphics gr)
		{
			Glyph.Draw(gr, punt1.Value);
			Glyph.Draw(gr, punt2.Value);
		}
	}
}
