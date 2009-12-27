<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:template name="ExecuteParameters" match="/">
<xsl:variable name="methodName" select="./@name" />
<xsl:variable name="sqlParameters" select="./parameters/parameter" />
<xsl:variable name="sqlParametersCount" select="count($sqlParameters)" />
<xsl:variable name="inputSqlParameters" select="./parameters/parameter[@direction='Input']" />
<xsl:variable name="inputSqlParametersCount" select="count($inputSqlParameters)" />
				<xsl:if test="$inputSqlParametersCount&gt;0">
				Before_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters">ref <xsl:value-of select="script:GetCamelCase(./@name)" /><xsl:if test="position()!=last()">, </xsl:if></xsl:for-each>);
				</xsl:if>
				<xsl:if test="$sqlParametersCount!=0">
				global::System.Data.SqlClient.SqlParameter[] parms = new global::System.Data.SqlClient.SqlParameter[]
				{<xsl:for-each select="$sqlParameters">
					new global::System.Data.SqlClient.SqlParameter("<xsl:value-of select="./@sqlName" />", global::System.Data.SqlDbType.<xsl:value-of select="./@sqlDbType" /><xsl:if test="./@size!='0'">, <xsl:value-of select="./@size" /></xsl:if>)<xsl:if test="not(position()=last())">,</xsl:if></xsl:for-each>
				};
				<xsl:for-each select="$sqlParameters"><xsl:choose><xsl:when test="./@direction='Input'">
				parms[<xsl:value-of select="position()-1" />].Value = <xsl:value-of select="script:GetCamelCase(./@name)" />;</xsl:when><xsl:otherwise>
				parms[<xsl:value-of select="position()-1" />].Direction = global::System.Data.ParameterDirection.<xsl:value-of select="./@direction" />;</xsl:otherwise></xsl:choose></xsl:for-each><xsl:text>
				</xsl:text></xsl:if>
</xsl:template>
</xsl:stylesheet>