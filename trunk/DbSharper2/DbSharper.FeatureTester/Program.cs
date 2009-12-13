using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbSharper.Library.Data;
using DbSharper.Schema.Code;
using DbSharper.Schema;
using System.IO;
using System.Reflection;

namespace DbSharper.FeatureTester
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Core.dbsx");

			Mapping mapping = MappingFactory.CreateMapping(file);

			

			
		}
	}

	public class UserModel : ModelBase
	{
		public override object GetPropertyValue(string propertyName)
		{
			throw new NotImplementedException();
		}

		public override void LoadField(System.Data.IDataRecord record, string fieldName, int index)
		{
			throw new NotImplementedException();
		}
	}

}
