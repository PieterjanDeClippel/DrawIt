namespace DrawIt
{
	partial class frmUpdate
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdate));
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.btnSluiten = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.lblHuidigeVersie = new System.Windows.Forms.Label();
			this.lblNieuweVersie = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnOpnieuw = new System.Windows.Forms.Button();
			this.lblProgressPercent = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.lblProgressBytes = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtStatus
			// 
			this.txtStatus.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.txtStatus, "txtStatus");
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.ReadOnly = true;
			// 
			// btnSluiten
			// 
			resources.ApplyResources(this.btnSluiten, "btnSluiten");
			this.btnSluiten.Name = "btnSluiten";
			this.btnSluiten.UseVisualStyleBackColor = true;
			this.btnSluiten.Click += new System.EventHandler(this.btnSluiten_Click);
			// 
			// btnUpdate
			// 
			resources.ApplyResources(this.btnUpdate, "btnUpdate");
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// lblHuidigeVersie
			// 
			resources.ApplyResources(this.lblHuidigeVersie, "lblHuidigeVersie");
			this.lblHuidigeVersie.Name = "lblHuidigeVersie";
			// 
			// lblNieuweVersie
			// 
			resources.ApplyResources(this.lblNieuweVersie, "lblNieuweVersie");
			this.lblNieuweVersie.Name = "lblNieuweVersie";
			// 
			// progressBar1
			// 
			resources.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			// 
			// btnOpnieuw
			// 
			resources.ApplyResources(this.btnOpnieuw, "btnOpnieuw");
			this.btnOpnieuw.Name = "btnOpnieuw";
			this.btnOpnieuw.UseVisualStyleBackColor = true;
			this.btnOpnieuw.Click += new System.EventHandler(this.btnOpnieuw_Click);
			// 
			// lblProgressPercent
			// 
			resources.ApplyResources(this.lblProgressPercent, "lblProgressPercent");
			this.lblProgressPercent.Name = "lblProgressPercent";
			// 
			// lblStatus
			// 
			resources.ApplyResources(this.lblStatus, "lblStatus");
			this.lblStatus.Name = "lblStatus";
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.txtStatus, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			// 
			// lblProgressBytes
			// 
			resources.ApplyResources(this.lblProgressBytes, "lblProgressBytes");
			this.lblProgressBytes.Name = "lblProgressBytes";
			// 
			// frmUpdate
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblProgressBytes);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lblProgressPercent);
			this.Controls.Add(this.btnOpnieuw);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.lblNieuweVersie);
			this.Controls.Add(this.lblHuidigeVersie);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.btnSluiten);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmUpdate";
			this.Load += new System.EventHandler(this.frmUpdate_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.Button btnSluiten;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Label lblHuidigeVersie;
		private System.Windows.Forms.Label lblNieuweVersie;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnOpnieuw;
		private System.Windows.Forms.Label lblProgressPercent;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label lblProgressBytes;
	}
}