<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:variable name="connectionStringName" select="/mapping/@connectionStringName" />
<xsl:variable name="camelCaseConnectionStringName" select="script:GetCamelCase($connectionStringName)" />
<xsl:template match="/">namespace <xsl:value-of select="$defaultNamespace" />.DataAccess
{
	public static partial class ConnectionStrings
	{
		#region Connection String "<xsl:value-of select="$connectionStringName" />"

		private static string <xsl:value-of select="$camelCaseConnectionStringName" /> = string.Empty;

		/// &lt;summary&gt;
		/// Value of connection string "<xsl:value-of select="$connectionStringName" />".
		/// &lt;/summary&gt;
		public static string <xsl:value-of select="$connectionStringName" />
		{
			get
			{
				if (<xsl:value-of select="$camelCaseConnectionStringName" />.Length == 0)
				{
					<xsl:value-of select="$camelCaseConnectionStringName" /> = global::System.Configuration.ConfigurationManager.ConnectionStrings["<xsl:value-of select="$connectionStringName" />"].ConnectionString;
				}

				return <xsl:value-of select="$camelCaseConnectionStringName" />;
			}
		}

		#endregion
	}
}
</xsl:template>
</xsl:stylesheet>