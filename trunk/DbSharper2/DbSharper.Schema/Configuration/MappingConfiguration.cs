using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Configuration
{
	internal sealed class MappingConfiguration
	{
		#region Fields

		private XmlDocument doc;
		private XmlNamespaceManager nsmgr;

		#endregion Fields

		#region Constructors

		internal MappingConfiguration(string configFileContent)
		{
			try
			{
				this.doc = new XmlDocument();

				this.doc.LoadXml(configFileContent);

				nsmgr = new XmlNamespaceManager(doc.NameTable);
				nsmgr.AddNamespace("m", "http://schemas.dbsharper.com/mapping");

				XmlNode configFileNode = this.doc.SelectSingleNode("/m:mapping/@configFile", this.nsmgr);

				if (configFileNode == null)
				{
					// TODO: Embed string into resource file later.
					throw new DbSharperException("Attribute \"configFile\" is required.");
				}
				else
				{
					this.ConfigFile = configFileNode.Value;
				}

				this.Model = new ModelSection();

				this.LoadRules(this.Model.IncludeRules, "/m:mapping/m:model/m:include/*");
				this.LoadRules(this.Model.ExcludeRules, "/m:mapping/m:model/m:exclude/*");

				string methodMask = this.doc.SelectSingleNode("/m:mapping/m:dataAccess/@methodMask", this.nsmgr).Value;

				this.DataAccess = new DataAccessSection(methodMask);

				this.LoadRules(this.DataAccess.IncludeRules, "/m:mapping/m:dataAccess/m:include/*");
				this.LoadRules(this.DataAccess.ExcludeRules, "/m:mapping/m:dataAccess/m:exclude/*");
			}
			catch (Exception ex)
			{
				// TODO: Embed string into resource file later.
				throw new DbSharperException("Mapping configuration initialization error.", ex);
			}
		}

		#endregion Constructors

		#region Properties

		internal string ConfigFile
		{
			get;
			set;
		}

		internal DataAccessSection DataAccess
		{
			get;
			set;
		}

		internal ModelSection Model
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		private static bool? GetAttributeBooleanValue(XmlNode node, string attributeName)
		{
			XmlAttribute attribute = node.Attributes[attributeName];

			if (attribute == null)
			{
				return null;
			}

			bool result;

			if (bool.TryParse(attribute.Value, out result))
			{
				return result;
			}

			return null;
		}

		private static string GetAttributeStringValue(XmlNode node, string attributeName)
		{
			XmlAttribute attribute = node.Attributes[attributeName];

			if (attribute == null)
			{
				return null;
			}

			return attribute.Value;
		}

		private static RuleType GetRuleType(XmlNode node)
		{
			switch (node.Name)
			{
				case "schema":
					return RuleType.Schema;
				case "procedure":
					return RuleType.Procedure;
				case "table":
					return RuleType.Table;
				case "view":
					return RuleType.View;
				default:
					// TODO: Embed string into resource file later.
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown rule type: {0}.", node.Name));
			}
		}

		private void LoadRules(Collection<Rule> conditions, string xpath)
		{
			XmlNodeList nodes = this.doc.SelectNodes(xpath, this.nsmgr);

			foreach (XmlNode node in nodes)
			{
				conditions.Add(
					new Rule(
						GetRuleType(node),
						GetAttributeStringValue(node, "name"),
						GetAttributeStringValue(node, "prefix"),
						GetAttributeBooleanValue(node, "trimPrefix")
					));
			}
		}

		#endregion Methods
	}
}