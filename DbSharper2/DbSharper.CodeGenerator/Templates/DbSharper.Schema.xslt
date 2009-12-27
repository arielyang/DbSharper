<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:template match="/">/*using System.Collections.Generic;
using DbSharper.Library.Data;

namespace <xsl:value-of select="$defaultNamespace" />.Schema
{
	#region Database Schema Entity

	/// &lt;summary&gt;
	/// Schema of database.
	/// &lt;/summary&gt;
	public static partial class DbSchema
	{
		private static Dictionary&lt;string, TableSchema&gt; tableSchemas = new Dictionary&lt;string,TableSchema&gt;();

		static DbSchema()
		{<xsl:for-each select="/dbMapping/models/entity">
			tableSchemas.Add("<xsl:value-of select="./@name" />", <xsl:value-of select="./@name" />);</xsl:for-each>
		}

		/// &lt;summary&gt;
		/// Get table schema by table name.
		/// &lt;/summary&gt;
		/// &lt;param name="name">Table name.&lt;/param&gt;
		/// &lt;returns>Table schema.&lt;/returns&gt;
		public static TableSchema GetTableSchema(string tableName)
		{
			return tableSchemas[tableName];
		}
		<xsl:for-each select="/dbMapping/models/entity">
		/// &lt;summary&gt;
		/// Schema of table <xsl:value-of select="./@name" />.
		/// &lt;/summary&gt;
		public static readonly <xsl:value-of select="./@name" />Schema <xsl:value-of select="./@name" /> = new <xsl:value-of select="./@name" />Schema("<xsl:value-of select="./@schema" />", "<xsl:value-of select="./@name" />");
	</xsl:for-each>}
	
	#endregion

	#region Table Schema Entities
	<xsl:for-each select="/dbMapping/models/entity">
	/// &lt;summary&gt;
	/// Schema entity of table <xsl:value-of select="./@name" />.
	/// &lt;/summary&gt;
	public class <xsl:value-of select="./@name" />Schema : TableSchema
	{<xsl:for-each select="./property">
		/// &lt;summary&gt;
		/// Schema of column <xsl:value-of select="./@column" />.
		/// &lt;/summary&gt;
		public readonly ColumnSchema <xsl:value-of select="./@column" /> = new ColumnSchema("<xsl:value-of select="./@column" />"<xsl:if test="script:EndsWith(./@referenceType,'Item')">, "<xsl:value-of select="script:RemoveItemPostfix(./@referenceType)"/>", "Id"</xsl:if>);
		</xsl:for-each>

		/// &lt;summary&gt;
		/// Constructor.
		/// &lt;/summary&gt;
		/// <param name="schema">Schema of table <xsl:value-of select="./@name" />.</param>
		/// <param name="name">Name of table <xsl:value-of select="./@name" />.</param>
		public <xsl:value-of select="./@name" />Schema(string schema, string name) : base(schema, name) { }

		/// &lt;summary&gt;
		/// Get all columns of table <xsl:value-of select="./@name" />.
		/// &lt;/summary&gt;
		/// <returns>Column Array.</returns>
		public override ColumnSchema[] GetColumns()
		{
			return new ColumnSchema[]
				{
					<xsl:for-each select="./property"><xsl:value-of select="./@column" /><xsl:if test="position()!=last()">,
					</xsl:if></xsl:for-each>
				};
		}
	}
	</xsl:for-each>
	#endregion
}*/
</xsl:template>
</xsl:stylesheet>