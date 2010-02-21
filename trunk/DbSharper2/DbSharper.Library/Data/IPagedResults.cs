using System.Collections.Generic;

namespace DbSharper2.Library.Data
{
	/// <summary>
	/// Implemented by types who has uniform paged results.
	/// </summary>
	public interface IPagedResults<T>
		where T : ModelBase
	{
		#region Properties

		/// <summary>
		/// Result list.
		/// </summary>
		IList<T> Results
		{
			get;
			set;
		}

		/// <summary>
		/// Total count of all objects.
		/// </summary>
		int TotalCount
		{
			get;
			set;
		}

		#endregion Properties
	}
}