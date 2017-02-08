namespace DrawIt
{
	partial class frmPuntCoordinaat
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPuntCoordinaat));
			this.btnAnnuleren = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtX = new System.Windows.Forms.TextBox();
			this.txtY = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.SuspendLayout();
			// 
			// btnAnnuleren
			// 
			resources.ApplyResources(this.btnAnnuleren, "btnAnnuleren");
			this.btnAnnuleren.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.helpProvider1.SetHelpKeyword(this.btnAnnuleren, resources.GetString("btnAnnuleren.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.btnAnnuleren, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("btnAnnuleren.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.btnAnnuleren, resources.GetString("btnAnnuleren.HelpString"));
			this.btnAnnuleren.Name = "btnAnnuleren";
			this.helpProvider1.SetShowHelp(this.btnAnnuleren, ((bool)(resources.GetObject("btnAnnuleren.ShowHelp"))));
			this.btnAnnuleren.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.helpProvider1.SetHelpKeyword(this.btnOK, resources.GetString("btnOK.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.btnOK, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("btnOK.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.btnOK, resources.GetString("btnOK.HelpString"));
			this.btnOK.Name = "btnOK";
			this.helpProvider1.SetShowHelp(this.btnOK, ((bool)(resources.GetObject("btnOK.ShowHelp"))));
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// txtX
			// 
			resources.ApplyResources(this.txtX, "txtX");
			this.helpProvider1.SetHelpKeyword(this.txtX, resources.GetString("txtX.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.txtX, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("txtX.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.txtX, resources.GetString("txtX.HelpString"));
			this.txtX.Name = "txtX";
			this.helpProvider1.SetShowHelp(this.txtX, ((bool)(resources.GetObject("txtX.ShowHelp"))));
			// 
			// txtY
			// 
			resources.ApplyResources(this.txtY, "txtY");
			this.helpProvider1.SetHelpKeyword(this.txtY, resources.GetString("txtY.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.txtY, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("txtY.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.txtY, resources.GetString("txtY.HelpString"));
			this.txtY.Name = "txtY";
			this.helpProvider1.SetShowHelp(this.txtY, ((bool)(resources.GetObject("txtY.ShowHelp"))));
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.helpProvider1.SetHelpKeyword(this.label1, resources.GetString("label1.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label1, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label1.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label1, resources.GetString("label1.HelpString"));
			this.label1.Name = "label1";
			this.helpProvider1.SetShowHelp(this.label1, ((bool)(resources.GetObject("label1.ShowHelp"))));
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.helpProvider1.SetHelpKeyword(this.label2, resources.GetString("label2.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label2, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label2.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label2, resources.GetString("label2.HelpString"));
			this.label2.Name = "label2";
			this.helpProvider1.SetShowHelp(this.label2, ((bool)(resources.GetObject("label2.ShowHelp"))));
			// 
			// helpProvider1
			// 
			resources.ApplyResources(this.helpProvider1, "helpProvider1");
			// 
			// frmPuntCoordinaat
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnAnnuleren;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtY);
			this.Controls.Add(this.txtX);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnAnnuleren);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.helpProvider1.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this, resources.GetString("$this.HelpString"));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPuntCoordinaat";
			this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
			this.Shown += new System.EventHandler(this.frmPuntCoordinaat_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnAnnuleren;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox txtX;
		public System.Windows.Forms.TextBox txtY;
		private System.Windows.Forms.HelpProvider helpProvider1;
	}
}