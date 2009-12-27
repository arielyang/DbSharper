using System;
using System.IO;
using System.Reflection;
using DbSharper.Library.Data;
using DbSharper.Schema;
using DbSharper.Schema.Code;
using DbSharper.Schema.Utility;
using DbSharper.CodeGenerator;

namespace DbSharper.FeatureTester
{
	public class Program
	{
		public static void Main(string[] args)
		{
			RefreshTemplates();

			string inputFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\DbSharperVerifier\\DbSharperVerifier\\Core.dbsx");
			string outputFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\DbSharperVerifier\\DbSharperVerifier\\Core.Schema.xml");

			GeneratorEngine engine = new GeneratorEngine(
				inputFilePath,
				File.ReadAllText(inputFilePath),
				".Schema.xml",
				"DbSharperVerifier",
				null);

			engine.ProgressChanged = (progress) => { Console.WriteLine("Progress {0}.", progress); };
			engine.BeforeFileItemAdded = (fileName) => { Console.WriteLine("Before {0} Added.", fileName); };
			engine.AfterFileItemAdded = (fileName) => { Console.WriteLine("After {0} Added.", fileName); };

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
