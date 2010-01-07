<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:template name="ExecuteOutputParameters" match="/">
<xsl:variable name="sqlParameters" select="parameters/parameter" />
<xsl:variable name="resultsCount" select="count(results/result)" />
				<xsl:for-each select="$sqlParameters">
				<xsl:if test="@direction!='Input'">
				result<xsl:if test="$resultsCount!=1">.<xsl:value-of select="@name" /></xsl:if> = <xsl:choose>
				<xsl:when test="@name='ReturnResult'">(ReturnResult)System.Convert.ToInt32(_db.GetParameterValue(_dbCommand, "<xsl:value-of select="@sqlName" />"));</xsl:when>
				<xsl:when test="@type='Guid'">new Guid(_db.GetParameterValue(_dbCommand, "<xsl:value-of select="@sqlName" />").ToString());</xsl:when>
				<xsl:when test="@type='Single'">System.Convert.ToFloat(_db.GetParameterValue(_dbCommand, "<xsl:value-of select="@sqlName" />"));</xsl:when>
				<xsl:otherwise>System.Convert.To<xsl:value-of select="@type" />(_db.GetParameterValue(_dbCommand, "<xsl:value-of select="@sqlName" />"));</xsl:otherwise>
				</xsl:choose>
				</xsl:if>
				</xsl:for-each>
</xsl:template>
</xsl:stylesheet>