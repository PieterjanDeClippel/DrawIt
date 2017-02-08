using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DrawIt.Tekenen;

namespace DrawIt
{
	public partial class frmLayers : Form
	{
		public frmLayers()
		{
			InitializeComponent();
		}
		
		public DialogResult ShowDialog(Tekening tek)
		{
			#region CheckedListbox invullen + link naar Layer
			listView1.Items.Clear();
			foreach(Layer layer in tek.Layers)
			{
				ListViewItem lvi = new ListViewItem() { Text = layer.Naam, Tag = layer, Checked = layer.Zichtbaar };
				layer.Tag = lvi;
				listView1.Items.Add(lvi);
			}
			#endregion
			DialogResult dr = this.ShowDialog();
			if (dr == DialogResult.OK)
			{
				// Layers verwijderen
				#region ListviewItems
				ListViewItem[] listviewitems = new ListViewItem[listView1.Items.Count];
				listView1.Items.CopyTo(listviewitems, 0);
				#endregion

				Layer[] to_remove = tek.Layers.Where(T => !listviewitems.Contains(T.Tag)).ToArray();
				Layer[] to_add = listviewitems.Select(T => (Layer)T.Tag).Except(tek.Layers).ToArray();
				Layer[] to_update = listviewitems.Select(T => (Layer)T.Tag).Intersect(tek.Layers).ToArray();

				foreach (Layer layer in to_remove)
					tek.RemoveLayer(layer);
				foreach(Layer layer in to_add)
				{
					ListViewItem lvi = (ListViewItem)layer.Tag;
					layer.Naam = lvi.Text;
					layer.Zichtbaar = lvi.Checked;
					tek.Layers.Add(layer);
				}
				foreach (Layer layer in to_update)
				{
					ListViewItem lvi = (ListViewItem)layer.Tag;
					layer.Naam = lvi.Text;
					layer.Zichtbaar = lvi.Checked;
				}
			}
			return dr;
		}
		
		private void btnBewerken_Click(object sender, EventArgs e)
		{
			switch (listView1.SelectedIndices.Count)
			{
				case 0:
					return;
				case 1:
					if (listView1.SelectedIndices[0] == 0) return;
					string oud = listView1.SelectedItems[0].Text;
					string nieuw = InputBox.Toon("Geef een nieuwe naam aan de layer " + oud, "Hernoem layer", oud);
					listView1.Items[listView1.SelectedIndices[0]].Text = nieuw;
					break;
			}
		}
		private void btnNieuweLayer_Click(object sender, EventArgs e)
		{
			#region Nieuwe id bepalen
			ListViewItem[] l = new ListViewItem[listView1.Items.Count];
			listView1.Items.CopyTo(l, 0);
			string[] t = l.Select(T => T.Text).ToArray();

			int id = 1;
			while (t.Contains("Layer " + id)) id++;
			#endregion
			ListViewItem lvi = new ListViewItem()
			{
				Text = "Layer " + id,
				Checked = true
			};
			Layer layer = new Layer(false)
			{
				Naam = "Layer " + id,
				Zichtbaar = true
			};

			lvi.Tag = layer;
			layer.Tag = lvi;
			listView1.Items.Add(lvi);
		}
		private void btnWisLayer_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem lvi in listView1.SelectedItems)
				if (!((Layer)lvi.Tag).IsDefault)
					listView1.Items.Remove(lvi);
		}

		private void frmLayers_Shown(object sender, EventArgs e)
		{
			btnOK.Focus();
		}
	}
}
