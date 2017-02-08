using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawIt
{
    public partial class InputBox : Form
    {
        private InputBox()
        {
            InitializeComponent();
        }

        public static string Toon(string Tekst, string Titel, string Standaard_Input)
        {
            InputBox b = new InputBox();
            b.Text = Titel;
            b.lblTekst.Text = Tekst;
            b.txtInput.Text = Standaard_Input;
            if (b.ShowDialog() == DialogResult.OK)
                return b.txtInput.Text;
            else
                return Standaard_Input;
        }
    }
}
