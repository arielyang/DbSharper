<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:template name="Model" match="/">
		#region Fields
		<xsl:for-each select="property">
		private <xsl:choose>
			<xsl:when test="boolean(@enumType)"><xsl:value-of select="@enumType" /></xsl:when>
			<xsl:otherwise><xsl:value-of select="script:CSharpAlias(@type)" /></xsl:otherwise>
		</xsl:choose><xsl:text> </xsl:text><xsl:value-of select="@camelCaseName" />;</xsl:for-each>

		#endregion

		#region Constructors

		/// &lt;summary&gt;
		/// Default constructor.
		/// &lt;/summary&gt;
		public <xsl:value-of select="@name" />Model() : base() { }

		/// &lt;summary&gt;
		/// Constructor using IDataRecord.
		/// &lt;/summary&gt;
		internal <xsl:value-of select="@name" />Model(IDataRecord record) : base(record) { }

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
			{<xsl:for-each select="property">
				case "<xsl:value-of select="@name" />":
					{<xsl:choose>
						<xsl:when test="@isExtended='true'">
						<xsl:variable name="previousPosition" select="position()-1" />
						<xsl:variable name="previousProperty" select="../property[position()=$previousPosition]" />
						string secondaryFieldName = GetSecondaryFieldName();

						if (secondaryFieldName != "<xsl:value-of select="@refPkName" />")
						{
							if (<xsl:value-of select="@camelCaseName" /> == null)
							{
								<xsl:value-of select="@camelCaseName" /> = new <xsl:value-of select="@type" />();
								<xsl:value-of select="@camelCaseName" />.<xsl:value-of select="@refPkName" /> = this.<xsl:value-of select="$previousProperty/@camelCaseName" />;
							}

							<xsl:value-of select="@camelCaseName" />.LoadField(record, secondaryFieldName, index);
						}
						else
						{
							if (!record.IsDBNull(index))
							{
								this.<xsl:value-of select="$previousProperty/@camelCaseName" /> = record.Get<xsl:value-of select="$previousProperty/@type" />(index);

								if (<xsl:value-of select="@camelCaseName" /> != null)
								{
									<xsl:value-of select="@camelCaseName" />.<xsl:value-of select="@refPkName" /> = this.<xsl:value-of select="$previousProperty/@camelCaseName" />;
								}
							}
						}
						</xsl:when>
						<xsl:when test="boolean(@enumType)">
						this.<xsl:value-of select="@camelCaseName" /> = (<xsl:value-of select="@enumType" />)record.Get<xsl:value-of select="@type" />(index);
						</xsl:when>
						<xsl:when test="@type='Byte[]'">
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="@camelCaseName" /> = new byte[(int)record.GetBytes(index, 0, null, 0, 0)];

							record.GetBytes(index, 0, this.<xsl:value-of select="@camelCaseName" />, 0, <xsl:value-of select="@camelCaseName" />.Length);
						}
						</xsl:when>
						<xsl:when test="@type='Single'">
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="@camelCaseName" /> = record.GetFloat(index);
						}
						</xsl:when>
						<xsl:when test="@type='Char'">
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="@camelCaseName" /> = record.GetString(index)[0];
						}
						</xsl:when>
						<xsl:otherwise>
						if (!record.IsDBNull(index))
						{
							this.<xsl:value-of select="@camelCaseName" /> = record.Get<xsl:value-of select="@type" />(index);
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
			{<xsl:for-each select="property[@isExtended='false']">
				case "<xsl:value-of select="@name" />":
					return this.<xsl:value-of select="@camelCaseName" />;</xsl:for-each>
				default:
					throw new ArgumentOutOfRangeException("propertyName");
			}
		}

		/// &lt;summary&gt;
		/// Creates a deep copy of the <xsl:value-of select="@name" />Model.
		/// &lt;/summary&gt;
		public <xsl:value-of select="@name" />Model Clone()
		{
			<xsl:value-of select="@name" />Model model = new <xsl:value-of select="@name" />Model()
			{
				<xsl:for-each select="property">
				<xsl:value-of select="@name" /> = this.<xsl:value-of select="@camelCaseName" /><xsl:if test="@isExtended='true'"> ?? this.<xsl:value-of select="@camelCaseName" />.Clone()</xsl:if>
				<xsl:if test="position()!=last()">,
				</xsl:if></xsl:for-each>
			};

			return model;
		}

		/// &lt;summary&gt;
		/// Get JSON string of this item.
		/// &lt;/summary&gt;
		/// &lt;returns&gt;JSON string.&lt;/returns&gt;
		public string ToJson()
		{
			JsonBuilder jb = new JsonBuilder(this);
			<xsl:for-each select="property">
			jb.Append("<xsl:value-of select="@name" />", this.<xsl:value-of select="@camelCaseName" />);</xsl:for-each>

			if (this.ExtendedFields != null)
			{
				foreach (string fieldName in this.ExtendedFields.Keys)
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
			return "<xsl:value-of select="@name" />Model";
		}
		
		#endregion
		<xsl:if test="@isView='false'">
		#region Extensibility Methods<xsl:for-each select="property[@isExtended='false']">

		/// &lt;summary&gt;
		/// Invoked before <xsl:value-of select="@name" /> changed.
		/// &lt;/summary&gt;
		partial void On<xsl:value-of select="@name" />Changing(<xsl:choose>
			<xsl:when test="boolean(@enumType)"><xsl:value-of select="@enumType" /></xsl:when>
			<xsl:otherwise><xsl:value-of select="script:CSharpAlias(@type)" /></xsl:otherwise>
		</xsl:choose> value);

		/// &lt;summary&gt;
		/// Invoked after <xsl:value-of select="@name" /> changed.
		/// &lt;/summary&gt;
		partial void On<xsl:value-of select="@name" />Changed();</xsl:for-each>

		#endregion
		</xsl:if>
		#region Properties<xsl:for-each select="property">

		/// &lt;summary&gt;
		/// <xsl:value-of select="@description" /><xsl:if test="@description=''">Summary of <xsl:value-of select="@name" />.</xsl:if>
		/// &lt;/summary&gt;
		[DataMember]
		public <xsl:choose>
			<xsl:when test="boolean(@enumType)"><xsl:value-of select="@enumType" /></xsl:when>
			<xsl:otherwise><xsl:value-of select="script:CSharpAlias(@type)" /></xsl:otherwise>
		</xsl:choose><xsl:text> </xsl:text><xsl:value-of select="@name" />
		{
			get
			{
				return this.<xsl:value-of select="@camelCaseName" />;
			}

			set
			{<xsl:choose><xsl:when test="../@isView='false' and @isExtended='false'">
				if (this.<xsl:value-of select="@camelCaseName" /> != value)
				{
					On<xsl:value-of select="@name" />Changing(value);

					this.<xsl:value-of select="@camelCaseName" /> = value;
					this.SetPropertyChanged("<xsl:value-of select="@name" />");

					On<xsl:value-of select="@name" />Changed();
				}</xsl:when>
				<xsl:otherwise>
				this.<xsl:value-of select="@camelCaseName" /> = value;</xsl:otherwise></xsl:choose>
			}
		}</xsl:for-each>

		#endregion</xsl:template>
</xsl:stylesheet>