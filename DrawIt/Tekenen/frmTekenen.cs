using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawIt.Tekenen;

namespace DrawIt
{
	public partial class frmTekenen : Form, IDocument
	{
		#region Constructor
		public frmTekenen()
		{
			InitializeComponent();
			tekening1.Focus();

			vorm_buttons = new ToolStripButton[] { tsbNieuwPunt, tsbNieuweRechte, tsbNieuweKromme, tsbNieuweCirkelboog, tsbNieuweCirkel3, tsbNieuweCirkel2, tsbCirkelSector, tsbCirkelSegment, tsbNieuweEllips, tsbGeslotenKromme, tsbVeelhoek, tsbTekst, tsbRaakBoog, tsbNieuweMaatlijn, tsbEvenwijdige };
			toolStripComboBox1.SelectedIndex = 0;

			Properties.Settings sett = new Properties.Settings();
			tsbVraagCoordinaat.Checked = tekening1.VraagCoordinaat = sett.coordinateprompt;
			tekening1.ReportLayers += Tekening1_ReportLayers;
			tekening1.BroadcastUndoRedoStack += Tekening1_BroadcastUndoRedoStack;
			
		}

		public event Tekening.BroadcastUndoRedoStackHandler BroadcastUndoRedoStack;
		private void Tekening1_BroadcastUndoRedoStack(object sender, UndoRedoStackEventArgs e)
		{
			if (BroadcastUndoRedoStack != null)
				BroadcastUndoRedoStack(this, e);
		}
		public void RequestUndoRedoStack()
		{
			tekening1.RequestUndoRedoStack();
		}

		private void Tekening1_ReportLayers(object sender, LayersReportedEventArgs e)
		{
			toolStripComboBox1.Items.Clear();
			toolStripComboBox1.Items.AddRange(e.Layers);
			toolStripComboBox1.SelectedIndex = e.ActiveLayerIndex;
		}
		#endregion
		#region New Object Buttons
		ToolStripButton[] vorm_buttons;
		private void tsbNieuwPunt_Click(object sender, EventArgs e)
		{
			tsbNieuwPunt.Checked = !tsbNieuwPunt.Checked;
			foreach(ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuwPunt }))
				tsb.Enabled = !tsbNieuwPunt.Checked;

			if(tsbNieuwPunt.Checked)
			{
				tekening1.NieuwPunt();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbNieuweRechte_Click(object sender, EventArgs e)
		{
			tsbNieuweRechte.Checked = !tsbNieuweRechte.Checked;
			foreach(ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuweRechte }))
				tsb.Enabled = !tsbNieuweRechte.Checked;

