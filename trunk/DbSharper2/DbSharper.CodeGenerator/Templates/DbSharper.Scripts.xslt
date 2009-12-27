<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:script="urn:scripts">
<msxsl:script language="C#" implements-prefix="script">
<![CDATA[
	public string ToLower(string str)
	{
		return str.ToLower();
	}

	public bool EndsWith(string str1, string str2)
	{
		return str1.EndsWith(str2, StringComparison.OrdinalIgnoreCase);
	}
	
	public bool StartsWith(string str1, string str2)
	{
		return str1.StartsWith(str2, StringComparison.OrdinalIgnoreCase);
	}
	
	public string GetCamelCase(string name)
	{
		if (name.Length == 0) return string.Empty;
		
		if (name.Length == 1) return name[0].ToString().ToLower();

		string camelName = name[0].ToString().ToLower() + name.Substring(1);
		
		if (IsCSharpKeyword(camelName))
		{
			return "_" + camelName;
		}
		
		return camelName;
	}

	public string GetItemType(string collectionType)
	{
		return collectionType.Substring(0, collectionType.Length - 10) + "Item";
	}
	
	public string RemoveItemPostfix(string item)
	{
		return item.Substring(0, item.Length - 4);
	}
	
	public string GetId(string column)
	{
		int i = column.LastIndexOf('_');

		if (i < 0 || i == column.Length - 1)
		{
			return column;
		}

		return column.Substring(i + 1);
	}

	public string TrimId(string name)
	{
		if (name.EndsWith("_Id", StringComparison.OrdinalIgnoreCase))
		{
			return name.Substring(0, name.Length - 3);
		}

		return name;
	}
	
	public string GetAnchor(string schema, string name)
	{
		return string.Format("#{0}.{1}", schema, name);
	}
	
	public string GetDescription(string description)
	{
		if (description == string.Empty)
		{
			return string.Empty;
		}
	
		return description.Replace("\r\n", "<br/>");
	}
	
	public string GetSummary(string summary, int indent)
	{
		if (string.IsNullOrEmpty(summary))
		{
			return string.Empty;
		}
	
		string[] summaries = summary.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
	
		StringBuilder sb = new StringBuilder();
			
		foreach (string s in summaries)
		{
			sb.Append('\t', indent);
			sb.Append("/// ");
			sb.AppendLine(s);
		}
		
		sb.Length = sb.Length - 2;
		
		return sb.ToString();
	}

	public string CSharpAlias(string name)
	{
		switch (name)
		{
			case "Boolean":
				return "bool";
			case "Byte":
				return "byte";
			case "Byte[]":
				return "byte[]";
			case "Char":
				return "char";
			case "Char[]":
				return "char[]";
			case "Decimal":
				return "decimal";
			case "Double":
				return "double";
			case "Int16":
				return "short";
			case "Int32":
				return "int";
			case "Int64":
				return "long";
			case "Object":
				return "object";
			case "Single":
				return "float";
			case "String":
				return "string";
			default:
				return name;
		}
	}
	
	private bool IsCSharpKeyword(string name)
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
			case "":
				return true;
		}

		return false;
	}
]]>
</msxsl:script>
</xsl:stylesheet>