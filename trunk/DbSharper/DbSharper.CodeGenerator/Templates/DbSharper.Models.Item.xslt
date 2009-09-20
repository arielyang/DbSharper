<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
<xsl:template name="Item" match="/">
<xsl:variable name="properties" select="./property" />
		#region Fields
		<xsl:for-each select="$properties"><xsl:if test="./@name!=./@columnName">
		private <xsl:value-of select="script:CSharpAlias(./@type)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(./@columnName)" />;</xsl:if>
		private <xsl:value-of select="script:CSharpAlias(./@referenceType)" /><xsl:text> </xsl:text><xsl:value-of select="script:GetCamelCase(@name)" />;</xsl:for-each>

		#endregion

		#region Constructors

		/// &lt;summary&gt;
		/// Default constructor.
		/// &lt;/summary&gt;
		public <xsl:value-of select="./@name" />Item() : base() { }

		/// &lt;summary&gt;
		/// Constructor using IDataRecord.
		/// &lt;/summary&gt;
		public <xsl:value-of select="./@name" />Item(IDataRecord record) : base(record) { }

		#endregion

		#region Methods

		/// &lt;summary&gt;
		/// Load field value through IDataRecord.
		/// &lt;/summary&gt;
		/// &lt;param name="record"&gt;Data Record.&lt;/param&gt;
		/// &lt;param name="fieldName"&gt;Field name.&lt;/param&gt;
		/// &lt;param name="index"&gt;Index of field.&lt;/param&gt;
		public override void LoadField(IDataRecord record, string fieldName, int index)
		{
			switch (fieldName)
			{<xsl:for-each select="$properties">
				case "<xsl:value-of select="./@name" />":
					{<xsl:choose>
						<xsl:when test="script:EndsWith(./@referenceType,'Item')">
						string secondaryFieldName = GetSecondaryFieldName();

						if (secondaryFieldName != "<xsl:value-of select="script:GetId(./@column)" />")
						{
							if (<xsl:value-of select="script:GetCamelCase(@name)" /> == null)
							{
								<xsl:value-of select="script:GetCamelCase(@name)" /> = new <xsl:value-of select="./@referenceType" />();
								<xsl:value-of select="script:GetCamelCase(@name)" />.<xsl:value-of select="script:GetId(./@column)" /> = this.<xsl:value-of select="script:GetCamelCase(./@columnName)" />;
							}

							<xsl:value-of select="script:GetCamelCase(@name)" />.LoadField(record, secondaryFieldName, index);
						}
						else
						{
							if (!record.IsDBNull(index))
							{
								this.<xsl:value-of select="script:GetCamelCase(./@columnName)" /> = record.Get<xsl:value-of select="./@type" />(index);

								if (<xsl:value-of select="script:GetCamelCase(@name)" /> != null)
								{
									<xsl:value-of select="script:GetCamelCase(@name)" />.<xsl:value-of select="script:GetId(./@column)" /> = this.<xsl:value-of select="script:GetCamelCase(./@columnName)" />;
								}
							}
						}
						</xsl:when>
						<xsl:when test="./@referenceType!=./@type">
						this.<xsl:value-of select="script:GetCamelCase(@name)" /> = (<xsl:value-of select="./@referenceType" />)record.Get<xsl:value-of select="./@type" />(index);
						</xsl:when>
						<xsl:when test="./@type='Byte[]'">
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="script:GetCamelCase(@name)" /> = new byte[(int)record.GetBytes(index, 0, null, 0, 0)];

							record.GetBytes(index, 0, this.<xsl:value-of select="script:GetCamelCase(@name)" />, 0, <xsl:value-of select="script:GetCamelCase(@name)" />.Length);
						}
						</xsl:when>
						<xsl:when test="./@type='Single'">
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="script:GetCamelCase(@name)" /> = record.GetFloat(index);
						}
						</xsl:when>
						<xsl:when test="./@type='Char'">
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="script:GetCamelCase(@name)" /> = record.GetString(index)[0];
						}
						</xsl:when>
						<xsl:otherwise>
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="script:GetCamelCase(@name)" /> = record.Get<xsl:value-of select="./@type" />(index);
						}
						</xsl:otherwise>
						</xsl:choose>
						return;
					}</xsl:for-each>
				default:
					{
						AddValue(fieldName, record.GetValue(index));

						return;
					}
			}
		}
		
		/// &lt;summary&gt;
		/// Get value of property by property name.
		/// &lt;/summary&gt;
		/// &lt;param name="propertyName"&gt;Property name.&lt;/param&gt;
		/// &lt;return&gt;Value of property.&lt;/return&gt;
		public override object GetPropertyValue(string propertyName)
		{
			switch (propertyName)
			{<xsl:for-each select="$properties">
				case "<xsl:value-of select="./@columnName" />":
					return this.<xsl:value-of select="script:GetCamelCase(./@columnName)" />;</xsl:for-each>
				default:
					return null;
			}
		}

		/// &lt;summary&gt;
		/// Creates a deep copy of the <xsl:value-of select="./@name" />Item.
		/// &lt;/summary&gt;
		public <xsl:value-of select="./@name" />Item Clone()
		{
			<xsl:value-of select="./@name" />Item item = new <xsl:value-of select="./@name" />Item()
			{
				<xsl:for-each select="$properties"><xsl:if test="./@name!=./@columnName">
				<xsl:value-of select="./@name" /> = this.<xsl:value-of select="script:GetCamelCase(@name)" /> == null ? null : this.<xsl:value-of select="script:GetCamelCase(@name)" />.Clone(),
				</xsl:if>
				<xsl:value-of select="./@columnName" /> = this.<xsl:value-of select="script:GetCamelCase(./@columnName)" />
				<xsl:if test="position()!=last()">,
				</xsl:if></xsl:for-each>
			};
			
			return item;
		}

		/// &lt;summary&gt;
		/// Get JSON string of this item.
		/// &lt;/summary&gt;
		/// &lt;returns&gt;JSON string.&lt;/returns&gt;
		public string ToJson()
		{
			JsonBuilder jb = new JsonBuilder(this);
			<xsl:for-each select="$properties"><xsl:if test="./@name!=./@columnName">
			jb.Append("<xsl:value-of select="./@columnName" />", this.<xsl:value-of select="script:GetCamelCase(./@columnName)" />);</xsl:if>
			jb.Append("<xsl:value-of select="./@name" />", this.<xsl:value-of select="script:GetCamelCase(@name)" />);</xsl:for-each>
			
			if (this.ExtendedFields != null)
			{
				foreach(string fieldName in this.ExtendedFields.Keys)
				{
					jb.Append(fieldName, this.ExtendedFields[fieldName]);
				}
			}
			
			return jb.ToString();
		}

		/// &lt;summary&gt;
		/// Return name of this class.
		/// &lt;/summary&gt;
		public override string ToString()
		{
			return "<xsl:value-of select="./@name" />Item";
		}
		
		#endregion
		<xsl:if test="./@isView='false'">
		#region Extensibility Methods<xsl:for-each select="$properties">
				
		/// &lt;summary&gt;
		/// Invoked before <xsl:value-of select="./@columnName" /> changed.
		/// &lt;/summary&gt;
		partial void On<xsl:value-of select="./@columnName" />Changing(<xsl:choose>
			<xsl:when test="./@referenceType=concat('Enums.',./@name)"><xsl:value-of select="./@referenceType" /></xsl:when>
			<xsl:otherwise><xsl:value-of select="script:CSharpAlias(./@type)" /></xsl:otherwise>
		</xsl:choose> value);

		/// &lt;summary&gt;
		/// Invoked after <xsl:value-of select="./@columnName" /> changed.
		/// &lt;/summary&gt;
		partial void On<xsl:value-of select="./@columnName" />Changed();</xsl:for-each>
		
		#endregion
		</xsl:if>
		#region Properties<xsl:for-each select="$properties"><xsl:if test="./@name!=./@columnName">
		
		/// &lt;summary&gt;
		/// <xsl:value-of select="./@description" /><xsl:if test="./@description=''">Summary of <xsl:value-of select="./@name" />.</xsl:if>
		/// &lt;/summary&gt;<xsl:if test="./attributes!=''">
		[<xsl:value-of select="./attributes" />]</xsl:if>
		public <xsl:value-of select="script:CSharpAlias(./@referenceType)" /><xsl:text> </xsl:text><xsl:value-of select="./@name" />
		{
			get
			{
				return this.<xsl:value-of select="script:GetCamelCase(@name)" />;
			}

			set
			{
				<xsl:choose><xsl:when test="../@isView='false'">this.<xsl:value-of select="script:GetCamelCase(@name)" /> = value;</xsl:when>
				<xsl:otherwise>
				this.<xsl:value-of select="script:GetCamelCase(@name)" /> = value;</xsl:otherwise></xsl:choose>
			}
		}</xsl:if>
		
		/// &lt;summary&gt;
		/// <xsl:value-of select="./@description" /><xsl:if test="./@description=''">Summary of <xsl:value-of select="./@columnName" />.</xsl:if>
		/// &lt;/summary&gt;<xsl:if test="./attributes!=''">
		[<xsl:value-of select="./attributes" />]</xsl:if>
		public <xsl:choose>
			<xsl:when test="./@referenceType=concat('Enums.',./@name)"><xsl:value-of select="./@referenceType" /></xsl:when>
			<xsl:otherwise><xsl:value-of select="script:CSharpAlias(./@type)" /></xsl:otherwise>
		</xsl:choose><xsl:text> </xsl:text><xsl:value-of select="./@columnName" />
		{
			get
			{
				return this.<xsl:value-of select="script:GetCamelCase(./@columnName)" />;
			}

			set
			{
				<xsl:choose><xsl:when test="../@isView='false'">if (this.<xsl:value-of select="script:GetCamelCase(./@columnName)" /> != value)
				{
					On<xsl:value-of select="./@columnName" />Changing(value);
					
					this.<xsl:value-of select="script:GetCamelCase(./@columnName)" /> = value;
					this.SetPropertyChanged("<xsl:value-of select="./@columnName" />");
					
					On<xsl:value-of select="./@columnName" />Changed();
				}</xsl:when>
				<xsl:otherwise>
				this.<xsl:value-of select="script:GetCamelCase(./@columnName)" /> = value;</xsl:otherwise></xsl:choose>
			}
		}</xsl:for-each>
		
		#endregion</xsl:template>
</xsl:stylesheet>