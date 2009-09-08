<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
<xsl:import href="DbSharper.DataAccess.ExecuteHeader.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteParameters.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteOutputParameters.xslt" />
<xsl:template name="ExecuteNonQueryWithoutCache" match="/">
<xsl:variable name="methodName" select="./@name" />
<xsl:variable name="sqlParameters" select="./parameters/parameter" />
<xsl:variable name="sqlParametersCount" select="count($sqlParameters)" />
<xsl:variable name="inputSqlParameters" select="./parameters/parameter[@direction='Input']" />
<xsl:variable name="resultsCount" select="count(./results/result)" />
<xsl:variable name="resultType" select="script:CSharpAlias(./results/result[1]/@type)" />
<xsl:variable name="resultClass" select="concat(./@name, 'Results')" />
<xsl:call-template name="ExecuteHeader" />
		{<xsl:call-template name="ExecuteParameters" />
				SqlHelper.ExecuteNonQuery(ConnectionStrings.<xsl:value-of select="/mapping/@connectionStringName" />, global::System.Data.CommandType.<xsl:value-of select="./@commandType" />, "<xsl:value-of select="./@commandText" />"<xsl:if test="$sqlParametersCount!=0">, parms</xsl:if>);
				
				<xsl:choose>
				<xsl:when test="$resultsCount=1">
				<xsl:value-of select="$resultType" /> result;
				</xsl:when>
				<xsl:when test="$resultsCount&gt;1">
				<xsl:value-of select="$resultClass" /> result = new <xsl:value-of select="$resultClass" />();
				</xsl:when>
				</xsl:choose>
				<xsl:call-template name="ExecuteOutputParameters" />
	
				After_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters"><xsl:value-of select="script:GetCamelCase(./@name)" /><xsl:if test="position()!=last()">, </xsl:if></xsl:for-each><xsl:if test="$resultsCount&gt;0">, result</xsl:if>);
		<xsl:if test="$resultsCount&gt;0">
				return result;
		</xsl:if>}

		#endregion</xsl:template>
</xsl:stylesheet>