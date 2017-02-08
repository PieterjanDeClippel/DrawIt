using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt
{
	public class UndoRedoStackEventArgs : EventArgs
	{
		public UndoRedoStackEventArgs(Actie[] UndoStack, Actie[] RedoStack)
		{
			undoStack = UndoStack;
			redoStack = RedoStack;
		}
		#region UndoStack
		private Actie[] undoStack;
		public Actie[] UndoStack
		{
			get { return undoStack; }
		}
		#endregion
		#region RedoStack
		private Actie[] redoStack;
		public Actie[] RedoStack
		{
			get { return redoStack; }
		}
		#endregion
	}
}
