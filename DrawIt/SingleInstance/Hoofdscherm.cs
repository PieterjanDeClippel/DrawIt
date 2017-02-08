using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Threading;

namespace DrawIt
{
	public partial class Hoofdscherm : Form
	{
		// Om de mogelijkheid te geven aan de gebruiker om de applicatie via url up te daten,
		// eerst en vooral Properties -> Updates -> Update Location instellen.
		//		vb: http://pieterjan.pro/DrawIt/
		// Daarna publishen en zippen
		// Uiteindelijk:
		// - Application Files
		// - DrawIt.application
		// - DrawIt.rar
		// - setup.exe
		// in eenzelfde map plaatsen op de webserver en DrawIt.rar
		// beschikbaar maken via een link op de site.
		// De koppeling voor de updates wordt automatisch gelegd naar de site
		// Indien dit niet lukt, eerder geinstalleerde versie verwijderen,
		// nieuwe versie DOWNLOADEN van de site en DEZE setup installeren
		
		public Hoofdscherm()
		{
			InitializeComponent();
			InitUndoRedoBoxes();
			OpenFile(Program.GetFileName());
			try
			{
				toolStripStatusLabel.Text = CultureInfo.CurrentCulture.Name;

				ToolStripMenuItem[] lijst = new ToolStripMenuItem[languageToolStripMenuItem.DropDownItems.Count];
				languageToolStripMenuItem.DropDownItems.CopyTo(lijst, 0);
				ToolStripMenuItem[] l = lijst.Where(T => T.Tag.ToString() == CultureInfo.CurrentCulture.Name).ToArray();
				if (l.Length != 0)
					l.First().Checked = true;
			}
			catch(Exception)
			{
			}
		}
		
		private void ShowWindow()
		{
			if (WindowState == FormWindowState.Minimized)
				WindowState = FormWindowState.Normal;

			Activate();
			bool top = TopMost;
			TopMost = true;
			TopMost = top;
		}

		//MdiClient MDI;
		static string CreateGuid()
		{
			//Functie eenmalig te gebruiken om een guid te genereren voor de Mutex
			Guid guid = Guid.NewGuid();
			string tekst = "{" + guid.ToString().ToUpper() + "}";
			Clipboard.SetText(tekst);
			return tekst;
		}

		private void ShowNewForm(object sender, EventArgs e)
		{
            dlgNieuw dlg = new dlgNieuw();
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				string p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DrawIt");
				int i = 1;
				switch(dlg.Resultaat)
				{
					case dlgNieuw.enNieuwDocType.Pictogram:
						string[] t = MdiChildren.Where(T => T.GetType() == typeof(frmIcon)).Select(T => T.Text).ToArray();
						while(File.Exists(Path.Combine(p, "Pictogram " + i)) | t.Contains("Pictogram " + i)) i++;

						frmIcon icon_form = new frmIcon();
						icon_form.MdiParent = this; //MDI;
						icon_form.Text = "Icoon " + i;
						icon_form.Show();
						break;
					case dlgNieuw.enNieuwDocType.Cursor:
						break;
					case dlgNieuw.enNieuwDocType.Afbeelding:
						string[] u = MdiChildren.Where(T => T.GetType() == typeof(frmImageEditor)).Select(T => T.Text).ToArray();
						while (File.Exists(Path.Combine(p, "Afbeelding " + i)) | u.Contains("Afbeelding " + i)) i++;

						frmImageEditor img_form = new frmImageEditor();
						img_form.MdiParent = this; //MDI;
						img_form.Text = "Afbeelding " + i;
						img_form.Show();
						break;
					default:
						string[] v = MdiChildren.Where(T => T.GetType() == typeof(frmTekenen)).Select(T => T.Text).ToArray();
						while(File.Exists(Path.Combine(p, "Tekening " + i + ".tek")) | v.Contains("Tekening " + i)) i++;

						frmTekenen tek_form = new frmTekenen();
						tek_form.MdiParent = this;
						tek_form.Text = "Tekening " + i;
						tek_form.Show();
						tek_form.EndMove += Tek_form_EndMove;
						tek_form.EndPan += Tek_form_EndPan;
						break;
				}
			}
		}


