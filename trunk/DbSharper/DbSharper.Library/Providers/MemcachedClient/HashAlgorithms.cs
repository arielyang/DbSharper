namespace DbSharper.Library.Providers.MemcachedClient
{
	using System;
	using System.Security.Cryptography;

	/// <summary>
	/// 
	/// </summary>
	public class Fnv1A32 : HashAlgorithm
	{
		#region Fields

		protected uint hash;

		private const uint FNV_prime = 16777619;
		private const uint offset_basis = 2166136261;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public Fnv1A32()
		{
			HashSizeValue = 32;
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		public override void Initialize()
		{
			hash = offset_basis;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="ibStart"></param>
		/// <param name="cbSize"></param>
		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			int length = ibStart + cbSize;

			for (int i = ibStart; i < length; i++)
			{
				hash = (hash ^ array[i]) * FNV_prime;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override byte[] HashFinal()
		{
			return BitConverter.GetBytes(hash);
		}

		#endregion Methods
	}

	/// <summary>
	/// 
	/// </summary>
	public class Fnv1V32 : HashAlgorithm
	{
		#region Fields

		protected uint hash;

		private const uint fnvPrime = 16777619;
		private const uint offsetBasis = 2166136261;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public Fnv1V32()
		{
			HashSizeValue = 32;
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		public override void Initialize()
		{
			hash = offsetBasis;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="ibStart"></param>
		/// <param name="cbSize"></param>
		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			int length = ibStart + cbSize;

			for (int i = ibStart; i < length; i++)
			{
				hash = (hash * fnvPrime) ^ array[i];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override byte[] HashFinal()
		{
			return BitConverter.GetBytes(hash);
		}

		#endregion Methods
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModifiedFNV1V32 : Fnv1V32
	{
		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override byte[] HashFinal()
		{
			hash += hash << 13;
			hash ^= hash >> 7;
			hash += hash << 3;
			hash ^= hash >> 17;
			hash += hash << 5;

			return BitConverter.GetBytes(hash);
		}

		#endregion Methods
	}
}