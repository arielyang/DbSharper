using System;
using System.Web;
using System.Collections.Specialized;

namespace DbSharper2.Library.Web
{
	public class MethodHttpHandler : IHttpHandler
	{
		#region IHttpHandler Members

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			var ma = GetMethodArgument(context.Request.QueryString);


		}

		private MethodArgument GetMethodArgument(NameValueCollection queryString)
		{
			var ma = new MethodArgument();

			

			return ma;
		}

		#endregion

		public struct MethodArgument
		{
			public string Method { get; set; }
			public string Argument { get; set; }
			public ResultType? ResultType { get; set; }
		}
	}
}
