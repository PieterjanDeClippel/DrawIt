using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class DPoint
	{
		private Point val;
		public DPoint()
		{
			val = new Point();
		}
		public DPoint(int x, int y)
		{
			val = new Point(x, y);
		}

		public Point Value
		{
			get { return val; }
			set { val = value; }
		}

		public static implicit operator DPoint(Point value)
		{
			return new DPoint() { val = value };
		}
		public static explicit operator Point(DPoint value)
		{
			return value.val;
		}
	}
}
