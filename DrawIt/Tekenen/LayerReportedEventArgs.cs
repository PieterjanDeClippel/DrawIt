using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawIt
{
	public class LayersReportedEventArgs
	{
		public LayersReportedEventArgs(string[] Layers, int ActiveLayerIndex)
		{
			layers = Layers;
			activeLayerIndex = ActiveLayerIndex;
		}

		#region Layers
		private string[] layers;
		public string[] Layers
		{
			get { return layers; }
		}
		#endregion
		private int activeLayerIndex;
		public int ActiveLayerIndex
		{
			get { return activeLayerIndex; }
		}
	}
}
