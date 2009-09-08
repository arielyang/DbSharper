namespace DbSharper.CodeGenerator
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

	internal partial class FormProcessing : Form
    {
        #region Constructors

        public FormProcessing()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void FormProcessing_Load(object sender, System.EventArgs e)
        {
			labelVersion.Text += UpdateService.GetExecutingVersion();
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