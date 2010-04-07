namespace DbSharper2.Schema.Infrastructure
{
	#region Enumerations

	/// <summary>
	/// Database method type.
	/// </summary>
	public enum MethodType : int
	{
		/// <summary>
		/// Database ExecuteNonQuery method.
		/// </summary>
		ExecuteNonQuery,

		/// <summary>
		/// Database ExecuteReader method.
		/// </summary>
		ExecuteReader
	}

	#endregion Enumerations
}