			if(tsbNieuweRechte.Checked)
			{
				tekening1.NieuweRechte();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbNieuweKromme_Click(object sender, EventArgs e)
		{
			tsbNieuweKromme.Checked = !tsbNieuweKromme.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuweKromme }))
				tsb.Enabled = !tsbNieuweKromme.Checked;

			if (tsbNieuweKromme.Checked)
			{
				tekening1.NieuweKromme();
			}
			else
			{
				tekening1.NieuweVormSubmit();
			}
		}
		private void tsbNieuweCirkelboog_Click(object sender, EventArgs e)
		{
			tsbNieuweCirkelboog.Checked = !tsbNieuweCirkelboog.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuweCirkelboog }))
				tsb.Enabled = !tsbNieuweCirkelboog.Checked;

			if (tsbNieuweCirkelboog.Checked)
			{
				tekening1.NieuweCirkelBoog();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbNieuweRaakboog_Click(object sender, EventArgs e)
		{
			tsbRaakBoog.Checked = !tsbRaakBoog.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbRaakBoog }))
				tsb.Enabled = !tsbRaakBoog.Checked;

			if (tsbRaakBoog.Checked)
			{
				tekening1.NieuweRaakBoog();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbNieuweCirkel3_Click(object sender, EventArgs e)
		{
			tsbNieuweCirkel3.Checked = !tsbNieuweCirkel3.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuweCirkel3 }))
				tsb.Enabled = !tsbNieuweCirkel3.Checked;

			if (tsbNieuweCirkel3.Checked)
			{
				tekening1.NieuweCirkel3();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbNieuweCirkel2_Click(object sender, EventArgs e)
		{
			tsbNieuweCirkel2.Checked = !tsbNieuweCirkel2.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuweCirkel2 }))
				tsb.Enabled = !tsbNieuweCirkel2.Checked;

			if (tsbNieuweCirkel2.Checked)
			{
				tekening1.NieuweCirkel2();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbCirkelSector_Click(object sender, EventArgs e)
		{
			tsbCirkelSector.Checked = !tsbCirkelSector.Checked;
			foreach(ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbCirkelSector }))
				tsb.Enabled = !tsbCirkelSector.Checked;

			if(tsbCirkelSector.Checked)
			{
				tekening1.NieuweCirkelSector();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbCirkelSegment_Click(object sender, EventArgs e)
		{
			tsbCirkelSegment.Checked = !tsbCirkelSegment.Checked;
			foreach(ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbCirkelSegment }))
				tsb.Enabled = !tsbCirkelSegment.Checked;

			if(tsbCirkelSegment.Checked)
			{
				tekening1.NieuwCirkelSegment();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbNieuweEllips_Click(object sender, EventArgs e)
		{
			tsbNieuweEllips.Checked = !tsbNieuweEllips.Checked;
			foreach(ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuweEllips }))
				tsb.Enabled = !tsbNieuweEllips.Checked;

			if(tsbNieuweEllips.Checked)
			{
				tekening1.NieuweEllips();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbGeslotenKromme_Click(object sender, EventArgs e)
		{
			tsbGeslotenKromme.Checked = !tsbGeslotenKromme.Checked;
			foreach(ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbGeslotenKromme }))
				tsb.Enabled = !tsbGeslotenKromme.Checked;

			if(tsbGeslotenKromme.Checked)
			{
				tekening1.NieuweGeslotenKromme();
			}
			else
			{
				tekening1.NieuweVormSubmit();
			}
		}
		private void tsbVeelhoek_Click(object sender, EventArgs e)
		{
			tsbVeelhoek.Checked = !tsbVeelhoek.Checked;
			foreach(ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbVeelhoek }))
				tsb.Enabled = !tsbVeelhoek.Checked;

			if(tsbVeelhoek.Checked)
			{
				tekening1.NieuweVeelhoek();
			}
			else
			{
				tekening1.NieuweVormSubmit();
			}
		}
		private void tsbTekst_Click(object sender, EventArgs e)
		{
			tsbTekst.Checked = !tsbTekst.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbTekst }))
				tsb.Enabled = !tsbTekst.Checked;

			if (tsbTekst.Checked)
			{
				tekening1.NieuweTekst();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tsbNieuweMaatlijn_Click(object sender, EventArgs e)
		{
			tsbNieuweMaatlijn.Checked = !tsbNieuweMaatlijn.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbNieuweMaatlijn }))
				tsb.Enabled = !tsbNieuweMaatlijn.Checked;

			if (tsbNieuweMaatlijn.Checked)
			{
				tekening1.NieuweMaatlijn();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		private void tekening1_NieuweVormVoltooid(object sender, EventArgs e)
		{
			foreach (ToolStripButton tsb in vorm_buttons)
			{
				tsb.Enabled = true;
				tsb.Checked = false;
			}
		}
		#endregion
		#region Advanced actions
		private void tsbEvenwijdige_Click(object sender, EventArgs e)
		{
			tsbEvenwijdige.Checked = !tsbEvenwijdige.Checked;
			foreach (ToolStripButton tsb in vorm_buttons.Except(new ToolStripButton[] { tsbEvenwijdige }))
				tsb.Enabled = !tsbEvenwijdige.Checked;

			if (tsbEvenwijdige.Checked)
			{
				tekening1.NieuweEvenwijdige();
			}
			else
			{
				tekening1.NieuweVormCancel();
			}
		}
		#endregion
		#region IDocument
		public void OpenFile(string filename)
		{
			Text = Path.GetFileNameWithoutExtension(filename);
			tekening1.OpenFile(filename);
		}
		public void Save(out DialogResult dr)
		{
			tekening1.Save(out dr);
		}
		public DialogResult SaveAs()
		{
			return tekening1.SaveAs();
		}
		public void SaveBackup(string map)
		{
			tekening1.SaveBackup(map);
		}
		public bool Changed
		{
			get { return tekening1.Changed; }
		}

		public bool CanPrint
		{
			get
			{
				return tekening1.CanPrint;
			}
		}
		public void PrintDirect()
		{
			tekening1.PrintDirect();
		}
		public void PrintWithDialog()
		{
			tekening1.PrintWithDialog();
		}
		public void ShowPrintPreview()
		{
			tekening1.ShowPrintPreview();
		}
		public void ShowPageSetup()
		{
			tekening1.ShowPageSetup();
		}

		public void Undo()
		{
			tekening1.Undo();
		}
		public void Redo()
		{
			tekening1.Redo();
		}
		public bool CanUndo
		{
			get { return tekening1.CanUndo; }
		}
		public bool CanRedo
		{
			get { return tekening1.CanRedo; }
		}

		public void Cut()
		{
			tekening1.Cut();
		}
		public void Copy()
		{
			tekening1.Copy();
		}
		public void Paste()
		{
			tekening1.Paste();
		}
		public bool CanCopy
		{
			get { return tekening1.CanCopy; }
		}
		public bool CanPaste
		{
			get
			{
				return tekening1.CanPaste;
			}
		}
		public void CopyFont()
		{
			tekening1.CopyFont();
		}
		public void PasteFont()
		{
			tekening1.PasteFont();
		}

		public void SelectAll()
		{
			tekening1.SelectAll();
		}
		public void RemoveSelection()
		{
			tekening1.RemoveSelection();
		}

		public string CurrentFileName
		{
			get { return tekening1.CurrentFileName; }
		}
		public string PreferredFileName
		{
			get
			{
				if (tekening1.CurrentFileName == "")
					return Text;
				else
					return Path.GetFileNameWithoutExtension(tekening1.CurrentFileName);
			}
		}
		#endregion


		/*public void Save(out bool cancel, out string filename)
		{
			tekening1.DemandSave(Text, false, out cancel, out filename);
			if(filename != "")
				Text = Path.GetFileNameWithoutExtension(filename);
		}

		public void SaveAs(out string filename)
		{
			filename = tekening1.SaveAs(Text);
			if(filename != "")
				Text = Path.GetFileNameWithoutExtension(filename);
		}*/



		public bool DoMove()
		{
			return tekening1.DoMove();
		}
		public bool TogglePan()
		{
			return tekening1.TogglePan();
		}


		

		private void frmTekenen_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing && Changed)
			{
				DialogResult dr = MessageBox.Show(string.Format(Properties.Resources.SaveChangesX, PreferredFileName), Properties.Resources.SaveChanges, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				DialogResult sfd_dr;
				switch (dr)
				{
					case DialogResult.Yes:
						Save(out sfd_dr);
						if (sfd_dr == DialogResult.Cancel)
							e.Cancel = true;
						break;
					case DialogResult.No:
						break;
					default:
						e.Cancel = true;
						break;
				}
			}
		}


		public event EventHandler EndMove;
		public event EventHandler EndPan;
		private void tekening1_EndMove(object sender, EventArgs e)
		{
			if(EndMove != null)
				EndMove(sender, e);
		}

		private void tekening1_EndPan(object sender, EventArgs e)
		{
			if(EndPan != null)
				EndPan(this, e);
		}


		private void tsbLayers_Click(object sender, EventArgs e)
		{
			frmLayers dlg = new frmLayers();
			if (dlg.ShowDialog(tekening1) == DialogResult.OK)
			{
				string txt = toolStripComboBox1.SelectedItem.ToString();
				toolStripComboBox1.Items.Clear();
				toolStripComboBox1.Items.AddRange(tekening1.Layers.Select(T => T.Naam).ToArray());
				if (toolStripComboBox1.Items.Contains(txt))
					toolStripComboBox1.SelectedIndex = toolStripComboBox1.Items.IndexOf(txt);
				else
					toolStripComboBox1.SelectedIndex = 0;
				tekening1.Invalidate();
			}
		}


		private void toolStripComboBox1_DropDownClosed(object sender, EventArgs e)
		{
			SendKeys.Send("{ESC}"); // irritante toolstripcombobox sluit-focus-problemen wegwerken
			Layer[] t = tekening1.Layers.Where(T => T.Naam == toolStripComboBox1.SelectedItem.ToString()).ToArray();
			if(t.Length == 1) tekening1.CurrentLayer = t.First();

		}

		private void tsbVraagCoordinaat_CheckedChanged(object sender, EventArgs e)
		{
			tekening1.VraagCoordinaat = tsbVraagCoordinaat.Checked;
			Properties.Settings sett = new Properties.Settings();
			sett.coordinateprompt = tsbVraagCoordinaat.Checked;
			sett.Save();
		}
	}
}
