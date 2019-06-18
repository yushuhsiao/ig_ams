using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Json.Internal;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory.Api
{
    internal class ApiResultExecutor
    {
        private static readonly Action<ILogger, string, Exception> _jsonResultExecuting = LoggerMessage.Define<string>(
            LogLevel.Information, 1, "Executing JsonResult, writing value {Value}.");

        private static readonly string DefaultContentType = "application/json; charset=utf-8";

        //private readonly IArrayPool<char> _charPool;

        public ApiResultExecutor(IHttpResponseStreamWriterFactory writerFactory, ILogger<JsonResultExecutor> logger/*, IOptions<MvcJsonOptions> options, ArrayPool<char> charPool*/)
        {
            //MediaTypeHeaderValue
            if (writerFactory == null)
            {
                throw new ArgumentNullException(nameof(writerFactory));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            //if (options == null)
            //{
            //    throw new ArgumentNullException(nameof(options));
            //}

            //if (charPool == null)
            //{
            //    throw new ArgumentNullException(nameof(charPool));
            //}

            WriterFactory = writerFactory;
            Logger = logger;
            //Options = options.Value;
            //_charPool = new JsonArrayPool<char>(charPool);
        }

        protected ILogger Logger { get; }

        //protected MvcJsonOptions Options { get; }

        protected IHttpResponseStreamWriterFactory WriterFactory { get; }

        static readonly object _null_value = new object();

        public static async Task ExecuteResultAsync(ActionContext context, IApiResult result)
        {
            ApiResultExecutor obj = context.HttpContext.RequestServices.GetService<ApiResultExecutor>();

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            var response = context.HttpContext.Response;

            ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                result.ContentType,
                response.ContentType,
                DefaultContentType,
                out var resolvedContentType,
                out var resolvedContentTypeEncoding);

            response.ContentType = resolvedContentType;

            if (result.HttpStatusCode != null)
            {
                response.StatusCode = (int)result.HttpStatusCode.Value;
            }

            if (result.Data == null)
                result.Data = _null_value;

            //var serializerSettings = result.SerializerSettings ?? Options.SerializerSettings;

            _jsonResultExecuting(obj.Logger, Convert.ToString(result.Data), null);
            using (var writer = obj.WriterFactory.CreateWriter(response.Body, resolvedContentTypeEncoding))
            using (var jsonWriter = JsonHelper.CreateWriter(writer))
            {
                //jsonWriter.ArrayPool = _charPool;
                jsonWriter.CloseOutput = false;
                jsonWriter.AutoCompleteOnClose = false;
                JsonHelper.SerializeObject(jsonWriter, result);
            }

            await Task.CompletedTask;
        }
    }
}

//namespace Microsoft.AspNetCore.Mvc
//{
//    public static class ApiResultExtensions
//    {
//        public static ApiResult Api(this Controller controller, object value) => new ApiResult(value);
//    }
//}