namespace DrawIt
{
	partial class frmSave
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSave));
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOpslaan = new System.Windows.Forms.Button();
			this.btnNegeren = new System.Windows.Forms.Button();
			this.btnAnnuleren = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			resources.ApplyResources(this.listBox1, "listBox1");
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// btnOpslaan
			// 
			resources.ApplyResources(this.btnOpslaan, "btnOpslaan");
			this.btnOpslaan.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOpslaan.Name = "btnOpslaan";
			this.btnOpslaan.UseVisualStyleBackColor = true;
			// 
			// btnNegeren
			// 
			resources.ApplyResources(this.btnNegeren, "btnNegeren");
			this.btnNegeren.DialogResult = System.Windows.Forms.DialogResult.Ignore;
			this.btnNegeren.Name = "btnNegeren";
			this.btnNegeren.UseVisualStyleBackColor = true;
			// 
			// btnAnnuleren
			// 
			resources.ApplyResources(this.btnAnnuleren, "btnAnnuleren");
			this.btnAnnuleren.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnAnnuleren.Name = "btnAnnuleren";
			this.btnAnnuleren.UseVisualStyleBackColor = true;
			// 
			// frmSave
			// 
			this.AcceptButton = this.btnOpslaan;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnAnnuleren;
			this.Controls.Add(this.btnAnnuleren);
			this.Controls.Add(this.btnNegeren);
			this.Controls.Add(this.btnOpslaan);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSave";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOpslaan;
		private System.Windows.Forms.Button btnNegeren;
		private System.Windows.Forms.Button btnAnnuleren;
	}
}