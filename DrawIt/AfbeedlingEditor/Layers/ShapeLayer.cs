using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class ShapeLayer : Layer
	{
		private Collectie<Vorm> vormen = new Collectie<Vorm>();
		public Collectie<Vorm> Vormen
		{
			get { return vormen; }
		}

		public override void Draw(Graphics gr)
		{
			foreach (Vorm v in vormen)
			{
				v.Draw(gr, false);
			}
		}
	}
}
