using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawIt.Tekenen
{
	public class Layer
	{
		public Layer(bool IsDefault)
		{
			isdefault = IsDefault;
		}

		private string naam;
		public string Naam
		{
			get { return naam; }
			set { naam = value; }
		}

		private bool zichtbaar = true;
		public bool Zichtbaar
		{
			get { return zichtbaar; }
			set { zichtbaar = value; }
		}

		private bool isdefault = false;
		public bool IsDefault
		{
			get { return isdefault; }
		}

		public static bool operator ==(Layer l1, Layer l2)
		{
			return ReferenceEquals(l1, l2);
		}
		public static bool operator != (Layer l1,Layer l2)
		{
			return !ReferenceEquals(l1, l2);
		}
		
		public override string ToString()
		{
			return naam + ";" + (zichtbaar ? "1" : "0") + ";" + (isdefault ? "1" : "0");
		}
		public static Layer FromString(string s)
		{
			string[] parts = s.Split(';');
			Layer res = new Layer(parts[2] == "1");
			res.naam = parts[0];
			res.zichtbaar = parts[1] == "1";
			return res;
		}

		private object tag;
		public object Tag
		{
			get { return tag; }
			set { tag = value; }
		}
	}
}