        #region 8-bit encoding
        string f = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "afbeelding.bmp");
        Bitmap MaakBitmap()
        {
            Bitmap bmp = new Bitmap(16, 16);
            Graphics gr = Graphics.FromImage(bmp);

            gr.Clear(Color.Gold);
            gr.DrawLine(Pens.Black, 3, 3, 12, 12);
            gr.DrawLine(Pens.Black, 3, 12, 12, 3);

            return bmp;
        }
        void save8bit(Bitmap bmp, string bestand)
        {
            var enc = GetCodecInfo("image/bmp");

            var parms = new EncoderParameters(2);
            parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionNone);
            parms.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 8);

            bmp.Save(bestand, enc, parms);
        }

        // http://www.ridgesolutions.ie/index.php/2014/02/06/save-8-bit-uncompressed-windows-bitmap-file-bmp-in-c-net/
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders().Where(T => T.MimeType == mimeType).ToArray();
            return encoders.FirstOrDefault();
        }
        #endregion

        private void Tek_form_EndPan(object sender, EventArgs e)
		{
			tmiPan.Checked = false;
		}

		private void Tek_form_EndMove(object sender, EventArgs e)
		{
			tmiMove.Checked = false;
		}

		private void OpenFile(object sender, EventArgs e)
		{
			string d = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DrawIt";
			if(!Directory.Exists(d)) Directory.CreateDirectory(d);
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = d;
			openFileDialog.Filter = "Alle ondersteunde bestandsformaten|*.tek;*.ico|Tekeningen|*.tek|Pictogrammen|*.ico";
			openFileDialog.Multiselect = true;
			if(openFileDialog.ShowDialog(this) == DialogResult.OK)
				foreach(string file in openFileDialog.FileNames)
					OpenFile(file);
		}
		private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void CutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.Cut();
		}
		private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.Copy();
		}
		private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.Paste();
		}
		private void tmiOpmaakKopieren_Click(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.CopyFont();
		}
		private void tmiOpmaakPlakken_Click(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.PasteFont();
		}
		private void tmiMove_Click(object sender, EventArgs e)
		{
			Form frm = (Form)ActiveMdiChild;
			if(frm == null) return;
			if(frm.GetType() != typeof(frmTekenen)) return;
			tmiMove.Checked = ((frmTekenen)frm).DoMove();
		}
		private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStrip.Visible = toolBarToolStripMenuItem.Checked;
		}
		private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			statusStrip.Visible = statusBarToolStripMenuItem.Checked;
		}
		private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}
		private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}
		private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}
		private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.ArrangeIcons);
		}
		private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach(Form childForm in MdiChildren)
			{
				childForm.Close();
			}
		}
		private void ShowPrintPreview(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.ShowPrintPreview();
		}
		private void printSetupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.ShowPageSetup();
		}
		private void DirectPrint(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.PrintDirect();
		}
		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.PrintWithDialog();
		}
		private void tsbUpdate_Click(object sender, EventArgs e)
		{
			frmUpdate updateform = new frmUpdate();
			updateform.ShowDialog();
            try
            {
                btnUpdate.Visible = ApplicationDeployment.CurrentDeployment.CheckForUpdate();
            }
            catch (Exception)
            {
            }
        }
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox1 frm = new AboutBox1();
			frm.ShowDialog();
		}
		
		public void OpenFile(string bestand)
		{
			if (bestand.Replace(" ", "") == "") return;
			if (!File.Exists(bestand)) return;
			IDocument[] vensters = MdiChildren.Select(T => (IDocument)T).Where(T => T.CurrentFileName == bestand).ToArray();
			if (vensters.Length == 0)
			{
				switch (Path.GetExtension(bestand))
				{
					case ".tek":
						frmTekenen childForm = new frmTekenen();
						childForm.MdiParent = this;
						childForm.Show();
						childForm.EndMove += Tek_form_EndMove;
						childForm.EndPan += Tek_form_EndPan;
						childForm.OpenFile(bestand);
						break;
					case ".ico":
						frmIcon childForm2 = new frmIcon();
						childForm2.MdiParent = this;
						childForm2.Show();
						childForm2.Open(bestand);
						break;
				}
			}
			else
			{
				vensters[0].Activate();
			}
		}

		private void Opslaan(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			DialogResult blob;
			if (active != null)
				active.Save(out blob);
		}

		private void OpslaanAls(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (active != null)
				active.SaveAs();
		}

		private void Hoofdscherm_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach(string file in files)
					switch (Path.GetExtension(file))
					{
						case ".tek":
						case ".ico":
						case ".icon":
							e.Effect = DragDropEffects.Copy;
							return;
					}
			}
		}

		private void Hoofdscherm_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach(string file in files)
				switch (Path.GetExtension(file))
				{
					case ".tek":
					case ".ico":
					case ".icon":
						OpenFile(file);
						break;
				}
		}

		private void editMenu_DropDownOpening(object sender, EventArgs e)
		{
			try
			{
				IDocument active = (IDocument)ActiveMdiChild;
				if (active == null)
				{
					undoToolStripMenuItem.Enabled = false;
					redoToolStripMenuItem.Enabled = false;
					tmiKnippen.Enabled = false;
					tmiKopieren.Enabled = false;
					tmiPlakken.Enabled = false;
					selectAllToolStripMenuItem.Enabled = false;
					selectieWissenToolStripMenuItem.Enabled = false;
					tmiOpmaakKopieren.Enabled = false;
					tmiOpmaakPlakken.Enabled = false;
				}
				else
				{
					undoToolStripMenuItem.Enabled = active.CanUndo;
					redoToolStripMenuItem.Enabled = active.CanRedo;
					tmiKnippen.Enabled = active.CanCopy;
					tmiKopieren.Enabled = active.CanCopy;
					tmiPlakken.Enabled = active.CanPaste;
					selectAllToolStripMenuItem.Enabled = true;
					selectieWissenToolStripMenuItem.Enabled = active.CanCopy;
					tmiOpmaakKopieren.Enabled = active.CanCopy;
					tmiOpmaakPlakken.Enabled = active.CanCopy;
				}
				//tmiPlakken.Enabled = Clipboard.ContainsData("Vorm_Array");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void editMenu_DropDownClosed(object sender, EventArgs e)
		{
			undoToolStripMenuItem.Enabled = true;
			redoToolStripMenuItem.Enabled = true;
			tmiKnippen.Enabled = true;
			tmiKopieren.Enabled = true;
			tmiPlakken.Enabled = true;
			selectAllToolStripMenuItem.Enabled = true;
			selectieWissenToolStripMenuItem.Enabled = true;
		}

		string ResolveShortCut(string shc)
		{
			// Add Reference -> COM -> Windows Script Host Object Model
			if (File.Exists(shc))
			{
				IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
				IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shc);
				return link.TargetPath;
			}
			else
			{
				return "";
			}
		}
		private void fileMenu_DropDownOpening(object sender, EventArgs e)
		{
			try
			{
				IDocument active = (IDocument)ActiveMdiChild;
				if (active == null)
				{
					saveToolStripMenuItem.Enabled = false;
					saveAsToolStripMenuItem.Enabled = false;
					printToolStripMenuItem.Enabled = false;
					printSetupToolStripMenuItem.Enabled = false;
					printPreviewToolStripMenuItem.Enabled = false;
				}
				else
				{
					saveToolStripMenuItem.Enabled = active.Changed;
					saveAsToolStripMenuItem.Enabled = true;
					printToolStripMenuItem.Enabled = active.CanPrint;
					printSetupToolStripMenuItem.Enabled = active.CanPrint;
					printPreviewToolStripMenuItem.Enabled = active.CanPrint;
				}

				// fill the MRU-list
				// http://stackoverflow.com/questions/20915753/last-accessed-files-on-windows

				tmiOnlangsGeopend.DropDownItems.Clear();
				string RecentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
				string[] bestanden = Directory.GetFiles(RecentFolder).Where(T => T.EndsWith(".tek.lnk") | T.EndsWith(".ico.lnk") | T.EndsWith(".icon.lnk")).ToArray();
				if (bestanden.Length == 0)
				{
					tmiOnlangsGeopend.DropDownItems.Add(new ToolStripMenuItem(Properties.Resources.NoRecent) { Enabled = false });
				}
				else
				{
					foreach (string shortcut in bestanden.OrderBy(T => File.GetLastWriteTime(T)).Reverse())
					{
						string target = ResolveShortCut(shortcut);
						ToolStripMenuItem tmi = new ToolStripMenuItem(Path.GetFileNameWithoutExtension(target));
						tmi.ToolTipText = target;
						tmi.Click += delegate
						{
							if (File.Exists(target))
								OpenFile(target);
							else
							{
								DialogResult dr = MessageBox.Show(string.Format(Properties.Resources.FileNoLongerExists, target), Properties.Resources.FileNotFound, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
								if (dr == DialogResult.Yes)
									File.Delete(shortcut);
							}
						};
						tmiOnlangsGeopend.DropDownItems.Add(tmi);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void fileMenu_DropDownClosed(object sender, EventArgs e)
		{
			saveToolStripMenuItem.Enabled = true;
			saveAsToolStripMenuItem.Enabled = true;
			printToolStripMenuItem.Enabled = true;
			printSetupToolStripMenuItem.Enabled = true;
			printPreviewToolStripMenuItem.Enabled = true;
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDocument frm = (IDocument)ActiveMdiChild;
			if(frm != null)
				if(frm.CanUndo)
					frm.Undo();
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDocument frm = (IDocument)ActiveMdiChild;
			if(frm != null)
				if(frm.CanRedo)
					frm.Redo();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmTekenen frm = (frmTekenen)ActiveMdiChild;
			if(frm != null)
				frm.SelectAll();
        }

		private void selectieWissenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmTekenen frm = (frmTekenen)ActiveMdiChild;
			if(frm != null)
				frm.RemoveSelection();
        }

		private void Hoofdscherm_FormClosing(object sender, FormClosingEventArgs e)
		{
			IDocument[] docs_changed = MdiChildren.Where(T => Implements(T, typeof(IDocument))).Select(T => (IDocument)T).Where(T => T.Changed).ToArray();
			if (docs_changed.Length != 0)
			{
				if (e.CloseReason == CloseReason.TaskManagerClosing | e.CloseReason == CloseReason.WindowsShutDown)
				{
					string map = Path.Combine(Program.EnvironmentFolder, "Backup");
					if (!Directory.Exists(map)) Directory.CreateDirectory(map);
					foreach (IDocument doc in docs_changed)
						doc.SaveBackup(map);
				}
				else
				{
					frmSave frm = new frmSave();
					frm.SetItems(docs_changed.Select(T => T.PreferredFileName));

					switch (frm.ShowDialog())
					{
						case DialogResult.OK:
							DialogResult dr;
							foreach (IDocument doc in docs_changed)
							{
								doc.Save(out dr);
								// indien bestand reeds op schijf     -> dr = DialogResult.None
								// indien nieuw bestand + OK-klik     -> dr = DialogResult.OK
								// indien nieuw bestand + Cancel-klik -> dr = DialogResult.Cancel
								if (dr == DialogResult.Cancel)
								{
									e.Cancel = true;
									return;
								}
								else
								{
									doc.Close();
								}
							}
							break;
						case DialogResult.Ignore:
							break;
						default:
							e.Cancel = true;
							break;
					}
				}
			}
		}
		private bool Implements(object obj, Type inf)
		{
			return obj.GetType().GetInterfaces().Contains(inf);
		}

		private void Hoofdscherm_Load(object sender, EventArgs e)
		{
			new Thread(delegate ()
			{
				try
				{
					bool upd = ApplicationDeployment.CurrentDeployment.CheckForUpdate();
					Invoke((Action)delegate { btnUpdate.Visible = upd; });
				}
				catch (Exception)
				{
				}
			}).Start();
		}

		private void tesgtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show(CultureInfo.CurrentCulture.Name);

			ComponentResourceManager resources = new ComponentResourceManager(typeof(Hoofdscherm));
			string txt = resources.GetString("fileMenu.Text");
			MessageBox.Show(txt);
			this.fileMenu.Text = txt;
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			Graphics gr = e.Graphics;
			GraphicsPath p = new GraphicsPath();
			int r = 50;
			gr.SmoothingMode = SmoothingMode.AntiAlias;

			p.AddArc(0, 10, 2 * r, 2 * r, 200, 80);
			p.CloseFigure();
			gr.FillPath(Brushes.Black, p);
		}

		private void tmiPan_Click(object sender, EventArgs e)
		{
			Form frm = ActiveMdiChild;
			if(frm == null) return;
			if(frm.GetType() != typeof(frmTekenen)) return;
			tmiPan.Checked = ((frmTekenen)frm).TogglePan();
		}

		private void tmiToonBestandsmap_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", Program.EnvironmentFolder);
		}

		#region Dynamic Localization
		void ChangeCulture(string cult)
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(Hoofdscherm));
			ChangeStrings(this, resources, new CultureInfo(cult));
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(cult);
			Thread.CurrentThread.CurrentCulture = new CultureInfo(cult);
			toolStripStatusLabel.Text = cult;

			foreach (ToolStripMenuItem it in languageToolStripMenuItem.DropDownItems)
				it.Checked = (it.Tag.ToString() == cult);
		}
		void ChangeStrings(Control parent, ComponentResourceManager man, CultureInfo culture)
		{
			//man.ApplyResources(parent, parent.Name, culture);
			string txt = man.GetString(parent.Name + ".Text", culture);
			if (txt != null) parent.Text = txt;

			if (parent.GetType() == typeof(ToolStrip) | parent.GetType() == typeof(MenuStrip) | parent.GetType() == typeof(ContextMenuStrip))
			{
				foreach (ToolStripItem tmi in ((ToolStrip)parent).Items)
					ChangeStrings(tmi, man, culture);
			}
			else if (parent.GetType() == typeof(MdiClient))
			{
				foreach (Form form in ((MdiClient)parent).MdiChildren)
				{
					ComponentResourceManager frm_res = new ComponentResourceManager(form.GetType());
					ChangeStrings(form, frm_res, culture);
				}
			}
			else if(parent.GetType() == typeof(Tekening))
			{
				ComponentResourceManager tek_res = new ComponentResourceManager(parent.GetType());
				foreach (Control ctl in parent.Controls)
					ChangeStrings(ctl, tek_res, culture);
				ChangeStrings(((Tekening)parent).cms, tek_res, culture);
			}
			else
			{
				foreach (Control ctl in parent.Controls)
					ChangeStrings(ctl, man, culture);
			}
		}
		void ChangeStrings(ToolStripItem parent, ComponentResourceManager man, CultureInfo culture)
		{
			man.ApplyResources(parent, parent.Name, culture);
			if (parent.GetType() == typeof(ToolStripMenuItem))
				foreach (ToolStripItem tmi in ((ToolStripMenuItem)parent).DropDownItems)
					ChangeStrings(tmi, man, culture);
		}

		private void tmiTaal_Click(object sender, EventArgs e)
		{
			ChangeCulture(((ToolStripMenuItem)sender).Tag.ToString());
		}
		#endregion

		IDocument prev_mdi_child;
		private void Hoofdscherm_MdiChildActivate(object sender, EventArgs e)
		{
			IDocument active = (IDocument)ActiveMdiChild;
			if (prev_mdi_child != null)
				prev_mdi_child.BroadcastUndoRedoStack -= MDI_Child_BroadcastUndoRedoStack;
			if (active != null)
			{
				active.BroadcastUndoRedoStack += MDI_Child_BroadcastUndoRedoStack;
				active.RequestUndoRedoStack();
			}
			else
			{
				lstUndo.Items.Clear();
				lstRedo.Items.Clear();
				undoToolstripButton.Enabled = false;
				redoToolstripButton.Enabled = false;
			}
			prev_mdi_child = active;
		}

		private void MDI_Child_BroadcastUndoRedoStack(object sender, UndoRedoStackEventArgs e)
		{
			if (!undoing)
			{
				lstUndo.Items.Clear();
				lstRedo.Items.Clear();
				lstUndo.Items.AddRange(e.UndoStack.Select(T => new ToolStripMenuItem(T.Beschrijving)).ToArray());
				lstRedo.Items.AddRange(e.RedoStack.Select(T => new ToolStripMenuItem(T.Beschrijving)).ToArray());
				undoToolstripButton.Enabled = e.UndoStack.Length != 0;
				redoToolstripButton.Enabled = e.RedoStack.Length != 0;
			}
		}

		bool undoing = false;
		ListBox lstUndo;
		ListBox lstRedo;
		ToolStripDropDown toolDropUndo;
		ToolStripDropDown toolDropRedo;

		void InitUndoRedoBoxes()
		{
			// http://stackoverflow.com/questions/8075040/visual-studio-style-undo-drop-down-button-custom-toolstripsplitbutton

			lstUndo = new ListBox();
			lstRedo = new ListBox();
			
			lstUndo.MaximumSize = lstRedo.MaximumSize = new Size(160, 240); // belangrijk
			lstUndo.SelectionMode = lstRedo.SelectionMode = SelectionMode.MultiSimple;
			lstUndo.ScrollAlwaysVisible = lstRedo.ScrollAlwaysVisible = true;
			
			lstUndo.MouseMove += LstUndo_MouseMove;
			lstRedo.MouseMove += LstRedo_MouseMove;
			lstUndo.MouseClick += LstUndo_MouseClick;
			lstRedo.MouseClick += LstRedo_MouseClick;
		}
		
		private void LstRedo_MouseClick(object sender, MouseEventArgs e)
		{
			IDocument frm = (IDocument)ActiveMdiChild;
			if (frm != null)
			{
				undoing = true;
				for (int i = 0; i <= lstRedo.IndexFromPoint(e.Location); i++)
					if (frm.CanRedo)
						frm.Redo();
				undoing = false;
				toolDropRedo.Close();
				frm.RequestUndoRedoStack();
			}
		}
		private void LstUndo_MouseClick(object sender, MouseEventArgs e)
		{
			IDocument frm = (IDocument)ActiveMdiChild;
			if (frm != null)
			{
				undoing = true;
				for (int i = 0; i <= lstUndo.IndexFromPoint(e.Location); i++)
					if (frm.CanUndo)
						frm.Undo();
				undoing = false;
				toolDropUndo.Close();
				frm.RequestUndoRedoStack();
			}
		}

		private void LstRedo_MouseMove(object sender, MouseEventArgs e)
		{
			int i = lstRedo.IndexFromPoint(e.Location);
			for (int j = 0; j < lstRedo.Items.Count; j++)
			{
				if(j <= i)
				{
					if (!lstRedo.SelectedIndices.Contains(j))
						lstRedo.SelectedIndices.Add(j);
				}
				else
				{
					if (lstRedo.SelectedIndices.Contains(j))
						lstRedo.SelectedIndices.Remove(j);
				}
			}
		}
		private void LstUndo_MouseMove(object sender, MouseEventArgs e)
		{
			int i = lstUndo.IndexFromPoint(e.Location);
			for (int j = 0; j < lstUndo.Items.Count; j++)
			{
				if (j <= i)
				{
					if (!lstUndo.SelectedIndices.Contains(j))
						lstUndo.SelectedIndices.Add(j);
				}
				else
				{
					if (lstUndo.SelectedIndices.Contains(j))
						lstUndo.SelectedIndices.Remove(j);
				}
			}
		}
		private void redoToolstripButton_DropDownOpening(object sender, EventArgs e)
		{
			ToolStripControlHost toolhost = new ToolStripControlHost(lstRedo);
			toolhost.Size = new Size(160, 240);
			toolhost.Margin = new Padding(0);
			toolDropRedo = new ToolStripDropDown();
			toolDropRedo.Padding = new Padding(0);
			toolDropRedo.Items.Add(toolhost);
			Rectangle rct = RectangleToScreen(redoToolstripButton.Bounds);
			toolDropRedo.Show(this, new Point(redoToolstripButton.Bounds.Left, redoToolstripButton.Bounds.Bottom + menuStrip.Height));
		}
		private void undoToolstripButton_DropDownOpening(object sender, EventArgs e)
		{
			ToolStripControlHost toolhost = new ToolStripControlHost(lstUndo);
			toolhost.Size = new Size(160, 240);
			toolhost.Margin = new Padding(0);
			toolDropUndo = new ToolStripDropDown();
			toolDropUndo.Padding = new Padding(0);
			toolDropUndo.Items.Add(toolhost);
			Rectangle rct = RectangleToScreen(undoToolstripButton.Bounds);
			toolDropUndo.Show(this, new Point(undoToolstripButton.Bounds.Left, undoToolstripButton.Bounds.Bottom + menuStrip.Height));
		}

		string helpFile = @"C:\Users\Pieterjan\Documents\HelpProjecten\HelpStudioSample\HelpStudioSample.chm";
		private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, helpFile, HelpNavigator.TableOfContents);
		}

		private void indexToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, helpFile, HelpNavigator.Index);
		}

		private void searchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, helpFile, HelpNavigator.Find, "");
		}
	}
}
