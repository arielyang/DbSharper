using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace DbSharper2.CodeGenerator
{
	[Serializable]
	public class CrossAppDomainReflector : MarshalByRefObject
	{
		#region Fields

		private string assemblyFilePath;
		private List<Type> enumTypes;
		private AppDomain appDomain;

		#endregion Fields

		#region Constructors

		public CrossAppDomainReflector(string assemblyFilePath)
		{
			this.assemblyFilePath = assemblyFilePath;
		}

		#endregion Constructors

		#region Methods

		public List<Type> GetPublicEnumTypes()
		{
			//AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			//{
			//    var n = args.Name;

			//    var asm = Assembly.Load(n);

			//    return asm;
			//};

			var ass = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var item in ass)
			{
				Debug.WriteLine(item.Location);
			}

			appDomain = AppDomain.CreateDomain(
				"TempDomain",
				null,
				Path.GetDirectoryName(this.GetType().Assembly.Location),
				//Path.GetDirectoryName(this.assemblyFilePath),
				null,
				true);

			AppDomainSetup se = appDomain.SetupInformation;

			//var asmss = asmGen.GetReferencedAssemblies();

			//foreach (var item in asmss)
			//{
			//    appDomain.Load(item);
			//}
			//appDomain.ExecuteAssemblyByName(this.GetType().Assembly.FullName);

			Debug.WriteLine(new string('-', 50));

			var ass2 = appDomain.GetAssemblies();

			foreach (var item in ass2)
			{
				Debug.WriteLine(item.Location);
			}


			appDomain.AssemblyResolve += (sender, args) =>
			{
				var n = args.Name;

				var asm = Assembly.Load(n);

				return asm;
			};

			

			appDomain.DoCallBack(LoaderCallback);

			AppDomain.Unload(appDomain);

			return this.enumTypes;
		}

		private void LoaderCallback()
		{
			var ass = AppDomain.CurrentDomain;
			var se = ass.SetupInformation;

			//var assembly = Assembly.LoadFile(this.assemblyFilePath);
			var assembly = appDomain.Load(AssemblyName.GetAssemblyName(this.assemblyFilePath));

			var types = assembly.GetTypes();

			this.enumTypes = new List<Type>();

			if (types == null)
			{
				return;
			}

			foreach (var type in types)
			{
				if (type.IsEnum && type.IsPublic)
				{
					this.enumTypes.Add(type);
				}
			}
		}

		#endregion Methods
	}
}