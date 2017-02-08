namespace DrawIt
{
	partial class Tekening
	{
		private System.ComponentModel.IContainer components;

		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tekening));
			this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tmiEigenschappen = new System.Windows.Forms.ToolStripMenuItem();
			this.tmiWissen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tmiNaarVoorgrond = new System.Windows.Forms.ToolStripMenuItem();
			this.tmiNaarVoor = new System.Windows.Forms.ToolStripMenuItem();
			this.tmiNaarAchter = new System.Windows.Forms.ToolStripMenuItem();
			this.tmiNaarAchtergrond = new System.Windows.Forms.ToolStripMenuItem();
			this.pd = new System.Drawing.Printing.PrintDocument();
			this.txtSchaal = new System.Windows.Forms.TextBox();
			this.cms.SuspendLayout();
			this.SuspendLayout();
			// 
			// cms
			// 
			this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiEigenschappen,
            this.tmiWissen,
            this.toolStripSeparator1,
            this.tmiNaarVoorgrond,
            this.tmiNaarVoor,
            this.tmiNaarAchter,
            this.tmiNaarAchtergrond});
			this.cms.Name = "contextMenuStrip1";
			resources.ApplyResources(this.cms, "cms");
			// 
			// tmiEigenschappen
			// 
			this.tmiEigenschappen.Name = "tmiEigenschappen";
			resources.ApplyResources(this.tmiEigenschappen, "tmiEigenschappen");
			this.tmiEigenschappen.Click += new System.EventHandler(this.tmiEigenschappen_Click);
			// 
			// tmiWissen
			// 
			this.tmiWissen.Name = "tmiWissen";
			resources.ApplyResources(this.tmiWissen, "tmiWissen");
			this.tmiWissen.Click += new System.EventHandler(this.TmiWissen_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// tmiNaarVoorgrond
			// 
			this.tmiNaarVoorgrond.Name = "tmiNaarVoorgrond";
			resources.ApplyResources(this.tmiNaarVoorgrond, "tmiNaarVoorgrond");
			this.tmiNaarVoorgrond.Click += new System.EventHandler(this.TmiNaarVoorgrond_Click);
			// 
			// tmiNaarVoor
			// 
			this.tmiNaarVoor.Name = "tmiNaarVoor";
			resources.ApplyResources(this.tmiNaarVoor, "tmiNaarVoor");
			this.tmiNaarVoor.Click += new System.EventHandler(this.TmiNaarVoor_Click);
			// 
			// tmiNaarAchter
			// 
			this.tmiNaarAchter.Name = "tmiNaarAchter";
			resources.ApplyResources(this.tmiNaarAchter, "tmiNaarAchter");
			this.tmiNaarAchter.Click += new System.EventHandler(this.TmiNaarAchter_Click);
			// 
			// tmiNaarAchtergrond
			// 
			this.tmiNaarAchtergrond.Name = "tmiNaarAchtergrond";
			resources.ApplyResources(this.tmiNaarAchtergrond, "tmiNaarAchtergrond");
			this.tmiNaarAchtergrond.Click += new System.EventHandler(this.TmiNaarAchtergrond_Click);
			// 
			// pd
			// 
			this.pd.OriginAtMargins = true;
			this.pd.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.pd_BeginPrint);
			this.pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd_PrintPage);
			// 
			// txtSchaal
			// 
			resources.ApplyResources(this.txtSchaal, "txtSchaal");
			this.txtSchaal.Name = "txtSchaal";
			this.txtSchaal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSchaal_KeyDown);
			// 
			// Tekening
			// 
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tekening_KeyDown);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tekening_MouseClick);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Tekening_MouseDoubleClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tekening_MouseDown);
			this.MouseLeave += new System.EventHandler(this.Tekening_MouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Tekening_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Tekening_MouseUp);
			this.Resize += new System.EventHandler(this.Tekening_Resize);
			this.cms.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		private System.Windows.Forms.ToolStripMenuItem tmiEigenschappen;
		private System.Windows.Forms.ToolStripMenuItem tmiWissen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem tmiNaarVoorgrond;
		private System.Windows.Forms.ToolStripMenuItem tmiNaarVoor;
		private System.Windows.Forms.ToolStripMenuItem tmiNaarAchter;
		private System.Windows.Forms.ToolStripMenuItem tmiNaarAchtergrond;
		private System.Windows.Forms.TextBox txtSchaal;
		private System.Drawing.Printing.PrintDocument pd;
		public System.Windows.Forms.ContextMenuStrip cms;
	}
}
