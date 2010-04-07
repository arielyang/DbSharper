using System;
using System.Runtime.Serialization;

namespace DbSharper2.Schema
{
	[Serializable]
	public sealed class DbSharperException : Exception
	{
		#region Constructors

		public DbSharperException()
		{
		}

		public DbSharperException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public DbSharperException(string message)
			: base(message)
		{
		}

		private DbSharperException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion Constructors
	}
}