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
	public partial class frmPuntCoordinaat : Form
	{
		public frmPuntCoordinaat()
		{
			InitializeComponent();
		}

		private void frmPuntCoordinaat_Load(object sender, EventArgs e)
		{
			txtX.Focus();
			txtX.SelectAll();
		}
	}
}
