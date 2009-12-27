<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:template name="ExecuteOutputParameters" match="/">
<xsl:variable name="sqlParameters" select="./parameters/parameter" />
<xsl:variable name="resultsCount" select="count(./results/result)" />
				<xsl:for-each select="$sqlParameters">
				<xsl:if test="./@direction!='Input'">
				result<xsl:if test="$resultsCount!=1">.<xsl:value-of select="./@name" /></xsl:if> = <xsl:choose>
				<xsl:when test="./@name='ReturnResult'">(ReturnResult)global::System.Convert.ToInt32(parms[<xsl:value-of select="position()-1" />].Value);</xsl:when>
				<xsl:when test="./@type='Guid'">new Guid(parms[<xsl:value-of select="position()-1" />].Value.ToString());</xsl:when>
				<xsl:when test="./@type='Single'">global::System.Convert.ToFloat(parms[<xsl:value-of select="position()-1" />].Value);</xsl:when>
				<xsl:otherwise>global::System.Convert.To<xsl:value-of select="./@type" />(parms[<xsl:value-of select="position()-1" />].Value);</xsl:otherwise>
				</xsl:choose>
				</xsl:if>
				</xsl:for-each>
</xsl:template>
</xsl:stylesheet>