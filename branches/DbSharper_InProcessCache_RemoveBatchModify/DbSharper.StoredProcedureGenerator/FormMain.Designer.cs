namespace DbSharper.StoredProcedureGenerator
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.splitContainerMain = new System.Windows.Forms.SplitContainer();
			this.treeViewDatabase = new System.Windows.Forms.TreeView();
			this.contextMenuStripTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.createStroredProceduresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
			this.tabControlProcedures = new System.Windows.Forms.TabControl();
			this.tabPageCreate = new System.Windows.Forms.TabPage();
			this.splitContainerForCreate = new System.Windows.Forms.SplitContainer();
			this.checkedListBoxForCreate = new System.Windows.Forms.CheckedListBox();
			this.textBoxForCreate = new System.Windows.Forms.TextBox();
			this.tabPageUpdate = new System.Windows.Forms.TabPage();
			this.tabPageGet = new System.Windows.Forms.TabPage();
			this.splitContainerMain.Panel1.SuspendLayout();
			this.splitContainerMain.Panel2.SuspendLayout();
			this.splitContainerMain.SuspendLayout();
			this.contextMenuStripTreeView.SuspendLayout();
			this.tabControlProcedures.SuspendLayout();
			this.tabPageCreate.SuspendLayout();
			this.splitContainerForCreate.Panel1.SuspendLayout();
			this.splitContainerForCreate.Panel2.SuspendLayout();
			this.splitContainerForCreate.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainerMain
			// 
			this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
			this.splitContainerMain.Name = "splitContainerMain";
			// 
			// splitContainerMain.Panel1
			// 
			this.splitContainerMain.Panel1.Controls.Add(this.treeViewDatabase);
			// 
			// splitContainerMain.Panel2
			// 
			this.splitContainerMain.Panel2.Controls.Add(this.tabControlProcedures);
			this.splitContainerMain.Size = new System.Drawing.Size(624, 414);
			this.splitContainerMain.SplitterDistance = 208;
			this.splitContainerMain.TabIndex = 0;
			// 
			// treeViewDatabase
			// 
			this.treeViewDatabase.ContextMenuStrip = this.contextMenuStripTreeView;
			this.treeViewDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewDatabase.ImageIndex = 0;
			this.treeViewDatabase.ImageList = this.imageListTreeView;
			this.treeViewDatabase.ItemHeight = 18;
			this.treeViewDatabase.Location = new System.Drawing.Point(0, 0);
			this.treeViewDatabase.Name = "treeViewDatabase";
			this.treeViewDatabase.SelectedImageIndex = 0;
			this.treeViewDatabase.Size = new System.Drawing.Size(208, 414);
			this.treeViewDatabase.TabIndex = 0;
			this.treeViewDatabase.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDatabase_AfterSelect);
			this.treeViewDatabase.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDatabase_NodeMouseClick);
			// 
			// contextMenuStripTreeView
			// 
			this.contextMenuStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStroredProceduresToolStripMenuItem});
			this.contextMenuStripTreeView.Name = "contextMenuStripTreeView";
			this.contextMenuStripTreeView.Size = new System.Drawing.Size(212, 26);
			// 
			// createStroredProceduresToolStripMenuItem
			// 
			this.createStroredProceduresToolStripMenuItem.Image = global::DbSharper.StoredProcedureGenerator.Properties.Resources.StoredProcedure;
			this.createStroredProceduresToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.createStroredProceduresToolStripMenuItem.Name = "createStroredProceduresToolStripMenuItem";
			this.createStroredProceduresToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.createStroredProceduresToolStripMenuItem.Text = "&Create Strored Procedures";
			this.createStroredProceduresToolStripMenuItem.Click += new System.EventHandler(this.createStroredProceduresToolStripMenuItem_Click);
			// 
			// imageListTreeView
			// 
			this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
			this.imageListTreeView.TransparentColor = System.Drawing.Color.Fuchsia;
			this.imageListTreeView.Images.SetKeyName(0, "Database");
			this.imageListTreeView.Images.SetKeyName(1, "Folder");
			this.imageListTreeView.Images.SetKeyName(2, "Table");
			this.imageListTreeView.Images.SetKeyName(3, "View");
			this.imageListTreeView.Images.SetKeyName(4, "Column");
			this.imageListTreeView.Images.SetKeyName(5, "PK");
			this.imageListTreeView.Images.SetKeyName(6, "FK");
			// 
			// tabControlProcedures
			// 
			this.tabControlProcedures.Controls.Add(this.tabPageCreate);
			this.tabControlProcedures.Controls.Add(this.tabPageUpdate);
			this.tabControlProcedures.Controls.Add(this.tabPageGet);
			this.tabControlProcedures.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlProcedures.Location = new System.Drawing.Point(0, 0);
			this.tabControlProcedures.Name = "tabControlProcedures";
			this.tabControlProcedures.SelectedIndex = 0;
			this.tabControlProcedures.Size = new System.Drawing.Size(412, 414);
			this.tabControlProcedures.TabIndex = 0;
			// 
			// tabPageCreate
			// 
			this.tabPageCreate.Controls.Add(this.splitContainerForCreate);
			this.tabPageCreate.Location = new System.Drawing.Point(4, 22);
			this.tabPageCreate.Name = "tabPageCreate";
			this.tabPageCreate.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCreate.Size = new System.Drawing.Size(404, 388);
			this.tabPageCreate.TabIndex = 0;
			this.tabPageCreate.Text = "Create";
			this.tabPageCreate.UseVisualStyleBackColor = true;
			// 
			// splitContainerForCreate
			// 
			this.splitContainerForCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerForCreate.Location = new System.Drawing.Point(3, 3);
			this.splitContainerForCreate.Name = "splitContainerForCreate";
			this.splitContainerForCreate.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerForCreate.Panel1
			// 
			this.splitContainerForCreate.Panel1.Controls.Add(this.checkedListBoxForCreate);
			// 
			// splitContainerForCreate.Panel2
			// 
			this.splitContainerForCreate.Panel2.Controls.Add(this.textBoxForCreate);
			this.splitContainerForCreate.Size = new System.Drawing.Size(398, 382);
			this.splitContainerForCreate.SplitterDistance = 235;
			this.splitContainerForCreate.TabIndex = 1;
			// 
			// checkedListBoxForCreate
			// 
			this.checkedListBoxForCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.checkedListBoxForCreate.FormattingEnabled = true;
			this.checkedListBoxForCreate.Location = new System.Drawing.Point(0, 0);
			this.checkedListBoxForCreate.Name = "checkedListBoxForCreate";
			this.checkedListBoxForCreate.Size = new System.Drawing.Size(398, 229);
			this.checkedListBoxForCreate.TabIndex = 0;
			// 
			// textBoxForCreate
			// 
			this.textBoxForCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxForCreate.Location = new System.Drawing.Point(0, 0);
			this.textBoxForCreate.Multiline = true;
			this.textBoxForCreate.Name = "textBoxForCreate";
			this.textBoxForCreate.Size = new System.Drawing.Size(398, 143);
			this.textBoxForCreate.TabIndex = 0;
			// 
			// tabPageUpdate
			// 
			this.tabPageUpdate.Location = new System.Drawing.Point(4, 22);
			this.tabPageUpdate.Name = "tabPageUpdate";
			this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageUpdate.Size = new System.Drawing.Size(404, 388);
			this.tabPageUpdate.TabIndex = 1;
			this.tabPageUpdate.Text = "Update";
			this.tabPageUpdate.UseVisualStyleBackColor = true;
			// 
			// tabPageGet
			// 
			this.tabPageGet.Location = new System.Drawing.Point(4, 22);
			this.tabPageGet.Name = "tabPageGet";
			this.tabPageGet.Size = new System.Drawing.Size(404, 388);
			this.tabPageGet.TabIndex = 2;
			this.tabPageGet.Text = "Get";
			this.tabPageGet.UseVisualStyleBackColor = true;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 414);
			this.Controls.Add(this.splitContainerMain);
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DbSharper Stored Procedure Generator";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.Resize += new System.EventHandler(this.FormMain_Resize);
			this.splitContainerMain.Panel1.ResumeLayout(false);
			this.splitContainerMain.Panel2.ResumeLayout(false);
			this.splitContainerMain.ResumeLayout(false);
			this.contextMenuStripTreeView.ResumeLayout(false);
			this.tabControlProcedures.ResumeLayout(false);
			this.tabPageCreate.ResumeLayout(false);
			this.splitContainerForCreate.Panel1.ResumeLayout(false);
			this.splitContainerForCreate.Panel2.ResumeLayout(false);
			this.splitContainerForCreate.Panel2.PerformLayout();
			this.splitContainerForCreate.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainerMain;
		private System.Windows.Forms.TreeView treeViewDatabase;
		private System.Windows.Forms.TabControl tabControlProcedures;
		private System.Windows.Forms.TabPage tabPageCreate;
		private System.Windows.Forms.TabPage tabPageUpdate;
		private System.Windows.Forms.TabPage tabPageGet;
		private System.Windows.Forms.ImageList imageListTreeView;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeView;
		private System.Windows.Forms.ToolStripMenuItem createStroredProceduresToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainerForCreate;
		private System.Windows.Forms.CheckedListBox checkedListBoxForCreate;
		private System.Windows.Forms.TextBox textBoxForCreate;
	}
}

