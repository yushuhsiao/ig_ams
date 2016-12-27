using casino;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
//[assembly: PreApplicationStartMethod(typeof(api), "Main")]

namespace casino
{
    public partial class api
	{
        //public static void Main()
        //{
        //    //log.message(null, "init start");

        //    SqlConfig.Cache.GetInstance();
        //    TextLogWriter.Enabled = true;
        //    System.Web.Mvc.MvcHandler.DisableMvcResponseHeader = true;
        //    System.Web.WebPages.WebPageHttpHandler.DisableWebPagesResponseHeader = true;

        //    //router.Initialize();
        //    //MessageManager.Initialize();

        //    //System.Web.Razor.RazorCodeLanguage.Languages.Add("html", new System.Web.Razor.CSharpRazorCodeLanguage());
        //    //System.Web.WebPages.WebPageHttpHandler.RegisterExtension("html");

        //    //using (SqlCmd sqlcmd = SqlCmd.Open(DB.DB01W))
        //    //{
        //    //	//Permission1.Cache.Init(sqlcmd);
        //    //	//User.Init(sqlcmd);
        //    //}

        //    HttpApplication.RegisterModule(typeof(HttpContextEx.Module));
        //    HttpApplication.RegisterModule(typeof(UserManager.Module));
        //}

        static void _Start(object sender, EventArgs e) { }
        static void _BeginRequest(object sender, EventArgs e) { }
		static void _AuthenticateRequest(object sender, EventArgs e) { }
		static void _PostAuthenticateRequest(object sender, EventArgs e) { }
		static void _AuthorizeRequest(object sender, EventArgs e) { }
		static void _PostAuthorizeRequest(object sender, EventArgs e) { }
		static void _ResolveRequestCache(object sender, EventArgs e) { }
		static void _PostResolveRequestCache(object sender, EventArgs e) { }
		static void _MapRequestHandler(object sender, EventArgs e) { }
		static void _PostMapRequestHandler(object sender, EventArgs e) { }
		static void _AcquireRequestState(object sender, EventArgs e) { }
		static void _PostAcquireRequestState(object sender, EventArgs e) { }
		static void _PreRequestHandlerExecute(object sender, EventArgs e) { }
		static void _PostRequestHandlerExecute(object sender, EventArgs e) { }
		static void _ReleaseRequestState(object sender, EventArgs e) { }
		static void _PostReleaseRequestState(object sender, EventArgs e) { }
		static void _UpdateRequestCache(object sender, EventArgs e) { }
		static void _PostUpdateRequestCache(object sender, EventArgs e) { }
		static void _LogRequest(object sender, EventArgs e) { }
		static void _PostLogRequest(object sender, EventArgs e) { }
        static void _EndRequest(object sender, EventArgs e) { }
		static void _PreSendRequestContent(object sender, EventArgs e) { }
        static void _PreSendRequestHeaders(object sender, EventArgs e) { }
		static void _Error(object sender, EventArgs e) { }
		static void _End(object sender, EventArgs e) { }


		//[api.http("~/debug/ping"), api.CommandQueue(ByCommand = true, ByUser = false)]
		//static object _ping(_HttpContext context, SqlCmd sqlcmd, string msg, string[] msgs, int? n, [api.JObject] dynamic dynamic_JObject, [api.JArray] dynamic dynamic_JArray, JObject _JObject, JArray _JArray)
		//{
		//	return msg;
		//}

		//[api.http("~/debug/comet"), api.CommandQueue(ByCommand = false, ByUser = false)]
		//static void xxx(_HttpContext context, SqlCmd sqlcmd, string msg, string[] msgs, int? n, [api.JObject] dynamic dynamic_JObject, [api.JArray] dynamic dynamic_JArray, JObject _JObject, JArray _JArray)
		//{
		//	apiRoute.Handler.Current.WaitMessage(3000, (Action)aaa);
		//}

		static void aaa()
		{
		}
	}

	#region Attributes

	partial class api
	{
        [DebuggerStepThrough, AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class httpAttribute : Attribute
        {
            public string Name { get; set; }
            public httpAttribute(string name = null)
            {
                this.Name = name;
            }
        }

        [DebuggerStepThrough, AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class CommandQueueAttribute : Attribute
        {
            public bool ByUser { get; set; }
            public bool ByCommand { get; set; }
            public CommandQueueAttribute()
            {
                this.ByUser = true;
                this.ByCommand = false;
            }
        }

        [DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
		public class argAttribute : Attribute
		{
			public string Name { get; set; }
			public argAttribute() { }
			public argAttribute(string name) { this.Name = name; }
		}

        [DebuggerStepThrough, AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
		public class JObjectAttribute : Attribute { }

        [DebuggerStepThrough, AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
		public class JArrayAttribute : Attribute { }

        [DebuggerStepThrough, AttributeUsage(AttributeTargets.Parameter)]
		public sealed class SqlCmdAttribute : Attribute
		{
			public string ConfigKey { get; set; }
			public string ConnectionString { get; set; }
			public SqlCmdAttribute(string configKey = null, string connectionString = null)
			{
				this.ConfigKey = configKey;
				this.ConnectionString = connectionString;
			}
		}

        [DebuggerStepThrough, AttributeUsage(AttributeTargets.Parameter)]
		public sealed class RedisAttribute : Attribute
		{
			public string ConfigKey { get; set; }
			public string ConnectionString { get; set; }
			public RedisAttribute(string configKey = null, string connectionString = null)
			{
				this.ConfigKey = configKey;
				this.ConnectionString = connectionString;
			}
		}
	}

	#endregion

    #region Config

    public class apiPathSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
		public apiPathElementCollection Instances
		{
			get { return (apiPathElementCollection)this[""]; }
			set { this[""] = value; }
		}
	}

	public class apiPathElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new apiPathElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((apiPathElement)element).Path;
		}
	}

	public class apiPathElement : ConfigurationElement
	{
		[ConfigurationProperty("path", IsKey = true, IsRequired = true)]
		public string Path
		{
			get { return (string)base["path"]; }
			set { base["path"] = value; }
		}

		[ConfigurationProperty("type", IsRequired = true)]
		public apiPathType Type
		{
			get { return (apiPathType)base["type"]; }
			set { base["type"] = value; }
		}
	}

	public enum apiPathType
	{
		allow, deny
	}

	#endregion
}