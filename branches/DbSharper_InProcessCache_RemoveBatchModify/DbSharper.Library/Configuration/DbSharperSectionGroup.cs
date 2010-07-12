using System.Configuration;

namespace DbSharper.Library.Configuration
{
	public sealed class DbSharperSectionGroup : ConfigurationSectionGroup
	{
		#region Fields

		private const string cachingService = "cachingService";

		#endregion Fields

		#region Properties

		[ConfigurationProperty("cachingService")]
		public CachingServiceSectionGroup CachingService
		{
			get { return (CachingServiceSectionGroup)base.SectionGroups["cachingService"]; }
		}

		#endregion Properties
	}
}