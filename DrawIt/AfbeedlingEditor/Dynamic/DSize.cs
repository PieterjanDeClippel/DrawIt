using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt.AfbeedlingEditor
{
	public class DSize
	{
		private Size val;
		public Size Value
		{
			get { return val; }
			set { val = value; }
		}

		public static implicit operator DSize(Size value)
		{
			return new DSize() { val = value };
		}
		public static explicit operator Size(DSize value)
		{
			return value.val;
		}
	}
}
