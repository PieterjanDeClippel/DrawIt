using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawIt
{
	public partial class frmSave : Form
	{
		public frmSave()
		{
			InitializeComponent();
		}

		public void SetItems(IEnumerable<string> lijst)
		{
			listBox1.Items.Clear();
			listBox1.Items.AddRange(lijst.Select(T => Path.GetFileNameWithoutExtension(T)).ToArray());
		}
	}
}
