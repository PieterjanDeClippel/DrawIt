using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawIt
{
	public partial class frmImageEditor : Form, IDocument
	{
		public frmImageEditor()
		{
			InitializeComponent();
			afbeeldingEditor1.Init();
		}

		public bool CanCopy
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool CanPaste
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool CanPrint
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool CanRedo
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool CanUndo
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool Changed
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string CurrentFileName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string PreferredFileName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public event Tekening.BroadcastUndoRedoStackHandler BroadcastUndoRedoStack;

		public void Copy()
		{
			throw new NotImplementedException();
		}

		public void CopyFont()
		{
			throw new NotImplementedException();
		}

		public void Cut()
		{
			throw new NotImplementedException();
		}

		public void OpenFile(string filename)
		{
			throw new NotImplementedException();
		}

		public void Paste()
		{
			throw new NotImplementedException();
		}

		public void PasteFont()
		{
			throw new NotImplementedException();
		}

		public void PrintDirect()
		{
			throw new NotImplementedException();
		}

		public void PrintWithDialog()
		{
			throw new NotImplementedException();
		}

		public void Redo()
		{
			throw new NotImplementedException();
		}

		public void RemoveSelection()
		{
			throw new NotImplementedException();
		}

		public void RequestUndoRedoStack()
		{
			throw new NotImplementedException();
		}

		public void Save(out DialogResult dr)
		{
			throw new NotImplementedException();
		}

		public DialogResult SaveAs()
		{
			throw new NotImplementedException();
		}

		public void SaveBackup(string map)
		{
			throw new NotImplementedException();
		}

		public void SelectAll()
		{
			throw new NotImplementedException();
		}

		public void ShowPageSetup()
		{
			throw new NotImplementedException();
		}

		public void ShowPrintPreview()
		{
			throw new NotImplementedException();
		}

		public void Undo()
		{
			throw new NotImplementedException();
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}
	}
}
