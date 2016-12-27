using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;

namespace casino
{
	class comet
	{
		public void SendMessage(int userID, string url, object message)
		{
		}
		public void WaitMessage(int millisecondsTimeout, Delegate callback, params object[] args)
		{
		}
	}
	//class _comet : IRouteHandler
	//{
	//	IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}

	//class comet : IHttpAsyncHandler, IRequiresSessionState
	//{
	//	IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	bool IHttpHandler.IsReusable
	//	{
	//		get { throw new NotImplementedException(); }
	//	}

	//	void IHttpHandler.ProcessRequest(HttpContext context)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
}
