using System;
using System.Collections.Generic;
using System.Reflection;

using DbSharper2.Schema.Code;

namespace DbSharper2.CodeGenerator
{
	[Serializable]
	public class AssemblyLoader : MarshalByRefObject
	{
		#region Methods

		public List<Enumeration> GetPublicEnumTypes(string path)
		{
			var assembly = Assembly.ReflectionOnlyLoadFrom(path);

			var types = assembly.GetTypes();

			var enumTypes = new List<Enumeration>();

			if (types == null)
			{
				return enumTypes;
			}

			foreach (var type in types)
			{
				if (type.IsEnum && type.IsPublic)
				{
					enumTypes.Add(new Enumeration(type));
				}
			}

			return enumTypes;
		}

		#endregion Methods
	}
}