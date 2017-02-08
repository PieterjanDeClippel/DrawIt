using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrawIt.Tekenen;

namespace DrawIt
{
	public abstract class Actie
	{
		public Actie(Vorm[] Vormen)
		{
			vormen = Vormen;
		}

		private Vorm[] vormen = new Vorm[] { };
		public Vorm[] Vormen
		{
			get { return vormen; }
		}

		public abstract void Undo();
		public abstract void Redo();
		public string Beschrijving { get; protected set; }
	}
}
