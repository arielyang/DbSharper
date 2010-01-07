using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema
{
	internal static class MappingExtensions
	{
		#region Fields

		private static Dictionary<string, string> irregularPluralWords = new Dictionary<string, string>();

		#endregion Fields

		#region Constructors

		static MappingExtensions()
		{
			irregularPluralWords.Add("afterlife", "afterlives");
			irregularPluralWords.Add("alga", "algae");
			irregularPluralWords.Add("alumna", "alumnae");
			irregularPluralWords.Add("alumnus", "alumni");
			irregularPluralWords.Add("analysis", "analyses");
			irregularPluralWords.Add("antenna", "antennae");
			irregularPluralWords.Add("appendix", "appendices");
			irregularPluralWords.Add("axis", "axes");
			irregularPluralWords.Add("bacillus", "bacilli");
			irregularPluralWords.Add("basis", "bases");
			irregularPluralWords.Add("Bedouin", "Bedouin");
			irregularPluralWords.Add("cactus", "cacti");
			irregularPluralWords.Add("calf", "calves");
			irregularPluralWords.Add("cherub", "cherubim");
			irregularPluralWords.Add("child", "children");
			irregularPluralWords.Add("cod", "cod");
			irregularPluralWords.Add("cookie", "cookies");
			irregularPluralWords.Add("criterion", "criteria");
			irregularPluralWords.Add("curriculum", "curricula");
			irregularPluralWords.Add("datum", "data");
			irregularPluralWords.Add("deer", "deer");
			irregularPluralWords.Add("diagnosis", "diagnoses");
			irregularPluralWords.Add("die", "dice");
			irregularPluralWords.Add("dormouse", "dormice");
			irregularPluralWords.Add("elf", "elves");
			irregularPluralWords.Add("elk", "elk");
			irregularPluralWords.Add("erratum", "errata");
			irregularPluralWords.Add("esophagus", "esophagi");
			irregularPluralWords.Add("fauna", "faunae");
			irregularPluralWords.Add("fish", "fish");
			irregularPluralWords.Add("flora", "florae");
			irregularPluralWords.Add("focus", "foci");
			irregularPluralWords.Add("foot", "feet");
			irregularPluralWords.Add("formula", "formulae");
			irregularPluralWords.Add("fundus", "fundi");
			irregularPluralWords.Add("fungus", "fungi");
			irregularPluralWords.Add("genie", "genii");
			irregularPluralWords.Add("genus", "genera");
			irregularPluralWords.Add("goose", "geese");
			irregularPluralWords.Add("grouse", "grouse");
			irregularPluralWords.Add("hake", "hake");
			irregularPluralWords.Add("half", "halves");
			irregularPluralWords.Add("headquarters", "headquarters");
			irregularPluralWords.Add("hippo", "hippos");
			irregularPluralWords.Add("hippopotamus", "hippopotami");
			irregularPluralWords.Add("hoof", "hooves");
			irregularPluralWords.Add("housewife", "housewives");
			irregularPluralWords.Add("hypothesis", "hypotheses");
			irregularPluralWords.Add("index", "indices");
			irregularPluralWords.Add("jackknife", "jackknives");
			irregularPluralWords.Add("knife", "knives");
			irregularPluralWords.Add("labium", "labia");
			irregularPluralWords.Add("larva", "larvae");
			irregularPluralWords.Add("leaf", "leaves");
			irregularPluralWords.Add("life", "lives");
			irregularPluralWords.Add("loaf", "loaves");
			irregularPluralWords.Add("louse", "lice");
			irregularPluralWords.Add("magus", "magi");
			irregularPluralWords.Add("man", "men");
			irregularPluralWords.Add("memorandum", "memoranda");
			irregularPluralWords.Add("midwife", "midwives");
			irregularPluralWords.Add("millennium", "millennia");
			irregularPluralWords.Add("moose", "moose");
			irregularPluralWords.Add("mouse", "mice");
			irregularPluralWords.Add("nebula", "nebulae");
			irregularPluralWords.Add("neurosis", "neuroses");
			irregularPluralWords.Add("nova", "novas");
			irregularPluralWords.Add("nucleus", "nuclei");
			irregularPluralWords.Add("oesophagus", "oesophagi");
			irregularPluralWords.Add("offspring", "offspring");
			irregularPluralWords.Add("ovum", "ova");
			irregularPluralWords.Add("ox", "oxen");
			irregularPluralWords.Add("papyrus", "papyri");
			irregularPluralWords.Add("passerby", "passersby");
			irregularPluralWords.Add("penknife", "penknives");
			irregularPluralWords.Add("person", "people");
			irregularPluralWords.Add("phenomenon", "phenomena");
			irregularPluralWords.Add("placenta", "placentae");
			irregularPluralWords.Add("pocketknife", "pocketknives");
			irregularPluralWords.Add("pupa", "pupae");
			irregularPluralWords.Add("radius", "radii");
			irregularPluralWords.Add("reindeer", "reindeer");
			irregularPluralWords.Add("retina", "retinae");
			irregularPluralWords.Add("rhinoceros", "rhinoceros");
			irregularPluralWords.Add("roe", "roe");
			irregularPluralWords.Add("salmon", "salmon");
			irregularPluralWords.Add("scarf", "scarves");
			irregularPluralWords.Add("self", "selves");
			irregularPluralWords.Add("seraph", "seraphim");
			irregularPluralWords.Add("series", "series");
			irregularPluralWords.Add("sheaf", "sheaves");
			irregularPluralWords.Add("sheep", "sheep");
			irregularPluralWords.Add("shelf", "shelves");
			irregularPluralWords.Add("species", "species");
			irregularPluralWords.Add("spectrum", "spectra");
			irregularPluralWords.Add("stimulus", "stimuli");
			irregularPluralWords.Add("stratum", "strata");
			irregularPluralWords.Add("supernova", "supernovas");
			irregularPluralWords.Add("swine", "swine");
			irregularPluralWords.Add("terminus", "termini");
			irregularPluralWords.Add("thesaurus", "thesauri");
			irregularPluralWords.Add("thesis", "theses");
			irregularPluralWords.Add("thief", "thieves");
			irregularPluralWords.Add("trout", "trout");
			irregularPluralWords.Add("vulva", "vulvae");
			irregularPluralWords.Add("wife", "wives");
			irregularPluralWords.Add("wildebeest", "wildebeest");
			irregularPluralWords.Add("wolf", "wolves");
			irregularPluralWords.Add("woman", "women");
			irregularPluralWords.Add("yen", "yen");
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Get camel case of a name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Camel case name string.</returns>
		internal static string ToCamelCase(this string name)
		{
			string pascalCaseName = name.ToPascalCase();

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
				case DbType.Time:
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
		/// Call this method to get the properly pluralized english version of the word.
		/// </summary>
		/// <param name="word">The word needing conditional pluralization.</param>
		/// <param name="count">The number of items the word refers to.</param>
		/// <returns>The pluralized word</returns>
		internal static string ToPlural(this string word)
		{
			int length = word.Length;

			if (IsPlural(word) == true)
			{
				return word; //it's already a plural
			}
			else if (irregularPluralWords.ContainsKey(word))
			//it's an irregular plural, use the word from the dictionary
			{
				return irregularPluralWords[word];
			}

			if (length <= 2)
			{
				return word; //not a word that can be pluralised!
			}

			////1. If the word ends in a consonant plus -y, change the -y into
			///-ie and add an -s to form the plural
			///e.g. enemy--enemies baby--babies
			switch (word.Substring(length - 2).ToLower())
			{
				case "by":
				case "cy":
				case "dy":
				case "fy":
				case "gy":
				case "hy":
				case "jy":
				case "ky":
				case "ly":
				case "my":
				case "ny":
				case "py":
				case "ry":
				case "sy":
				case "ty":
				case "vy":
				case "wy":
				case "xy":
				case "zy":
					{
						return word.Substring(0, length - 1) + "ies";
					}
				//2. For words that end in -is, change the -is to -es to make the plural form.
				//synopsis--synopses
				//thesis--theses
				case "is":
					{
						return word.Substring(0, length - 1) + "es";
					}
				//3. For words that end in a "hissing" sound (s,z,x,ch,sh), add an -es to form the plural.
				//box--boxes
				//church--churches
				case "ch":
				case "sh":
					{
						return word + "es";
					}
				default:
					{
						switch (word.Substring(length - 1))
						{
							case "s":
							case "z":
							case "x":
								{
									return word + "es";
								}
							default:
								{
									//4. Assume add an -s to form the plural of most words.
									return word + "s";
								}
						}
					}
			}
		}

		/// <summary>
		/// Call this method to get the singular 
		/// version of a plural English word.
		/// </summary>
		/// <param name="word">The word to turn into a singular</param>
		/// <returns>The singular word</returns>
		internal static string ToSingular(this string word)
		{
			if (irregularPluralWords.ContainsValue(word.ToLower()))
			{
				foreach (KeyValuePair<string, string> kvp in irregularPluralWords)
				{
					if (kvp.Value.ToLower() == word.ToLower()) return kvp.Key.ToPascalCase();
				}
			}

			int length = word.Length;

			if (word[length - 1] != 's')
			{
				return word; // not a plural word if it doesn't end in S
			}

			if (length <= 2)
			{
				return word; // not a word that can be made singular if only two letters!
			}

			if (length >= 4)
			{
				//1. If the word ends in a consonant plus -y, change the -y into -ie and add an -s to form the plural – so reverse engineer it to get the singular
				// e.g. enemy--enemies baby--babies family--families
				switch (word.Substring(length - 4).ToLower())
				{
					case "bies":
					case "cies":
					case "dies":
					case "fies":
					case "gies":
					case "hies":
					case "jies":
					case "kies":
					case "lies":
					case "mies":
					case "nies":
					case "pies":
					case "ries":
					case "sies":
					case "ties":
					case "vies":
					case "wies":
					case "xies":
					case "zies":
						{
							return word.Substring(0, length - 3) + "y";
						}
					//3. For words that end in a "hissing" sound (s,z,x,ch,sh), add an -es to form the plural.
					//church--churches
					case "ches":
					case "shes":
						{
							return word.Substring(0, length - 2);
						}
				}
			}

			if (length >= 3)
			{
				switch (word.Substring(length - 3).ToLower())
				{
					//box--boxes
					case "ses":
					//NOTE some false positives here - For words that end in -is, change the -is to -es to make the plural form.
					//synopsis--synopses
					//thesis--theses
					case "zes":
					case "xes":
						{
							return word.Substring(0, length - 2);
						}
				}
			}

			if (length >= 3)
			{
				switch (word.Substring(length - 2).ToLower())
				{
					case "es":
						{
							return word.Substring(0, length - 1) + "is";
						}
					//4. Assume add an -s to form the plural of most words.
					default:
						{
							return word.Substring(0, length - 1);
						}
				}
			}

			return word;
		}

		/// <summary>
		/// Remove a primary key name string from a column name string which ends with the primary key name string.
		/// </summary>
		/// <param name="primaryKeyName">Primary key name string .</param>
		/// <returns>Trimmed string.</returns>
		internal static string TrimPrimaryKeyName(this string name, string primaryKeyName)
		{
			int length = name.Length - primaryKeyName.Length;

			if (length <= 0)
			{
				return null;
			}

			string tail = name.Substring(length);

			if (string.Compare(tail, primaryKeyName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return name.Substring(0, length).TrimEnd('_');
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
		/// test if a word is plural
		/// </summary>
		/// <param name="word">word to test</param>
		/// <returns>true if a word is plural</returns>
		private static bool IsPlural(string word)
		{
			word = word.ToLower();

			int length = word.Length;

			if (length <= 2)
			{
				return false; // not a word that can be made singular if only two letters!
			}
			if (irregularPluralWords.ContainsValue(word))
			{
				return true; //it's definitely already a plural
			}
			if (length >= 4)
			{
				//1. If the word ends in a consonant plus -y, change the -y into -ie and add an -s to form the plural
				// e.g. enemy--enemies baby--babies family--families
				switch (word.Substring(length - 4))
				{
					case "bies":
					case "cies":
					case "dies":
					case "fies":
					case "gies":
					case "hies":
					case "jies":
					case "kies":
					case "lies":
					case "mies":
					case "nies":
					case "pies":
					case "ries":
					case "sies":
					case "ties":
					case "vies":
					case "wies":
					case "xies":
					case "zies":
					case "ches":
					case "shes":
						{
							return true;
						}
				}
			}

			if (length >= 3)
			{
				switch (word.Substring(length - 3))
				{
					//box--boxes
					case "ses":
					case "zes":
					case "xes":
						{
							return true;
						}
				}
			}

			if (length >= 3)
			{
				switch (word.Substring(length - 2))
				{
					case "es":
						{
							return true;
						}
				}
			}

			if (word.Substring(length - 1) != "s")
			{
				return false; // not a plural word if it doesn't end in S
			}

			return true;
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
				camelCaseName = name[0].ToString().ToLowerInvariant() + name.Substring(1);
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

			if (name.ToUpperInvariant() == name)
			{
				return name[0].ToString().ToUpperInvariant() + name.Substring(1).ToLowerInvariant();
			}
			else
			{
				return name[0].ToString().ToUpperInvariant() + name.Substring(1);
			}
		}

		#endregion Methods
	}
}