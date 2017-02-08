using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawIt
{
	interface IDocument
	{
		void OpenFile(string filename);
		void Save(out DialogResult dr);
		void SaveBackup(string map);
		DialogResult SaveAs();
		bool Changed { get; }

		bool CanPrint { get; }
		void PrintDirect();
		void PrintWithDialog();
		void ShowPrintPreview();
		void ShowPageSetup();

		void Undo();
		void Redo();
		bool CanUndo { get; }
		bool CanRedo { get; }

		void Cut();
		void Copy();
		void Paste();
		bool CanCopy { get; }
		bool CanPaste { get; }
		void CopyFont();
		void PasteFont();

		void SelectAll();
		void RemoveSelection();

		string CurrentFileName { get; }
		string PreferredFileName { get; }

		void Activate();
		void Close();

		event Tekening.BroadcastUndoRedoStackHandler BroadcastUndoRedoStack;
		void RequestUndoRedoStack();
	}
}
