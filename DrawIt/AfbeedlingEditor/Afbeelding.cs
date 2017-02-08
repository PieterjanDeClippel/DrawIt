using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class Afbeelding
	{
		private Collectie<Layer> layers = new Collectie<Layer>();
		public Collectie<Layer> Layers
		{
			get { return layers; }
		}

		private Size dimensie = new Size(16, 16);
		public Size Dimensie
		{
			get { return dimensie; }
			set
			{
				dimensie = value;
				foreach (PaintLayer l in layers.Where(T => T.GetType() == typeof(PaintLayer)).Select(T => (PaintLayer)T))
					l.Dimensie = value;
			}
		}

		public Bitmap GetBitmap()
		{
			Bitmap bmp = new Bitmap(dimensie.Width, dimensie.Height);
			Graphics gr = Graphics.FromImage(bmp);
			gr.Clear(Color.White);
			foreach(Layer l in layers)
			{
				l.Draw(gr);
			}
			return bmp;
		}

		public void Save(Stream stream)
		{

		}
	}
}
