namespace DbSharper.Library.Data
{
	public static class DatabaseFactory<TDatabase> where TDatabase : Database, new()
	{
		public static Database Create(string connectionString)
		{
			Database db = new TDatabase();

			db.ConnectionString = connectionString;

			return db;
		}
	}
}

