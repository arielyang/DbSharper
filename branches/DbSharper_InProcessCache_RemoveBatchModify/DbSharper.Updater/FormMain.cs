using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace DbSharper.Updater
{
	public partial class FormMain : Form
	{
		#region Fields

		private UpdateService service;

		#endregion Fields

		#region Constructors

		public FormMain()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Methods

		private void FormMain_Load(object sender, EventArgs e)
		{
			const string message = "Current version: {0}    Latest version: {1}";

			labelVersion.Text = string.Format(message, UpdateService.GetExecutingVersion(), "Checking...");

			service = new UpdateService();
			service.DownloadProgressChanged += new DownloadProgressChangedEventHandler(service_DownloadProgressChanged);

			labelVersion.Text = string.Format(message, UpdateService.GetExecutingVersion(), service.GetLatestVersion());
			textBoxSummary.Text = service.GetLatestUpdateSummary();

			if (service.IsLatestVersion())
			{
				Application.Exit();
			}

			this.Show();

			service.Download();
		}

		private void linkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://" + ((LinkLabel)sender).Text);
		}

		private void service_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			progressBarDownload.Value = e.ProgressPercentage;

			labelDownload.Text = string.Format(
				"Download: {0:p0}      {1:n0}KB / {2:n0}KB       {3:n0}KB/s.",
				(float)e.BytesReceived / e.TotalBytesToReceive,
				e.BytesReceived / 1024,
				e.TotalBytesToReceive / 1024,
				e.BytesReceived / 1024 / service.DownloadElapsedSeconds);
		}

		#endregion Methods
	}
}