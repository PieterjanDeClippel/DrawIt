using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using DrawIt.Tekenen;

namespace DrawIt
{
	public class PropertyChangedActie : Actie
	{
		public PropertyChangedActie(Vorm Vorm, string PropertyName, object OldValue, object NewValue)
			: base(new Vorm[] { Vorm })
		{
			propertyName = PropertyName;
			oldValue = OldValue;
			newValue = NewValue;
			Beschrijving = string.Format("Change {0} from {1} to {2}", PropertyName, OldValue, NewValue);
		}

		private object oldValue;
		public object OldValue
		{
			get { return oldValue; }
		}

		private object newValue;
		public object NewValue
		{
			get { return newValue; }
		}

		private string propertyName;
		public string PropertyName
		{
			get { return propertyName; }
		}

		public override void Undo()
		{
			PropertyInfo info = Vormen.First().GetType().GetProperty(propertyName);
			foreach(Vorm v in Vormen)
			{
				v.CanRaiseVeranderdEvent = false;
				info.SetValue(v, oldValue, null);
				v.CanRaiseVeranderdEvent = true;
			}
		}
		public override void Redo()
		{
			PropertyInfo info = Vormen.First().GetType().GetProperty(propertyName);
			foreach(Vorm v in Vormen)
			{
				v.CanRaiseVeranderdEvent = false;
				info.SetValue(v, newValue, null);
				v.CanRaiseVeranderdEvent = true;
			}
		}
	}
}
