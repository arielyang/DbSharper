using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DbSharper.Updater
{
	internal class UpdateService
	{
		#region Fields

		private XmlDocument manifestDoc;
		private Stopwatch stopwatch;
		private string tempFile;
		private WebClient webClient;

		#endregion Fields

		#region Constructors

		public UpdateService()
		{
			stopwatch = new Stopwatch();

			this.webClient = new WebClient();
			this.webClient.Encoding = Encoding.UTF8;
			this.webClient.Headers.Add("User-Agent", "DbSharperCodeGenerator/" + GetExecutingVersion());

			try
			{
				string manifest = this.webClient.DownloadString("http://www.dbsharper.com/Service/GetVersion.ashx");

				if (string.IsNullOrEmpty(manifest))
				{
					manifestDoc = null;
				}
				else
				{
					manifestDoc = new XmlDocument();

					manifestDoc.LoadXml(manifest);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "DbSharper Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion Constructors

		#region Events

		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		#endregion Events

		#region Properties

		/// <summary>
		/// Elapsed seconds since download started.
		/// </summary>
		public double DownloadElapsedSeconds
		{
			get
			{
				return stopwatch.Elapsed.TotalSeconds;
			}
		}

		#endregion Properties

		#region Methods

		public static string GetExecutingPath()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		public static string GetExecutingVersion()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;

			return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
		}

		public void Download()
		{
			string downloadFileName = GetDownloadFileName();

			this.webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
			this.webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

			string tempPath = Path.Combine(Path.GetTempPath(), "DbSharper");
			tempFile = Path.Combine(tempPath, downloadFileName);

			if (Directory.Exists(tempPath))
			{
				Directory.Delete(tempPath, true);
			}

			Directory.CreateDirectory(tempPath);

			this.stopwatch.Start();

			this.webClient.DownloadFileAsync(new Uri("http://www.dbsharper.com/Downloads/" + downloadFileName), tempFile);
		}

		public string GetLatestUpdateSummary()
		{
			if (manifestDoc == null)
			{
				return null;
			}

			XmlNode node = manifestDoc.SelectSingleNode("/manifest/version/description");

			if (node == null)
			{
				return null;
			}

			return node.InnerText.Trim();
		}

		public string GetLatestVersion()
		{
			if (manifestDoc == null)
			{
				return null;
			}

			XmlNode node = manifestDoc.SelectSingleNode("/manifest/version/@value");

			if (node == null)
			{
				return null;
			}

			return node.Value;
		}

		public bool IsLatestVersion()
		{
			string latestVersion = GetLatestVersion();

			// If we can not get latest version info, we recognize it as the latest version.
			if (latestVersion == null)
			{
				return true;
			}

			return GetExecutingVersion() == latestVersion;
		}

		private string GetDownloadFileName()
		{
			XmlNode node = manifestDoc.SelectSingleNode("/manifest/version/fileName");

			if (node == null)
			{
				return null;
			}

			return node.InnerText;
		}

		private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			stopwatch.Stop();

			MessageBox.Show("Click OK to install the latest version of DbSharper.", "DbSharper", MessageBoxButtons.OK, MessageBoxIcon.Information);

			Process.Start(tempFile);

			Application.Exit();
		}

		private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			DownloadProgressChanged(sender, e);
		}

		#endregion Methods

		#region Other

		//public void ExpandFiles()
		//{
		//	Process p = RunProcess(
		//		"expand",
		//		string.Format(CultureInfo.InvariantCulture, "-F:* \"{0}\" \"{1}\"", tempFile, tempPath));
		//	while (!p.HasExited) ;
		//	if (p.ExitCode != 0)
		//	{
		//		string message = p.StandardOutput.ReadToEnd();
		//		MessageBox.Show(message, "DbSharper Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//		Application.Exit();
		//	}
		//}
		///// <summary>
		///// Run process.
		///// </summary>
		///// <param name="fileName"></param>
		///// <param name="arguments"></param>
		///// <returns></returns>
		//private Process RunProcess(string fileName, string arguments)
		//{
		//	ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
		//	processStartInfo.CreateNoWindow = true;
		//	processStartInfo.RedirectStandardOutput = true;
		//	processStartInfo.UseShellExecute = false;
		//	processStartInfo.WorkingDirectory = tempPath;
		//	Process p = Process.Start(processStartInfo);
		//	return p;
		//}
		//public void RegisterAsm()
		//{
		//	Process p = RunProcess(
		//		Path.Combine(Environment.SystemDirectory, @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe"),
		//		 Path.Combine(GetExecutingPath(), "DbSharper.CodeGenerator.dll") + " /codebase");
		//}
		//public void UnregisterAsm()
		//{
		//	Process p = RunProcess(
		//		Path.Combine(Environment.SystemDirectory, @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe"),
		//		Path.Combine(GetExecutingPath(), "DbSharper.CodeGenerator.dll") + " /unregister");
		//}
		//public string[] GetFiles()
		//{
		//	XmlNodeList nodeList = manifestDoc.SelectNodes("/manifest/version/package/folder/file");
		//	List<string> list = new List<string>();
		//	foreach (XmlNode node in nodeList)
		//	{
		//		list.Add(node.Attributes["name"].Value);
		//	}
		//	return list.ToArray();
		//}

		#endregion Other
	}
}