using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DbSharper2.Schema.Utility
{
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

			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

			ns.Add(string.Empty, string.Empty);

			StringBuilder sb = new StringBuilder();

			using (XmlWriter xw = XmlTextWriter.Create(sb, settings))
			{
				XmlSerializer xs = new XmlSerializer(value.GetType());

				xs.Serialize(xw, value, ns);
			}

			return sb.ToString();
		}

		#endregion Methods
	}
}