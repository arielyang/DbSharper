namespace DbSharper.Library.Data
{
    using System;
	using System.Text;
    using System.Xml;

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
        }

        public XmlBuilder(string tableElement, string rowElement)
        {
            if (string.IsNullOrEmpty(tableElement))
            {
                throw new ArgumentNullException("tableElement");
            }

            if (string.IsNullOrEmpty(rowElement))
            {
                throw new ArgumentNullException("rowElement");
            }

            this.tableElement = tableElement;
            this.rowElement = rowElement;

            sb = new StringBuilder();

            xmlWriter = XmlTextWriter.Create(sb);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(this.tableElement);
        }

        #endregion Constructors

        #region Methods

        public void Append(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            xmlWriter.WriteStartElement(this.rowElement);

            if (value != null)
            {
                xmlWriter.WriteAttributeString(name, value.ToString());
            }
            else
            {
                xmlWriter.WriteAttributeString(name, string.Empty);
            }

            xmlWriter.WriteEndElement();
        }

        public void Append(string[] names, object[] values)
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
                    throw new ArgumentNullException("names");
                }
            }

            xmlWriter.WriteStartElement(this.rowElement);

            for (int i = 0; i < length; i++)
            {
                xmlWriter.WriteAttributeString(names[i], values[i].ToString());

                if (values[i] != null)
                {
                    xmlWriter.WriteAttributeString(names[i], values[i].ToString());
                }
                else
                {
                    xmlWriter.WriteAttributeString(names[i], string.Empty);
                }
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