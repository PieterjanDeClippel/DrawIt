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
	// http://www.codersource.net/2010/02/02/image-processing-xor-xnor/
	public partial class frmIcon : Form, IDocument
	{
		public frmIcon()
		{
			InitializeComponent();
		}

		public void SaveIcons(Bitmap[] bmp, string filename)
		{
			// https://www.daubnet.com/en/file-format-ico
			// http://www.blitzmax.com/Community/posts.php?topic=75770

			MemoryStream ms = new MemoryStream();

			// HEADER
			// - reserved
			ms.WriteByte(0);
			ms.WriteByte(0);
			// - type: 1 icon ; 2 cursor
			ms.WriteByte(1);
			ms.WriteByte(0);
			// - aantal icons
			ms.WriteByte((byte)bmp.Length);
			ms.WriteByte((byte)(bmp.Length >> 8));

			// ICON-HEADERS
			foreach(Bitmap b in bmp)
			{
				// * breedte
				ms.WriteByte(b.Width == 256 ? (byte)0 : (byte)b.Width);
				// * hoogte
				ms.WriteByte(b.Height == 256 ? (byte)0 : (byte)b.Height);
				// * color count
				ms.WriteByte(0);
				// * reserved
				ms.WriteByte(0);
				// * color planes
				ms.WriteByte(1);
				ms.WriteByte(0);
				// * bits/pixel
				ms.WriteByte(8);
				ms.WriteByte(0);
				// * bitmap-data size
				ms.WriteByte(0); //15 + k * 16
				ms.WriteByte(0);
				ms.WriteByte(0);
				ms.WriteByte(0);
				// * bitmap-data positie
				ms.WriteByte(0); // 19 + k * 16
				ms.WriteByte(0);
				ms.WriteByte(0);
				ms.WriteByte(0);
			}

			List<long> sizes = new List<long>();
			List<long> positions = new List<long>();

			foreach(Bitmap b in bmp)
			{
				positions.Add(ms.Position);
				b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				sizes.Add(ms.Position - positions.Last());
			}

			for(int k = 0; k < bmp.Length; k++)
			{
				ms.Seek(15 + k * 16, SeekOrigin.Begin);
				ms.WriteByte((byte)sizes[k]);
				ms.WriteByte((byte)(sizes[k] >> 8));
				ms.WriteByte((byte)(sizes[k] >> 16));
				ms.WriteByte((byte)(sizes[k] >> 24));
				ms.WriteByte((byte)positions[k]);
				ms.WriteByte((byte)(positions[k] >> 8));
				ms.WriteByte((byte)(positions[k] >> 16));
				ms.WriteByte((byte)(positions[k] >> 24));
			}
			ms.Seek(0, SeekOrigin.Begin);
			FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            ms.CopyTo(fs);
            fs.Close();
		}


		List<Bitmap> bitmaps = new List<Bitmap>();
		private void btnAdd_Click(object sender, EventArgs e)
		{
			
		}

        Pictogram pic;
        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
			try
			{
				pictureBox1.Image = pic.Icons[listView1.SelectedIndices[0]].GetBitmap();
			}
			catch (Exception)
			{
			}
        }

        private void btnToonData_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            //frmData.Toon(pic.Icons[listView1.SelectedIndices[0]].Data);
        }

		public void Open(string filename)
		{
			pic = new Pictogram();
			pic.Load(filename);

			int i = 0;
			listView1.Items.Clear();
			foreach (Subpictogram icon in pic.Icons)
			{
				ListViewItem lvi = new ListViewItem(i++.ToString());
				lvi.SubItems.Add(icon.Width + " x " + icon.Height);
				lvi.SubItems.Add(icon.ColorCount.ToString());
				lvi.SubItems.Add(icon.ColorPlanes.ToString());
				lvi.SubItems.Add(icon.BitCount.ToString());
				lvi.SubItems.Add(icon.DataSize.ToString());
				lvi.SubItems.Add(icon.DataOffset.ToString());
				lvi.SubItems.Add(icon.Compression.ToString());
				listView1.Items.Add(lvi);
			}
		}

		public void OpenFile(string filename)
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

		public void PrintDirect()
		{
			throw new NotImplementedException();
		}

		public void PrintWithDialog()
		{
			throw new NotImplementedException();
		}

		public void ShowPrintPreview()
		{
			throw new NotImplementedException();
		}

		public void ShowPageSetup()
		{
			throw new NotImplementedException();
		}

		public void Undo()
		{
			throw new NotImplementedException();
		}

		public void Redo()
		{
			throw new NotImplementedException();
		}

		public void Cut()
		{
			throw new NotImplementedException();
		}

		public void Copy()
		{
			throw new NotImplementedException();
		}

		public void Paste()
		{
			throw new NotImplementedException();
		}

		public void SelectAll()
		{
			throw new NotImplementedException();
		}

		public void RemoveSelection()
		{
			throw new NotImplementedException();
		}

		public void SaveBackup(string map)
		{
			throw new NotImplementedException();
		}

		public void CopyFont()
		{
			throw new NotImplementedException();
		}

		public void PasteFont()
		{
			throw new NotImplementedException();
		}

		public void RequestUndoRedoStack()
		{
			throw new NotImplementedException();
		}

		#region CurrentFileName
		private string currentfilename;

		public event Tekening.BroadcastUndoRedoStackHandler BroadcastUndoRedoStack;

		public string CurrentFileName
		{
			get { return currentfilename; }
			set { currentfilename = value; }
		}

		public bool Changed
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

		public bool CanUndo
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

		public string PreferredFileName
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion

	}
}
