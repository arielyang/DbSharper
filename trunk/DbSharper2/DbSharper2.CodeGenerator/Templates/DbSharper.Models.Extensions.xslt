<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:template name="Extensions" match="/">
		#region Methods
		
		/// &lt;summary&gt;
		/// Creates a deep copy of the <xsl:value-of select="@name" /> list.
		/// &lt;/summary&gt;
		public static global::System.Collections.Generic.List&lt;<xsl:value-of select="@name" />Model&gt; Clone(this global::System.Collections.Generic.List&lt;<xsl:value-of select="@name" />Model&gt; list)
		{
			var newList = new global::System.Collections.Generic.List&lt;<xsl:value-of select="@name" />Model&gt;();

			foreach (var model in list)
			{
				newList.Add(model.Clone());
			}

			return newList;
		}

		/// &lt;summary&gt;
		/// Get JSON string of this list.
		/// &lt;/summary&gt;
		/// &lt;returns&gt;JSON string.&lt;/returns&gt;
		public static string ToJson(this global::System.Collections.Generic.List&lt;<xsl:value-of select="@name" />Model&gt; list)
		{
			var jb = new global::DbSharper.Library.Data.JsonBuilder(list);

			return jb.ToString();
		}

		#endregion</xsl:template>
</xsl:stylesheet>