namespace DbSharper.Library.Providers.MemcachedClient
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MemcachedClientException : ApplicationException
    {
		/// <summary>
		/// 
		/// </summary>
		public MemcachedClientException()
			: base()
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MemcachedClientException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MemcachedClientException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MemcachedClientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
