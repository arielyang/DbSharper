<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.DataAccess.ExecuteHeader.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteParameters.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteOutputParameters.xslt" />
<xsl:template name="ExecuteNonQuery" match="/">
<xsl:variable name="cacheKey" select="concat(../@name,'_',./@name)" />
<xsl:variable name="methodName" select="@name" />
<xsl:variable name="sqlParametersCount" select="count(./parameters/parameter)" />
<xsl:variable name="inputSqlParameters" select="parameters/parameter[@direction='Input']" />
<xsl:variable name="inputSqlParametersCount" select="count(./parameters/parameter[@direction='Input'])" />
<xsl:variable name="results" select="results/result" />
<xsl:variable name="resultsCount" select="count(./results/result)" />
<xsl:variable name="resultType" select="script:CSharpAlias(./results/result[1]/@type)" />
<xsl:variable name="resultClass" select="concat(./@name, 'Results')" />
<xsl:call-template name="ExecuteHeader" />
		{
			global::DbSharper.Library.Caching.CachingService cache = new global::DbSharper.Library.Caching.CachingService(<xsl:value-of select="$cacheKey" /><xsl:for-each select="$inputSqlParameters">, <xsl:value-of select="@camelCaseName" />
				<xsl:choose>
					<xsl:when test="@type='DateTime'">.ToShortDateString()</xsl:when>
					<xsl:when test="@type!='String'">.ToString()</xsl:when>
				</xsl:choose>
			</xsl:for-each>);

			<xsl:choose>
			<xsl:when test="$resultsCount=1">object result = cache.Get();</xsl:when>
			<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /> result = cache.Get() as <xsl:value-of select="$resultClass" />;</xsl:when>
		</xsl:choose>

			if (result == null)
			{<xsl:call-template name="ExecuteParameters" />
				this.db.ExecuteNonQuery(_dbCommand);
				<xsl:if test="$resultsCount&gt;1">
				result = new <xsl:value-of select="$resultClass" />();
				</xsl:if>
				<xsl:call-template name="ExecuteOutputParameters" />

				After_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters"><xsl:value-of select="@camelCaseName" />, </xsl:for-each>(<xsl:choose>
			<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when>
			<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /></xsl:when>
		</xsl:choose>)result);

				cache.Insert(result);
			}

			return (<xsl:choose>
			<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when>
			<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /></xsl:when>
		</xsl:choose>)result;
		}

		/// &lt;summary&gt;
		/// Remove cache of method <xsl:value-of select="$methodName" />.
		/// &lt;/summary&gt;
		internal void Remove_<xsl:value-of select="$methodName" />()
		{
			global::DbSharper.Library.Caching.CachingService cache = new global::DbSharper.Library.Caching.CachingService(<xsl:value-of select="$cacheKey" />);

			<xsl:choose>
				<xsl:when test="$inputSqlParametersCount=0">cache.Remove();</xsl:when>
				<xsl:otherwise>cache.RemoveBatch();</xsl:otherwise>
			</xsl:choose>
		}
		<xsl:for-each select="$inputSqlParameters">
		<xsl:variable name="cacheKeyPosition" select="position()" />
		/// &lt;summary&gt;
		/// Remove cache of method <xsl:value-of select="$methodName" />.
		/// &lt;/summary&gt;
		internal void Remove_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters">
				<xsl:if test="position()&lt;=$cacheKeyPosition">
					<xsl:value-of select="script:CSharpAlias(@type)" />
					<xsl:text> </xsl:text>
					<xsl:value-of select="@camelCaseName" />
					<xsl:if test="position()!=$cacheKeyPosition">, </xsl:if>
				</xsl:if>
			</xsl:for-each>)
		{
			global::DbSharper.Library.Caching.CachingService cache = new global::DbSharper.Library.Caching.CachingService(<xsl:value-of select="$cacheKey" /><xsl:for-each select="$inputSqlParameters">
				<xsl:if test="position()&lt;=$cacheKeyPosition">, <xsl:value-of select="@camelCaseName" />
				<xsl:choose>
					<xsl:when test="@type='DateTime'">.ToShortDateString()</xsl:when>
					<xsl:when test="@type!='String'">.ToString()</xsl:when>
				</xsl:choose>
				</xsl:if>
			</xsl:for-each>);

			<xsl:choose>
				<xsl:when test="$cacheKeyPosition=$inputSqlParametersCount">cache.Remove();</xsl:when>
				<xsl:otherwise>cache.RemoveBatch();</xsl:otherwise>
			</xsl:choose>
		}
		</xsl:for-each>
		#endregion</xsl:template>
</xsl:stylesheet>