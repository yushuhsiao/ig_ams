using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace ams
{
    using System.Web.Mvc;

    [_DebuggerStepThrough]
    public abstract class _Controller : Controller
    {
        public const string url_AddRow = "/AddRow";
        public const string url_Details = "/Details";

        public _Controller()
        {
            this._HttpContext = _HttpContext.Current;
        }

        public ActionDescriptor ActionDescriptor
        {
            get; private set;
        }

        public _HttpContext _HttpContext
        {
            get; private set;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.ActionDescriptor = filterContext.ActionDescriptor;
            this._HttpContext = filterContext.HttpContext as _HttpContext;
            base.OnActionExecuting(filterContext);
        }

        public bool IsDetails;
        public bool IsAddRow;

        protected ActionResult View_Details(Func<ActionResult> cb) { this.IsDetails = true; return cb(); }
        protected ActionResult View_AddRow(Func<ActionResult> cb) { this.IsAddRow = true; return cb(); }

        //protected bool IsDetail()
        //{
        //    for (int i = 0; i < this.Request.QueryString.Count; i++)
        //        if (string.Compare(this.Request.QueryString.Get(i), "details", true) == 0)
        //            return true;
        //    return false;
        //}

        //protected override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    this.ActionDescriptor = filterContext.ActionDescriptor;
        //    base.OnAuthorization(filterContext);
        //}
    }
}
namespace ams
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    [_DebuggerStepThrough]
    public abstract class _ApiController : ApiController
    {
        public _ApiController()
        {
            this._HttpContext = _HttpContext.Current;
        }

        public _HttpContext _HttpContext
        {
            get; private set;
        }

        [DebuggerStepThrough]
        protected T Null<T>(T args)
        {
            if (args == null)
                throw new _Exception(Status.InvalidParameter);
            return args;
        }

        protected T Validate<T>(bool _json, T args, Action valid)
        {
            if (_json)
                json.PopulateObject(_HttpContext.Arguments, this);
            else
                Null<T>(args);
            (valid ?? _null.noop)();
            this.ModelState.IsValid();
            return args;
        }

        protected T Validate<T>(T args, Action valid) => this.Validate<T>(false, args, valid);

        //protected ListRequest_2<T> Validate<T>(ListRequest_2<T> args, bool valid_user = true) => ListRequest_2<T>.Valid(ModelState, args, valid_user);
    }

    //[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public class ControllerArgumentsAttribute : Attribute { }

    [_DebuggerStepThrough]
    class ModelStateJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ModelStateDictionary);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue ?? serializer.Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ModelStateDictionary)
            {
                ModelStateDictionary m1 = (ModelStateDictionary)value;
                writer.WriteStartObject();
                foreach (var m2 in m1)
                {
                    writer.WritePropertyName(m2.Key);
                    if (m2.Value.Errors != null)
                    {
                        writer.WriteStartArray();
                        foreach (var m3 in m2.Value.Errors)
                        {
                            if (m3.Exception is _Exception)
                            serializer.Serialize(writer, m3.Exception);
                        }
                        writer.WriteEndArray();
                        continue;
                    }
                    serializer.Serialize(writer, m2.Value);
                    //writer.WriteValue(m2.Value);
                }
                writer.WriteEndObject();
            }
            else serializer.Serialize(writer, value);
        }
    }

}

