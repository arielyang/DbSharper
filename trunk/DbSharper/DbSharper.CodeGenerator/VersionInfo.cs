using System;

namespace DbSharper.CodeGenerator
{
	internal sealed class VersionInfo : IEquatable<VersionInfo>
	{
		#region Constructors

		public VersionInfo(string version)
			: this(version, null)
		{
		}

		public VersionInfo(string version, string summary)
		{
			this.Version = version;
			this.Summary = summary;
		}

		#endregion Constructors

		#region Properties

		public static VersionInfo Null
		{
			get
			{
				return new VersionInfo(null, null);
			}
		}

		public bool IsNull
		{
			get
			{
				return string.IsNullOrEmpty(this.Version) && string.IsNullOrEmpty(this.Summary);
			}
		}

		public string Summary
		{
			get; set;
		}

		public string Version
		{
			get; set;
		}

		#endregion Properties

		#region Methods

		public bool Equals(VersionInfo other)
		{
			if (this.IsNull || other.IsNull)
			{
				return false;
			}

			return this.Version == other.Version;
		}

		public override string ToString()
		{
			return this.Version;
		}

		#endregion Methods
	}
}