namespace DbSharper.Schema.Enums
{
    using System.Xml.Serialization;

    #region Enumerations

    public enum CommonType
    {
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