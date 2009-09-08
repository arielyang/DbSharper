namespace DbSharper.CodeGenerator
{
    using System.Net;
    using System.Reflection;
    using System.Xml;
	using System;

    internal class UpdateService
    {
        #region Methods

		public static string GetExecutingVersion()
        {
			Version version = Assembly.GetExecutingAssembly().GetName().Version;

			return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        public string[] GetLatestVersionInfo()
        {
            string version;
            string summary;

            WebClient webClient = new WebClient();

			webClient.Headers.Add("User-Agent", "DbSharperCodeGenerator/" + GetExecutingVersion());

            try
            {
                string manifest = webClient.DownloadString("http://www.dbsharper.com/Service/GetVersion.ashx");

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(manifest);

                XmlNode versionNode = doc.SelectSingleNode("/manifest/version/@value");

                if (versionNode == null)
                {
                    return new string[] { string.Empty, string.Empty };
                }
                else
                {
                    version = versionNode.Value;
                }

                XmlNode summaryNode = doc.SelectSingleNode("/manifest/version/summary");

                if (summaryNode == null)
                {
                    summary = null;
                }
                else
                {
                    summary = summaryNode.InnerText;
                }

                return new string[] { version, summary };
            }
            catch
            {
                return new string[] { null, null };
            }
        }

        #endregion Methods
    }
}