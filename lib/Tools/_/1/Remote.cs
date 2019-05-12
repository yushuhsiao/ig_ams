using Tools.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace Tools
{
    public interface IExecute
    {
        string Execute(string str);
    }
}
namespace Tools.Remote
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public abstract partial class Settings
    {
        public static string log1 = "Execute\t{0:X4}\t{1}";
        public static string log2 = "Result \t{0:X4}\t{1}";
        public static string log3 = "Error  \t{0:X4}\t{1}";

        static int s_MsgID;
        [DebuggerStepThrough]
        static int AllocMsgID()
        {
            Interlocked.CompareExchange(ref s_MsgID, 0, 0x10000);
            return Interlocked.Increment(ref s_MsgID) & 0xffff;
        }

        internal Settings() { }
    }

    [_DebuggerStepThrough]
    public class RemotingServer : MarshalByRefObject, IExecute
    {
        [DebuggerStepThrough]
        static RemotingServer()
        {
            try { RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off; }
            catch { }
            try { LifetimeServices.LeaseTime = TimeSpan.Zero; }
            catch { }
        }

        TcpServerChannel channel;
        public Uri Url { get; private set; }

        public string InitUrl { get; private set; }

        public int InitPortRange { get; private set; }

        public void Start(string url, int port_range)
        {
            this.InitUrl = url;
            this.InitPortRange = port_range;
            try
            {
                lock (this)
                {
                    if (this.channel != null) return;
                    Uri uri = new Uri(this.InitUrl);
                    string name = uri.AbsolutePath;
                    while (name.StartsWith("/") || name.StartsWith("\\"))
                        name = name.Remove(0, 1);
                    while (name.EndsWith("/") || name.EndsWith("\\"))
                        name = name.Remove(name.Length - 1);
                    for (int port = uri.Port, cnt = this.InitPortRange; ; )
                    {
                        IServerChannelSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider() { TypeFilterLevel = TypeFilterLevel.Full, Next = new ClientIPInjectorSinkProvider(), };
                        try
                        {
                            this.channel = new TcpServerChannel(name, port, sinkProvider);
                            this.Url = new Uri(string.Format("{0}://{1}:{2}/{3}", uri.Scheme, Dns.GetHostName(), port, name));
                            //ChannelServices.RegisterChannel(serverChannel, false);
                            log.message(null, "Remoting url : {0}", this.Url);
                            break;
                        }
                        catch
                        {
                            if (--cnt <= 0) throw;
                            port += cnt > 0 ? 1 : -1;
                        }
                    }
                    if (this.channel != null)
                    {
                        RemotingConfiguration.RegisterWellKnownServiceType(typeof(IExecute), name, WellKnownObjectMode.Singleton);
                        RemotingServices.Marshal(this, name);
                    }
                }
            }
            catch (Exception ex)
            {
                log.message(null, "Remoting : {0}", ex.Message);
            }
        }



        string IExecute.Execute(string str)
        {
            return Settings.ExecuteCommand(this.ExecuteCommand, this.ExecuteCommand, str);
        }

        public virtual string ExecuteCommand(string json) { return null; }

        public virtual object ExecuteCommand(object command, string str) { return null; }
    }

    [System.ComponentModel.ToolboxItem(false)]
    [_DebuggerStepThrough, WebServiceBinding(ConformsTo = WsiProfiles.None)]
    public class WebServiceServer : WebService, IExecute
    {
        [WebMethod]
        public string Execute(string str)
        {
            return Settings.ExecuteCommand(this.ExecuteCommand, this.ExecuteCommand, str);
        }

        [WebMethod(EnableSession = true)]
        public virtual string sExecute(string str)
        {
            return this.Execute(str);
        }

        public virtual string ExecuteCommand(string str) { return null; }

        public virtual object ExecuteCommand(object command, string str) { return null; }
    }

    partial class Settings
    {
        internal delegate string exe1(string s);
        internal delegate object exe2(object o, string s);
        internal static string ExecuteCommand(exe1 e1, exe2 e2, string str)
        {
            if (str == null)
                return null;
            int msgID = Settings.AllocMsgID();
            try
            {
                log.message(null, log1, msgID, str);
                string retstr = e1(str);
                if (retstr == null)
                {
                    object obj;
                    Assembly asm;
                    if (e2.Target != null)
                        asm = e2.Target.GetType().Assembly;
                    else
                        asm = e2.Method.DeclaringType.Assembly;
                    if (JsonProtocol.Deserialize(str, out obj, asm))
                    {
                        object retobj = e2(obj, str);
                        retstr = JsonProtocol.SerializeObject(retobj);
                        //JsonProtocol.Serialize(retobj, out retstr);
                    }
                }
                log.message(null, log2, msgID, retstr ?? "<null>");
                return retstr;
            }
            catch (Exception ex)
            {
                log.message(null, log3, msgID, ex.ToString());
                throw ex;
            }
        }
    }


    [_DebuggerStepThrough]
    public sealed class RemotingClient : Settings
    {
        RemotingClient() { }

        static RemotingClient instance = new RemotingClient();

        static DictionaryQueue<string, IExecute> objs = new DictionaryQueue<string, IExecute>();

        [DebuggerStepThrough]
        public static object Execute(string url, object obj, object result)
        {
            return instance._Execute(url, obj, result);
        }
        [DebuggerStepThrough]
        public static string Execute(string url, object obj)
        {
            return instance._Execute(url, obj);
        }
        [DebuggerStepThrough]
        public static string Execute(string url, string json)
        {
            return instance._Execute(url, json);
        }

        protected override string _Execute(string url, string json)
        {
            IExecute obj = objs.Dequeue(url);
            if (obj == null)
                obj = (IExecute)RemotingServices.Connect(typeof(IExecute), url);
            try { return obj.Execute(json); }
            catch
            {
                obj = null;
                throw;
            }
            finally { if (obj != null) objs.Enqueue(url, obj); }
        }

        #region //
        
        //protected override IExecute OnCreateInstance(string url)
        //{
        //    return (IExecute)RemotingServices.Connect(typeof(IExecute), url);
        //}

        //protected override void OnError(string url, IExecute obj, Exception ex)
        //{
        //}

        //public static string Execute(string url, string str)
        //{
        //    return instance.OnExecute(url, str);
        //}

        //public static object Execute(string url, object command)
        //{
        //    return instance.OnExecute(url, command, null);
        //}

        //public static object Execute(string url, string str, object result)
        //{
        //    return instance.OnExecute(url, str, result);
        //}

        //public static object Execute(string url, object command, object result)
        //{
        //    return instance.OnExecute(url, command, result);
        //}

        //public static T Execute<T>(string url, object command)
        //{
        //    return instance.OnExecute<T>(url, command);
        //}

        #endregion
    }

    [_DebuggerStepThrough]
    public sealed class WebServiceClient : Settings
    {
        #region class SoapHttpClient
        [_DebuggerStepThrough, WebServiceBinding(Name = "WebService", Namespace = "http://chess.org/")]
        class SoapHttpClient : SoapHttpClientProtocol, IExecute
        {
            [SoapDocumentMethodAttribute("http://chess.org/Execute", RequestNamespace = "http://chess.org/", ResponseNamespace = "http://chess.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
            public string Execute(string str)
            {
                return (string)this.Invoke("Execute", new object[] { str })[0];
            }
        }
        #endregion

        WebServiceClient() { }

        static WebServiceClient instance = new WebServiceClient();

        static DictionaryQueue<string, SoapHttpClient> objs = new DictionaryQueue<string, SoapHttpClient>();

        [DebuggerStepThrough]
        public static object Execute(string url, object obj, object result)
        {
            return instance._Execute(url, obj, result);
        }
        [DebuggerStepThrough]
        public static string Execute(string url, object obj)
        {
            return instance._Execute(url, obj);
        }
        [DebuggerStepThrough]
        public static string Execute(string url, string json)
        {
            return instance._Execute(url, json);
        }

        protected override string _Execute(string url, string json)
        {
            SoapHttpClient obj = objs.Dequeue(url);
            if (obj == null)
                obj = new SoapHttpClient() { Url = url };
            try { return obj.Execute(json); }
            catch
            {
                using (obj) { }
                obj = null;
                throw;
            }
            finally { if (obj != null) objs.Enqueue(url, obj); }
        }

        #region //

        //protected override IExecute OnCreateInstance(string url)
        //{
        //    return new SoapHttpClient() { Url = url };
        //}

        //protected override void OnError(string url, IExecute obj, Exception ex)
        //{
        //    using (obj as SoapHttpClient) return;
        //}

        //public static string Execute(string url, string str)
        //{
        //    return instance.OnExecute(url, str);
        //}

        //public static object Execute(string url, object command)
        //{
        //    return instance.OnExecute(url, command, null);
        //}

        //public static object Execute(string url, string str, object result)
        //{
        //    return instance.OnExecute(url, str, result);
        //}

        //public static object Execute(string url, object command, object result)
        //{
        //    return instance.OnExecute(url, command, result);
        //}

        //public static T Execute<T>(string url, object command)
        //{
        //    return instance.OnExecute<T>(url, command);
        //}

        #endregion
    }

    partial class Settings
    {
        protected object _Execute(string url, object obj, object result)
        {
            string json_r = _Execute(url, obj);
            if (result == null)
            {
                Assembly asm = null;
                if (obj != null)
                    asm = obj.GetType().Assembly;
                JsonProtocol.Deserialize(json_r, out result, asm);
            }
            else
                JsonProtocol.Populate(json_r, result);
            return result;
        }

        protected string _Execute(string url, object obj)
        {
            string json;
            if (JsonProtocol.Serialize(obj, out json))
                return _Execute(url, json);
            return null;
        }

        protected abstract string _Execute(string url, string json);

        #region

        //Dictionary<string, Queue<IExecute>> instances = new Dictionary<string, Queue<IExecute>>();

        //protected abstract IExecute OnCreateInstance(string url);

        //protected abstract void OnError(string url, IExecute obj, Exception ex);

        //protected string OnExecute(string url, string str)
        //{
        //    if (str == null)
        //        return null;
        //    IExecute obj = null;
        //    int msgID = AllocMsgID();
        //    try
        //    {
        //        log.message(null, log1, msgID, str);
        //        lock (instances)
        //        {
        //            if (!instances.ContainsKey(url))
        //                instances[url] = new Queue<IExecute>();
        //            if (instances[url].Count > 0)
        //                obj = instances[url].Dequeue();
        //            else
        //                obj = OnCreateInstance(url);
        //        }
        //        string ret = obj.Execute(str);
        //        lock (instances)
        //            instances[url].Enqueue(obj);
        //        log.message(null, log2, msgID, str);
        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.message(null, log3, msgID, ex.ToString());
        //        this.OnError(url, obj, ex);
        //        throw ex;
        //    }
        //}

        //protected object OnExecute(string url, object command, object result)
        //{
        //    string json;
        //    if (JsonProtocol.Serialize(command, out json))
        //    {
        //        string ret = OnExecute(url, json);
        //        Assembly asm = null;
        //        if (command != null)
        //            asm = command.GetType().Assembly;
        //        if (result == null)
        //        {
        //            if (!JsonProtocol.Deserialize(json, out result, asm))
        //                result = null;
        //        }
        //        else
        //        {
        //            JsonProtocol.Populate(ret, result);
        //        }
        //    }
        //    return result;
        //}

        //protected T OnExecute<T>(string url, object command)
        //{
        //    object result = OnExecute(url, command, null);
        //    if (result is T) return (T)result;
        //    return default(T);
        //}

        #endregion
    }


    [DebuggerStepThrough]
    class DictionaryQueue<TKey, TValue> : Dictionary<TKey, Queue<TValue>>
    {
        public TValue Dequeue(TKey key)
        {
            lock (this)
            {
                if (this.ContainsKey(key))
                    if (this[key].Count > 0)
                        return this[key].Dequeue();
                return default(TValue);
            }
        }
        public void Enqueue(TKey key, TValue obj)
        {
            lock (this)
            {
                if (!this.ContainsKey(key))
                    this[key] = new Queue<TValue>();
                this[key].Enqueue(obj);
            }
        }
    }




    //public abstract class Settings
    //{

    //    internal Settings() { }

    //    Dictionary<string, Queue<IExecute2>> obj_all = new Dictionary<string, Queue<IExecute2>>();

    //    protected abstract IExecute2 OnCreateObj(string url);

    //    protected abstract void OnError(string url, IExecute2 obj, Exception ex);

    //    protected string OnExecute(string url, string str)
    //    {
    //        IExecute2 obj = null;
    //        try
    //        {
    //            log.message("Invoke", Chess.Remote.Settings.log1, s_MsgID = Settings.MsgID(), id, str);
    //            Queue<IExecute2> objs;
    //            lock (obj_all)
    //                if (obj_all.ContainsKey(url))
    //                    objs = obj_all[url];
    //                else
    //                    objs = obj_all[url] = new Queue<IExecute2>();
    //            lock (objs)
    //                if (objs.Count > 0)
    //                    obj = objs.Dequeue();
    //            if (obj == null)
    //                obj = this.OnCreateObj(url);
    //            string result_str = obj.Execute(str);
    //            log.message("Result", Chess.Remote.Settings.log2, s_MsgID, result_str ?? "<null>");
    //            lock (objs)
    //                objs.Enqueue(obj);
    //            return result_str;
    //        }
    //        catch (Exception ex)
    //        {
    //            this.OnError(url, obj, ex);
    //            throw ex;
    //        }
    //    }

    //    protected object OnExecute(string url, object command, object result)
    //    {
    //        if (command != null)
    //        {
    //            string commandID, commandString;
    //            if (JsonProtocol2.Serialize(command, out commandID, out commandString))
    //            {
    //                string result_str = this.OnExecute(url, commandID, commandString);
    //                if ((result != null) || !string.IsNullOrEmpty(result_str))
    //                    JsonProtocol2.Populate(result, result_str);
    //            }
    //        }
    //        return result;
    //    }





    //    public static string Execute(StringExecuteHandler e1, ObjectExecuteHandler e2, string id, string str)
    //    {
    //        if (string.IsNullOrEmpty(str))
    //            return null;
    //        int msgID = Settings.MsgID();
    //        string result_str = null;
    //        try
    //        {
    //            log.message("Execute", log1, msgID, id, str);
    //            if (e1 != null)
    //            {
    //                result_str = e1(id, str);
    //                if (result_str != null)
    //                    return result_str;
    //            }

    //            if (e2 != null)
    //            {
    //                Type t;
    //                if (e2.Target != null)
    //                    t = e2.Target.GetType();
    //                else
    //                    t = e2.Method.DeclaringType;
    //                object command;
    //                if (JsonProtocol2.Deserialize(t.Assembly, id, str, out command))
    //                {
    //                    object result = e2(command, id, str);
    //                    if (result != null)
    //                        return result_str = JsonProtocol2.Serialize(result);
    //                }
    //            }
    //        }
    //        finally
    //        {
    //            log.message("Return", log2, msgID, result_str ?? "<null>");
    //        }
    //        return result_str;
    //    }
    //}
}