<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteNonQueryWithoutCache.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteNonQueryForEntity.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteNonQuery.xslt" />
<xsl:import href="DbSharper.DataAccess.ExecuteReader.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:param name="schemaNamespace" />
<xsl:variable name="connectionStringName" select="/mapping/@connectionStringName" />
<xsl:template match="/">namespace <xsl:value-of select="$defaultNamespace" />.DataAccess.<xsl:value-of select="$schemaNamespace" />
{<xsl:for-each select="/mapping/dataAccesses/namespace[@name=$schemaNamespace]/dataAccess">
	<xsl:variable name="dataAccessName" select="@name" />
	<xsl:variable name="model" select="/mapping/models/namespace/model[@name=$dataAccessName]" />
	#region I<xsl:value-of select="@name" /> Interface

	/// &lt;summary&gt;
	/// Data access interface of <xsl:value-of select="@name" />.
	/// &lt;/summary&gt;
	[global::System.ServiceModel.ServiceContract]
	public partial interface I<xsl:value-of select="@name" />
	{<xsl:for-each select="method">
		<xsl:variable name="resultsCount" select="count(results/result)" />
		<xsl:variable name="inputSqlParameters" select="parameters/parameter[@direction='Input']" />
		/// &lt;summary&gt;
<xsl:value-of select="script:GetSummaryComment(@description, 2)" />
<xsl:if test="@description=''">		/// Summary of <xsl:value-of select="@name" />.</xsl:if>
		/// &lt;/summary&gt;
		[global::System.ServiceModel.OperationContract]
		<xsl:choose>
		<xsl:when test="$resultsCount=0">void</xsl:when>
		<xsl:when test="$resultsCount=1"><xsl:value-of select="script:CSharpAlias(results/result[1]/@type)" /></xsl:when>
		<xsl:otherwise><xsl:value-of select="concat(../@name,'.',@name,'Results')" /></xsl:otherwise>
		</xsl:choose><xsl:text> </xsl:text>
		<xsl:value-of select="@name" />(<xsl:choose>
			<xsl:when test="@methodType='ExecuteNonQuery' and count(/mapping/models/namespace/model[@name=$dataAccessName])&gt;0 and (@name='Create' or @name='Update')">Models.<xsl:value-of select="$model/../@name" />.<xsl:value-of select="$model/@name" />Model model<xsl:for-each select="$inputSqlParameters">
				<xsl:variable name="parameterName" select="@name" />
				<xsl:if test="not(boolean($model/property[@columnName=$parameterName]))">, <xsl:value-of select="script:CSharpAlias(@type)" /><xsl:text> </xsl:text><xsl:value-of select="@camelCaseName" />
				</xsl:if>
			</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="parameters/parameter[@direction='Input']">
					<xsl:value-of select="script:CSharpAlias(@type)" />
					<xsl:text> </xsl:text>
					<xsl:value-of select="@camelCaseName" />
					<xsl:if test="position()!=last()">, </xsl:if>
				</xsl:for-each>
			</xsl:otherwise>
			</xsl:choose>);
	</xsl:for-each>}

	#endregion

	#region <xsl:value-of select="@name" /> Class

	/// &lt;summary&gt;
	/// Data access methods of <xsl:value-of select="@name" />.
	/// &lt;/summary&gt;
	public partial class <xsl:value-of select="@name" /> : global::DbSharper.Library.Data.DataAccess, I<xsl:value-of select="@name" />
	{
		#region Constructors
		
		public <xsl:value-of select="@name" />()
			: this(global::<xsl:value-of select="$defaultNamespace" />.DataAccess.ConnectionStrings.<xsl:value-of select="/mapping/@connectionStringName" />)
		{ }

		public <xsl:value-of select="@name" />(string connectionString)
		{
			this.db = global::DbSharper.Library.Data.DatabaseFactory.CreateDatabase&lt;<xsl:value-of select="/mapping/@databaseType" />&gt;(connectionString);
		}

		#endregion

		#region Cache keys of methods.
		<xsl:for-each select="method">
		/// &lt;summary&gt;
		/// Cache key of method <xsl:value-of select="@name" />.
		/// &lt;/summary&gt;
		internal const string <xsl:value-of select="../@name" />_<xsl:value-of select="@name" /> = "<xsl:value-of select="$connectionStringName" />.<xsl:value-of select="$schemaNamespace" />.<xsl:value-of select="../@name" />.<xsl:value-of select="@name" />";
		</xsl:for-each>
		#endregion<xsl:for-each select="method">

		#region Method <xsl:value-of select="@name" />
		<xsl:if test="count(results/result)&gt;1">

		/// &lt;summary&gt;
		/// Results of method <xsl:value-of select="@name" />.
		/// &lt;/summary&gt;
		[global::System.Serializable]
		[global::System.Runtime.Serialization.DataContract]
		public partial class <xsl:value-of select="@name" />Results : global::DbSharper.Library.Data.IJson
		{<xsl:for-each select="results/result">
			[global::System.Runtime.Serialization.DataMember]
			public <xsl:choose>
			<xsl:when test="boolean(@enumType)"><xsl:value-of select="@enumType" /></xsl:when>
			<xsl:otherwise><xsl:value-of select="script:CSharpAlias(@type)" /></xsl:otherwise>
			</xsl:choose>
			<xsl:text> </xsl:text><xsl:value-of select="@name" /> { get; set; }
		</xsl:for-each>
			/// &lt;summary&gt;
			/// Get JSON string of this result.
			/// &lt;/summary&gt;
			/// &lt;returns&gt;JSON string.&lt;/returns&gt;
			public string ToJson()
			{
				global::DbSharper.Library.Data.JsonBuilder jb = new global::DbSharper.Library.Data.JsonBuilder(this);
				<xsl:for-each select="results/result">
				jb.Append("<xsl:value-of select="@name" />", this.<xsl:value-of select="@name" />);</xsl:for-each>

				return jb.ToString();
			}

			/// &lt;summary&gt;
			/// Return name of this class.
			/// &lt;/summary&gt;
			public override string ToString()
			{
				return "<xsl:value-of select="@name" />Results";
			}
		}</xsl:if>
			<xsl:choose>
				<xsl:when test="@methodType='ExecuteNonQuery' and count(/mapping/models/namespace/model[@name=$dataAccessName])&gt;0 and (@name='Create' or @name='Update')">
					<xsl:call-template name="ExecuteNonQueryForEntity" />
				</xsl:when>
				<xsl:when test="@methodType='ExecuteNonQuery' and count(parameters/parameter[@direction='InputOutput' or @direction='Output'])=0">
					<xsl:call-template name="ExecuteNonQueryWithoutCache" />
				</xsl:when>
				<xsl:when test="@methodType='ExecuteNonQuery'">
					<xsl:call-template name="ExecuteNonQuery" />
				</xsl:when>
				<xsl:when test="@methodType='ExecuteReader'">
					<xsl:call-template name="ExecuteReader" />
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	}

	#endregion
</xsl:for-each>}
</xsl:template>
</xsl:stylesheet>