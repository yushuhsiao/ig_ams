using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
using Chess.Json;
using Chess;

namespace System.Web.Services
{
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    [_DebuggerStepThrough, WebServiceBinding(ConformsTo = WsiProfiles.None)]
    public abstract class WebServiceServer : System.Web.Services.WebService, ObjectInvoke
    {
        [WebMethod]
        public virtual string Execute(string command, string json)
        {
            return Chess.Remote.Remoting.Execute(ExecuteCommand, ExecuteCommand, command, json);
        }

        public virtual string ExecuteCommand(string commandID, string commandString)
        {
            return null;
        }

        public virtual object ExecuteCommand(object command, string commandID, string commandString)
        {
            return this.InvokeCommand(command, commandID, commandString);
        }
    }

    [_DebuggerStepThrough]
    public class WebServiceClient
    {
        [_DebuggerStepThrough, WebServiceBinding(Name = "WebService", Namespace = "http://chess.org/")]
        class SoapHttpClient : SoapHttpClientProtocol, IExecute
        {
            [SoapDocumentMethodAttribute("http://chess.org/Execute", RequestNamespace = "http://chess.org", ResponseNamespace = "http://chess.org", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
            public string Execute(string command, string json)
            {
                return (string)this.Invoke("Execute", new object[] { command, json })[0];
            }
        }

        SoapHttpClient protocol = new SoapHttpClient();

        public WebServiceClient() { }
        public WebServiceClient(string url) { this.Url = url; }

        public string Url
        {
            get { return protocol.Url; }
            set { protocol.Url = value; }
        }

        public string Invoke(string commandID, string commandString)
        {
            return protocol.Execute(commandID, commandString);
        }
        public object Invoke(object command, object result)
        {
            if (command != null)
            {
                string commandID;
                string commandString;
                if (JsonConvert.Serialize(command, out commandID, out commandString))
                {
                    string result_str = protocol.Execute(commandID, commandString);
                    if ((result != null) && !string.IsNullOrEmpty(result_str))
                        JsonConvert.Populate(result, result_str);
                }
            }
            return result;
        }
        public TResult Invoke<TResult>(object command)
        {
            return (TResult)Invoke(command, Activator.CreateInstance<TResult>());
        }

        public static string Execute(string url, string command, string json)
        {
            return new SoapHttpClient() { Url = url, }.Execute(command, json);
        }
    }
}