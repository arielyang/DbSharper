using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace DbSharper.Library.Data
{
	/// <summary>
	/// Build Json string.
	/// </summary>
	public class JsonBuilder
	{
		#region Fields

		private bool isCollection;
		private StringBuilder sb = new StringBuilder();

		#endregion Fields

		#region Constructors

		/// <summary>
		/// JsonBuilder constructor.
		/// </summary>
		/// <param name="obj">Object to be converted</param>
		public JsonBuilder(object obj)
		{
			IList list = obj as IList;

			if (list != null)
			{
				isCollection = true;

				sb.Append('[');

				int count = list.Count;

				for (int i = 0; i < count; i++)
				{
					WriteValue(list[i]);

					sb.Append(',');
				}

				// Remove the trailing comma.
				if (count > 0)
				{
					sb.Length--;
				}

				sb.Append(']');
			}
			else
			{
				isCollection = false;

				sb.Append('{');
			}
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Append property to the JSON object.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Property value</param>
		public void Append(string name, object value)
		{
			if (isCollection)
			{
				throw new InvalidOperationException("Can not apply \"Append\" method to an object implements IList interface.");
			}

			sb.Append('"');
			sb.Append(name);
			sb.Append('"');
			sb.Append(':');

			WriteValue(value);

			sb.Append(',');
		}

		/// <summary>
		/// Get JSON format string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (!isCollection)
			{
				// Remove the trailing comma.
				if (sb.Length > 1)
				{
					sb.Length--;
				}

				sb.Append('}');
			}

			return sb.ToString();
		}

		private void WriteString(string str)
		{
			sb.Append('"');

			foreach (char c in str)
			{
				switch (c)
				{
					case '"':
						sb.Append("\\\"");
						break;
					case '\\':
						sb.Append("\\\\");
						break;
					case '\b':
						sb.Append("\\b");
						break;
					case '\f':
						sb.Append("\\f");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					default:
						{
							int i = (int)c;

							if (i < 32 || i > 127)
							{
								sb.AppendFormat("\\u{0:X04}", i);
							}
							else
							{
								sb.Append(c);
							}

							break;
						}
				}
			}

			sb.Append('"');
		}

		private void WriteValue(object value)
		{
			if (value == null || value == DBNull.Value)
			{
				sb.Append("null");
			}
			else if (value is string || value is char)
			{
				WriteString(value.ToString());
			}
			else if (value is double || value is float || value is long || value is int || value is short || value is byte || value is decimal)
			{
				sb.Append(value);
			}
			else if (value is DateTime)
			{
				DateTime dt = (DateTime)value;

				sb.Append("new Date(");
				sb.Append(dt.Year);
				sb.Append(',');
				sb.Append(dt.Month - 1);
				sb.Append(',');
				sb.Append(dt.Day);
				sb.Append(',');
				sb.Append(dt.Hour);
				sb.Append(',');
				sb.Append(dt.Minute);
				sb.Append(',');
				sb.Append(dt.Second);
				sb.Append(")");
			}
			else if (value is bool)
			{
				sb.Append((bool)value ? "true" : "false");
			}
			else if (value is Guid)
			{
				sb.Append('"');
				sb.Append(value.ToString());
				sb.Append('"');
			}
			else if (value is Enum)
			{
				Type underlyingType = Enum.GetUnderlyingType(value.GetType());

				sb.Append(Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture));
			}
			else if (value is IJson)
			{
				sb.Append((value as IJson).ToJson());
			}
			else
			{
				throw new NotImplementedException("Unknown JSON type.");
			}
		}

		#endregion Methods
	}
}