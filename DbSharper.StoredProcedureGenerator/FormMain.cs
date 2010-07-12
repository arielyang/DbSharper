using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

using DbSharper.Schema;
using DbSharper.Schema.Collections;
using DbSharper.Schema.Database;

namespace DbSharper.StoredProcedureGenerator
{
	public partial class FormMain : Form
	{
		#region Fields

		private const string connectionString = "Data Source=192.168.1.8;Initial Catalog=www_test_com;User ID=sa;Password=P@ssw0rd;Persist Security Info=True";

		private Database dbSchema;

		#endregion Fields

		#region Constructors

		public FormMain()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Methods

		private void FormMain_Load(object sender, EventArgs e)
		{
			SchemaProvider provider = new SchemaProvider();

			dbSchema = provider.GetSchema(connectionString);

			InitTreeView(dbSchema);
		}

		private void FormMain_Resize(object sender, EventArgs e)
		{
			tabControlProcedures.Refresh();
		}

		private string GetColumnName(Column column, string imageKey)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(column.Name);
			sb.Append(" (");

			if (imageKey == "PK")
			{
				sb.Append("PK");
				sb.Append(", ");
			}

			if (imageKey == "FK")
			{
				sb.Append("FK");
				sb.Append(", ");
			}

			sb.Append(column.SqlDbType.ToString().ToLower());

			if (column.Size != 0 &&
				column.SqlDbType != SqlDbType.Image &&
				column.SqlDbType != SqlDbType.NText &&
				column.SqlDbType != SqlDbType.Text)
			{
				sb.Append('(');

				if (column.Size > 0)
				{
					sb.Append(column.Size);
				}
				else
				{
					sb.Append("max");
				}

				sb.Append(')');
			}

			if (!string.IsNullOrEmpty(column.Default))
			{
				sb.Append(' ');
				sb.Append(column.Default);
			}

			sb.Append(']');

			return sb.ToString();
		}

		private string GetCreateStoredProcedure()
		{
			return string.Empty;
		}

		//private string GetFromTemplate(string templateName, string storedProcedureName, XmlDocument sourceDocument)
		//{
		//    string templateFileName = GetTemplateFileName(templateName);
		//    string codeContent;

		//    StringWriter outputWriter = null;

		//    try
		//    {
		//        outputWriter = new StringWriter();

		//        XsltArgumentList args = new XsltArgumentList();
		//        args.AddParam("defaultNamespace", string.Empty, this.DefaultNamespace + "." + Path.GetFileNameWithoutExtension(this.InputFilePath));
		//        args.AddParam("namespace", string.Empty, GetNamespace(element));

		//        XslCompiledTransform transformer;

		//        transformer = new XslCompiledTransform();
		//        transformer.Load(templateFileName, new XsltSettings(false, true), new XmlUrlResolver());
		//        transformer.Transform(sourceDocument, args, outputWriter);
		//    }
		//    catch (Exception ex)
		//    {
		//        outputWriter.WriteLine("/*");
		//        outputWriter.WriteLine("\tERROR: Unable to generate output for template:");
		//        outputWriter.WriteLine("\t'{0}'", this.InputFilePath);
		//        outputWriter.WriteLine("\tUsing template:");
		//        outputWriter.WriteLine("\t'{0}'", templateFileName);
		//        outputWriter.WriteLine("");
		//        outputWriter.WriteLine(ex.ToString());
		//        outputWriter.WriteLine("*/");
		//    }
		//    finally
		//    {
		//        codeContent = outputWriter.ToString();

		//        outputWriter.Close();
		//    }

		//    return codeContent;
		//}

		private string GetNameWithSchema(ISchema obj)
		{
			return obj.Schema + "." + obj.Name;
		}

		private string GetTemplateFileName(string templateName)
		{
			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			string fileName = string.Format(@"Templates\{0}.xslt", templateName);

			return Path.Combine(assemblyPath, fileName);
		}

		private void InitCreateStoredProcedure(ISchema table, NamedCollection<Column> columns)
		{
			foreach (var column in columns)
			{
				checkedListBoxForCreate.Items.Add(column.Name, string.IsNullOrEmpty(column.Default));
			}
		}

		private void InitTreeView(Database dbSchema)
		{
			TreeNode databaseNode = treeViewDatabase.Nodes.Add("Database", "Database", "Database", "Database");
			TreeNode tablesNode = databaseNode.Nodes.Add("Tables", "Tables", "Folder", "Folder");
			TreeNode viewsNode = databaseNode.Nodes.Add("Views", "Views", "Folder", "Folder");

			databaseNode.Expand();

			string name;
			TreeNode node;
			string imageKey;

			foreach (var table in dbSchema.Tables)
			{
				name = GetNameWithSchema(table);

				node = tablesNode.Nodes.Add(name, name, "Table", "Table");

				foreach (var column in table.Columns)
				{
					imageKey = "Column";

					if (table.PrimaryKey.Columns.Contains(column.Name))
					{
						imageKey = "PK";
					}
					else
					{
						foreach (var fk in table.ForeignKeys)
						{
							if (fk.Columns.Contains(column.Name))
							{
								imageKey = "FK";
							}
						}
					}

					node.Nodes.Add(column.Name, column.Name, imageKey, imageKey);
				}
			}

			foreach (var view in dbSchema.Views)
			{
				name = GetNameWithSchema(view);

				node = viewsNode.Nodes.Add(name, name, "View", "View");

				foreach (var column in view.Columns)
				{
					node.Nodes.Add(column.Name, column.Name, "Column", "Column");
				}
			}
		}

		private void createStroredProceduresToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeViewDatabase.SelectedNode;

			if (selectedNode.Parent.Text == "Tables")
			{
				Table tb = dbSchema.Tables[selectedNode.Text];

				InitCreateStoredProcedure(tb, tb.Columns);
			}
		}

		private void treeViewDatabase_AfterSelect(object sender, TreeViewEventArgs e)
		{
			createStroredProceduresToolStripMenuItem.Visible = (e.Node.Level == 2);
		}

		private void treeViewDatabase_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			((TreeView)sender).SelectedNode = e.Node;
		}

		#endregion Methods
	}
}