<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
<xsl:import href="DbSharper.Scripts.xslt" />
<xsl:import href="DbSharper.Models.Item.xslt" />
<xsl:import href="DbSharper.Models.Collection.xslt" />
<xsl:output omit-xml-declaration="yes" method="text" />
<xsl:param name="defaultNamespace" />
<xsl:param name="namespace" />
<xsl:template match="/">using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DbSharper.Library.Data;
using DbSharper.Library.Model;

using <xsl:value-of select="$defaultNamespace" />.Enums;
<xsl:for-each select="/mapping/models/namespace">using <xsl:value-of select="$defaultNamespace" />.Models.<xsl:value-of select="./@name"/>;
</xsl:for-each>
namespace <xsl:value-of select="$defaultNamespace" />.Models.<xsl:value-of select="$namespace" />
{<xsl:for-each select="/mapping/models/namespace[@name=$namespace]/model">
	#region Model <xsl:value-of select="./@name" />

	/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(./@description, 1)" />
<xsl:if test="./@description=''">	/// Business model used to model <xsl:value-of select="./@name" /> item.</xsl:if>
	/// &lt;/summary&gt;
	[Serializable]
	public partial class <xsl:value-of select="./@name" />Item : ItemBase, IJson
	{<xsl:call-template name="Item" />
	}

	/// &lt;summary&gt;
<xsl:value-of select="script:GetSummary(./@description, 1)" />
<xsl:if test="./@description=''">	/// Business model collection used to model <xsl:value-of select="./@name" /> items.</xsl:if>
	/// &lt;/summary&gt;
	[Serializable]
	public partial class <xsl:value-of select="./@name" />Collection : CollectionBase&lt;Models.<xsl:value-of select="./@schema" />.<xsl:value-of select="./@name" />Item&gt;, IJson
	{<xsl:call-template name="Collection" />
	}

	#endregion
</xsl:for-each>}
</xsl:template>
</xsl:stylesheet>