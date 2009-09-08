namespace DbSharper.Schema
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public static class Serializer
    {
        #region Methods

        public static object Deserialize(Type type, string value)
        {
            using (StringReader sr = new StringReader(value))
            {
                XmlSerializer xz = new XmlSerializer(type);

                return xz.Deserialize(sr);
            }
        }

        public static string Serialize(object value)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "\t"
            };

            StringBuilder sb = new StringBuilder();

            using (XmlWriter xw = XmlTextWriter.Create(sb, settings))
            {
                XmlSerializer xs = new XmlSerializer(value.GetType());

                xs.Serialize(xw, value);
            }

            return sb.ToString().Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
        }

        #endregion Methods
    }
}