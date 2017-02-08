using System;
using System.Windows.Forms;

namespace DrawIt
{
	partial class frmTekenen
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTekenen));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tsbNieuwPunt = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbNieuweRechte = new System.Windows.Forms.ToolStripButton();
			this.tsbNieuweKromme = new System.Windows.Forms.ToolStripButton();
			this.tsbNieuweCirkelboog = new System.Windows.Forms.ToolStripButton();
			this.tsbRaakBoog = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbNieuweCirkel3 = new System.Windows.Forms.ToolStripButton();
			this.tsbNieuweCirkel2 = new System.Windows.Forms.ToolStripButton();
			this.tsbCirkelSector = new System.Windows.Forms.ToolStripButton();
			this.tsbCirkelSegment = new System.Windows.Forms.ToolStripButton();
			this.tsbNieuweEllips = new System.Windows.Forms.ToolStripButton();
			this.tsbGeslotenKromme = new System.Windows.Forms.ToolStripButton();
			this.tsbVeelhoek = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbTekst = new System.Windows.Forms.ToolStripButton();
			this.tsbNieuweMaatlijn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbEvenwijdige = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.tsbLayers = new System.Windows.Forms.ToolStripButton();
			this.tsbVraagCoordinaat = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tekening1 = new DrawIt.Tekening();
			this.toolStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tsbNieuwPunt,
            this.toolStripSeparator1,
            this.tsbNieuweRechte,
            this.tsbNieuweKromme,
            this.tsbNieuweCirkelboog,
            this.tsbRaakBoog,
            this.toolStripSeparator2,
            this.tsbNieuweCirkel3,
            this.tsbNieuweCirkel2,
            this.tsbCirkelSector,
            this.tsbCirkelSegment,
            this.tsbNieuweEllips,
            this.tsbGeslotenKromme,
            this.tsbVeelhoek,
            this.toolStripSeparator3,
            this.tsbTekst,
            this.tsbNieuweMaatlijn,
            this.toolStripSeparator4,
            this.tsbEvenwijdige,
            this.toolStripSeparator5,
            this.toolStripComboBox1,
            this.tsbLayers,
            this.tsbVraagCoordinaat});
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Name = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
			// 
			// tsbNieuwPunt
			// 
			this.tsbNieuwPunt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbNieuwPunt, "tsbNieuwPunt");
			this.tsbNieuwPunt.Name = "tsbNieuwPunt";
			this.tsbNieuwPunt.Click += new System.EventHandler(this.tsbNieuwPunt_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// tsbNieuweRechte
			// 
			this.tsbNieuweRechte.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbNieuweRechte, "tsbNieuweRechte");
			this.tsbNieuweRechte.Name = "tsbNieuweRechte";
			this.tsbNieuweRechte.Click += new System.EventHandler(this.tsbNieuweRechte_Click);
			// 
			// tsbNieuweKromme
			// 
			this.tsbNieuweKromme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbNieuweKromme, "tsbNieuweKromme");
			this.tsbNieuweKromme.Name = "tsbNieuweKromme";
			this.tsbNieuweKromme.Click += new System.EventHandler(this.tsbNieuweKromme_Click);
			// 
			// tsbNieuweCirkelboog
			// 
			this.tsbNieuweCirkelboog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbNieuweCirkelboog, "tsbNieuweCirkelboog");
			this.tsbNieuweCirkelboog.Name = "tsbNieuweCirkelboog";
			this.tsbNieuweCirkelboog.Click += new System.EventHandler(this.tsbNieuweCirkelboog_Click);
			// 
			// tsbRaakBoog
			// 
			this.tsbRaakBoog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbRaakBoog.Image = global::DrawIt.Properties.Resources.favicon;
			this.tsbRaakBoog.Name = "tsbRaakBoog";
			resources.ApplyResources(this.tsbRaakBoog, "tsbRaakBoog");
			this.tsbRaakBoog.Click += new System.EventHandler(this.tsbNieuweRaakboog_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// tsbNieuweCirkel3
			// 
			this.tsbNieuweCirkel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbNieuweCirkel3, "tsbNieuweCirkel3");
			this.tsbNieuweCirkel3.Name = "tsbNieuweCirkel3";
			this.tsbNieuweCirkel3.Click += new System.EventHandler(this.tsbNieuweCirkel3_Click);
			// 
			// tsbNieuweCirkel2
			// 
			this.tsbNieuweCirkel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbNieuweCirkel2, "tsbNieuweCirkel2");
			this.tsbNieuweCirkel2.Name = "tsbNieuweCirkel2";
			this.tsbNieuweCirkel2.Click += new System.EventHandler(this.tsbNieuweCirkel2_Click);
			// 
			// tsbCirkelSector
			// 
			this.tsbCirkelSector.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbCirkelSector.Image = global::DrawIt.Properties.Resources.IcoonCirkelSector;
			resources.ApplyResources(this.tsbCirkelSector, "tsbCirkelSector");
			this.tsbCirkelSector.Name = "tsbCirkelSector";
			this.tsbCirkelSector.Click += new System.EventHandler(this.tsbCirkelSector_Click);
			// 
			// tsbCirkelSegment
			// 
			this.tsbCirkelSegment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbCirkelSegment.Image = global::DrawIt.Properties.Resources.iconCirkelSegment;
			this.tsbCirkelSegment.Name = "tsbCirkelSegment";
			resources.ApplyResources(this.tsbCirkelSegment, "tsbCirkelSegment");
			this.tsbCirkelSegment.Click += new System.EventHandler(this.tsbCirkelSegment_Click);
			// 
			// tsbNieuweEllips
			// 
			this.tsbNieuweEllips.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbNieuweEllips.Image = global::DrawIt.Properties.Resources.iconEllips;
			resources.ApplyResources(this.tsbNieuweEllips, "tsbNieuweEllips");
			this.tsbNieuweEllips.Name = "tsbNieuweEllips";
			this.tsbNieuweEllips.Click += new System.EventHandler(this.tsbNieuweEllips_Click);
			// 
			// tsbGeslotenKromme
			// 
			this.tsbGeslotenKromme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbGeslotenKromme.Image = global::DrawIt.Properties.Resources.IconGeslotenKromme;
			resources.ApplyResources(this.tsbGeslotenKromme, "tsbGeslotenKromme");
			this.tsbGeslotenKromme.Name = "tsbGeslotenKromme";
			this.tsbGeslotenKromme.Click += new System.EventHandler(this.tsbGeslotenKromme_Click);
			// 
			// tsbVeelhoek
			// 
			this.tsbVeelhoek.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbVeelhoek.Image = global::DrawIt.Properties.Resources.IconVeelhoek;
			resources.ApplyResources(this.tsbVeelhoek, "tsbVeelhoek");
			this.tsbVeelhoek.Name = "tsbVeelhoek";
			this.tsbVeelhoek.Click += new System.EventHandler(this.tsbVeelhoek_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			// 
			// tsbTekst
			// 
			this.tsbTekst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbTekst.Image = global::DrawIt.Properties.Resources.tekst1;
			this.tsbTekst.Name = "tsbTekst";
			resources.ApplyResources(this.tsbTekst, "tsbTekst");
			this.tsbTekst.Click += new System.EventHandler(this.tsbTekst_Click);
			// 
			// tsbNieuweMaatlijn
			// 
			this.tsbNieuweMaatlijn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbNieuweMaatlijn.Image = global::DrawIt.Properties.Resources.iconMaatlijn;
			resources.ApplyResources(this.tsbNieuweMaatlijn, "tsbNieuweMaatlijn");
			this.tsbNieuweMaatlijn.Name = "tsbNieuweMaatlijn";
			this.tsbNieuweMaatlijn.Click += new System.EventHandler(this.tsbNieuweMaatlijn_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// tsbEvenwijdige
			// 
			this.tsbEvenwijdige.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbEvenwijdige, "tsbEvenwijdige");
			this.tsbEvenwijdige.Name = "tsbEvenwijdige";
			this.tsbEvenwijdige.Click += new System.EventHandler(this.tsbEvenwijdige_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			// 
			// toolStripComboBox1
			// 
			this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBox1.Items.AddRange(new object[] {
            resources.GetString("toolStripComboBox1.Items")});
			this.toolStripComboBox1.Name = "toolStripComboBox1";
			resources.ApplyResources(this.toolStripComboBox1, "toolStripComboBox1");
			this.toolStripComboBox1.DropDownClosed += new System.EventHandler(this.toolStripComboBox1_DropDownClosed);
			// 
			// tsbLayers
			// 
			this.tsbLayers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbLayers.Image = global::DrawIt.Properties.Resources.iconLayers;
			this.tsbLayers.Name = "tsbLayers";
			resources.ApplyResources(this.tsbLayers, "tsbLayers");
			this.tsbLayers.Click += new System.EventHandler(this.tsbLayers_Click);
			// 
			// tsbVraagCoordinaat
			// 
			this.tsbVraagCoordinaat.Checked = true;
			this.tsbVraagCoordinaat.CheckOnClick = true;
			this.tsbVraagCoordinaat.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsbVraagCoordinaat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbVraagCoordinaat.Image = global::DrawIt.Properties.Resources.favicon2;
			this.tsbVraagCoordinaat.Name = "tsbVraagCoordinaat";
			resources.ApplyResources(this.tsbVraagCoordinaat, "tsbVraagCoordinaat");
			this.tsbVraagCoordinaat.CheckedChanged += new System.EventHandler(this.tsbVraagCoordinaat_CheckedChanged);
			// 
			// statusStrip1
			// 
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.Name = "statusStrip1";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tekening1);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// tekening1
			// 
			this.tekening1.Actie = DrawIt.enActie.Selecteren;
			this.tekening1.Cursor = System.Windows.Forms.Cursors.Default;
			resources.ApplyResources(this.tekening1, "tekening1");
			this.tekening1.Name = "tekening1";
			this.tekening1.Offset = ((System.Drawing.PointF)(resources.GetObject("tekening1.Offset")));
			this.tekening1.Schaal = 1F;
			this.tekening1.VraagCoordinaat = true;
			this.tekening1.EndMove += new System.EventHandler(this.tekening1_EndMove);
			this.tekening1.EndPan += new System.EventHandler(this.tekening1_EndPan);
			this.tekening1.NieuweVormVoltooid += new System.EventHandler(this.tekening1_NieuweVormVoltooid);
			// 
			// frmTekenen
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "frmTekenen";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTekenen_FormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbNieuwPunt;
		private System.Windows.Forms.ToolStripButton tsbNieuweRechte;
		private System.Windows.Forms.ToolStripButton tsbNieuweKromme;
		private System.Windows.Forms.ToolStripButton tsbNieuweCirkelboog;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsbNieuweCirkel3;
		private System.Windows.Forms.ToolStripButton tsbNieuweCirkel2;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripButton tsbCirkelSector;
		private System.Windows.Forms.ToolStripButton tsbNieuweEllips;
		private System.Windows.Forms.ToolStripButton tsbGeslotenKromme;
		private System.Windows.Forms.ToolStripButton tsbVeelhoek;
		private ToolStripButton tsbTekst;
		private Panel panel1;
		private ToolStripButton tsbRaakBoog;
		private ToolStripButton tsbCirkelSegment;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripButton tsbLayers;
		private ToolStripLabel toolStripLabel1;
        private ToolStripButton tsbNieuweMaatlijn;
		public ToolStripComboBox toolStripComboBox1;
		private ToolStripButton tsbVraagCoordinaat;
		private Tekening tekening1;
		private ToolStripButton tsbEvenwijdige;
		private ToolStripSeparator toolStripSeparator5;
	}
}