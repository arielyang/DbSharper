using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace DbSharper.Library.Data
{
	public class XmlBuilder
	{
		#region Fields

		private string rowElement;
		private StringBuilder sb;
		private string tableElement;
		private XmlWriter xmlWriter;

		#endregion Fields

		#region Constructors

		public XmlBuilder()
			: this("t", "r")
		{
			// TODO: XML control char validation.
		}

		public XmlBuilder(string tableElementName, string rowElementName)
		{
			if (string.IsNullOrEmpty(tableElementName))
			{
				throw new ArgumentNullException("tableElement");
			}

			if (string.IsNullOrEmpty(rowElementName))
			{
				throw new ArgumentNullException("rowElement");
			}

			this.tableElement = tableElementName;
			this.rowElement = rowElementName;

			sb = new StringBuilder();

			xmlWriter = XmlTextWriter.Create(sb);

			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(this.tableElement);
		}

		#endregion Constructors

		#region Methods

		public void Append(string name, string value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			xmlWriter.WriteStartElement(this.rowElement);
			xmlWriter.WriteAttributeString(name, value ?? string.Empty);
			xmlWriter.WriteEndElement();
		}

		public void Append(string[] names, string[] values)
		{
			int length = names.Length;

			if (length != values.Length)
			{
				throw new ArgumentException("Length of names is not equal to length of values.");
			}

			for (int i = 0; i < length; i++)
			{
				if (string.IsNullOrEmpty(names[i]))
				{
					// TODO: Embed string into resource file later.
					throw new ArgumentNullException(
						"names",
						string.Format(
							CultureInfo.InvariantCulture,
							"There are null name in the names, position: {0}.",
							length)
						);
				}
			}

			xmlWriter.WriteStartElement(this.rowElement);

			for (int i = 0; i < length; i++)
			{
				xmlWriter.WriteAttributeString(names[i], values[i] ?? string.Empty);
			}

			xmlWriter.WriteEndElement();
		}

		public override string ToString()
		{
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();

			xmlWriter.Close();

			return sb.ToString();
		}

		#endregion Methods
	}
}