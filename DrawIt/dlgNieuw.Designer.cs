namespace DrawIt
{
	partial class dlgNieuw
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgNieuw));
			this.rbnTekening = new System.Windows.Forms.RadioButton();
			this.rbnPictogram = new System.Windows.Forms.RadioButton();
			this.rbnCursor = new System.Windows.Forms.RadioButton();
			this.btnAnnuleren = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.rbnAfbeelding = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// rbnTekening
			// 
			resources.ApplyResources(this.rbnTekening, "rbnTekening");
			this.rbnTekening.Checked = true;
			this.rbnTekening.Name = "rbnTekening";
			this.rbnTekening.TabStop = true;
			this.rbnTekening.UseVisualStyleBackColor = true;
			// 
			// rbnPictogram
			// 
			resources.ApplyResources(this.rbnPictogram, "rbnPictogram");
			this.rbnPictogram.Name = "rbnPictogram";
			this.rbnPictogram.TabStop = true;
			this.rbnPictogram.UseVisualStyleBackColor = true;
			// 
			// rbnCursor
			// 
			resources.ApplyResources(this.rbnCursor, "rbnCursor");
			this.rbnCursor.Name = "rbnCursor";
			this.rbnCursor.TabStop = true;
			this.rbnCursor.UseVisualStyleBackColor = true;
			// 
			// btnAnnuleren
			// 
			resources.ApplyResources(this.btnAnnuleren, "btnAnnuleren");
			this.btnAnnuleren.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnAnnuleren.Name = "btnAnnuleren";
			this.btnAnnuleren.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// rbnAfbeelding
			// 
			resources.ApplyResources(this.rbnAfbeelding, "rbnAfbeelding");
			this.rbnAfbeelding.Name = "rbnAfbeelding";
			this.rbnAfbeelding.TabStop = true;
			this.rbnAfbeelding.UseVisualStyleBackColor = true;
			// 
			// dlgNieuw
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnAnnuleren;
			this.Controls.Add(this.rbnAfbeelding);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnAnnuleren);
			this.Controls.Add(this.rbnCursor);
			this.Controls.Add(this.rbnPictogram);
			this.Controls.Add(this.rbnTekening);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "dlgNieuw";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbnTekening;
		private System.Windows.Forms.RadioButton rbnPictogram;
		private System.Windows.Forms.RadioButton rbnCursor;
		private System.Windows.Forms.Button btnAnnuleren;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.RadioButton rbnAfbeelding;
	}
}