<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:import href="DbSharper.Models.Model.xslt" />
<xsl:import href="DbSharper.Models.Extensions.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:param name="schemaNamespace" />
<xsl:template match="/">namespace <xsl:value-of select="$defaultNamespace" />.Models.<xsl:value-of select="$schemaNamespace" />
{<xsl:for-each select="/mapping/models/namespace[@name=$schemaNamespace]/model">
	#region <xsl:value-of select="@name" /> Model

	/// &lt;summary&gt;
<xsl:value-of select="script:GetSummaryComment(@description, 1)" />
<xsl:if test="@description=''">	/// Business model used to model <xsl:value-of select="@name" />.</xsl:if>
	/// &lt;/summary&gt;
	[global::DbSharper.Library.Schema.<xsl:choose>
		<xsl:when test="@isView='true'">View</xsl:when>
		<xsl:otherwise>Table</xsl:otherwise>
	</xsl:choose>("<xsl:value-of select="@mappingName" />")]
	[global::System.Serializable]
	[global::System.Runtime.Serialization.DataContract]
	public partial class <xsl:value-of select="@name" />Model : global::DbSharper.Library.Data.ModelBase, global::DbSharper.Library.Data.IJson
	{<xsl:call-template name="Model" />
	}

	/// &lt;summary&gt;
	/// Extensions of List&lt;<xsl:value-of select="@name" />&gt; type.
	/// &lt;/summary&gt;
	public static partial class <xsl:value-of select="@name" />Extensions
	{<xsl:call-template name="Extensions" />
	}

	#endregion
</xsl:for-each>}
</xsl:template>
</xsl:stylesheet>