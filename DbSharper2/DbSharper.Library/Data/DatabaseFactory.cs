namespace DbSharper2.Library.Data
{
	public static class DatabaseFactory
	{
		#region Methods

		public static Database CreateDatabase<TDatabase>(string connectionString)
			where TDatabase : Database, new()
		{
			Database db = new TDatabase();

			db.ConnectionString = connectionString;

			return db;
		}

		#endregion Methods
	}
}