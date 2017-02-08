using DrawIt.Tekenen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt
{
	public class PropertyBundleChangedActie : Actie
	{
		public PropertyBundleChangedActie(Vorm[] vormen, string Beschrijving)
			: base(vormen)
		{
			base.Beschrijving = Beschrijving;
		}

		private List<Actie> items = new List<Actie>();
		public List<Actie> Items
		{
			get { return items; }
		}
		
		public override void Redo()
		{
			foreach (Actie actie in items)
				actie.Redo();
		}

		public override void Undo()
		{
			foreach (Actie actie in items.Reverse<Actie>())
				actie.Undo();
		}
	}
}
