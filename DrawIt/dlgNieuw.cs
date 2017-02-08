using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawIt
{
	public partial class dlgNieuw : Form
	{
		public dlgNieuw()
		{
			InitializeComponent();
		}

		public enNieuwDocType Resultaat
		{
			get
			{
				if (rbnPictogram.Checked) return enNieuwDocType.Pictogram;
				else if (rbnCursor.Checked) return enNieuwDocType.Cursor;
				else if (rbnAfbeelding.Checked) return enNieuwDocType.Afbeelding;
				else return enNieuwDocType.Tekening;
			}
			set
			{
				switch (value)
				{
					case enNieuwDocType.Pictogram:
						rbnPictogram.Checked = true;
						break;
					case enNieuwDocType.Cursor:
						rbnCursor.Checked = true;
						break;
					case enNieuwDocType.Afbeelding:
						rbnAfbeelding.Checked = true;
						break;
					default:
						rbnTekening.Checked = true;
						break;
				}
			}
		}
		public enum enNieuwDocType
		{
			Tekening,
			Afbeelding,
			Pictogram,
			Cursor
		}
	}
}
