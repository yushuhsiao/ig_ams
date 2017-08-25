using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    public static class _MediaTypeFormatters
    {
        /// <summary>
        /// 將 webapi 預設的 json 處理器改為 Newtonsoft.Json
        /// </summary>
        /// <param name="config"></param>
        public static void amsInitMediaTypeFormatters(this HttpConfiguration config)
        {
            for (int i = config.Formatters.Count - 1; i >= 0; i--)
            {
                MediaTypeFormatter f = config.Formatters[i];
                if (f.GetType() == typeof(JsonMediaTypeFormatter))
                {
                    config.Formatters.RemoveAt(i);
                    config.Formatters.Add(new _JsonMediaTypeFormatter());
                }
                else if (f.GetType() == typeof(JQueryMvcFormUrlEncodedFormatter))
                {
                    config.Formatters.RemoveAt(i);
                    config.Formatters.Add(new _JQueryMvcFormUrlEncodedFormatter(config));
                }
                else if (f.GetType() == typeof(FormUrlEncodedMediaTypeFormatter))
                {
                    config.Formatters.RemoveAt(i);
                    config.Formatters.Add(new _FormUrlEncodedMediaTypeFormatter());
                }
                else if (f.GetType() == typeof(XmlMediaTypeFormatter))
                {
                    config.Formatters.RemoveAt(i);
                    //config.Formatters.Add(new _XmlMediaTypeFormatter());
                }
            }
        }

        [_DebuggerStepThrough]
        internal class _JQueryMvcFormUrlEncodedFormatter : FormUrlEncodedMediaTypeFormatter
        {
            private readonly HttpConfiguration _configuration = null;

            public _JQueryMvcFormUrlEncodedFormatter(HttpConfiguration config)
            {
                _configuration = config;
            }

            public override bool CanReadType(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }

                return true;
            }

            public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
            {
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }

                if (readStream == null)
                {
                    throw new ArgumentNullException("readStream");
                }

                // For simple types, defer to base class
                if (base.CanReadType(type))
                {
                    return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
                }

                return ReadFromStreamAsyncCore(type, readStream, content, formatterLogger);
            }

            private async Task<object> ReadFromStreamAsyncCore(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
            {
                object obj = await base.ReadFromStreamAsync(typeof(FormDataCollection), readStream, content, formatterLogger);
                FormDataCollection fd = (FormDataCollection)obj;

                try
                {
                    return fd.ReadAs(type, String.Empty, RequiredMemberSelector, formatterLogger, _configuration);
                }
                catch (Exception e)
                {
                    if (formatterLogger == null)
                    {
                        throw;
                    }
                    formatterLogger.LogError(String.Empty, e);
                    return GetDefaultValueForType(type);
                }
            }
        }

        [_DebuggerStepThrough]
        class _XmlMediaTypeFormatter : XmlMediaTypeFormatter { }

        [_DebuggerStepThrough]
        class _FormUrlEncodedMediaTypeFormatter : FormUrlEncodedMediaTypeFormatter { }

        [_DebuggerStepThrough]
        class _JsonMediaTypeFormatter : JsonMediaTypeFormatter
        {
            //public override DataContractJsonSerializer CreateDataContractSerializer(Type type)
            //{
            //    return base.CreateDataContractSerializer(type);
            //}
            //public override JsonSerializer CreateJsonSerializer()
            //{
            //    return json.GetJsonSerializer();
            //}
            //public override JsonReader CreateJsonReader(Type type, Stream readStream, Encoding effectiveEncoding)
            //{
            //    _ApiUser user = _User.Current as _ApiUser;
            //    if (user != null)
            //    {
            //        readStream = new RSAStream(readStream, System.Security.Cryptography.CryptoStreamMode.Read) { Base64CspBlob = user.apiAuth.apikey };
            //    }
            //    return new json._Reader(new StreamReader(readStream, effectiveEncoding));
            //}
            //public override JsonWriter CreateJsonWriter(Type type, Stream writeStream, Encoding effectiveEncoding)
            //{
            //    return new json._Writer(new StreamWriter(writeStream, effectiveEncoding)) { Formatting = Indent ? Formatting.Indented : Formatting.None };
            //}

            public override object ReadFromStream(Type type, Stream readStream, Encoding effectiveEncoding, IFormatterLogger formatterLogger)
            {
                _HttpContext context = _HttpContext.Current;
                _ApiUser user = _User.Current as _ApiUser;
                string json_string;
                if (user != null)
                {
                    using (FromBase64Transform r1 = new FromBase64Transform())
                    using (CryptoStream r2 = new CryptoStream(readStream, r1, CryptoStreamMode.Read))
                    using (RSADecryptStream r3 = new RSADecryptStream(r2, true) { Base64CspBlob = user.apiAuth.apikey })
                    using (StreamReader r4 = new StreamReader(r3, effectiveEncoding))
                        json_string = r4.ReadToEnd();
                    log.message("api2", $@"{context?.RequestIP}
{context.Request.AppRelativeCurrentExecutionFilePath}
{json_string}");
                    //using (json._Reader r5 = new json._Reader(r4))
                    //    return json.GetJsonSerializer().Deserialize(r5, type);
                }
                else
                {
                    using (StreamReader r1 = new StreamReader(readStream, effectiveEncoding, true, 4096, true))
                        json_string = r1.ReadToEnd();
                    log.message("api1", $@"{context?.RequestIP}
{context.Request.AppRelativeCurrentExecutionFilePath}
{json_string}");
                    //using (json._Reader r2 = new json._Reader(r1))
                    //    return json.GetJsonSerializer().Deserialize(r2, type);
                }
                if (context != null)
                    context.Arguments = json_string;
                if (type == typeof(_empty))
                    return _empty.instance;
                return json.DeserializeObject(type, json_string);
                //return base.ReadFromStream(type, readStream, effectiveEncoding, formatterLogger);
            }

            public override void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
            {
                using (StreamWriter w1 = new StreamWriter(writeStream, effectiveEncoding, 4096, true))
                using (json._Writer w2 = new json._Writer(w1))
                    json.GetJsonSerializer().Serialize(w2, value);
                //base.WriteToStream(type, value, writeStream, effectiveEncoding);
            }
        }
    }
}