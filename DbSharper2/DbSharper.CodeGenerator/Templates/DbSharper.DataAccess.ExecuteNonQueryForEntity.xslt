<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.DataAccess.ExecuteOutputParameters.xslt" />
<xsl:template name="ExecuteNonQueryForEntity" match="/">
<xsl:variable name="dataAccessName" select="../@name" />
<xsl:variable name="model" select="/mapping/models/namespace/model[@name=$dataAccessName]" />
<xsl:variable name="sqlParameters" select="parameters/parameter" />
<xsl:variable name="sqlParametersCount" select="count(parameters/parameter)" />
<xsl:variable name="inputSqlParameters" select="parameters/parameter[@direction='Input']" />
<xsl:variable name="inputSqlParametersCount" select="count($inputSqlParameters)" />
<xsl:variable name="outputSqlParametersCount" select="count(parameters/parameter[@direction!='Input'])" />
<xsl:variable name="resultsCount" select="count(results/result)" />
<xsl:variable name="resultType" select="script:CSharpAlias(results/result[1]/@type)" />
<xsl:variable name="resultClass" select="concat(@name, 'Results')" />
		<xsl:if test="$inputSqlParametersCount&gt;0 or (@name='Update' and @commandType='Text')">

		/// &lt;summary&gt;
		/// Invoked before executing method <xsl:value-of select="@name" />.
		/// &lt;/summary&gt;
		partial void Before_<xsl:value-of select="@name" />(ref Models.<xsl:value-of select="$model/../@name" />.<xsl:value-of select="$model/@name" />Model model<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, ref <xsl:value-of select="script:CSharpAlias(@type)" /><xsl:text> </xsl:text><xsl:value-of select="@camelCaseName" />
			</xsl:if>
		</xsl:for-each>);
		</xsl:if>
		/// &lt;summary&gt;
		/// Invoked after executed method <xsl:value-of select="@name" />.
		/// &lt;/summary&gt;
		partial void After_<xsl:value-of select="@name" />(Models.<xsl:value-of select="$model/../@name" />.<xsl:value-of select="$model/@name" />Model model<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:CSharpAlias(@type)" /><xsl:text> </xsl:text><xsl:value-of select="@camelCaseName" />
			</xsl:if>
		</xsl:for-each>
		<xsl:choose>
			<xsl:when test="$resultsCount=1">, <xsl:value-of select="$resultType" /> result</xsl:when>
			<xsl:when test="$resultsCount&gt;1">, <xsl:value-of select="$resultClass" /> result</xsl:when>
		</xsl:choose>);

		/// &lt;summary&gt;
