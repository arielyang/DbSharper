<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:import href="DbSharper.Models.Model.xslt" />
<xsl:import href="DbSharper.Models.Extension.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:param name="schemaNamespace" />
<xsl:template match="/">using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

using DbSharper.Library.Data;

namespace <xsl:value-of select="$defaultNamespace" />.Models.<xsl:value-of select="$schemaNamespace" />
{<xsl:for-each select="/mapping/models/namespace[@name=$schemaNamespace]/model">
	#region Model <xsl:value-of select="@name" />

	/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(./@description, 1)" />
<xsl:if test="@description=''">	/// Business model used to model <xsl:value-of select="@name" />.</xsl:if>
	/// &lt;/summary&gt;
	[Serializable]
	[DataContract]
	public partial class <xsl:value-of select="@name" />Model : ModelBase, IJson
	{<xsl:call-template name="Model" />
	}

	/// &lt;summary&gt;
	/// Extensions of List&lt;<xsl:value-of select="@name" />&lt; type.
	/// &lt;/summary&gt;
	public static partial class <xsl:value-of select="@name" />Extensions
	{
	}

	#endregion
</xsl:for-each>}
</xsl:template>
</xsl:stylesheet>