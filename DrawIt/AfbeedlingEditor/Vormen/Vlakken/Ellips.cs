using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class Ellips : Vlak, IRectangularShape
	{
		#region Locatie
		private DPoint locatie;
		public DPoint Locatie
		{
			get { return locatie; }
			set { locatie = value; }
		}
		#endregion
		#region Afmeting
		private DSize afmeting;
		public DSize Afmeting
		{
			get { return afmeting; }
			set { afmeting = value; }
		}
		#endregion
		#region Hoek
		private int hoek;
		public int Hoek
		{
			get { return hoek; }
			set { hoek = value; }
		}
		#endregion

		public override void Draw(Graphics gr, bool widepen)
		{
			Matrix oud = gr.Transform.Clone();
			gr.TranslateTransform(locatie.Value.X, locatie.Value.Y);
			gr.RotateTransform(hoek);
			gr.DrawEllipse(GetPen(false), new Rectangle(new Point(), afmeting.Value));

			if (Geselecteerd)
			{
				gr.DrawRectangle(new Pen(Color.Black) { DashStyle = DashStyle.Dot }, new Rectangle(new Point(), afmeting.Value));
				/*DrawGlyph(gr, new Point());
				DrawGlyph(gr, new Point(afmeting.Width / 2, 0));
				DrawGlyph(gr, new Point(afmeting.Width, 0));
				DrawGlyph(gr, new Point(afmeting.Width, afmeting.Height / 2));
				DrawGlyph(gr, new Point(afmeting.Width, afmeting.Height));
				DrawGlyph(gr, new Point(afmeting.Width / 2, afmeting.Height));
				DrawGlyph(gr, new Point(0, afmeting.Height));
				DrawGlyph(gr, new Point(0, afmeting.Height / 2));*/
			}
			gr.Transform = oud;
		}

		public override Rectangle Bounds(Graphics gr)
		{
			return new Rectangle(locatie.Value, afmeting.Value);
		}

		/*public override DPoint[] GetPunten()
		{
			throw new NotImplementedException();
		}*/
	}
}
