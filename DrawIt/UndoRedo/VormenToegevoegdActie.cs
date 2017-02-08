using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrawIt.Tekenen;

namespace DrawIt
{
	public class VormenToegevoegdActie : Actie
	{
		public VormenToegevoegdActie(Vorm[] Vormen, Tekening tek, string Beschrijving)
			: base(Vormen)
		{
			this.tek = tek;
			this.Beschrijving = Beschrijving;
		}

		private Tekening tek;
		public override void Redo()
		{
			tek.Vormen.CanRaiseEvents = false;
			tek.Vormen.AddRange(Vormen);
			tek.Vormen.CanRaiseEvents = true;
		}

		public override void Undo()
		{
			tek.Vormen.CanRaiseEvents = false;
			foreach(Vorm v in Vormen)
				tek.Vormen.Remove(v);
			tek.Vormen.CanRaiseEvents = true;
		}
	}
}
