<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output omit-xml-declaration="yes" method="xml" />
<xsl:variable name="connectionStringName" select="/mapping/@connectionStringName" />
<xsl:template match="/">
<configuration><xsl:text>
	</xsl:text>
	<configSections><xsl:text>
		</xsl:text>
		<sectionGroup name="dbSharper" type="DbSharper.Library.Configuration.ConfigurationSectionGroup, DbSharper.Library"><xsl:text>
			</xsl:text>
			<sectionGroup name="cachingService" type="DbSharper.Library.Configuration.CachingServiceGroup, DbSharper.Library"><xsl:text>
				</xsl:text>
				<section name="providers" type="DbSharper.Library.Configuration.CacheProvidersSection, DbSharper.Library" /><xsl:text>
				</xsl:text>
				<section name="cacheSettings" type="DbSharper.Library.Configuration.CacheSettingsSection, DbSharper.Library" /><xsl:text>
			</xsl:text>
			</sectionGroup><xsl:text>
		</xsl:text>
		</sectionGroup><xsl:text>
	</xsl:text>
	</configSections><xsl:text>
	</xsl:text>
	<dbSharper><xsl:text>
		</xsl:text>
		<cachingService><xsl:text>
			</xsl:text>
			<providers defaultProvider="InProcessCacheProvider"><xsl:text>
				</xsl:text>
				<add name="InProcessCacheProvider" type="DbSharper.Library.Providers.InProcessCacheProvider, DbSharper.Library" /><xsl:text>
				</xsl:text>
				<add name="MemcachedCacheProvider" type="DbSharper.Library.Providers.MemcachedCacheProvider, DbSharper.Library" /><xsl:text>
			</xsl:text>
			</providers><xsl:text>
			</xsl:text>
			<cacheSettings enabled="true" defaultDuration="60">
				<xsl:text disable-output-escaping="yes">
				&lt;!--
				Examples:
				</xsl:text>
				<method key="[Schmea].[Class_Method]" enable="true" duration="60" provider="InProcessCacheProvider" /><xsl:text>
				</xsl:text>
				<method key="[Schmea].[Class_Method]" enable="false" />
				<xsl:text disable-output-escaping="yes">
				--&gt;</xsl:text>
				<xsl:for-each select="//method"><xsl:text>
				</xsl:text>
				<add>
					<xsl:attribute name="name"><xsl:value-of select="$connectionStringName" />.<xsl:value-of select="../@schema" />.<xsl:value-of select="../@name" />.<xsl:value-of select="./@name" /></xsl:attribute>
					<xsl:attribute name="enabled">true</xsl:attribute>
					<xsl:attribute name="duration">60</xsl:attribute>
				</add>
				</xsl:for-each><xsl:text>
			</xsl:text>
			</cacheSettings><xsl:text>
		</xsl:text>
		</cachingService><xsl:text>
	</xsl:text>
	</dbSharper><xsl:text>
</xsl:text>
</configuration>
</xsl:template>
</xsl:stylesheet>