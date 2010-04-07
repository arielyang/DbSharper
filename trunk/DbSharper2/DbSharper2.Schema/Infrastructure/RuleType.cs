namespace DbSharper2.Schema.Infrastructure
{
	#region Enumerations

	/// <summary>
	/// Mapping rule type for dbsx file.
	/// </summary>
	public enum RuleType : int
	{
		/// <summary>
		/// Database schema.
		/// </summary>
		Schema,

		/// <summary>
		/// Database table.
		/// </summary>
		Table,

		/// <summary>
		/// Database view.
		/// </summary>
		View,

		/// <summary>
		/// Database stored procedure.
		/// </summary>
		Procedure
	}

	#endregion Enumerations
}