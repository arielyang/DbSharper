using System;
using System.IO;
using System.Reflection;
using DbSharper2.Library.Data;
using DbSharper2.Schema;
using DbSharper2.Schema.Code;
using DbSharper2.Schema.Utility;
using DbSharper2.CodeGenerator;
using System.Collections.Generic;

namespace DbSharper2.FeatureTester
{
	public class Program
	{
		public static void Main(string[] args)
		{
			RefreshTemplates();

			string inputFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\DbSharperVerifier\\DbSharperVerifier\\Core.dbsx");
			string outputFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\DbSharperVerifier\\DbSharperVerifier\\Core.Mapping.xml");

			string[] files = Directory.GetFiles(Path.GetDirectoryName(inputFilePath));

			List<string> fileList = new List<string>();

			string fileName;

			foreach (var file in files)
			{
				fileName = Path.GetFileName(file);

				if (fileName.StartsWith("Core.") && fileName != "Core.dbsx" && fileName != "Core.Mapping.xml")
				{
					fileList.Add(fileName);
				}
			}

			var references = new List<string>();

			//references.Add(@"D:\Projects\DbSharper4GoogleCode\trunk\DbSharper2\DbSharper.Library\bin\Debug\DbSharper.Library.dll");
			//references.Add(@"D:\Projects\DbSharper4GoogleCode\trunk\DbSharper2\DbSharperVerifier\DbSharperVerifierRef\bin\Debug\DbSharperVerifierRef.exe");
			references.Add(@"D:\Projects\DbSharper4GoogleCode\trunk\DbSharper2\DbSharperVerifier\DbSharperVerifier\bin\Debug\DbSharperVerifier.exe");

			GeneratorEngine engine = new GeneratorEngine(
				inputFilePath,
				File.ReadAllText(inputFilePath),
				".Mapping.xml",
				"DbSharperVerifier",
				references,
				fileList);

			engine.ProgressChanged = (progress) => { Console.WriteLine("Progress {0}.", progress); };
			engine.BeforeFileItemAdded = (file) => { Console.WriteLine("Before {0} Added.", file); };
			engine.AfterFileItemAdded = (file) => { Console.WriteLine("After {0} Added.", file); };

			byte[] bytes = engine.Generate();

			File.WriteAllBytes(outputFilePath, bytes);

			//Console.ReadKey();
		}

		private static void RefreshTemplates()
		{
			string sourceDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\DbSharper.CodeGenerator\\Templates");
			string targetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Templates\\");

			if (!Directory.Exists(targetDir))
			{
				Directory.CreateDirectory(targetDir);
			}

			string[] files = Directory.GetFiles(sourceDir);

			foreach (var file in files)
			{
				File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);
			}
		}
	}
}
