using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class PaintLayer : Layer
	{
		public Size Dimensie
		{
			get { return bmp.Size; }
			set
			{
				Bitmap bmp2 = new Bitmap(value.Width, value.Height);
				Graphics gr = Graphics.FromImage(bmp2);
				gr.DrawImage(bmp, 0, 0);
				bmp = bmp2;
			}
		}

		public override void Draw(Graphics gr)
		{
			gr.DrawImage(bmp, 0, 0);
		}

		#region Bitmap
		private Bitmap bmp = new Bitmap(16, 16);
		public Bitmap Bitmap
		{
			get { return bmp; }
		}
		#endregion
	}
}
