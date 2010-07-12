using System;
using System.Collections.Generic;
using System.Text;

namespace DbSharper.Library.Data
{
	/// <summary>
	/// Implemented by types who has uniform paged results.
	/// </summary>
	public interface IPagedResults
	{
		#region Properties

		/// <summary>
		/// Total count of all objects.
		/// </summary>
		int TotalCount
		{
			get; set;
		}

		#endregion Properties
	}
}