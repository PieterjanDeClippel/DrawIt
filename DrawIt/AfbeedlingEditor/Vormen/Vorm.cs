using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public abstract class Vorm
	{
		public abstract void Draw(Graphics gr, bool widepen);
		public abstract Rectangle Bounds(Graphics gr);
		public bool Geselecteerd { get; set; }
		public int Zichtbaarheid { get; set; } = 100;
	}
}
