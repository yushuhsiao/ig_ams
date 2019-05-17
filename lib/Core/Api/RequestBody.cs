using InnateGlory;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public class RequestBody : IDisposable
    {
        private static RequestBody _Null = new RequestBody(null);

        internal static RequestBody GetRequestBody(IServiceProvider s)
        {
            HttpContext httpContext = s.GetService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
                return _Null;
            if (httpContext.Request.Method.IsNotEquals("post"))
                return _Null;
            if ((httpContext.Request.ContentLength ?? 0) <= 0)
                return _Null;
            return httpContext.GetItem(_create);
            //if (httpContext.GetItem(out RequestBody result))
            //    return result;
            //result = new RequestBody(httpContext);
            //httpContext.Response.RegisterForDispose(result);
            //return httpContext.SetItem(result);
        }

        private static RequestBody _create(HttpContext httpContext)
        {
            RequestBody result = new RequestBody(httpContext);
            httpContext.Response.RegisterForDispose(result);
            return result;
        }

        private HttpContext _httpContext;
        public string request_text { get; private set; }
        public string body_text { get; private set; }
        private JObject json;
        private object obj;

        private bool _parsed;

        private RequestBody(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        void IDisposable.Dispose()
        {
            this.Require_ContentType = null;
            this.Require_Method = null;
            this.Require_ContentType = null;
            this.Require_Encoding = null;

            this.body_text = null;
            this.request_text = null;
            this.json = null;
            this.obj = null;
            this._httpContext = null;
        }

        public Encoding Require_Encoding { get; set; }
        public string Require_Method { get; set; }
        public string Require_ContentType { get; set; }
        public string rsa_key = null;

        private bool GetApiAuth(HttpRequest request, RequestHeaders headers, out ApiAuth result)
        {
            //var xx = _httpContext.RequestServices.GetCurrentUser();
            var xx = _httpContext.User.GetUserId();
            ;
            //bool is_internal;
            //if (request.Headers[_Consts.Api.Header2].ToString().ToBoolean(out is_internal) && is_internal)
            //{
            //    var u = _services.GetService<IUserManager>();
            //    if (false == u.InternalApiServer)
            //        return _null.noop(false, out result);
            //}


            //string auth_corp = null;
            //string auth_user = request.Headers[_Consts.Api.Header1].ToString();
            //int n = auth_user.LastIndexOf('@');
            //if (n >= 0)
            //{
            //    auth_corp = auth_user.Substring(n + 1);
            //    auth_user = auth_user.Substring(0, n);
            //}
            return _null.noop(false, out result);
        }

        public bool Parse()
        {
            try
            {
                if (_httpContext == null) return false;
                if (_parsed) return true;

                HttpRequest request = _httpContext.Request;
                var headers = request.GetTypedHeaders();

                Require_Method = Require_Method ?? request.Method;
                Require_ContentType = Require_ContentType ?? headers.ContentType.MediaType.ToString();

                if (request.Method.IsEquals(Require_Method, true) &&
                    headers.ContentType.MediaType.Equals(Require_ContentType, StringComparison.OrdinalIgnoreCase))
                {
                    Require_Encoding = Require_Encoding ?? Encoding.UTF8;
                    request.EnableRewind(default(IOptions<FormOptions>));
                    long position = request.Body.Position;
                    try
                    {
                        string body_text, request_text;
                        request.Body.Seek(0, SeekOrigin.Begin);
                        using (StreamReader r2 = new StreamReader(request.Body, Require_Encoding, true, 1024, true))
                            body_text = request_text = r2.ReadToEnd()?.Trim();

                        if (GetApiAuth(request, headers, out var apiAuth))
                        {
                            ;
                        }

                        if (rsa_key != null)
                        {
                            request.Body.Seek(0, SeekOrigin.Begin);
                            using (FromBase64Transform r3 = new FromBase64Transform())
                            using (CryptoStream r4 = new CryptoStream(request.Body, r3, CryptoStreamMode.Read))
                            using (RSADecryptStream r5 = new RSADecryptStream(r4, true) { Base64CspBlob = rsa_key })
                            using (StreamReader r6 = new StreamReader(r5, Require_Encoding, true, 1024, true))
                                body_text = r6.ReadToEnd()?.Trim();
                        }
                        if (string.IsNullOrEmpty(body_text))
                            return false;
                        this.body_text = body_text;
                        this.request_text = request_text;
                        this._parsed = true;
                        return true;
                    }
                    finally
                    {
                        request.Body.Position = position;
                    }
                }
            }
            catch { }
            return false;
        }

        public bool GetBodyJson(out JObject obj)
        {
            obj = this.json;
            if (obj == null && this.Parse())
            {
                try
                {
                    obj = JsonHelper.ToJObject(body_text);
                    this.json = obj;
                    return obj != null;
                }
                catch { }
            }
            return obj != null;// _null.noop(false, out obj);
        }

        public bool GetBodyJson<T>(out T obj)
        {
            if (this.obj.TryCast<T>(out obj))
                return true;
            if (this.Parse())
            {
                try
                {
                    obj = JsonHelper.DeserializeObject<T>(body_text);
                    this.obj = obj;
                    return obj != null;
                }
                catch { }
            }
            return _null.noop(false, out obj);
        }
    }
}