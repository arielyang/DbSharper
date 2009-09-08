namespace DbSharper.Updater
{
	partial class FormMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.progressBarDownload = new System.Windows.Forms.ProgressBar();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.labelVersion = new System.Windows.Forms.Label();
			this.linkLabelWebsite = new System.Windows.Forms.LinkLabel();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelDownload = new System.Windows.Forms.Label();
			this.textBoxSummary = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// progressBarDownload
			// 
			this.progressBarDownload.Location = new System.Drawing.Point(12, 239);
			this.progressBarDownload.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.progressBarDownload.Name = "progressBarDownload";
			this.progressBarDownload.Size = new System.Drawing.Size(450, 23);
			this.progressBarDownload.Step = 1;
			this.progressBarDownload.TabIndex = 0;
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
			this.pictureBoxLogo.Location = new System.Drawing.Point(12, 12);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(32, 32);
			this.pictureBoxLogo.TabIndex = 1;
			this.pictureBoxLogo.TabStop = false;
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.BackColor = System.Drawing.Color.Transparent;
			this.labelVersion.Location = new System.Drawing.Point(50, 31);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(196, 13);
			this.labelVersion.TabIndex = 12;
			this.labelVersion.Text = "Current version: {0}    Latest version: {1}";
			// 
			// linkLabelWebsite
			// 
			this.linkLabelWebsite.AutoSize = true;
			this.linkLabelWebsite.BackColor = System.Drawing.Color.Transparent;
			this.linkLabelWebsite.Font = new System.Drawing.Font("Arial", 9F);
			this.linkLabelWebsite.Location = new System.Drawing.Point(341, 29);
			this.linkLabelWebsite.Name = "linkLabelWebsite";
			this.linkLabelWebsite.Size = new System.Drawing.Size(121, 15);
			this.linkLabelWebsite.TabIndex = 11;
			this.linkLabelWebsite.TabStop = true;
			this.linkLabelWebsite.Text = "www.dbsharper.com";
			this.linkLabelWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWebsite_LinkClicked);
			// 
			// labelTitle
			// 
			this.labelTitle.AutoSize = true;
			this.labelTitle.BackColor = System.Drawing.Color.Transparent;
			this.labelTitle.Location = new System.Drawing.Point(50, 12);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(136, 13);
			this.labelTitle.TabIndex = 10;
			this.labelTitle.Text = "DbSharper Code Generator";
			// 
			// labelDownload
			// 
			this.labelDownload.AutoSize = true;
			this.labelDownload.Location = new System.Drawing.Point(12, 272);
			this.labelDownload.Name = "labelDownload";
			this.labelDownload.Size = new System.Drawing.Size(61, 13);
			this.labelDownload.TabIndex = 13;
			this.labelDownload.Text = "Download: ";
			// 
			// textBoxSummary
			// 
			this.textBoxSummary.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxSummary.Location = new System.Drawing.Point(12, 50);
			this.textBoxSummary.Multiline = true;
			this.textBoxSummary.Name = "textBoxSummary";
			this.textBoxSummary.ReadOnly = true;
			this.textBoxSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxSummary.Size = new System.Drawing.Size(450, 176);
			this.textBoxSummary.TabIndex = 14;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(474, 294);
			this.Controls.Add(this.textBoxSummary);
			this.Controls.Add(this.labelDownload);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.linkLabelWebsite);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.pictureBoxLogo);
			this.Controls.Add(this.progressBarDownload);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DbSharper Updater";
			this.Load += new System.EventHandler(this.FormMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBarDownload;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.LinkLabel linkLabelWebsite;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label labelDownload;
		private System.Windows.Forms.TextBox textBoxSummary;
	}
}

