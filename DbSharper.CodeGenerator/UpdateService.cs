using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DbSharper.CodeGenerator
{
	internal class UpdateService
	{
		#region Properties

		public static VersionInfo ExecutingVersionInfo
		{
			get
			{
				Version version = Assembly.GetExecutingAssembly().GetName().Version;

				return new VersionInfo(
					string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build),
					null);
			}
		}

		#endregion Properties

		#region Methods

		public static string GetNewVersionInformation(VersionInfo versionInfo)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("New verion {0} released.", versionInfo.Version);
			sb.AppendLine();
			sb.AppendLine();
			sb.Append(versionInfo.Summary);

			return sb.ToString();
		}

		public VersionInfo GetLatestVersionInfo()
		{
			WebClient webClient = new WebClient();

			webClient.Headers.Add("User-Agent", "DbSharperCodeGenerator/" + ExecutingVersionInfo);

			try
			{
				string manifest = webClient.DownloadString("http://www.dbsharper.com/Service/GetVersion.ashx");

				XmlDocument doc = new XmlDocument();

				doc.LoadXml(manifest);

				XmlNode versionNode = doc.SelectSingleNode("/manifest/version/@value");

				if (versionNode == null)
				{
					return VersionInfo.Null;
				}

				XmlNode summaryNode = doc.SelectSingleNode("/manifest/version/summary");

				if (summaryNode == null)
				{
					return new VersionInfo(versionNode.Value, null);
				}

				return new VersionInfo(versionNode.Value, summaryNode.InnerText);
			}
			catch
			{
				return VersionInfo.Null;
			}
		}

		#endregion Methods
	}
}