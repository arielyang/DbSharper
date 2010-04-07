namespace DbSharper2.Library.Data
{
	/// <summary>
	/// Implemented by types whose values can be converted to JSON format string.
	/// </summary>
	public interface IJson
	{
		#region Methods

		/// <summary>
		/// Get JSON format string of this object.
		/// </summary>
		/// <returns>JSON format string.</returns>
		string ToJson();

		#endregion Methods
	}
}