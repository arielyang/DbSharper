<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:param name="defaultNamespace" />
<xsl:template name="ExecuteParameters" match="/">
<xsl:variable name="sqlParameters" select="parameters/parameter" />
<xsl:variable name="sqlParametersCount" select="count($sqlParameters)" />
<xsl:variable name="inputSqlParameters" select="parameters/parameter[@direction='Input']" />
<xsl:variable name="inputSqlParametersCount" select="count($inputSqlParameters)" />
				<xsl:if test="$inputSqlParametersCount&gt;0">
				Before_<xsl:value-of select="@name" />(<xsl:for-each select="$inputSqlParameters">ref <xsl:value-of select="@camelCaseName" /><xsl:if test="position()!=last()">, </xsl:if></xsl:for-each>);
				</xsl:if>
				global::System.Data.Common.DbCommand _dbCommand = this.db.<xsl:choose>
					<xsl:when test="@commandType='StoredProcedure'">GetStoredProcCommand</xsl:when>
					<xsl:when test="@commandType='Text'">GetSqlStringCommand</xsl:when>
				</xsl:choose>("<xsl:value-of select="@commandText" />");
				
				<xsl:for-each select="$sqlParameters">
				<xsl:choose>
				<xsl:when test="@direction='Input'">this.db.AddInParameter(_dbCommand, "<xsl:value-of select="@sqlName" />", global::System.Data.DbType.<xsl:value-of select="@dbType" />, <xsl:value-of select="@camelCaseName" />);
				</xsl:when>
				<xsl:when test="@direction='InputOutput' or @direction='Output'">this.db.AddOutParameter(_dbCommand, "<xsl:value-of select="@sqlName" />", global::System.Data.DbType.<xsl:value-of select="@dbType" />, <xsl:value-of select="@size" />);
				</xsl:when>
				<xsl:otherwise>this.db.AddParameter(_dbCommand, "<xsl:value-of select="@sqlName" />", global::System.Data.DbType.<xsl:value-of select="@dbType" />, <xsl:value-of select="@size" />, global::System.Data.ParameterDirection.<xsl:value-of select="@direction" />);
				</xsl:otherwise>
				</xsl:choose>
				</xsl:for-each>
</xsl:template>
</xsl:stylesheet>