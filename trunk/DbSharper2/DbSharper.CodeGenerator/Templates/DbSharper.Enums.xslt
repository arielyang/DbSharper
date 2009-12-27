<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:template match="/">using System.ComponentModel;

namespace <xsl:value-of select="$defaultNamespace" />.Enums
{<xsl:for-each select="/mapping/database/enumerations/enumeration">
	#region Enum <xsl:value-of select="@name" />

	/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(@description, 1)" />
<xsl:if test="@description=''">	/// Summary of enum <xsl:value-of select="@name" />.</xsl:if>
	/// &lt;/summary&gt;<xsl:if test="@hasFlagsAttribute='true'">
	[Flags]</xsl:if>
	public enum <xsl:value-of select="@name" /> : <xsl:value-of select="script:CSharpAlias(@baseType)" />
	{<xsl:for-each select="member">
		/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(@description, 2)" />
<xsl:if test="@description=''">		/// Summary of enum member <xsl:value-of select="@name" />.</xsl:if>
		/// &lt;/summary&gt;
		[Description("<xsl:value-of select="@description" />")]
		<xsl:value-of select="@name" /> = <xsl:value-of select="@value" /><xsl:if test="position()!=last()">,<xsl:text></xsl:text></xsl:if><xsl:text>
	</xsl:text>
	</xsl:for-each>}
	
	#endregion
</xsl:for-each>}
</xsl:template>
</xsl:stylesheet>