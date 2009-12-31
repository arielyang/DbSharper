using System;
using System.Runtime.Serialization;

namespace DbSharper.Library.Data
{
	#region Enumerations

	/// <summary>
	/// Return result.
	/// </summary>
	[Serializable]
	[DataContract]
	public enum ReturnResult : int
	{
		/// <summary>
		/// Failed.
		/// </summary>
		[EnumMember]
		Failed = -1,

		/// <summary>
		/// Unknown.
		/// </summary>
		[EnumMember]
		Unknown = 0,

		/// <summary>
		/// Success
		/// </summary>
		[EnumMember]
		Success = 1
	}

	#endregion Enumerations
}