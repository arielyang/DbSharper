using System.Data.SqlClient;

using DbSharper2.Library.Data;

namespace DbSharper2.Data.MySql
{
	public class MySqDatabase : Database
	{
		#region Constructors

		public MySqDatabase()
			: base(SqlClientFactory.Instance)
		{
		}

		#endregion Constructors

		#region Methods

		public override string BuildParameterName(string name)
		{
			if (name[0] != '#')
			{
				return "#" + name;
			}

			return name;
		}

		#endregion Methods
	}
}
