using DrawIt.AfbeedlingEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt
{
	public class Glyph
	{
		public Glyph(DPoint Punt)
		{
			punt = Punt;
		}

		#region Punt
		private DPoint punt;
		public DPoint Punt
		{
			get { return punt; }
		}
		#endregion
		
		public static void Draw(Graphics gr, Point p)
		{
			Rectangle r = new Rectangle(p, new Size());
			r.Inflate(4, 4);
			gr.FillEllipse(new SolidBrush(Color.Green), r);
			gr.DrawEllipse(Pens.Black, r);
		}
	}
}
