using System.Collections.ObjectModel;
using System.Xml;

using DbSharper.Schema.Enums;

namespace DbSharper.Schema.Configuration
{
	internal class MappingConfiguration
	{
		#region Fields

		private XmlDocument doc;
		private XmlNamespaceManager nsmgr;

		#endregion Fields

		#region Constructors

		public MappingConfiguration(string configFileContent)
		{
			this.doc = new XmlDocument();

			this.doc.LoadXml(configFileContent);

			nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("m", "http://schemas.dbsharper.com/mapping");

			XmlNode configFileNode = this.doc.SelectSingleNode("/m:mapping/@configFile", this.nsmgr);

			if (configFileNode == null)
			{
				throw new DbSharperException("Attribute \"configFile\" is required.");
			}
			else
			{
				this.ConfigFile = configFileNode.Value;
			}

			this.Model = new ModelSection();

			this.LoadConditions(this.Model.SchemaIncludeConditions, "/m:mapping/m:model/m:include[m:schema]/*");
			this.LoadConditions(this.Model.SchemaExcludeConditions, "/m:mapping/m:model/m:exclude[m:schema]/*");
			this.LoadConditions(this.Model.IncludeConditions, "/m:mapping/m:model/m:include[m:table or m:view]/*");
			this.LoadConditions(this.Model.ExcludeConditions, "/m:mapping/m:model/m:exclude[m:table or m:view]/*");

			string methodMask = this.doc.SelectSingleNode("/m:mapping/m:dataAccess/@methodMask", this.nsmgr).Value;

			this.DataAccess = new DataAccessSection(methodMask);

			this.LoadConditions(this.DataAccess.SchemaIncludeConditions, "/m:mapping/m:dataAccess/m:include[m:schema]/*");
			this.LoadConditions(this.DataAccess.SchemaExcludeConditions, "/m:mapping/m:dataAccess/m:exclude[m:schema]/*");
			this.LoadConditions(this.DataAccess.IncludeConditions, "/m:mapping/m:dataAccess/m:include[m:procedure]/*");
			this.LoadConditions(this.DataAccess.ExcludeConditions, "/m:mapping/m:dataAccess/m:exclude[m:procedure]/*");
		}

		#endregion Constructors

		#region Properties

		public string ConfigFile
		{
			get;
			set;
		}

		public DataAccessSection DataAccess
		{
			get;
			set;
		}

		public ModelSection Model
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		private static bool GetAttributeBooleanValue(XmlNode node, string attributeName)
		{
			XmlAttribute attribute = node.Attributes[attributeName];

			if (attribute == null)
			{
				return false;
			}

			bool result;

			if (bool.TryParse(attribute.Value, out result))
			{
				return result;
			}

			return false;
		}

		private static ConditionType GetAttributeConditionType(XmlNode node)
		{
			switch (node.Name)
			{
				case "schema":
					return ConditionType.Schema;
				case "procedure":
					return ConditionType.Procedure;
				case "table":
					return ConditionType.Table;
				case "view":
					return ConditionType.View;
				default:
					return ConditionType.None;
			}
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

		private void LoadConditions(Collection<Condition> conditions, string xpath)
		{
			XmlNodeList nodes = this.doc.SelectNodes(xpath, this.nsmgr);

			foreach (XmlNode node in nodes)
			{
				conditions.Add(
					new Condition()
					{
						Name = GetAttributeStringValue(node, "name"),
						Prefix = GetAttributeStringValue(node, "prefix"),
						TrimPrefix = GetAttributeBooleanValue(node, "prefix"),
						ConditionType = GetAttributeConditionType(node)
					});
			}
		}

		#endregion Methods
	}
}