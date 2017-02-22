using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System.Web
{
	public static class Extensions
	{
		public static string ToQueryString(this NameValueCollection src)
		{
			string delimiter = "";
			StringBuilder q = new StringBuilder();
			for (int i = 0, n = src.Count; i < n; i++)
			{
				string key = src.GetKey(i);
				q.Append(delimiter);
				q.Append(System.Net.WebUtility.HtmlEncode(key));
				q.Append('=');
				q.Append(System.Net.WebUtility.HtmlEncode(src[key]));
                delimiter = "&";

            }
			return q.ToString();
		}
	}
}