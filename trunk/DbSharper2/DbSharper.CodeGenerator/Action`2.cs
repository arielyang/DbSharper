namespace System
{
	#region Delegates

	/// <summary>
	/// Encapsulates a method that has two parameters and does not return a value.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
	/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this delegate encapsulates. </param>
	public delegate void Action<T1, T2>(T1 arg1, T2 arg2);

	#endregion Delegates
}