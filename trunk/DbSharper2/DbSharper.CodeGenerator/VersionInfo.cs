using System;

namespace DbSharper2.CodeGenerator
{
	/// <summary>
	/// Version info.
	/// </summary>
	internal sealed class VersionInfo : IEquatable<VersionInfo>
	{
		#region Constructors

		/// <summary>
		/// Constructor with version number.
		/// </summary>
		/// <param name="version">Version number.</param>
		public VersionInfo(string version)
			: this(version, null)
		{
		}

		/// <summary>
		/// Constructor with version number and version summary.
		/// </summary>
		/// <param name="version">Version number.</param>
		/// <param name="summary">Versioni summary.</param>
		public VersionInfo(string version, string summary)
		{
			this.Version = version;
			this.Summary = summary;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// A null version info.
		/// </summary>
		public static VersionInfo Null
		{
			get
			{
				return new VersionInfo(null, null);
			}
		}

		/// <summary>
		/// If this version info is null.
		/// </summary>
		public bool IsNull
		{
			get
			{
				return string.IsNullOrEmpty(this.Version) && string.IsNullOrEmpty(this.Summary);
			}
		}

		/// <summary>
		/// Version summary.
		/// </summary>
		public string Summary
		{
			get; set;
		}

		/// <summary>
		/// Version number.
		/// </summary>
		public string Version
		{
			get; set;
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// If other version info equals this one.
		/// </summary>
		/// <param name="other">Version info to compare.</param>
		/// <returns>True if equals.</returns>
		public bool Equals(VersionInfo other)
		{
			if (this.IsNull || other.IsNull)
			{
				return false;
			}

			return this.Version == other.Version;
		}

		/// <summary>
		/// Return version number.
		/// </summary>
		/// <returns>Version number.</returns>
		public override string ToString()
		{
			return this.Version;
		}

		#endregion Methods
	}
}