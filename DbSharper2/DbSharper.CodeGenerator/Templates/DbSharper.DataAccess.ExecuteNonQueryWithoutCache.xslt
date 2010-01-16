<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.DataAccess.ExecuteHeader.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteParameters.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteOutputParameters.xslt" />
<xsl:template name="ExecuteNonQueryWithoutCache" match="/">
<xsl:variable name="inputSqlParameters" select="parameters/parameter[@direction='Input']" />
<xsl:call-template name="ExecuteHeader" />
		{<xsl:call-template name="ExecuteParameters" />
				this.db.ExecuteNonQuery(_dbCommand);

				After_<xsl:value-of select="@name" />(<xsl:for-each select="$inputSqlParameters"><xsl:value-of select="@camelCaseName" /><xsl:if test="position()!=last()">, </xsl:if></xsl:for-each>);
		}

		#endregion</xsl:template>
</xsl:stylesheet>