using Bridge;
using System;

namespace AngularJS
{
    [External]
    [Name("angular")]
    public class Angular
    {
        [Template("{this}.module({name}, [])")]
        public static Module Module(string name) => default(Module);
    }
    [External]
    public class Module
    {
        [Template("{this}.service({name}, {function})")]
        public Module Service(string name, Delegate function)
        {
            return this;
        }
    }

    [External]
    public class Http
    {
        [Template("{this}.post({url}, {data}, {config})")]
        public HttpPromise<T> Post<T>(string url, dynamic data, HttpConfig config = null) => default(HttpPromise<T>);
    }

    //[External]
    public class HttpConfig
    {
        /// <summary>
        /// ""	            - DOMString (this is the default value)
        /// "arraybuffer"   - ArrayBuffer
        /// "blob"          - Blob
        /// "document"      - Document
        /// "json"          - JSON
        /// "text"          - DOMString
        /// </summary>
        [Name("responseType")]
        public string ResponseType;

        [Name("transformResponse")]
        public Func<object, object, int, object> TransformResponse;
    }

    [External]
    public class HttpPromise<T>
    {
        [Template("{this}.then({onFulfilled}, {onRejected}, {onProgress})")]
        public HttpPromise<T> Then(Action<HttpResponse<T>> onFulfilled, Action<HttpResponse<T>> onRejected, Action<HttpResponse<T>> onProgress) => this;

        [Template("{this}.catch({onRejected})")]
        public HttpPromise<T> Catch(Action<HttpResponse<T>> onRejected) => this;

        [Template("{this}.finally({callback})")]
        public HttpPromise<T> Finally(Action<HttpResponse<T>> callback) => this;

        [Template("{this}.done({onFulfilled}, {onRejected}, {onProgress})")]
        public HttpPromise<T> Done(Action<HttpResponse<T>> onFulfilled, Action<HttpResponse<T>> onRejected, Action<HttpResponse<T>> onProgress) => this;
    }

    [External]
    public class HttpResponse<T>
    {
        [Name("config")]
        public HttpConfig Config { get; set; }

        [Name("data")]
        public T Data { get; set; }

        [Name("headers")]
        public object Headers { get; set; }

        [Name("status")]
        public int Status { get; set; }

        [Name("statusText")]
        public string StatusText { get; set; }

        [Name("xhrStatus")]
        public string XhrStatus { get; set; }
    }
}