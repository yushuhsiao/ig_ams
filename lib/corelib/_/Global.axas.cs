using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace casino
{
    public class HttpApplicationEx : System.Web.HttpApplication
    {
        static HttpApplicationEx()
        {
            TraceLogWriter.Enabled = TextLogWriter.Enabled = JsonTextLogWriter.Enabled = true;
        }
        public override void Init()
        {
            base.Init();
            api.Init(this);
        }

        //protected void Application_Start(object sender, EventArgs e)
        //{
        //    //lock (this)
        //    //{
        //    //    try { Application.Lock(); new CorpRowCommand() { ID = 0, }.insert(null, null); }
        //    //    catch (Exception ex) { log.error(ex); }
        //    //    finally { Application.UnLock(); }
        //    //}
        //}
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //}
        //void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs e)
        //{
        //    //web._Global.Authenticate(e);
        //    //Global.AccessControl(e.Context);
        //    //web.UserManager<web.Admin, web.Guest1>.OnAuthenticate(e);
        //}
        //protected void Application_AuthenticateRequest(object sender, EventArgs e) { }
        //protected void Application_PostAuthenticateRequest(object sender, EventArgs e) { }
        //protected void Application_AuthorizeRequest(object sender, EventArgs e) { }
        //protected void Application_PostAuthorizeRequest(object sender, EventArgs e) { }
        //protected void Application_ResolveRequestCache(object sender, EventArgs e) { }
        //protected void Application_PostResolveRequestCache(object sender, EventArgs e) { }
        //protected void Application_MapRequestHandler(object sender, EventArgs e) { }
        //protected void Application_PostMapRequestHandler(object sender, EventArgs e) { }
        //protected void Application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    //Admin.UserList.AcquireRequestState(HttpContext.Current);
        //    //Global.AccessControl(HttpContext.Current);
        //}
        //protected void Application_PostAcquireRequestState(object sender, EventArgs e) { }
        //protected void Application_PreRequestHandlerExecute(object sender, EventArgs e) { }
        //protected void Application_PostRequestHandlerExecute(object sender, EventArgs e) { }
        //protected void Application_ReleaseRequestState(object sender, EventArgs e) { }
        //protected void Application_PostReleaseRequestState(object sender, EventArgs e) { }
        //protected void Application_UpdateRequestCache(object sender, EventArgs e) { }
        //protected void Application_PostUpdateRequestCache(object sender, EventArgs e) { }
        //protected void Application_LogRequest(object sender, EventArgs e) { }
        //protected void Application_PostLogRequest(object sender, EventArgs e) { }
        //protected void Application_EndRequest(object sender, EventArgs e) { }
        //protected void Application_PreSendRequestContent(object sender, EventArgs e) { }
        //protected void Application_PreSendRequestHeaders(object sender, EventArgs e) { }
        //protected void Application_Error(object sender, EventArgs e) { }
        //protected void Application_End(object sender, EventArgs e) { }
        //protected void Session_Start(object sender, EventArgs e) { }
        //protected void Session_End(object sender, EventArgs e) { }

        #region
        //protected void Application_Start(object sender, EventArgs e)
        //{

        //}

        //protected void Session_Start(object sender, EventArgs e)
        //{

        //}

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{

        //}

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{

        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{

        //}

        //protected void Session_End(object sender, EventArgs e)
        //{

        //}

        //protected void Application_End(object sender, EventArgs e)
        //{

        //}
        #endregion
    }
}