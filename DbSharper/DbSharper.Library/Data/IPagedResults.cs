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
		int TotalCount { get; set; }
	}
}
