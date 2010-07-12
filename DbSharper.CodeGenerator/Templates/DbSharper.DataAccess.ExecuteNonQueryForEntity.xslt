<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
<xsl:import href="DbSharper.DataAccess.ExecuteOutputParameters.xslt" />
<xsl:template name="ExecuteNonQueryForEntity" match="/">
<xsl:variable name="methodName" select="./@name" />
<xsl:variable name="dataAccessName" select="../@name" />
<xsl:variable name="model" select="/mapping/models/namespace/model[@name=$dataAccessName]" />
<xsl:variable name="results" select="./results/result" />
<xsl:variable name="sqlParameters" select="./parameters/parameter" />
<xsl:variable name="sqlParametersCount" select="count(./parameters/parameter)" />
<xsl:variable name="inputSqlParameters" select="./parameters/parameter[@direction='Input']" />
<xsl:variable name="inputSqlParametersCount" select="count($inputSqlParameters)" />
<xsl:variable name="outputSqlParametersCount" select="count(./parameters/parameter[@direction!='Input'])" />
<xsl:variable name="resultsCount" select="count(./results/result)" />
<xsl:variable name="resultType" select="script:CSharpAlias(./results/result[1]/@type)" />
<xsl:variable name="resultClass" select="concat(./@name, 'Results')" />
		<xsl:if test="$inputSqlParametersCount&gt;0">

		/// &lt;summary&gt;
		/// Invoked before executing method <xsl:value-of select="$methodName" />.
		/// &lt;/summary&gt;
		static partial void Before_<xsl:value-of select="$methodName" />(ref Models.<xsl:value-of select="$model/@schema" />.<xsl:value-of select="$model/@name" />Item item<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="./@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, ref <xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@name)" />
			</xsl:if>
		</xsl:for-each>);
		</xsl:if>
		/// &lt;summary&gt;
		/// Invoked after executed method <xsl:value-of select="$methodName" />.
		/// &lt;/summary&gt;
		static partial void After_<xsl:value-of select="$methodName" />(Models.<xsl:value-of select="$model/@schema" />.<xsl:value-of select="$model/@name" />Item item<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="./@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@name)" />
			</xsl:if>
		</xsl:for-each>
		<xsl:choose>
			<xsl:when test="$resultsCount=1">, <xsl:value-of select="$resultType" /> result</xsl:when>
			<xsl:when test="$resultsCount&gt;1">, <xsl:value-of select="$resultClass" /> result</xsl:when>
		</xsl:choose>);

		/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(./@description, 2)" />
<xsl:if test="./@description=''">		/// Summary of <xsl:value-of select="$methodName" />.</xsl:if>
		/// &lt;/summary&gt;
		public static <xsl:choose>
			<xsl:when test="$resultsCount=0">void</xsl:when>
			<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when>
			<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /></xsl:when>
		</xsl:choose><xsl:text> </xsl:text><xsl:value-of select="$methodName" />(Models.<xsl:value-of select="$model/@schema" />.<xsl:value-of select="$model/@name" />Item item<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="./@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@name)" />
			</xsl:if>
		</xsl:for-each>)
		{
			<xsl:if test="$resultsCount&gt;0">return </xsl:if><xsl:value-of select="$methodName" />(item<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="./@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:GetCamelCase(./@name)" /></xsl:if>
			</xsl:for-each>, ConnectionStrings.<xsl:value-of select="/mapping/@connectionStringName" />);
		}
		
		/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(./@description, 2)" />
<xsl:if test="./@description=''">		/// Summary of <xsl:value-of select="$methodName" />.</xsl:if>
		/// &lt;/summary&gt;
		public static <xsl:choose>
			<xsl:when test="$resultsCount=0">void</xsl:when>
			<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when>
			<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /></xsl:when>
		</xsl:choose><xsl:text> </xsl:text><xsl:value-of select="$methodName" />(Models.<xsl:value-of select="$model/@schema" />.<xsl:value-of select="$model/@name" />Item item<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="./@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@name)" />
			</xsl:if>
		</xsl:for-each>, string connectionString)
		{
			Before_<xsl:value-of select="$methodName" />(ref item<xsl:for-each select="$inputSqlParameters">
				<xsl:variable name="parameterName" select="./@name" />
				<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, ref <xsl:value-of select="script:GetCamelCase(./@name)" />
				</xsl:if>
			</xsl:for-each>);
			<xsl:if test="$sqlParametersCount!=0">
			global::System.Data.SqlClient.SqlParameter[] parms = new global::System.Data.SqlClient.SqlParameter[]
			{<xsl:for-each select="$sqlParameters">
				new global::System.Data.SqlClient.SqlParameter("<xsl:value-of select="./@sqlName" />", global::System.Data.SqlDbType.<xsl:value-of select="./@sqlDbType" /><xsl:if test="./@size!='0'">, <xsl:value-of select="./@size" /></xsl:if>)<xsl:if test="not(position()=last())">,</xsl:if></xsl:for-each>
			};
			<xsl:for-each select="$sqlParameters">
			<xsl:variable name="parameterName" select="./@name" />
			<xsl:choose><xsl:when test="./@direction='Input'">
			parms[<xsl:value-of select="position()-1" />].Value = <xsl:choose>
				<xsl:when test="boolean($model/property[@columnName=$parameterName])">item.<xsl:value-of select="./@name" /></xsl:when>
				<xsl:otherwise><xsl:value-of select="script:GetCamelCase(./@name)" /></xsl:otherwise>
			</xsl:choose>;</xsl:when><xsl:otherwise>
			parms[<xsl:value-of select="position()-1" />].Direction = global::System.Data.ParameterDirection.<xsl:value-of select="./@direction" />;</xsl:otherwise></xsl:choose></xsl:for-each><xsl:text>
			</xsl:text></xsl:if>
	SqlHelper.ExecuteNonQuery(connectionString, global::System.Data.CommandType.<xsl:value-of select="./@commandType" />, "<xsl:value-of select="./@commandText" />"<xsl:if test="$sqlParametersCount!=0">, parms</xsl:if>);
			<xsl:if test="$resultsCount&gt;1"><xsl:text>
			</xsl:text>
			<xsl:value-of select="$resultClass" /> result = new <xsl:value-of select="$resultClass" />();
			</xsl:if>
			<xsl:if test="$resultsCount=1"><xsl:text>
			</xsl:text>
			<xsl:value-of select="$resultType" /> result;
			</xsl:if>
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
			
			After_<xsl:value-of select="$methodName" />(item<xsl:for-each select="$inputSqlParameters">
				<xsl:variable name="parameterName" select="./@name" />
				<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:GetCamelCase(./@name)" />
				</xsl:if>
			</xsl:for-each>
			<xsl:if test="$resultsCount&gt;0">, result</xsl:if>);<xsl:if test="$resultsCount&gt;0">
			
			return result;</xsl:if>
		}

		#endregion</xsl:template>
</xsl:stylesheet>
