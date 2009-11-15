using System;
using System.Data;
using System.Globalization;
using System.Text;

using DbSharper.Schema.Enums;

namespace DbSharper.Schema
{
	internal static class MappingHelper
	{
		#region Methods

		/*

		public static string GetCamelCase(string name)
		{
			string pascalCaseName = GetPascalCase(name);

			return GetCamelCaseInternal(pascalCaseName);
		}

		private static string GetCamelCaseInternal(string name)
		{
			if (name.Length == 0)
			{
				return string.Empty;
			}

			if (name.Length == 1)
			{
				return name[0].ToString().ToLowerInvariant();
			}

			string camelCaseName = name[0].ToString().ToLowerInvariant() + name.Substring(1);

			if (IsCSharpKeyword(camelCaseName))
			{
				return "_" + camelCaseName;
			}

			return camelCaseName;
		}

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
				case "yield":
					return true;
			}

			return false;
		}

		*/

		public static CommonType GetCommonType(SqlDbType sqlDbType)
		{
			switch (sqlDbType)
			{
				case SqlDbType.BigInt:
					return CommonType.Int64;
				case SqlDbType.Binary:
				case SqlDbType.Image:
				case SqlDbType.VarBinary:
					return CommonType.ByteArray;
				case SqlDbType.Bit:
					return CommonType.Boolean;
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.VarChar:
				case SqlDbType.Text:
				case SqlDbType.Xml:
					return CommonType.String;
				case SqlDbType.DateTime:
				case SqlDbType.SmallDateTime:
					return CommonType.DateTime;
				case SqlDbType.Float:
					return CommonType.Double;
				case SqlDbType.Int:
					return CommonType.Int32;
				case SqlDbType.Decimal:
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					return CommonType.Decimal;
				case SqlDbType.Real:
					return CommonType.Single;
				case SqlDbType.SmallInt:
					return CommonType.Int16;
				case SqlDbType.Variant:
					return CommonType.Object;
				case SqlDbType.TinyInt:
					return CommonType.Byte;
				case SqlDbType.UniqueIdentifier:
					return CommonType.Guid;
				case SqlDbType.Timestamp:
					return CommonType.ByteArray;
				default:
					throw new ArgumentException("Unknown sqlDbType.", "sqlDbType");
			}
		}

		public static string GetCommonTypeString(CommonType commonType)
		{
			switch (commonType)
			{
				case CommonType.ByteArray:
					return "Byte[]";
				case CommonType.CharArray:
					return "Char[]";
				default:
					return commonType.ToString();
			}
		}

		public static MethodType GetMethodType(string methodName)
		{
			return methodName.StartsWith("Get", StringComparison.OrdinalIgnoreCase) ? MethodType.ExecuteReader : MethodType.ExecuteNonQuery;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static ParameterDirection GetParameterDirection(string direction)
		{
			switch (direction)
			{
				case "IN":
					return ParameterDirection.Input;
				case "OUT":
					return ParameterDirection.Output;
				case "INOUT":
					return ParameterDirection.InputOutput;
				default:
					throw new ArgumentException("Unknown parameter direction.", "direction");
			}
		}

		public static string GetParameterName(string parameter)
		{
			return parameter.TrimStart('@');
		}

		public static string GetPascalCase(string name)
		{
			if (name.IndexOf('_') == -1)
			{
				return GetPascalCaseInternal(name);
			}

			string[] strs = name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

			StringBuilder sb = new StringBuilder(name.Length);

			foreach (string str in strs)
			{
				if (str == str.ToUpperInvariant())
				{
					sb.Append(GetPascalCaseInternal(str.ToLowerInvariant()));
				}
				else
				{
					sb.Append(GetPascalCaseInternal(str));
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Get SqlDbType enum according to sqlDbType string from database.
		/// </summary>
		/// <param name="sqlDbType">SqlDbType string from database</param>
		/// <returns>SqlDbType enum</returns>
		public static SqlDbType GetSqlDbType(string sqlDbType)
		{
			switch (sqlDbType)
			{
				case "bigint":
					return SqlDbType.BigInt;
				case "binary":
					return SqlDbType.Binary;
				case "bit":
					return SqlDbType.Bit;
				case "char":
					return SqlDbType.Char;
				case "datetime":
					return SqlDbType.DateTime;
				case "decimal":
				case "numeric":
					return SqlDbType.Decimal;
				case "float":
					return SqlDbType.Float;
				case "image":
					return SqlDbType.Image;
				case "int":
					return SqlDbType.Int;
				case "money":
					return SqlDbType.Money;
				case "nchar":
					return SqlDbType.NChar;
				case "ntext":
					return SqlDbType.NText;
				case "nvarchar":
					return SqlDbType.NVarChar;
				case "real":
					return SqlDbType.Real;
				case "uniqueidentifier":
					return SqlDbType.UniqueIdentifier;
				case "smalldatetime":
					return SqlDbType.SmallDateTime;
				case "smallint":
					return SqlDbType.SmallInt;
				case "smallmoney":
					return SqlDbType.SmallMoney;
				case "text":
					return SqlDbType.Text;
				case "timestamp":
					return SqlDbType.Timestamp;
				case "tinyint":
					return SqlDbType.TinyInt;
				case "varbinary":
					return SqlDbType.VarBinary;
				case "varchar":
					return SqlDbType.VarChar;
				case "sql_variant":
					return SqlDbType.Variant;
				case "xml":
					return SqlDbType.Xml;
				case "date":
					return SqlDbType.Date;
				case "time":
					return SqlDbType.Time;
				case "datetime2":
					return SqlDbType.DateTime2;
				case "datetimeoffset":
					return SqlDbType.DateTimeOffset;
				default:
					throw new ArgumentException("Unknown sqlDbType.", "sqlDbType");
			}
		}

		public static string TrimId(string name)
		{
			if (name.EndsWith("_Id", StringComparison.OrdinalIgnoreCase))
			{
				return name.Substring(0, name.Length - 3);
			}

			return name;
		}

		private static string GetPascalCaseInternal(string name)
		{
			if (name.Length == 0)
			{
				return string.Empty;
			}

			char c = name[0];

			if (c < 'a' && c > 'z')
			{
				return name;
			}

			if (name.Length == 1)
			{
				return name[0].ToString().ToUpper(CultureInfo.InvariantCulture);
			}

			return name[0].ToString().ToUpper(CultureInfo.InvariantCulture) + name.Substring(1);
		}

		#endregion Methods
	}
}