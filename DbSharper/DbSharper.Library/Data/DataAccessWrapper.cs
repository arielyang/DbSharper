////namespace DbSharper.Library.Data
////{
////    using DbSharper.Library.Model;
////    using System.Data.Common;
////    using System.Text;
////    using System;
////    using System.Data;
////    public class DataAccessWrapper<TDatabase> where TDatabase : Database, new()
////    {
////        private string connectionString;
////        private ItemBase item;
////        private string itemName;
////        private Database db;
////        public DataAccessWrapper(string connectionString, ItemBase item)
////        {
////            this.connectionString = connectionString;
////            this.item = item;
////            this.itemName = RemoveTrailingItem(item.ToString());
////        }
////        public bool Update()
////        {
////            string query = string.Empty;
////            db = DatabaseFactory<TDatabase>.Create(this.connectionString);
////            DbCommand dbCommand = db.GetSqlStringCommand(query);
////            db.AddInParameter(dbCommand, "", DbType.Binary, "");
////            return false;
////        }
////        public bool Create()
////        {
////            return false;
////        }
////        private string BuildUpdateQuery()
////        {
////            StringBuilder sb = new StringBuilder();
////            sb.Append("UPDATE ");
////            sb.Append(itemName);
////            return sb.ToString();
////        }
////        private string RemoveTrailingItem(string itemName)
////        {
////            if (!itemName.EndsWith("Item"))
////            {
////                throw new ArgumentException("The value of itemName must end with \"Item\".");
////            }
////            return itemName.Substring(0, itemName.Length - 4);
////        }
////    }
////}