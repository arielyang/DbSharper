<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
<xsl:import href="DbSharper.DataAccess.ExecuteHeader.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteParameters.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteOutputParameters.xslt" />
<xsl:template name="ExecuteReader" match="/">
<xsl:variable name="cacheKey" select="concat(../@name,'_',./@name)" />
<xsl:variable name="methodName" select="./@name" />
<xsl:variable name="sqlParametersCount" select="count(./parameters/parameter)" />
<xsl:variable name="inputSqlParameters" select="./parameters/parameter[@direction='Input']" />
<xsl:variable name="inputSqlParametersCount" select="count(./parameters/parameter[@direction='Input'])" />
<xsl:variable name="results" select="./results/result[@isOutputParameter='false']" />
<xsl:variable name="resultsCount" select="count(./results/result)" />
<xsl:variable name="resultType" select="./results/result[1]/@type" />
<xsl:variable name="resultClass" select="concat($methodName, 'Results')" />
<xsl:call-template name="ExecuteHeader" />
		{
			CachingService cache = new CachingService(<xsl:value-of select="$cacheKey" /><xsl:for-each select="$inputSqlParameters">, <xsl:value-of select="script:GetCamelCase(./@name)" />
				<xsl:choose>
					<xsl:when test="./@type='DateTime'">.ToShortDateString()</xsl:when>
					<xsl:when test="./@type!='String'">.ToString()</xsl:when>
				</xsl:choose>
			</xsl:for-each>);

			<xsl:choose>
				<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /> result = cache.Get() as <xsl:value-of select="$resultType" />;</xsl:when>
				<xsl:otherwise><xsl:value-of select="$resultClass" /> result = cache.Get() as <xsl:value-of select="$resultClass" />;</xsl:otherwise>
			</xsl:choose>

			if (result == null)
			{<xsl:call-template name="ExecuteParameters" />
				result = new <xsl:choose><xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when><xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /></xsl:when></xsl:choose>();

				using (global::System.Data.SqlClient.SqlDataReader reader = SqlHelper.ExecuteReader(connectionString, global::System.Data.CommandType.<xsl:value-of select="./@commandType" />, "<xsl:value-of select="./@commandText" />"<xsl:if test="$sqlParametersCount!=0">, parms</xsl:if>))
				{<xsl:for-each select="$results">
					<xsl:if test="position()!=1">

					reader.NextResult();
					</xsl:if>
					<xsl:choose>
					<xsl:when test="script:EndsWith(./@type,'Item')">
					if(reader.Read())
					{
						result<xsl:if test="$resultsCount&gt;1">.<xsl:value-of select="./@name" /></xsl:if> = new <xsl:value-of select="./@type" />(reader);
					}
					else
					{
						result<xsl:if test="$resultsCount&gt;1">.<xsl:value-of select="./@name" /></xsl:if> = null;
					}</xsl:when>
					<xsl:when test="script:EndsWith(./@type,'Collection')">
					result<xsl:if test="$resultsCount&gt;1">.<xsl:value-of select="./@name" /></xsl:if> = new <xsl:value-of select="./@type" />();
					
					while (reader.Read())
					{
						result<xsl:if test="$resultsCount&gt;1">.<xsl:value-of select="./@name" /></xsl:if>.Add(new <xsl:value-of select="script:GetItemType(./@type)" />(reader));
					}</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				}
				<xsl:call-template name="ExecuteOutputParameters" />
				After_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters"><xsl:value-of select="script:GetCamelCase(./@name)" /><xsl:if test="position()!=last()">, </xsl:if></xsl:for-each><xsl:if test="$inputSqlParametersCount&gt;0 and $resultsCount&gt;0">, </xsl:if>result as <xsl:choose>
					<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when>
					<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /></xsl:when>
				</xsl:choose>);

				cache.Insert(result);
			}
			
			return result;
		}
		
		/// &lt;summary&gt;
		/// Remove cache of method <xsl:value-of select="$methodName" />.
		/// &lt;/summary&gt;
		public static void Remove_<xsl:value-of select="$methodName" />()
		{
			CachingService cache = new CachingService(<xsl:value-of select="$cacheKey" />);
			
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
		public static void Remove_<xsl:value-of select="$methodName" />(<xsl:for-each select="$inputSqlParameters">
				<xsl:if test="position()&lt;=$cacheKeyPosition">
					<xsl:value-of select="script:CSharpAlias(./@type)" />
					<xsl:text> </xsl:text>
					<xsl:value-of select="script:GetCamelCase(./@name)" />
					<xsl:if test="position()!=$cacheKeyPosition">, </xsl:if>
				</xsl:if>
			</xsl:for-each>)
		{
			CachingService cache = new CachingService(<xsl:value-of select="$cacheKey" /><xsl:for-each select="$inputSqlParameters">
				<xsl:if test="position()&lt;=$cacheKeyPosition">, <xsl:value-of select="script:GetCamelCase(./@name)" />
				<xsl:choose>
					<xsl:when test="./@type='DateTime'">.ToShortDateString()</xsl:when>
					<xsl:when test="./@type!='String'">.ToString()</xsl:when>
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