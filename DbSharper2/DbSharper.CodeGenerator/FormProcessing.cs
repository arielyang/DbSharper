using System.Diagnostics;
using System.Windows.Forms;

namespace DbSharper2.CodeGenerator
{
	internal partial class FormProcessing : Form
	{
		#region Constructors

		public FormProcessing()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Methods

		public void ShowLogMessage(string message, IconKey iconKey)
		{
			ShowLogMessage(message, 10, iconKey.ToString());
		}

		public void ShowLogMessage(string message, int step, IconKey iconKey)
		{
			ShowLogMessage(message, step, iconKey.ToString());
		}

		public void ShowLogMessage(string message, int step, string iconKey)
		{
			this.listViewMessage.Items.Add(" " + message, iconKey);

			if (this.progressBarGenerating.Value + step < 100)
			{
				this.progressBarGenerating.Value += step;
			}
			else
			{
				this.progressBarGenerating.Value = 100;
			}

			this.Update();
		}

		private void FormProcessing_Load(object sender, System.EventArgs e)
		{
			labelVersion.Text += UpdateService.ExecutingVersionInfo;
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void linkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://" + ((LinkLabel)sender).Text);
		}

		#endregion Methods
	}
}