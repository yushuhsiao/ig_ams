using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;
using Tools;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace casino
{
	[_DebuggerStepThrough, DebuggerDisplay("{path}")]
	public class apiRoute : IRouteHandler//, IHttpAsyncHandler, IRequiresSessionState
	{
		#region writelog

		const string prefix = "api";

		public static void writelog(_HttpContext context, string msg)
		{
			log.message(prefix, "{0}\t{1}", context.RequestIP, msg);
		}

		public static void writelog(_HttpContext context, string format, params object[] args)
		{
			writelog(context, string.Format(format, args));
		}

		#endregion

		static int _id;
		internal readonly int ID = Interlocked.Increment(ref _id);
		readonly MethodInfo method;
		readonly api.httpAttribute attr;
		public readonly string path;
		readonly _parameter_info[] param;
		readonly _field_info[] fields;
		readonly _property_info[] properties;
		//readonly CommandQueue2.List command_queue2;
		internal readonly RouteData RouteData;
		readonly api.CommandQueueAttribute command_queue_attr;

		internal apiRoute(api.router router, MethodInfo method, api.httpAttribute attr, string path)
		{
			if (method == null || attr == null || path == null) throw new ArgumentNullException();
			this.method = method;
			this.attr = attr;
			this.path = path;
			ParameterInfo[] param = method.GetParameters();
			this.param = new _parameter_info[param.Length];
			for (int i = 0; i < param.Length; i++)
				this.param[i] = new _parameter_info(param[i]);
			List<_field_info> fields = new List<_field_info>();
			List<_property_info> properties = new List<_property_info>();
			if (!method.IsStatic)
			{
				foreach (FieldInfo f1 in method.DeclaringType.GetFields(api._BindingFlags))
				{
					var f2 = new _field_info(f1);
					if (f2.arg != null) fields.Add(f2);
				}

				foreach (PropertyInfo p1 in method.DeclaringType.GetProperties(api._BindingFlags))
				{
					var p2 = new _property_info(p1);
					if (p2.arg != null) properties.Add(p2);
				}
			}
			this.fields = fields.ToArray();
			this.properties = properties.ToArray();
			this.command_queue_attr = this.method.GetCustomAttribute<api.CommandQueueAttribute>() ?? new api.CommandQueueAttribute();
			//if (command_queue_attr.ByCommand)
			//	this.command_queue2 = new CommandQueue2.List();
			//else
			//	this.command_queue2 = CommandQueue2.global;

			this.RouteData = new RouteData(router, this);
		}

		IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext) { return new Handler(requestContext, this); }

		public class Handler : IHttpAsyncHandler, IRequiresSessionState, IDisposable
		{
			readonly apiRoute route;
			_HttpContext context;
			AsyncCallback cb;
			object extraData;
			long tick1;
			double Elapsed
			{
				get { return TimeSpan.FromTicks(DateTime.Now.Ticks - Interlocked.Read(ref tick1)).TotalMilliseconds; }
			}

			internal Handler(RequestContext requestContext, apiRoute owner)
			{
				this.route = owner;
				IDictionary items = requestContext.HttpContext.Items;
				lock (items) items[typeof(Handler)] = this;
			}

			void IDisposable.Dispose()
			{
				(Interlocked.Exchange(ref this.cb, null) ?? _null.noop)(AsyncResult.NotCompleted);
				if (this.context != null)
				{
					IDictionary items = this.context.Items;
					lock (items)
						if (this.context.Items.Contains(typeof(Handler)))
							this.context.Items.Remove(typeof(Handler));
				}
			}

			public static Handler Current
			{
				get { IDictionary items = _HttpContext.Current.Items; lock (items) return items[typeof(Handler)] as Handler; }
			}

			IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext _context, AsyncCallback cb, object extraData)
			{
				_HttpContext context = this.context = _HttpContext.GetContext(_context);
				User user = context._User;
				Interlocked.Exchange(ref this.cb, cb);
				this.extraData = extraData;
				Interlocked.Exchange(ref tick1, DateTime.Now.Ticks);

				bool f1 = this.route.command_queue_attr.ByUser;
				bool f2 = this.route.command_queue_attr.ByCommand;
				(f1 ? user : User.Default).QueueCommand(f2 ? route : null, this);
				return AsyncResult.NotCompleted;
			}

			void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result) { }

			bool IHttpHandler.IsReusable { get { return false; } }

			void IHttpHandler.ProcessRequest(HttpContext context) { }

			long wait_time;
			Delegate wait_cb;
			object[] wait_args;

			public void WaitMessage(int timeout, Delegate callback, params object[] args)
			{
				if (Interlocked.CompareExchange(ref wait_cb, callback, null) == null)
				{
					Interlocked.Exchange(ref this.wait_time, DateTime.Now.AddMilliseconds((double)timeout).Ticks);
					Interlocked.Exchange(ref this.wait_args, args);
				}
			}

			internal void ProcessRequest()
			{
				try
				{
					_HttpContext.Current = this.context;
					context.Response.ContentType = "text/json";
					NameValueCollection request_args = this.context.Request.Form ?? this.context.Request.QueryString;
					if (request_args.Count == 0) request_args = this.context.Request.QueryString;
					object obj = null;
					object[] args = new object[this.route.param.Length];
					if (!this.route.method.IsStatic)
						obj = Activator.CreateInstance(this.route.method.DeclaringType);
					for (int i = 0; i < this.route.param.Length; i++)
						args[i] = this.route.param[i].DeserializeValue(this.context, request_args);
					for (int i = 0; i < this.route.fields.Length; i++)
						try { this.route.fields[i].src.SetValue(obj, this.route.fields[i].DeserializeValue(this.context, request_args)); }
						catch { }
					for (int i = 0; i < this.route.properties.Length; i++)
						try { this.route.properties[i].src.SetValue(obj, this.route.properties[i].DeserializeValue(this.context, request_args)); }
						catch { }
					try
					{
						object data = this.route.method.Invoke2(obj, args);
						Delegate wait_cb = Interlocked.CompareExchange(ref this.wait_cb, null, null);
						using (this) this.WriteResult(data);
					}
					catch (api.Exception ex)
					{
						using (this) this.WriteResult(ex);
						this.context.SetResponseStatusCode(ex.HttpStatusCode);
					}
				}
				catch (Exception ex)
				{
					using (this)
					{
						this.context.SetResponseStatusCode(HttpStatusCode.InternalServerError);
						writelog(this.context, ex.Message);
					}
				}
			}

			void WriteResult(object data)
			{
				if (data is api.result)
					((api.result)data)._elapsed = this.Elapsed;
				else if (data is api.Exception)
					((api.Exception)data)._elapsed = this.Elapsed;
				else data = new
				{
					status = Status.Success,
					data = data,
					//_elapsed = this.Elapsed
				};
				string json_r = json.SerializeObject(data);
				this.context.Response.Write(json_r);
			}
		}

		//public abstract class HttpTaskAsyncHandler : IHttpAsyncHandler
		//{
		//	IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
		//	{
		//		Task task = ProcessRequest(context);
		//		if (task == null)
		//		{
		//			return null;
		//		}

		//		TaskWrapperAsyncResult resultToReturn = new TaskWrapperAsyncResult(task, extraData);

		//		bool actuallyCompletedSynchronously = task.IsCompleted;
		//		if (actuallyCompletedSynchronously)
		//		{
		//			resultToReturn.ForceCompletedSynchronously();
		//		}

		//		if (cb != null)
		//		{
		//			if (actuallyCompletedSynchronously)
		//			{
		//				cb(resultToReturn);
		//			}
		//			else
		//			{
		//				task.ContinueWith(_ => cb(resultToReturn));
		//			}
		//		}

		//		return resultToReturn;
		//	}

		//	public async Task ProcessRequest(HttpContext context)
		//	{
		//	}

		//	void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
		//	{
		//		if (result == null)
		//		{
		//			throw new ArgumentNullException("ar");
		//		}

		//		// Make sure the incoming parameter is actually the correct type.
		//		TaskWrapperAsyncResult taskWrapper = result as TaskWrapperAsyncResult;
		//		if (taskWrapper == null)
		//		{
		//			// extraction failed
		//			//throw new ArgumentException(SR.GetString(SR.TaskAsyncHelper_ParameterInvalid), "ar");
		//		}

		//		// The End* method doesn't actually perform any actual work, but we do need to maintain two invariants:
		//		// 1. Make sure the underlying Task actually *is* complete.
		//		// 2. If the Task encountered an exception, observe it here.
		//		// (TaskAwaiter.GetResult() handles both of those, and it rethrows the original exception rather than an AggregateException.)
		//		taskWrapper.Task.GetAwaiter().GetResult();
		//	}

		//	bool IHttpHandler.IsReusable { get { return false; } }

		//	void IHttpHandler.ProcessRequest(HttpContext context) { }
		//}

		//internal sealed class TaskWrapperAsyncResult : IAsyncResult
		//{
		//	private bool _forceCompletedSynchronously;

		//	internal TaskWrapperAsyncResult(Task task, object asyncState)
		//	{
		//		Task = task;
		//		AsyncState = asyncState;
		//	}

		//	public object AsyncState
		//	{
		//		get;
		//		private set;
		//	}

		//	public WaitHandle AsyncWaitHandle
		//	{
		//		get { return ((IAsyncResult)Task).AsyncWaitHandle; }
		//	}

		//	public bool CompletedSynchronously
		//	{
		//		get { return _forceCompletedSynchronously || ((IAsyncResult)Task).CompletedSynchronously; }
		//	}

		//	public bool IsCompleted
		//	{
		//		get { return ((IAsyncResult)Task).IsCompleted; }
		//	}

		//	internal Task Task
		//	{
		//		get;
		//		private set;
		//	}

		//	internal void ForceCompletedSynchronously()
		//	{
		//		_forceCompletedSynchronously = true;
		//	}

		//}


		#region class _arg_info

		[_DebuggerStepThrough, DebuggerDisplay("{src.Name}")]
		abstract partial class _arg_info<T>
		{
			public api.argAttribute arg;
			public T src;
			public readonly string Name;
			public readonly Type ValueType;
			//public readonly bool arg;
			public readonly JsonObjectAttribute JsonObject;
			public readonly bool JObject;
			public readonly bool JArray;
			public readonly api.SqlCmdAttribute SqlCmd;
			public readonly api.RedisAttribute Redis;

			public _arg_info(T src, string name, Type valueType)
			{
				this.src = src;
				this.Name = name;
				this.ValueType = valueType;
				ICustomAttributeProvider cc = (ICustomAttributeProvider)src;
				foreach (api.argAttribute a in cc.GetCustomAttributes(typeof(api.argAttribute), false))
				{
					this.arg = a;
					this.Name = a.Name ?? name;
					break;
				}

				//this.arg = p.GetCustomAttribute<argAttribute>() != null;
				foreach (api.SqlCmdAttribute a in cc.GetCustomAttributes(typeof(api.SqlCmdAttribute), true))
				{ this.SqlCmd = a; break; }
				this.JsonObject = valueType.GetCustomAttribute<JsonObjectAttribute>();
				this.JObject = valueType == typeof(JObject);
				this.JArray = valueType == typeof(JArray);
				if (valueType == typeof(object))
				{
					foreach (api.JObjectAttribute a in cc.GetCustomAttributes(typeof(api.JObjectAttribute), true))
					{ this.JObject |= true; break; }
					foreach (api.JArrayAttribute a in cc.GetCustomAttributes(typeof(api.JArrayAttribute), true))
					{ this.JArray |= true; break; }
					//this.JObject |= cc.GetCustomAttribute<JObjectAttribute>() != null;
					//this.JArray |= cc.GetCustomAttribute<JArrayAttribute>() != null;
				}
			}

			public object DeserializeValue(_HttpContext context, NameValueCollection args)
			{
				try
				{
					string arg_n = args[Name];
					if (arg_n != null)
					{
						arg_n = arg_n.Trim();
						int len = arg_n.Length;
						if (len >= 2 && arg_n[0] == '"' && arg_n[len - 1] == '"')
							arg_n = arg_n.Substring(1, len - 2);
					}

					if (ValueType == typeof(HttpContext))
						return context._context;
					if (ValueType == typeof(_HttpContext))
						return context;
					if (ValueType == typeof(User) || ValueType.IsSubclassOf(typeof(User)))
						return context._User;
					if (ValueType == typeof(SqlCmd) && this.SqlCmd != null)
						return context.GetSqlCmd(this.SqlCmd.ConfigKey, this.SqlCmd.ConnectionString);
					if (ValueType == typeof(RedisClient) && this.Redis != null)
						return context.GetRedis(this.Redis.ConfigKey, this.Redis.ConnectionString);

					if (arg_n == null)
						return null;
					if (ValueType == typeof(string))
						return arg_n;
					if (ValueType == typeof(string[]))
						return json.DeserializeObject(ValueType, arg_n);
					if (this.JObject)
						return json.ToJObject(arg_n);
					if (this.JArray)
						return json.ToJArray(arg_n);
					if (this.JsonObject != null)
						return json.DeserializeObject(ValueType, arg_n);

					TypeConverter c = TypeDescriptor.GetConverter(ValueType);
					if (c != null)
						if (c.CanConvertFrom(typeof(string)))
							return c.ConvertFrom(arg_n);
				}
				catch { }
				return null;
			}
		}
		sealed class _parameter_info : _arg_info<ParameterInfo> { public _parameter_info(ParameterInfo p) : base(p, p.Name, p.ParameterType) { } }
		sealed class _field_info : _arg_info<FieldInfo> { public _field_info(FieldInfo f) : base(f, f.Name, f.FieldType) { } }
		sealed class _property_info : _arg_info<PropertyInfo> { public _property_info(PropertyInfo p) : base(p, p.Name, p.PropertyType) { } }

		#endregion

		#region api_list.cshtml

		[_DebuggerStepThrough]
		public static IEnumerable<apiRoute> GetHandlers()
		{
			lock (api.router.Items)
				foreach (RouteData r in api.router.Items.Values)
					if (r.RouteHandler is apiRoute)
						yield return (apiRoute)r.RouteHandler;
		}

		partial class _arg_info<T>
		{
			public bool test_form(out dynamic n)
			{
				n = null;
				if (ValueType.IsSubclassOf(typeof(User))
					|| ValueType == typeof(HttpContext)
					|| ValueType == typeof(_HttpContext)
					|| ValueType == typeof(User)
					|| ValueType == typeof(SqlCmd)
					|| ValueType == typeof(redis.RedisClient))
					return false;
				dynamic obj = new System.Dynamic.ExpandoObject();
				obj.Name = this.Name;
				obj.ValueType = ValueType;
				if (false
					|| (ValueType == typeof(string))
					|| (ValueType.IsNullable() && Nullable.GetUnderlyingType(ValueType).IsPrimitive)
					|| (ValueType.IsPrimitive)
					|| (ValueType == typeof(casino.VirtualPath)))
					obj.InputType = "textbox";
				else
					obj.InputType = "textarea";
				n = obj;
				return true;
			}
		}

		public IEnumerable<dynamic> test_form()
		{
			dynamic n;
			for (int i = 0; i < this.param.Length; i++)
				if (param[i].test_form(out n))
					yield return n;
			for (int i = 0; i < this.fields.Length; i++)
				if (fields[i].test_form(out n))
					yield return n;
			for (int i = 0; i < this.properties.Length; i++)
				if (properties[i].test_form(out n))
					yield return n;
		}

		#endregion
	}
	partial class api
	{
		#region class result

		public class result : System.Dynamic.DynamicObject
		{
			public Status status = Status.Success;
			public double _elapsed;
			public object data;
			Dictionary<string, object> values;

			public override IEnumerable<string> GetDynamicMemberNames()
			{
				yield return "status";
				yield return "data";
				//yield return "_elapsed";
				if (this.values != null)
					foreach (string s in this.values.Keys)
						yield return s;
				//yield return "status";
				//return base.GetDynamicMemberNames();
			}

			public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
			{
				if (this.values == null)
					result = null;
				else
					this.values.TryGetValue(binder.Name, out result);
				return true;
			}

			public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
			{
				if (this.values == null)
					this.values = new Dictionary<string, object>();
				this.values[binder.Name] = value;
				return true;
			}
		}

		#endregion

	}
}