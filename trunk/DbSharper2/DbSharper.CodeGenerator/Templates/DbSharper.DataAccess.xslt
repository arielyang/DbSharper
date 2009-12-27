<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteNonQueryWithoutCache.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteNonQueryForEntity.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteNonQuery.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteReader.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:param name="namespace" />
<xsl:variable name="connectionStringName" select="/mapping/@connectionStringName" />
<xsl:template match="/">using System;

using DbSharper.Library.Caching;
using DbSharper.Library.Data;
using DbSharper.Library.Model;

using <xsl:value-of select="$defaultNamespace" />.Enums;
using <xsl:value-of select="$defaultNamespace" />.Models;
<xsl:for-each select="/mapping/models/namespace">using <xsl:value-of select="$defaultNamespace" />.Models.<xsl:value-of select="./@name"/>;
</xsl:for-each>
namespace <xsl:value-of select="$defaultNamespace" />.DataAccess.<xsl:value-of select="$namespace" />
{<xsl:for-each select="/mapping/dataAccesses/namespace[@name=$namespace]/dataAccess">
<xsl:variable name="currentName" select="./@name" />
	#region <xsl:value-of select="$currentName" />

	/// &lt;summary&gt;
	/// Data access methods of <xsl:value-of select="$currentName" />.
	/// &lt;/summary&gt;
	public static partial class <xsl:value-of select="$currentName" />
	{
		#region Cache keys of methods.
		<xsl:for-each select="./method">
		/// &lt;summary&gt;
		/// Cache key of method <xsl:value-of select="./@name" />.
		/// &lt;/summary&gt;
		public const string <xsl:value-of select="$currentName" />_<xsl:value-of select="./@name" /> = "<xsl:value-of select="$connectionStringName" />.<xsl:value-of select="$namespace" />.<xsl:value-of select="$currentName" />.<xsl:value-of select="./@name" />";
		</xsl:for-each>
		#endregion<xsl:for-each select="./method">

		#region Method <xsl:value-of select="./@name" />
		<xsl:if test="count(./results/result)&gt;1">

		/// &lt;summary&gt;
		/// Results of method <xsl:value-of select="./@name" />.
		/// &lt;/summary&gt;
		[Serializable]
		public partial class <xsl:value-of select="./@name" />Results : IJson
		{<xsl:for-each select="./results/result">
			public <xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="./@name" /> { get; set; }</xsl:for-each>

			/// &lt;summary&gt;
			/// Get JSON string of this result.
			/// &lt;/summary&gt;
			/// &lt;returns&gt;JSON string.&lt;/returns&gt;
			public string ToJson()
			{
				JsonBuilder jb = new JsonBuilder(this);
				<xsl:for-each select="./results/result">
				jb.Append("<xsl:value-of select="./@name" />", this.<xsl:value-of select="./@name" />);</xsl:for-each>

				return jb.ToString();
			}

			/// &lt;summary&gt;
			/// Return name of this class.
			/// &lt;/summary&gt;
			public override string ToString()
			{
				return "<xsl:value-of select="./@name" />Results";
			}
		}</xsl:if><xsl:choose>
				<xsl:when test="./@methodType='ExecuteNonQuery' and count(/mapping/models/namespace/model[@name=$currentName])&gt;0 and (./@name='Create' or ./@name='Update')">
					<xsl:call-template name="ExecuteNonQueryForEntity" />
				</xsl:when>
				<xsl:when test="./@methodType='ExecuteNonQuery' and (count(./results/result)=0 or not(script:StartsWith(./@name, 'Get')))">
					<xsl:call-template name="ExecuteNonQueryWithoutCache" />
				</xsl:when>
				<xsl:when test="./@methodType='ExecuteNonQuery'">
					<xsl:call-template name="ExecuteNonQuery" />
				</xsl:when>
				<xsl:when test="./@methodType='ExecuteReader'">
					<xsl:call-template name="ExecuteReader" />
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	}

	#endregion
</xsl:for-each>}
</xsl:template>
</xsl:stylesheet>