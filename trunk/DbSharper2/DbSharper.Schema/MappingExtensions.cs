using System;
using System.Data;
using System.Text;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema
{
	internal static class MappingExtensions
	{
		#region Methods

		/// <summary>
		/// Get camel case of a name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Camel case name string.</returns>
		internal static string ToCamelCase(this string name)
		{
			string pascalCaseName = ToPascalCaseInternal(name);

			return ToCamelCaseInternal(pascalCaseName);
		}

		/// <summary>
		/// Get common type from according to a dbType.
		/// </summary>
		/// <param name="dbType">DbType.</param>
		/// <returns>Common Type.</returns>
		internal static CommonType ToCommonType(this DbType dbType)
		{
			switch (dbType)
			{
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				case DbType.Xml:
					return CommonType.String;
				case DbType.Binary:
					return CommonType.ByteArray;
				case DbType.Boolean:
					return CommonType.Boolean;
				case DbType.Byte:
					return CommonType.Byte;
				case DbType.Currency:
				case DbType.Decimal:
					return CommonType.Decimal;
				case DbType.Date:
				case DbType.DateTime:
					return CommonType.DateTime;
				case DbType.DateTime2:
					return CommonType.DateTime2;
				case DbType.DateTimeOffset:
					return CommonType.DateTimeOffset;
				case DbType.Double:
					return CommonType.Double;
				case DbType.Guid:
					return CommonType.Guid;
				case DbType.Int16:
					return CommonType.Int16;
				case DbType.Int32:
					return CommonType.Int32;
				case DbType.Int64:
					return CommonType.Int64;
				case DbType.Object:
					return CommonType.Object;
				case DbType.Single:
					return CommonType.Single;
				case DbType.Time:
					return CommonType.DateTime;
				case DbType.SByte:
				case DbType.UInt16:
				case DbType.UInt32:
				case DbType.UInt64:
				case DbType.VarNumeric:
				default:
					// TODO: Embed string into resource file later.
					throw new ArgumentException("Unknown dbType.", "dbType");
			}
		}

		/// <summary>
		/// Get pascal case of a name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Pascal case name string.</returns>
		internal static string ToPascalCase(this string name)
		{
			// Has not "_" in the string.
			if (name.IndexOf('_') == -1)
			{
				return ToPascalCaseInternal(name);
			}

			string[] strs = name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

			StringBuilder sb = new StringBuilder(name.Length);

			foreach (string str in strs)
			{
				sb.Append(ToPascalCaseInternal(str));
			}

			return sb.ToString();
		}

		/// <summary>
		/// Remove "_Id" from a string which ends with "_Id".
		/// </summary>
		/// <param name="name">Name string.</param>
		/// <returns>Trimmed string.</returns>
		internal static string TrimId(this string name)
		{
			const string idString = "_Id";

			if (name.EndsWith(idString, StringComparison.OrdinalIgnoreCase))
			{
				return name.Substring(0, name.Length - idString.Length);
			}

			return name;
		}

		/// <summary>
		/// Returns if a name is a C sharp keyword.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private static bool IsCSharpKeyword(string name)
		{
			switch (name)
			{
				case "abstract":
				case "as":
				case "base":
				case "bool":
				case "break":
				case "byte":
				case "case":
				case "catch":
				case "char":
				case "checked":
				case "class":
				case "const":
				case "continue":
				case "decimal":
				case "default":
				case "delegate":
				case "do":
				case "double":
				case "else":
				case "enum":
				case "event":
				case "explicit":
				case "extern":
				case "false":
				case "finally":
				case "fixed":
				case "float":
				case "for":
				case "foreach":
				case "goto":
				case "if":
				case "implicit":
				case "in":
				case "int":
				case "interface":
				case "internal":
				case "is":
				case "lock":
				case "long":
				case "namespace":
				case "new":
				case "null":
				case "object":
				case "operator":
				case "out":
				case "override":
				case "params":
				case "private":
				case "protected":
				case "public":
				case "readonly":
				case "ref":
				case "return":
				case "sbyte":
				case "sealed":
				case "short":
				case "sizeof":
				case "stackalloc":
				case "static":
				case "string":
				case "struct":
				case "switch":
				case "this":
				case "throw":
				case "true":
				case "try":
				case "typeof":
				case "uint":
				case "ulong":
				case "unchecked":
				case "unsafe":
				case "ushort":
				case "using":
				case "virtual":
				case "void":
				case "volatile":
				case "while":
					return true;
			}

			return false;
		}

		/// <summary>
		/// Get camel case of a name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Camel case name string.</returns>
		private static string ToCamelCaseInternal(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException(name);
			}

			string camelCaseName;

			if (name.Length == 1)
			{
				camelCaseName = name.ToUpperInvariant();
			}
			else
			{
				camelCaseName = name[0].ToString().ToUpperInvariant() + name.Substring(1);
			}

			if (IsCSharpKeyword(camelCaseName))
			{
				return "_" + camelCaseName;
			}

			return camelCaseName;
		}

		/// <summary>
		/// Get pascal case of a name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Pascal case name string.</returns>
		private static string ToPascalCaseInternal(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException(name);
			}

			if (name.Length == 1)
			{
				return name.ToUpperInvariant();
			}

			return name[0].ToString().ToUpperInvariant() + name.Substring(1).ToLowerInvariant();
		}

		#endregion Methods
	}
}