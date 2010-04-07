using System.Xml.Serialization;

namespace DbSharper2.Schema.Infrastructure
{
	#region Enumerations

	/// <summary>
	/// Common type.
	/// </summary>
	public enum CommonType : int
	{
		Unknown,
		Boolean,
		Byte,
		[XmlEnum("Byte[]")]
		ByteArray,
		Char,
		[XmlEnum("Char[]")]
		CharArray,
		DateTime,
		DateTime2,
		DateTimeOffset,
		Decimal,
		Double,
		Guid,
		Int16,
		Int32,
		Int64,
		Object,
		Single,
		String,
		TimeSpan
	}

	#endregion Enumerations
}