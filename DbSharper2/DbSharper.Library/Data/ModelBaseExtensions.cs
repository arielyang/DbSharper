using System;
using System.Collections.Generic;
using System.Text;

namespace DbSharper.Library.Data
{
	public static class ModelBaseExtensions
	{
		#region Methods

		/// <summary>
		/// Get series of property string value joinned by ",".
		/// </summary>
		/// <param name="list">Model list.</param>
		/// <param name="func">Function to get a property string value.</param>
		/// <returns>Series of property value string.</returns>
		public static string JoinValues<T>(this List<T> list, Func<T, string> func)
		{
			return JoinValues(list, func, ",");
		}

		/// <summary>
		/// Get series of property value joinned by ",".
		/// </summary>
		/// <param name="list">Model list.</param>
		/// <param name="func">Function to get a property string value.</param>
		/// <param name="separator">Separator.</param>
		/// <returns>Series of property value string.</returns>
		public static string JoinValues<T>(this List<T> list, Func<T, string> func, string separator)
		{
			var sb = new StringBuilder();

			foreach (var model in list)
			{
				sb.Append(func(model));
				sb.Append(separator);
			}

			if (sb.Length > 0)
			{
				sb.Length -= separator.Length;
			}

			return sb.ToString();
		}

		/// <summary>
		/// Get series of property string value joinned by ",".
		/// </summary>
		/// <param name="list">Model list.</param>
		/// <param name="func1">Function to get the first property string value.</param>
		/// <param name="func2">Function to get the second property string value.</param>
		/// <returns>Series of property value string.</returns>
		public static string JoinValues<T>(this List<T> list, Func<T, string> func1, Func<T, string> func2)
		{
			return JoinValues(list, func1, func2, ",", ";");
		}

		/// <summary>
		/// Get series of property value joinned by ",".
		/// </summary>
		/// <param name="list">Model list.</param>
		/// <param name="func1">Function to get the first property string value.</param>
		/// <param name="func2">Function to get the second property string value.</param>
		/// <param name="separator1">The first separator.</param>
		/// <param name="separator2">The second separator.</param>
		/// <returns>Series of property value string.</returns>
		public static string JoinValues<T>(this List<T> list, Func<T, string> func1, Func<T, string> func2, string separator1, string separator2)
		{
			var sb = new StringBuilder();

			foreach (var model in list)
			{
				sb.Append(func1(model));
				sb.Append(separator1);
				sb.Append(func2(model));
				sb.Append(separator2);
			}

			if (sb.Length > 0)
			{
				sb.Length -= separator2.Length;
			}

			return sb.ToString();
		}

		#endregion Methods
	}
}