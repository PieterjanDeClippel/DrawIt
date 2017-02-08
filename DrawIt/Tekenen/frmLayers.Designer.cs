namespace DrawIt
{
	partial class frmLayers
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLayers));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnBewerken = new System.Windows.Forms.Button();
			this.btnWisLayer = new System.Windows.Forms.Button();
			this.btnNieuweLayer = new System.Windows.Forms.Button();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.listView1 = new System.Windows.Forms.ListView();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.btnBewerken, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.btnWisLayer, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnNieuweLayer, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// btnBewerken
			// 
			resources.ApplyResources(this.btnBewerken, "btnBewerken");
			this.btnBewerken.Name = "btnBewerken";
			this.btnBewerken.UseVisualStyleBackColor = true;
			this.btnBewerken.Click += new System.EventHandler(this.btnBewerken_Click);
			// 
			// btnWisLayer
			// 
			resources.ApplyResources(this.btnWisLayer, "btnWisLayer");
			this.btnWisLayer.Name = "btnWisLayer";
			this.btnWisLayer.UseVisualStyleBackColor = true;
			this.btnWisLayer.Click += new System.EventHandler(this.btnWisLayer_Click);
			// 
			// btnNieuweLayer
			// 
			resources.ApplyResources(this.btnNieuweLayer, "btnNieuweLayer");
			this.btnNieuweLayer.Name = "btnNieuweLayer";
			this.btnNieuweLayer.UseVisualStyleBackColor = true;
			this.btnNieuweLayer.Click += new System.EventHandler(this.btnNieuweLayer_Click);
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.btnOK, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.btnCancel, 1, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.listView1);
			this.panel1.Name = "panel1";
			// 
			// listView1
			// 
			resources.ApplyResources(this.listView1, "listView1");
			this.listView1.CheckBoxes = true;
			this.listView1.FullRowSelect = true;
			this.listView1.Name = "listView1";
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.List;
			// 
			// frmLayers
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "frmLayers";
			this.Shown += new System.EventHandler(this.frmLayers_Shown);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnWisLayer;
		private System.Windows.Forms.Button btnNieuweLayer;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBewerken;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListView listView1;
	}
}