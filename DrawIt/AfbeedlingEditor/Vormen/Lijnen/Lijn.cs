using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public abstract class Lijn : Vorm
	{
		Pen pen = new Pen(Color.Black);

		#region LijnKleur
		public Color LijnKleur
		{
			get { return pen.Color; }
			set { pen.Color = value; }
		}
		#endregion

		public float[] DashPattern
		{
			get
			{
				switch (pen.DashStyle)
				{
					case DashStyle.Solid:
						return new float[] { 1.0f };
					default:
						return pen.DashPattern;
				}
			}
			set
			{
				pen.DashPattern = value;
			}
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
		public Pen GetPen(bool print)
		{
			return new Pen(Color.FromArgb(Convert.ToInt32(2.55f * Zichtbaarheid), pen.Color));
		}
	}
}
