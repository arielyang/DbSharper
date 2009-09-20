using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace DbSharper.CodeGenerator
{
	public abstract class VsMultipleFileGenerator<IterativeElementType> : IEnumerable<IterativeElementType>, IVsSingleFileGenerator, IObjectWithSite
	{
		#region Fields

		protected bool cancelGenerating = false;
		protected List<string> oldFileNames;

		private string defaultNamespace;
		private string inputFileContents;
		private string inputFilePath;
		private List<string> newFileNames;
		private EnvDTE.Project project;
		private ServiceProvider serviceProvider = null;
		private object site;

		#endregion Fields

		#region Constructors

		public VsMultipleFileGenerator()
		{
			EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));

			Array ary = (Array)dte.ActiveSolutionProjects;

			if (ary.Length > 0)
			{
				project = (EnvDTE.Project)ary.GetValue(0);
			}

			newFileNames = new List<string>();
			oldFileNames = new List<string>();
		}

		#endregion Constructors

		#region Properties

		protected string DefaultNamespace
		{
			get { return defaultNamespace; }
		}

		protected string InputFileContents
		{
			get { return inputFileContents; }
		}

		protected string InputFilePath
		{
			get { return inputFilePath; }
		}

		protected EnvDTE.Project Project
		{
			get { return project; }
		}

		private ServiceProvider SiteServiceProvider
		{
			get
			{
				if (serviceProvider == null)
				{
					Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleServiceProvider = site as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
					serviceProvider = new ServiceProvider(oleServiceProvider);
				}

				return serviceProvider;
			}
		}

		#endregion Properties

		#region Methods

		public void Generate(string inputFilePath, string inputFileContents, string defaultNamespace, out IntPtr outputFileContents, out int output, IVsGeneratorProgress generateProgress)
		{
			try
			{
				if (cancelGenerating)
				{
					byte[] fileData = File.ReadAllBytes(Path.ChangeExtension(inputFilePath, GetDefaultExtension()));

					// return our summary data, so that Visual Studio may write it to disk.
					outputFileContents = Marshal.AllocCoTaskMem(fileData.Length);

					Marshal.Copy(fileData, 0, outputFileContents, fileData.Length);

					output = fileData.Length;

					OnCanceling();

					return;
				}

				this.inputFileContents = inputFileContents;
				this.inputFilePath = inputFilePath;
				this.defaultNamespace = defaultNamespace;
				this.newFileNames.Clear();

				int iFound = 0;
				uint itemId = 0;
				EnvDTE.ProjectItem item;
				Microsoft.VisualStudio.Shell.Interop.VSDOCUMENTPRIORITY[] pdwPriority = new Microsoft.VisualStudio.Shell.Interop.VSDOCUMENTPRIORITY[1];

				// obtain a reference to the current project as an IVsProject type
				Microsoft.VisualStudio.Shell.Interop.IVsProject VsProject = VsHelper.ToVsProject(project);
				// this locates, and returns a handle to our source file, as a ProjectItem
				VsProject.IsDocumentInProject(InputFilePath, out iFound, pdwPriority, out itemId);

				// if our source file was found in the project (which it should have been)
				if (iFound != 0 && itemId != 0)
				{
					Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSp = null;
					VsProject.GetItemContext(itemId, out oleSp);

					if (oleSp != null)
					{
						ServiceProvider sp = new ServiceProvider(oleSp);
						// convert our handle to a ProjectItem
						item = sp.GetService(typeof(EnvDTE.ProjectItem)) as EnvDTE.ProjectItem;
					}
					else
					{
						throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem");
					}
				}
				else
				{
					throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem");
				}

				oldFileNames.Clear();
				newFileNames.Clear();

				foreach (EnvDTE.ProjectItem childItem in item.ProjectItems)
				{
					oldFileNames.Add(childItem.Name);

					if (!childItem.Name.EndsWith(".cs"))
					{
						childItem.Properties.Item("BuildAction").Value = 0;
					}
				}

				// now we can start our work, iterate across all the 'elements' in our source file
				foreach (IterativeElementType element in this)
				{
					try
					{
						// obtain a name for this target file
						string fileName = GetFileName(element);
						// add it to the tracking cache
						newFileNames.Add(fileName);
						// fully qualify the file on the filesystem
						string strFile = Path.Combine(inputFilePath.Substring(0, inputFilePath.LastIndexOf(Path.DirectorySeparatorChar)), fileName);

						if (oldFileNames.Contains(fileName))
						{
							// check out this file.
							if (project.DTE.SourceControl.IsItemUnderSCC(strFile) &&
								!project.DTE.SourceControl.IsItemCheckedOut(strFile))
							{
								project.DTE.SourceControl.CheckOutItem(strFile);
							}
						}

						// create the file
						FileStream fs = File.Create(strFile);

						try
						{
							// generate our target file content
							byte[] data = GenerateContent(element);

							// write it out to the stream
							fs.Write(data, 0, data.Length);

							fs.Close();

							OnGenerateFile(strFile);
							/*
							 * Here you may wish to perform some addition logic
							 * such as, setting a custom tool for the target file if it
							 * is intented to perform its own generation process.
							 * Or, set the target file as an 'Embedded Resource' so that
							 * it is embedded into the final Assembly.

							EnvDTE.Property prop = itm.Properties.Item("CustomTool");

							if (String.IsNullOrEmpty((string)prop.Value) || !String.Equals((string)prop.Value, typeof(AnotherCustomTool).Name))
							{
								prop.Value = typeof(AnotherCustomTool).Name;
							}
							*/
						}
						catch (Exception)
						{
							fs.Close();

							if (File.Exists(strFile))
							{
								File.Delete(strFile);
							}
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}

				// perform some clean-up, making sure we delete any old (stale) target-files
				foreach (EnvDTE.ProjectItem childItem in item.ProjectItems)
				{
					if (!(childItem.Name.EndsWith(GetDefaultExtension()) || newFileNames.Contains(childItem.Name)))
					{
						// then delete it
						childItem.Delete();
					}
				}

				foreach (var newFileName in newFileNames)
				{
					if (!oldFileNames.Contains(newFileName))
					{
						string fileName = Path.Combine(inputFilePath.Substring(0, inputFilePath.LastIndexOf(Path.DirectorySeparatorChar)), newFileName);

						//// add the newly generated file to the solution, as a child of the source file...
						EnvDTE.ProjectItem itm = item.ProjectItems.AddFromFile(fileName);

						//// set buildaction to none
						if (!newFileName.EndsWith(".cs"))
						{
							itm.Properties.Item("BuildAction").Value = 0;
						}
					}
				}

				// generate our summary content for our 'single' file
				byte[] summaryData = GenerateSummaryContent();

				if (summaryData == null)
				{
					outputFileContents = IntPtr.Zero;

					output = 0;
				}
				else
				{
					// return our summary data, so that Visual Studio may write it to disk.
					outputFileContents = Marshal.AllocCoTaskMem(summaryData.Length);

					Marshal.Copy(summaryData, 0, outputFileContents, summaryData.Length);

					output = summaryData.Length;
				}
			}
			catch (Exception ex)
			{
				OnError(ex);

				LogException(ex);

				outputFileContents = IntPtr.Zero;
				output = 0;
			}
		}

		public abstract byte[] GenerateContent(IterativeElementType element);

		public abstract byte[] GenerateSummaryContent();

		public abstract string GetDefaultExtension();

		public abstract IEnumerator<IterativeElementType> GetEnumerator();

		public void GetSite(ref Guid riid, out IntPtr ppvSite)
		{
			if (this.site == null)
			{
				throw new Win32Exception(-2147467259);
			}

			IntPtr objectPointer = Marshal.GetIUnknownForObject(this.site);

			try
			{
				Marshal.QueryInterface(objectPointer, ref riid, out ppvSite);

				if (ppvSite == IntPtr.Zero)
				{
					throw new Win32Exception(-2147467262);
				}
			}
			finally
			{
				if (objectPointer != IntPtr.Zero)
				{
					Marshal.Release(objectPointer);
					objectPointer = IntPtr.Zero;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void SetSite(object pUnkSite)
		{
			this.site = pUnkSite;
		}

		protected abstract string GetFileName(IterativeElementType element);

		protected void LogException(Exception ex)
		{
			string path = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "log");

			using (FileStream stm = File.Open(path, FileMode.Append))
			{
				using (StreamWriter sw = new StreamWriter(stm))
				{
					sw.WriteLine(DateTime.Now);

					sw.WriteLine(new string('-', 50));

					sw.WriteLine(ex.ToString());

					sw.WriteLine();

					sw.WriteLine(ex.Message);

					sw.WriteLine();

					sw.WriteLine(ex.StackTrace);

					sw.WriteLine();
				}
			}
		}

		protected virtual void OnCanceling()
		{
		}

		protected virtual void OnError(Exception ex)
		{
		}

		protected virtual void OnGenerateFile(string filePath)
		{
		}

		#endregion Methods
	}
}