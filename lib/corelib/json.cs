using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    public static class json
    {
        public static bool MapName
        {
            get { return _ContractResolver.Instance.MapName; }
            set { _ContractResolver.Instance.MapName = value; }
        }

        #region Serialize / Deserialize

        public static JObject ToJObject(string json_string)
        {
            if (string.IsNullOrEmpty(json_string)) return null;
            using (StringReader r1 = new StringReader(json_string))
            using (_Reader r2 = new _Reader(r1))
                return JObject.Load(r2);
        }

        public static JArray ToJArray(string json_string)
        {
            if (string.IsNullOrEmpty(json_string)) return null;
            using (StringReader r1 = new StringReader(json_string))
            using (_Reader r2 = new _Reader(r1))
                return JArray.Load(r2);
        }

        public static string SerializeObject(object value, Formatting formatting = Formatting.None, bool quoteName = true, char? quoteChar = null)
        {
            return json.SerializeObject(new StringBuilder(), value, formatting, quoteName, quoteChar);
        }
        public static string SerializeObject(StringBuilder str, object value, Formatting formatting = Formatting.None, bool quoteName = true, char? quoteChar = null)
        {
            if (value == null) return null;
            str = str ?? new StringBuilder();
            using (StringWriter w1 = new StringWriter(str))
            using (_Writer w2 = new _Writer(w1))
            {
                if (quoteChar.HasValue)
                    w2.QuoteChar = quoteChar.Value;
                w2.QuoteName = quoteName;
                w2.Formatting = formatting;
                _Serializer.Instance1.Serialize(w2, value);
            }
            return str.ToString();
        }

        public static bool DeserializeObject(Type type, string json_string, out object result)
        {
            if (string.IsNullOrEmpty(json_string))
            {
                result = null;
                return false;
            }
            using (StringReader r1 = new StringReader(json_string))
            using (_Reader r2 = new _Reader(r1))
                result = _Serializer.Instance1.Deserialize(r2, type);
            return result != null;
        }

        public static object DeserializeObject(Type type, string json_string)
        {
            object result;
            DeserializeObject(type, json_string, out result);
            return result;
        }

        public static T DeserializeObject<T>(string json_string)
        {
            if (json_string == null) return default(T);
            using (StringReader r1 = new StringReader(json_string))
            using (_Reader r2 = new _Reader(r1))
                return _Serializer.Instance1.Deserialize<T>(r2);
        }

        public static bool PopulateObject(string json_string, object obj)
        {
            if (json_string == null) return false;
            using (StringReader r1 = new StringReader(json_string))
            using (_Reader r2 = new _Reader(r1))
                _Serializer.Instance1.Populate(r2, obj);
            return true;
        }

        #endregion

        //internal static void Initialize()
        //{
        //    ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
        //    ValueProviderFactories.Factories.Add(new _ValueProviderFactory());
        //}

        //class _ValueProviderFactory : ValueProviderFactory
        //{
        //    public override System.Web.Mvc.IValueProvider GetValueProvider(ControllerContext controllerContext)
        //    {
        //        // first make sure we have a valid context
        //        if (controllerContext == null)
        //            throw new ArgumentNullException("controllerContext");

        //        // now make sure we are dealing with a json request
        //        if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
        //            return null;

        //        // use JSON.NET to deserialize object to a dynamic (expando) object
        //        Object json_obj;

        //        // get a generic stream reader (get reader for the http stream)
        //        using (StreamReader streamReader = new StreamReader(controllerContext.HttpContext.Request.InputStream))
        //        {
        //            // convert stream reader to a JSON Text Reader
        //            using (json._Reader reader = new json._Reader(streamReader))
        //            {
        //                if (!reader.Read())
        //                    return null;

        //                if (reader.TokenType == JsonToken.StartArray)
        //                    json_obj = json._Serializer.Instance2.Deserialize<List<ExpandoObject>>(reader);
        //                else
        //                    json_obj = json._Serializer.Instance2.Deserialize<ExpandoObject>(reader);
        //            }
        //        }

        //        // create a backing store to hold all properties for this deserialization
        //        Dictionary<string, object> backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        //        // add all properties to this backing store
        //        AddToBackingStore(backingStore, String.Empty, json_obj);

        //        // return the object in a dictionary value provider so the MVC understands it
        //        return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
        //    }

        //    private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
        //    {
        //        IDictionary<string, object> d = value as IDictionary<string, object>;
        //        if (d != null)
        //        {
        //            foreach (KeyValuePair<string, object> entry in d)
        //            {
        //                AddToBackingStore(backingStore, MakePropertyKey(prefix, entry.Key), entry.Value);
        //            }
        //            return;
        //        }

        //        IList l = value as IList;
        //        if (l != null)
        //        {
        //            for (int i = 0; i < l.Count; i++)
        //            {
        //                AddToBackingStore(backingStore, MakeArrayKey(prefix, i), l[i]);
        //            }
        //            return;
        //        }

        //        backingStore[prefix] = value;
        //    }

        //    private static string MakeArrayKey(string prefix, int index)
        //    {
        //        return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        //    }

        //    private static string MakePropertyKey(string prefix, string propertyName)
        //    {
        //        return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
        //    }
        //}

        #region Serializer / Reader / Writer / ContractResolver

        [_DebuggerStepThrough]
        public class _Reader : JsonTextReader
        {
            public _Reader(TextReader reader) : base(reader) { }

            public override bool Read()
            {
                bool result = base.Read();
                if (this.TokenType == JsonToken.Date && this.Value is DateTime)
                {
                    try
                    {
                        DateTime t = (DateTime)this.Value ;
                        base.SetToken(this.TokenType, t.ToLocalTime());
                    }
                    catch { }
                }
                return result;
            }

            //T _Read<T>(Func<T> r) where T : class
            //{
            //    try { return r(); }
            //    catch { SetToken(JsonToken.Null); return null; }
            //}

            //T? _Read<T>(Func<T?> r) where T : struct
            //{
            //    try { return r(); }
            //    catch { SetToken(JsonToken.Null); return null; }
            //}

            //public override string ReadAsString()
            //{
            //    return base.ReadAsString();
            //}

            //public override bool? ReadAsBoolean()
            //{
            //    return _Read(base.ReadAsBoolean);
            //}

            //public override DateTime? ReadAsDateTime()
            //{
            //    return _Read(base.ReadAsDateTime);
            //}

            //public override DateTimeOffset? ReadAsDateTimeOffset()
            //{
            //    return _Read(base.ReadAsDateTimeOffset);
            //}

            //public override decimal? ReadAsDecimal()
            //{
            //    return _Read(base.ReadAsDecimal);
            //}

            //public override double? ReadAsDouble()
            //{
            //    return _Read(base.ReadAsDouble);
            //}

            //public override int? ReadAsInt32()
            //{
            //    return _Read(base.ReadAsInt32);
            //}

            //public override byte[] ReadAsBytes()
            //{
            //    return _Read(base.ReadAsBytes);
            //}
        }

        [_DebuggerStepThrough]
        public class _Writer : JsonTextWriter
        {
            public _Writer(TextWriter textWriter)
                : base(textWriter)
            {
                base.QuoteChar = '\"';
                base.QuoteName = true;
                base.Formatting = Formatting.None;
            }

            public override void WriteValue(DateTime value)
            {   // protocol 的日期一律轉換成 utc
                base.WriteValue(value.ToUniversalTime());
            }

            //public override void WriteValue(DateTime? value)
            //{
            //    DateTime? value2;
            //    if (value.HasValue)
            //        value2 = value.Value.ToUniversalTime();
            //    else
            //        value2 = value;
            //    base.WriteValue(value2);
            //}
        }

        public static JsonSerializer GetJsonSerializer(bool expandoObject = false)
        {
            return expandoObject ? _Serializer.Instance1 : _Serializer.Instance2;
        }

        [_DebuggerStepThrough]
        internal class _Serializer : JsonSerializer
        {
            public static readonly _Serializer Instance1 = new _Serializer(false);
            public static readonly _Serializer Instance2 = new _Serializer(true);

            public _Serializer(bool expandoObject = false)
            {
                if (expandoObject)
                    base.Converters.Add(new ExpandoObjectConverter());
                base.NullValueHandling = NullValueHandling.Ignore;
                base.ContractResolver = _ContractResolver.Instance;
            }
        }

        [_DebuggerStepThrough]
        class _ContractResolver : DefaultContractResolver
        {
            public static readonly _ContractResolver Instance = new _ContractResolver();

            public bool MapName = true;

            //protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
            //{
            //    JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);
            //    UnderlyingValueInDictionaryKeyAttribute.Apply(contract);
            //    return contract;
            //}

            //protected override JsonContract CreateContract(Type objectType)
            //{
            //    JsonContract c = base.CreateContract(objectType);
            //    if (objectType.IsEnum)
            //    {
            //        c.Converter = c.Converter ?? StringEnumAttribute.GetStringEnumConverter(objectType.GetCustomAttribute<StringEnumAttribute>()); ;
            //        //if (c.Converter is json.StringEnumConverter) { }
            //        //else
            //        //    c.Converter = StringEnumAttribute.GetStringEnumConverter(objectType.GetCustomAttribute<StringEnumAttribute>());
            //    }
            //    return c;
            //}

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty p = base.CreateProperty(member, memberSerialization);
                if (MapName == false)
                    p.PropertyName = member.Name;
                Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                if (t.IsEnum)
                {
                    JsonContract cc = this.ResolveContract(t);
                    p.Converter = p.Converter ?? cc.Converter ?? StringEnumAttribute.GetStringEnumConverter(member.GetCustomAttribute<StringEnumAttribute>() ?? t.GetCustomAttribute<StringEnumAttribute>());
                    //p.Converter = p.Converter ?? StringEnumAttribute.GetStringEnumConverter(member.GetCustomAttribute<StringEnumAttribute>() ?? t.GetCustomAttribute<StringEnumAttribute>());
                }
                else if (t == typeof(string))
                {
                    p.Converter = p.Converter ?? new StringJsonConverter(member, p);
                }
                if (p.MemberConverter == null)
                {
                    if (t == typeof(Guid))
                        p.MemberConverter = new GuidJsonConverter();
                    //    else if (t == typeof(Boolean))
                    //        p.MemberConverter = new BooleanJsonConverter();
                    //    else if (t == typeof(string))
                    //        p.MemberConverter = new StringJsonConverter(member, p);
                }
                return p;
            }
        }

        #endregion

        #region Converters

        [_DebuggerStepThrough]
        public class BooleanJsonConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.Value is Boolean)
                    return reader.Value;
                string input = reader.Value as string;
                if (string.IsNullOrEmpty(input))
                    return null;
                input = input.ToLower();
                if (Regex.IsMatch(input, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
                    return false;
                if (Regex.IsMatch(input, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
                    return true;
                return null;
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public class StringAttribute : Attribute
        {
            public bool Trim = false;
            public bool Empty = false;
        }

        [_DebuggerStepThrough]
        public class StringJsonConverter : JsonConverter
        {
            static int _id = 0;
            readonly int ID = Interlocked.Increment(ref _id);
            readonly JsonProperty p;
            readonly StringAttribute a;

            public StringJsonConverter() : this(null, null) { }
            public StringJsonConverter(MemberInfo member, JsonProperty p)
            {
                this.p = p;
                if (member != null)
                    foreach (StringAttribute a in member.GetCustomAttributes<StringAttribute>(true))
                        this.a = a;
                this.a = this.a ?? new StringAttribute() { Trim = true, Empty = false };
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                object value = serializer.Deserialize(reader);
                string s = value as string;
                if (s != null)
                {
                    if (this.a.Trim)
                        s = s.Trim();
                    if ((s == string.Empty) && (this.a.Empty == false))
                        s = null;
                }
                return s;
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        //[_DebuggerStepThrough]
        //public class Int32JsonConverter : JsonConverter
        //{
        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override bool CanConvert(Type objectType)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        [_DebuggerStepThrough]
        public class GuidJsonConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                try { return new Guid(reader.Value as string); }
                catch { if (objectType.IsNullable()) return null; else return Guid.Empty; }
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Enum)]
        public class UnderlyingValueInDictionaryKeyAttribute : Attribute
        {
            static List<UnderlyingValueInDictionaryKeyAttribute> cache = new List<UnderlyingValueInDictionaryKeyAttribute>();

            public static void Apply(JsonDictionaryContract contract)
            {
                if (contract == null) return;
                if (!contract.DictionaryKeyType.IsEnum) return;
                //if (contract.PropertyNameResolver != null) return;
                lock (cache)
                {
                    foreach (UnderlyingValueInDictionaryKeyAttribute a in cache)
                    {
                        if (a.type != contract.DictionaryKeyType) continue;
                        contract.DictionaryKeyResolver = a.ResolvePropertyName;
                        //contract.PropertyNameResolver = a.ResolvePropertyName;
                        return;
                    }
                    foreach (UnderlyingValueInDictionaryKeyAttribute a in contract.DictionaryKeyType.GetCustomAttributes<UnderlyingValueInDictionaryKeyAttribute>(true))
                    {
                        a.type = contract.DictionaryKeyType;
                        contract.DictionaryKeyResolver = a.ResolvePropertyName;
                        //contract.PropertyNameResolver = a.ResolvePropertyName;
                        cache.Add(a);
                        return;
                    }
                }
            }

            Type type;
            string ResolvePropertyName(string propertyName)
            {
                if (Enum.IsDefined(type, propertyName))
                {
                    object o1 = Enum.Parse(type, propertyName);
                    object o2 = Convert.ChangeType(o1, Enum.GetUnderlyingType(type));
                    return Convert.ToString(o2);
                }
                return propertyName;
            }
        }

        public class StringEnumConverter : Newtonsoft.Json.Converters.StringEnumConverter
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                try { return base.ReadJson(reader, objectType, existingValue, serializer); }
                catch { return null; }
            }
        }

        #endregion

        //REF: http://kirkchen.logdown.com/posts/146604-using-aspnet-mvc-to-create-web-api-13-use-jsonnet-parse-json
        //public class JsonNetResult : JsonResult
        //{
        //    public Formatting Formatting { get; set; }

        //    public JsonNetResult() { }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        if (context == null) throw new ArgumentNullException("context");
        //        HttpResponseBase response = context.HttpContext.Response;
        //        response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
        //        if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;
        //        if (Data != null)
        //        {
        //            using (json._Writer writer = new json._Writer(response.Output) { Formatting = this.Formatting })
        //            {
        //                json._Serializer.Instance1.Serialize(writer, Data);
        //                writer.Flush();
        //            }
        //        }
        //    }
        //}

        //public class Controller : System.Web.Mvc.Controller
        //{
        //    protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        //    {
        //        if (behavior == JsonRequestBehavior.DenyGet && string.Equals(this.Request.HttpMethod, "GET",
        //                 StringComparison.OrdinalIgnoreCase))
        //            //Call JsonResult to throw the same exception as JsonResult
        //            return new JsonResult();
        //        return new JsonNetResult()
        //        {
        //            Data = data,
        //            ContentType = contentType,
        //            ContentEncoding = contentEncoding
        //        };
        //    }
        //}

        //public class ApiController : System.Web.Http.ApiController
        //{
        //}

        //public class ApiController : System.Web.Http.ApiController
        //{
        //    protected override JsonResult<T> Json<T>(T content, JsonSerializerSettings serializerSettings, Encoding encoding)
        //    {
        //        return base.Json<T>(content, serializerSettings, encoding);
        //    }
        //}
    }

    public class JsonNetResult : System.Web.Mvc.JsonResult
    {
        public override void ExecuteResult(System.Web.Mvc.ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == System.Web.Mvc.JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var scriptSerializer = json.GetJsonSerializer();
            scriptSerializer.Serialize(response.Output, this.Data);
        }
    }

    public class JsonHandlerAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            var jsonResult = filterContext.Result as System.Web.Mvc.JsonResult;

            if (jsonResult != null)
            {
                filterContext.Result = new JsonNetResult
                {
                    ContentEncoding = jsonResult.ContentEncoding,
                    ContentType = jsonResult.ContentType,
                    Data = jsonResult.Data,
                    JsonRequestBehavior = jsonResult.JsonRequestBehavior
                };
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
namespace Newtonsoft.Json.Converters
{

    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
    public class StringEnumAttribute : Attribute
    {
        public bool AsString { get; set; }

        public StringEnumAttribute(bool asString = true)
        {
            this.AsString = asString;
        }

        internal static StringEnumConverter GetStringEnumConverter(StringEnumAttribute attr)
        {
            if (attr == null)
                return new ams.json.StringEnumConverter();
            else if (attr.AsString)
                return new ams.json.StringEnumConverter();
            return null;
        }
    }
}