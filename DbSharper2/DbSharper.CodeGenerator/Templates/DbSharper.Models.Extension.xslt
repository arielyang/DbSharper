<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:template name="Extension" match="/">
<xsl:variable name="modelName" select="@name" />
<xsl:variable name="getItemBy" select="property[@canGetItemBy='true']" />
<xsl:variable name="getCollectionBy" select="property[@canGetCollectionBy='true' and @canGetItemBy='false']" />
		#region Fields
		<xsl:for-each select="$getCollectionBy">
		private Dictionary&lt;<xsl:value-of select="script:CSharpAlias(./@type)" />, <xsl:value-of select="$modelName" />Collection&gt; <xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer;</xsl:for-each>

		#endregion

		#region Constructors

		public <xsl:value-of select="$modelName" />Collection() : base() { }

		#endregion

		#region Methods
		<xsl:for-each select="$getItemBy">
		/// &lt;summary&gt;
		/// Get series of <xsl:value-of select="@columnName" /> string joinned by ",".
		/// &lt;/summary&gt;
		/// &lt;returns&gt;<xsl:value-of select="@columnName" /> string.&lt;/returns&gt;
		public string Get<xsl:value-of select="@columnName" />s()
		{
			return Get<xsl:value-of select="@columnName" />s(",");
		}

		/// &lt;summary&gt;
		/// Get series of <xsl:value-of select="@columnName" /> string joinned by separator.
		/// &lt;/summary&gt;
		/// &lt;param name="separator"&gt;Separator.&lt;/param&gt;
		/// &lt;returns&gt;<xsl:value-of select="@columnName" /> string.&lt;/returns&gt;
		public string Get<xsl:value-of select="@columnName" />s(string separator)
		{
			StringBuilder sb = new StringBuilder();

			int length = this.Count;

			for (int i = 0; i &lt; length; i++)
			{
				sb.Append(this[i].<xsl:value-of select="@columnName" />);
				sb.Append(separator);
			}

			if (length > 0)
			{
				sb.Length -= separator.Length;
			}

			return sb.ToString();
		}

		/// &lt;summary&gt;
		/// Get <xsl:value-of select="$modelName" /> item by <xsl:value-of select="script:GetCamelCase(./@columnName)" />.
		/// &lt;/summary&gt;
		/// &lt;param name="<xsl:value-of select="script:GetCamelCase(./@columnName)" />"&gt;<xsl:value-of select="@columnName" />.&lt;/param&gt;
		/// &lt;returns&gt;<xsl:value-of select="$modelName" /> item.&lt;/returns&gt;
		public <xsl:value-of select="$modelName" />Item GetBy<xsl:value-of select="@columnName" />(<xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@columnName)" />)
		{
			for (int i = 0, length = this.Count; i &lt; length; i++)
			{
				if (this[i].<xsl:value-of select="@columnName" /> == <xsl:value-of select="script:GetCamelCase(./@columnName)" />)
				{
					return this[i];
				}
			}

			return null;
		}</xsl:for-each><xsl:for-each select="$getCollectionBy">

		/// &lt;summary&gt;
		/// Get series of <xsl:value-of select="@columnName" /> string joinned by ",".
		/// &lt;/summary&gt;
		/// &lt;returns&gt;<xsl:value-of select="@columnName" /> string.&lt;/returns&gt;
		public string Get<xsl:value-of select="@columnName" />s()
		{
			return Get<xsl:value-of select="@columnName" />s(",");
		}

		/// &lt;summary&gt;
		/// Get series of <xsl:value-of select="@columnName" /> string joinned by separator.
		/// &lt;/summary&gt;
		/// &lt;param name="separator"&gt;Separator.&lt;/param&gt;
		/// &lt;returns&gt;<xsl:value-of select="@columnName" /> string.&lt;/returns&gt;
		public string Get<xsl:value-of select="@columnName" />s(string separator)
		{
			StringBuilder sb = new StringBuilder();

			int length = this.Count;

			for (int i = 0; i &lt; length; i++)
			{
				sb.Append(this[i].<xsl:value-of select="@columnName" />);
				sb.Append(separator);
			}

			if (length > 0)
			{
				sb.Length -= separator.Length;
			}

			return sb.ToString();
		}

		/// &lt;summary&gt;
		/// Get <xsl:value-of select="$modelName" /> items by <xsl:value-of select="script:GetCamelCase(./@columnName)" />.
		/// &lt;/summary&gt;
		/// &lt;param name="<xsl:value-of select="script:GetCamelCase(./@columnName)" />"&gt;<xsl:value-of select="@columnName" />.&lt;/param&gt;
		/// &lt;returns&gt;<xsl:value-of select="$modelName" /> items.&lt;/returns&gt;
		public <xsl:value-of select="$modelName" />Collection GetBy<xsl:value-of select="@columnName" />(<xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@columnName)" />)
		{
			if (<xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer == null || this.InnerListChanged)
			{
				<xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer = new Dictionary&lt;<xsl:value-of select="script:CSharpAlias(./@type)" />, <xsl:value-of select="$modelName" />Collection&gt;();

				<xsl:value-of select="$modelName" />Item item;

				for (int i = 0, length = this.Count; i &lt; length; i++)
				{
					item = this[i];

					if (!<xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer.ContainsKey(item.<xsl:value-of select="@columnName" />))
					{
						<xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer.Add(item.<xsl:value-of select="@columnName" />, new <xsl:value-of select="$modelName" />Collection());
					}

					<xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer[item.<xsl:value-of select="@columnName" />].Add(item);
				}

				this.InnerListChanged = false;
			}

			if (!<xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer.ContainsKey(<xsl:value-of select="script:GetCamelCase(./@columnName)" />))
			{
				return null;
			}

			return <xsl:value-of select="script:GetCamelCase(./@columnName)" />CollectionIndexer[<xsl:value-of select="script:GetCamelCase(./@columnName)" />];
		}</xsl:for-each>

		/// &lt;summary&gt;
		/// Creates a deep copy of the <xsl:value-of select="@name" />Collection.
		/// &lt;/summary&gt;
		public <xsl:value-of select="@name" />Collection Clone()
		{
			<xsl:value-of select="@name" />Collection col = new <xsl:value-of select="@name" />Collection();

			for (int i = 0, length = this.Count; i &lt; length; i++)
			{
				col.Add(this[i].Clone());
			}

			return col;
		}

		/// &lt;summary&gt;
		/// Get JSON string of this collection.
		/// &lt;/summary&gt;
		/// &lt;returns&gt;JSON string.&lt;/returns&gt;
		public string ToJson()
		{
			JsonBuilder jb = new JsonBuilder(this);

			return jb.ToString();
		}

		/// &lt;summary&gt;
		/// Return name of this class.
		/// &lt;/summary&gt;
		public override string ToString()
		{
			return "<xsl:value-of select="@name" />Collection";
		}

		#endregion</xsl:template>
</xsl:stylesheet>