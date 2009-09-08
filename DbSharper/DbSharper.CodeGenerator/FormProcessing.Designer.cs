namespace DbSharper.CodeGenerator
{
	partial class FormProcessing
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
			if (disposing && (components != null))
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcessing));
			this.progressBarGenerating = new System.Windows.Forms.ProgressBar();
			this.buttonOk = new System.Windows.Forms.Button();
			this.listViewMessage = new System.Windows.Forms.ListView();
			this.columnHeaderMessage = new System.Windows.Forms.ColumnHeader();
			this.imageListIcon = new System.Windows.Forms.ImageList(this.components);
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.labelTitle = new System.Windows.Forms.Label();
			this.linkLabelWebsite = new System.Windows.Forms.LinkLabel();
			this.labelVersion = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// progressBarGenerating
			// 
			this.progressBarGenerating.Location = new System.Drawing.Point(12, 50);
			this.progressBarGenerating.MarqueeAnimationSpeed = 50;
			this.progressBarGenerating.Name = "progressBarGenerating";
			this.progressBarGenerating.Size = new System.Drawing.Size(450, 23);
			this.progressBarGenerating.TabIndex = 1;
			// 
			// buttonOk
			// 
			this.buttonOk.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.buttonOk.Enabled = false;
			this.buttonOk.Location = new System.Drawing.Point(200, 339);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 4;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = false;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// listViewMessage
			// 
			this.listViewMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listViewMessage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderMessage});
			this.listViewMessage.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listViewMessage.Location = new System.Drawing.Point(12, 79);
			this.listViewMessage.MultiSelect = false;
			this.listViewMessage.Name = "listViewMessage";
			this.listViewMessage.ShowItemToolTips = true;
			this.listViewMessage.Size = new System.Drawing.Size(450, 245);
			this.listViewMessage.SmallImageList = this.imageListIcon;
			this.listViewMessage.TabIndex = 5;
			this.listViewMessage.UseCompatibleStateImageBehavior = false;
			this.listViewMessage.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderMessage
			// 
			this.columnHeaderMessage.Width = 400;
			// 
			// imageListIcon
			// 
			this.imageListIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcon.ImageStream")));
			this.imageListIcon.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListIcon.Images.SetKeyName(0, "Start");
			this.imageListIcon.Images.SetKeyName(1, "Schema");
			this.imageListIcon.Images.SetKeyName(2, "dbsx");
			this.imageListIcon.Images.SetKeyName(3, "config");
			this.imageListIcon.Images.SetKeyName(4, "cs");
			this.imageListIcon.Images.SetKeyName(5, "html");
			this.imageListIcon.Images.SetKeyName(6, "xml");
			this.imageListIcon.Images.SetKeyName(7, "Error");
			this.imageListIcon.Images.SetKeyName(8, "Success");
			this.imageListIcon.Images.SetKeyName(9, "Failed");
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
			this.pictureBoxLogo.Location = new System.Drawing.Point(12, 12);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(32, 32);
			this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBoxLogo.TabIndex = 6;
			this.pictureBoxLogo.TabStop = false;
			// 
			// labelTitle
			// 
			this.labelTitle.AutoSize = true;
			this.labelTitle.Location = new System.Drawing.Point(50, 12);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(136, 13);
			this.labelTitle.TabIndex = 7;
			this.labelTitle.Text = "DbSharper Code Generator";
			// 
			// linkLabelWebsite
			// 
			this.linkLabelWebsite.AutoSize = true;
			this.linkLabelWebsite.Font = new System.Drawing.Font("Arial", 9F);
			this.linkLabelWebsite.Location = new System.Drawing.Point(341, 29);
			this.linkLabelWebsite.Name = "linkLabelWebsite";
			this.linkLabelWebsite.Size = new System.Drawing.Size(121, 15);
			this.linkLabelWebsite.TabIndex = 8;
			this.linkLabelWebsite.TabStop = true;
			this.linkLabelWebsite.Text = "www.dbsharper.com";
			this.linkLabelWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWebsite_LinkClicked);
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new System.Drawing.Point(50, 31);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(45, 13);
			this.labelVersion.TabIndex = 9;
			this.labelVersion.Text = "Version ";
			// 
			// FormProcessing
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(474, 394);
			this.ControlBox = false;
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.linkLabelWebsite);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.pictureBoxLogo);
			this.Controls.Add(this.listViewMessage);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.progressBarGenerating);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProcessing";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Generating code - DbSharper";
			this.Load += new System.EventHandler(this.FormProcessing_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.ProgressBar progressBarGenerating;
		internal System.Windows.Forms.Button buttonOk;
		internal System.Windows.Forms.ListView listViewMessage;
		private System.Windows.Forms.ImageList imageListIcon;
		private System.Windows.Forms.ColumnHeader columnHeaderMessage;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.LinkLabel linkLabelWebsite;
		private System.Windows.Forms.Label labelVersion;

	}
}