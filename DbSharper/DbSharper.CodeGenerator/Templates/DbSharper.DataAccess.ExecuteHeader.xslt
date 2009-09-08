<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
<xsl:template name="ExecuteHeader" match="/">
<xsl:variable name="methodName" select="./@name" />
<xsl:variable name="results" select="./results/result" />
<xsl:variable name="resultsCount" select="count($results)" />
<xsl:variable name="resultClass" select="concat($methodName, 'Results')" />
<xsl:variable name="resultType" select="script:CSharpAlias($results[1]/@type)" />
<xsl:variable name="inputSqlParameters" select="./parameters/parameter[@direction='Input']" />
<xsl:variable name="inputSqlParametersCount" select="count($inputSqlParameters)" />
		<xsl:if test="$inputSqlParametersCount&gt;0">

		/// &lt;summary&gt;
		/// Invoked before executing method <xsl:value-of select="$methodName" />.
		/// &lt;/summary&gt;
		static partial void Before_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters">ref <xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@name)" /><xsl:if test="position()!=last()">, </xsl:if></xsl:for-each>);
		</xsl:if>
		/// &lt;summary&gt;
		/// Invoked after executed method <xsl:value-of select="$methodName" />.
		/// &lt;/summary&gt;
		static partial void After_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters"><xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@name)" /><xsl:if test="position()!=last()">, </xsl:if></xsl:for-each>
		<xsl:if test="$inputSqlParametersCount&gt;0 and $resultsCount&gt;0">, </xsl:if><xsl:choose>
			<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /> result</xsl:when>
			<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /> result</xsl:when>
		</xsl:choose>);

		/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(./@description, 2)" />
<xsl:if test="./@description=''">		/// Summary of <xsl:value-of select="$methodName" />.</xsl:if>
		/// &lt;/summary&gt;
		public static <xsl:choose>
		<xsl:when test="$resultsCount=0">void</xsl:when>
		<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when>
		<xsl:otherwise><xsl:value-of select="$resultClass" /></xsl:otherwise>
		</xsl:choose><xsl:text> </xsl:text>
		<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters">
		<xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@name)" />
		<xsl:if test="position()!=last()">, </xsl:if></xsl:for-each>)</xsl:template>
</xsl:stylesheet>