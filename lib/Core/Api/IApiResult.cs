using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace InnateGlory
{
    public interface IApiResult : IActionResult
    {
        Status StatusCode { get; set; }
        string StatusText { get; }
        string Message { get; set; }
        object Data { get; set; }
        IDictionary<string, ApiErrorEntry> Errors { get; set; }

//#if wasm || jslib
#if !jslib
        HttpStatusCode? HttpStatusCode { get; set; }
        string ContentType { get; set; }
#endif
    }
}