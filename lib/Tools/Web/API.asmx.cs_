using Tools.Remote;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Web.Services;

namespace WebTools
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
    /// <summary>
    ///Service1 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://chess.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [_DebuggerStepThrough]
    public class API : WebServiceServer
    {
        public override string ExecuteCommand(string str)
        {
            return base.ExecuteCommand(str);
        }
        public override object ExecuteCommand(object command, string str)
        {
            return ObjectInvoke.Invoke(command, this, command, null, str);
        }

        public static object CallAPI(string url, object sender, object command, object result)
        {
            url = url ?? ConfigurationManager.AppSettings[typeof(API).Name];
            if (url == null)
                return CallAPI(sender, command, result);
            return WebServiceClient.Execute(url, command, result);
        }


        public static object CallAPI(object sender, object command, object result)
        {
            return ObjectInvoke.Invoke(command, sender, command, result, string.Empty);
        }
    }
}
