<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:script="urn:my-scripts">
	<xsl:import href="DbSharper.Scripts.xslt" />
	<xsl:output omit-xml-declaration="yes" method="html" />
	<xsl:variable name="tables" select="/mapping/database/tables/table" />
	<xsl:variable name="tablesCount" select="ceiling(count($tables) div 4)" />
	<xsl:variable name="views" select="/mapping/database/views/view" />
	<xsl:variable name="viewsCount" select="ceiling(count($views) div 4)" />
	<xsl:variable name="storedProcedures" select="/mapping/database/storedProcedures/storedProcedure" />
	<xsl:variable name="storedProceduresCount" select="ceiling(count($storedProcedures) div 2)" />
	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<title>Database Design Documents</title>
				<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
				<style type="text/css">
					body { margin:30px; }
					p { font:12px Verdana; }
					a { font-weight:normal; text-decoration:none; }
					a:hover { font-weight:normal; text-decoration:underline; }
					ul { margin:0px 10px 50px 10px; }
					li { line-height:1.5; font:12px Verdana; margin:2px; float:left; }
					table { border-width:0; border-collapse:collapse; font:12px Verdana; width:100%; margin:30px 0; }
					th { border-width:0; }
					td { border:dashed 1px #AAA; color:#222; border-width:1px 0; }
					.tables { margin-top:0; }
					.tables td { border-width:0; }
					.storedprocedures { margin-top:0; }
					.storedprocedures td { border-width:0; }
					.tabletitle { font:bolder 16px Verdana; }
					.tabledesc { border-width:0; border-top:dashed 1px #999; color:DimGray; padding:5px; }
					.specialchar { font:12px Wingdings; }
					.tableheader { background-color:#CDF; font-weight:bold; }
					.tableheader td { border:solid 1px #AAA; border-width:1px 0; }
					.subtable { margin:0; }
					.subtable td { border-width:0; }
					.default { color:Green; }
					.keyword { color:Blue; }
					.description { color:Gray; }
					.schema { color:Green; }
					.name { color:Blue; }
				</style>
			</head>
			<body>
				<a name="_DocumentTop">
					<span></span>
				</a>
				<h1>
					Database Design Document - <i>
						Database <xsl:value-of select="/mapping/@connectionStringName" />
					</i>
				</h1>
				<hr />
				<h2>Tables</h2>
				<table class="tables" cellpadding="1">
					<xsl:for-each select="$tables">
						<xsl:if test="position() &lt;= $tablesCount">
							<xsl:variable name="t0" select="position() + $tablesCount" />
							<xsl:variable name="t1" select="position() + $tablesCount * 2" />
							<xsl:variable name="t2" select="position() + $tablesCount * 3" />
							<tr>
								<td>
									<a>
										<xsl:attribute name="href">
											<xsl:value-of select="script:GetAnchor(./@schema,./@name)" />
										</xsl:attribute>
										<xsl:attribute name="title">
											<xsl:value-of select="./@description" />
										</xsl:attribute>
										<span class="schema">
											<xsl:value-of select="./@schema" />
										</span>.<span class="name">
											<xsl:value-of select="./@name" />
										</span>
									</a>
								</td>
								<td>
									<a>
										<xsl:attribute name="href">
											<xsl:value-of select="script:GetAnchor($tables[$t0]/@schema,$tables[$t0]/@name)" />
										</xsl:attribute>
										<xsl:attribute name="title">
											<xsl:value-of select="$tables[$t0]/@description" />
										</xsl:attribute>
										<span class="schema">
											<xsl:value-of select="$tables[$t0]/@schema" />
										</span>.<span class="name">
											<xsl:value-of select="$tables[$t0]/@name" />
										</span>
									</a>
								</td>
								<td>
									<a>
										<xsl:attribute name="href">
											<xsl:value-of select="script:GetAnchor($tables[$t1]/@schema,$tables[$t1]/@name)" />
										</xsl:attribute>
										<xsl:attribute name="title">
											<xsl:value-of select="$tables[$t1]/@description" />
										</xsl:attribute>
										<span class="schema">
											<xsl:value-of select="$tables[$t1]/@schema" />
										</span>.<span class="name">
											<xsl:value-of select="$tables[$t1]/@name" />
										</span>
									</a>
								</td>
								<td>
									<xsl:if test="$t2 &lt;= count($tables)">
										<a>
											<xsl:attribute name="href">
												<xsl:value-of select="script:GetAnchor($tables[$t2]/@schema,$tables[$t2]/@name)" />
											</xsl:attribute>
											<xsl:attribute name="title">
												<xsl:value-of select="$tables[$t2]/@description" />
											</xsl:attribute>
											<span class="schema">
												<xsl:value-of select="$tables[$t2]/@schema" />
											</span>.<span class="name">
												<xsl:value-of select="$tables[$t2]/@name" />
											</span>
										</a>
									</xsl:if>
								</td>
							</tr>
						</xsl:if>
					</xsl:for-each>
				</table>
				<hr />
				<h2>Views</h2>
				<table class="tables" cellpadding="1">
					<xsl:for-each select="$views">
						<xsl:if test="position() &lt;= $viewsCount">
							<xsl:variable name="t0" select="position() + $viewsCount" />
							<xsl:variable name="t1" select="position() + $viewsCount * 2" />
							<xsl:variable name="t2" select="position() + $viewsCount * 3" />
							<tr>
								<td>
									<a>
										<xsl:attribute name="href">
											<xsl:value-of select="script:GetAnchor(./@schema,./@name)" />
										</xsl:attribute>
										<xsl:attribute name="title">
											<xsl:value-of select="./@description" />
										</xsl:attribute>
										<span class="schema">
											<xsl:value-of select="./@schema" />
										</span>.<span class="name">
											<xsl:value-of select="./@name" />
										</span>
									</a>
								</td>
								<td>
									<xsl:if test="$views[$t0]">
										<a>
											<xsl:attribute name="href">
												<xsl:value-of select="script:GetAnchor($tables[$t0]/@schema,$tables[$t0]/@name)" />
											</xsl:attribute>
											<xsl:attribute name="title">
												<xsl:value-of select="$views[$t0]/@description" />
											</xsl:attribute>
											<span class="schema">
												<xsl:value-of select="$views[$t0]/@schema" />
											</span>.<span class="name">
												<xsl:value-of select="$views[$t0]/@name" />
											</span>
										</a>
									</xsl:if>
								</td>
								<td>
									<xsl:if test="$views[$t1]">
										<a>
											<xsl:attribute name="href">
												<xsl:value-of select="script:GetAnchor($tables[$t1]/@schema,$tables[$t1]/@name)" />
											</xsl:attribute>
											<xsl:attribute name="title">
												<xsl:value-of select="$views[$t1]/@description" />
											</xsl:attribute>
											<span class="schema">
												<xsl:value-of select="$views[$t1]/@schema" />
											</span>.<span class="name">
												<xsl:value-of select="$views[$t1]/@name" />
											</span>
										</a>
									</xsl:if>
								</td>
								<td>
									<xsl:if test="$views[$t2]">
										<a>
											<xsl:attribute name="href">
												<xsl:value-of select="script:GetAnchor($tables[$t2]/@schema,$tables[$t2]/@name)" />
											</xsl:attribute>
											<xsl:attribute name="title">
												<xsl:value-of select="$views[$t2]/@description" />
											</xsl:attribute>
											<span class="schema">
												<xsl:value-of select="$views[$t2]/@schema" />
											</span>.<span class="name">
												<xsl:value-of select="$views[$t2]/@name" />
											</span>
										</a>
									</xsl:if>
								</td>
							</tr>
						</xsl:if>
					</xsl:for-each>
				</table>
				<hr />
				<a name="_StoredProcedureTop">
					<span></span>
				</a>
				<h2>Stored Procedures</h2>
				<table class="storedprocedures" cellpadding="1">
					<xsl:for-each select="$storedProcedures">
						<xsl:if test="position() &lt;= $storedProceduresCount">
							<xsl:variable name="p" select="position() + $storedProceduresCount" />
							<tr>
								<td>
									<a>
										<xsl:attribute name="href">
											<xsl:value-of select="script:GetAnchor(./@schema,./@name)" />
										</xsl:attribute>
										<xsl:attribute name="title">
											<xsl:value-of select="./@description" />
										</xsl:attribute>
										<span class="schema">
											<xsl:value-of select="./@schema" />
										</span>.<span class="name">
											<xsl:value-of select="./@name" />
										</span>
									</a>
								</td>
								<td>
									<xsl:if test="$storedProcedures[$p]">
										<a>
											<xsl:attribute name="href">
												<xsl:value-of select="script:GetAnchor($storedProcedures[$p]/@schema,$storedProcedures[$p]/@name)" />
											</xsl:attribute>
											<xsl:attribute name="title">
												<xsl:value-of select="$storedProcedures[$p]/@description" />
											</xsl:attribute>
											<span class="schema">
												<xsl:value-of select="$storedProcedures[$p]/@schema" />
											</span>.<span class="name">
												<xsl:value-of select="$storedProcedures[$p]/@name" />
											</span>
										</a>
									</xsl:if>
								</td>
							</tr>
						</xsl:if>
					</xsl:for-each>
				</table>
				<hr />
				<h2>Tables Detail Information</h2>
				<xsl:for-each select="$tables">
					<a>
						<xsl:attribute name="name">
							<xsl:value-of select="./@schema" />.<xsl:value-of select="./@name" />
						</xsl:attribute>
						<span></span>
					</a>
					<table border="1" cellpadding="3">
						<tr>
							<th align="left" colspan="6" class="tabletitle">
								[<xsl:value-of select="./@schema" />].[<xsl:value-of select="./@name" />]
							</th>
							<th align="right">
								<span class="specialchar">é</span>
								<a href="#_DocumentTop">Back to top</a>
							</th>
						</tr>
						<tr>
							<td colspan="7" class="tabledesc">
								<xsl:value-of select="script:GetDescription(./@description)" disable-output-escaping="yes" />
							</td>
						</tr>
						<tr class="tableheader">
							<td width="120">Name</td>
							<td width="110">Data Type</td>
							<td width="30" align="center">Nulls</td>
							<td width="25" align="center">PK</td>
							<td width="25" align="center">FK</td>
							<td width="100" align="center">Default</td>
							<td>Description</td>
						</tr>
						<xsl:for-each select="./columns/column">
							<tr>
								<td>
									<xsl:value-of select="./@name" />
								</td>
								<td class="keyword">
									<xsl:value-of select="script:ToLower(./@sqlDbType)" />
									<xsl:choose>
										<xsl:when test="./@size=-1">(max)</xsl:when>
										<xsl:when test="./@size!=0">
											(<xsl:value-of select="./@size" />)
										</xsl:when>
									</xsl:choose>
								</td>
								<td align="center">
									<xsl:if test="./@nullable='true'">
										<span class="specialchar">ü</span>
									</xsl:if>
								</td>
								<td align="center">
									<xsl:if test="../../primaryKey/column/@name=./@name">
										<span class="specialchar">ü</span>
									</xsl:if>
								</td>
								<td align="center">
									<xsl:if test="../../foreignKeys/foreignKey/column/@name=./@name">
										<span class="specialchar">ü</span>
									</xsl:if>
								</td>
								<td align="center" class="default">
									<xsl:value-of select="./@default" />
								</td>
								<td class="description">
									<xsl:value-of select="script:GetDescription(./@description)" disable-output-escaping="yes" />
								</td>
							</tr>
						</xsl:for-each>
					</table>
				</xsl:for-each>
				<hr />
				<h2>Views Detail Information</h2>
				<xsl:for-each select="$views">
					<a>
						<xsl:attribute name="name">
							<xsl:value-of select="./@schema" />.<xsl:value-of select="./@name" />
						</xsl:attribute>
						<span></span>
					</a>
					<table border="1" cellpadding="3">
						<tr>
							<th align="left" colspan="3" class="tabletitle">
								[<xsl:value-of select="./@schema" />].[<xsl:value-of select="./@name" />]
							</th>
							<th align="right">
								<span class="specialchar">é</span>
								<a href="#_DocumentTop">Back to top</a>
							</th>
						</tr>
						<tr>
							<td colspan="4" class="tabledesc">
								<xsl:value-of select="script:GetDescription(./@description)" disable-output-escaping="yes" />
							</td>
						</tr>
						<tr class="tableheader">
							<td width="120">Name</td>
							<td width="110">Data Type</td>
							<td width="30" align="center">Nulls</td>
							<td>Description</td>
						</tr>
						<xsl:for-each select="./columns/column">
							<tr>
								<td>
									<xsl:value-of select="./@name" />
								</td>
								<td class="keyword">
									<xsl:value-of select="script:ToLower(./@sqlDbType)" />
									<xsl:choose>
										<xsl:when test="./@size=-1">(max)</xsl:when>
										<xsl:when test="./@size!=0">
											(<xsl:value-of select="./@size" />)
										</xsl:when>
									</xsl:choose>
								</td>
								<td align="center">
									<xsl:if test="./@nullable='true'">
										<span class="specialchar">ü</span>
									</xsl:if>
								</td>
								<td class="description">
									<xsl:value-of select="script:GetDescription(./@description)" disable-output-escaping="yes" />
								</td>
							</tr>
						</xsl:for-each>
					</table>
				</xsl:for-each>
				<hr />
				<h2>Stored Procedure Detail Information</h2>
				<xsl:for-each select="$storedProcedures">
					<a>
						<xsl:attribute name="name">
							<xsl:value-of select="./@schema" />.<xsl:value-of select="./@name" />
						</xsl:attribute>
						<span></span>
					</a>
					<table border="1" cellpadding="3">
						<tr>
							<th align="left" class="tabletitle">
								[<xsl:value-of select="./@schema" />].[<xsl:value-of select="./@name" />]
							</th>
							<th align="right">
								<span class="specialchar">é</span>
								<a href="#_StoredProcedureTop">Back to top</a>
							</th>
						</tr>
						<tr>
							<td colspan="2" class="tabledesc">
								<xsl:value-of select="script:GetDescription(./@description)" disable-output-escaping="yes" />
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<xsl:choose>
									<xsl:when test="count(./parameter[@direction!='ReturnValue'])&gt;0">
										<table class="subtable" border="0" cellpadding="2">
											<xsl:for-each select="./parameter[@direction!='ReturnValue']">
												<tr>
													<td>
														<xsl:value-of select="./@name" />
													</td>
													<td class="keyword">
														<nobr>
															<xsl:value-of select="script:ToLower(./@sqlDbType)" />
															<xsl:choose>
																<xsl:when test="./@size=-1">(max)</xsl:when>
																<xsl:when test="./@size!=0">
																	(<xsl:value-of select="./@size" />)
																</xsl:when>
															</xsl:choose>
														</nobr>
													</td>
													<td class="keyword">
														<xsl:if test="./@direction='InputOutput'">OUTPUT</xsl:if>
													</td>
													<td width="100%"></td>
												</tr>
											</xsl:for-each>
										</table>
									</xsl:when>
									<xsl:otherwise>No parameters.</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
					</table>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