<xsl:value-of select="script:GetSummaryComment(@description, 2)" />
<xsl:if test="@description=''">		/// Summary of <xsl:value-of select="@name" />.</xsl:if>
		/// &lt;/summary&gt;
		public <xsl:choose>
			<xsl:when test="$resultsCount=0">void</xsl:when>
			<xsl:when test="$resultsCount=1"><xsl:value-of select="$resultType" /></xsl:when>
			<xsl:when test="$resultsCount&gt;1"><xsl:value-of select="$resultClass" /></xsl:when>
		</xsl:choose><xsl:text> </xsl:text><xsl:value-of select="@name" />(Models.<xsl:value-of select="$model/../@name" />.<xsl:value-of select="$model/@name" />Model model<xsl:for-each select="$inputSqlParameters">
			<xsl:variable name="parameterName" select="@name" />
			<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:CSharpAlias(@type)" /><xsl:text> </xsl:text><xsl:value-of select="@camelCaseName" />
			</xsl:if>
		</xsl:for-each>)
		{
			Before_<xsl:value-of select="@name" />(ref model<xsl:for-each select="$inputSqlParameters">
				<xsl:variable name="parameterName" select="@name" />
				<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, ref <xsl:value-of select="@camelCaseName" />
				</xsl:if>
			</xsl:for-each>);
			
			<xsl:choose>
			<xsl:when test="@name='Update' and @commandType='Text'">
			global::DbSharper.Library.Data.DataAccessHelper.Update&lt;Models.<xsl:value-of select="$model/../@name" />.<xsl:value-of select="$model/@name" />Model&gt;(this.db, model);
			</xsl:when>
			<xsl:otherwise>
			global::System.Data.Common.DbCommand _dbCommand = this.db.<xsl:choose>
				<xsl:when test="@commandType='StoredProcedure'">GetStoredProcCommand</xsl:when>
				<xsl:when test="@commandType='Text'">GetSqlStringCommand</xsl:when>
			</xsl:choose>("<xsl:value-of select="@commandText" />");

			<xsl:for-each select="$sqlParameters">
			<xsl:variable name="parameterName" select="@name" />
			<xsl:choose>
			<xsl:when test="@direction='Input'">this.db.AddInParameter(_dbCommand, "<xsl:value-of select="@sqlName" />", System.Data.DbType.<xsl:value-of select="@dbType" />, <xsl:choose><xsl:when test="boolean($model/property[@columnName=$parameterName])">model.<xsl:value-of select="@name" /></xsl:when><xsl:otherwise><xsl:value-of select="@camelCaseName" /></xsl:otherwise></xsl:choose>);
			</xsl:when>
			<xsl:when test="@direction='InputOutput' or @direction='Output'">this.db.AddOutParameter(_dbCommand, "<xsl:value-of select="@sqlName" />", global::System.Data.DbType.<xsl:value-of select="@dbType" />, <xsl:value-of select="@size" />);
			</xsl:when>
			<xsl:otherwise>this.db.AddParameter(_dbCommand, "<xsl:value-of select="@sqlName" />", global::System.Data.DbType.<xsl:value-of select="@dbType" />, <xsl:value-of select="@size" />, global::System.Data.ParameterDirection.<xsl:value-of select="@direction" />);
			</xsl:otherwise>
			</xsl:choose>
			</xsl:for-each>
			this.db.ExecuteNonQuery(_dbCommand);
			<xsl:if test="$resultsCount&gt;1"><xsl:text>
			</xsl:text>
			<xsl:value-of select="$resultClass" /> result = new <xsl:value-of select="$resultClass" />();
			</xsl:if>
			<xsl:if test="$resultsCount=1"><xsl:text>
			</xsl:text>
			<xsl:value-of select="$resultType" /> result;
			</xsl:if>
			<xsl:for-each select="$sqlParameters">
			<xsl:if test="@direction!='Input'">
			result<xsl:if test="$resultsCount!=1">.<xsl:value-of select="@name" /></xsl:if> = <xsl:choose>
			<xsl:when test="@type='Guid'">new global::System.Guid(this.db.GetParameterValue(_dbCommand, "<xsl:value-of select="@sqlName" />").ToString());</xsl:when>
			<xsl:when test="@type='Single'">global::System.Convert.ToFloat(this.db.GetParameterValue(_dbCommand, "<xsl:value-of select="@sqlName" />"));</xsl:when>
			<xsl:otherwise>global::System.Convert.To<xsl:value-of select="@type" />(this.db.GetParameterValue(_dbCommand, "<xsl:value-of select="@sqlName" />"));</xsl:otherwise>
			</xsl:choose>
			</xsl:if>
			</xsl:for-each>
			</xsl:otherwise>
			</xsl:choose>

			After_<xsl:value-of select="@name" />(model<xsl:for-each select="$inputSqlParameters">
				<xsl:variable name="parameterName" select="@name" />
				<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="@camelCaseName" />
				</xsl:if>
			</xsl:for-each>
			<xsl:if test="$resultsCount&gt;0">, result</xsl:if>);<xsl:if test="$resultsCount&gt;0">

			return result;</xsl:if>
		}

		#endregion</xsl:template>
</xsl:stylesheet>
