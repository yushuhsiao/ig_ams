//using AngularJS;
using Bridge;
using Bridge.Html5;
using Bridge.jQuery2;
using InnateGlory;
using Newtonsoft.Json;
using Retyped;
using System;

namespace InnateGlory
{
    public delegate void ApiSuccessHandler(object data, ApiResult result);
    public delegate void ApiFailedHandler(Status status, ApiResult result);
    public delegate void ApiCompleteHandler(Status status, ApiResult result);
}
//namespace AngularJS
//{
//    /// <summary>
//    /// HttpConfig for angular $http
//    /// </summary>
//    /// <typeparam name="TData">result data type</typeparam>
//    public class HttpConfig<TData> : HttpConfig
//    {
//        public ApiSuccessHandler<TData> ApiSuccess { get; set; }
//        public ApiFailedHandler<TData> ApiFailed { get; set; }
//        public ApiCompleteHandler<TData> ApiComplete { get; set; }
//    }
//}
namespace InnateGlory
{
    /// <summary>
    /// ApiOptions for jQuery.ajax
    /// </summary>
    /// <typeparam name="TData">result data type</typeparam>
    //public class jqApiOptions<TData> : AjaxOptions
    //{
    //    public ApiSuccessHandler<TData> ApiSuccess { get; set; }
    //    public ApiFailedHandler<TData> ApiFailed { get; set; }
    //    public ApiCompleteHandler<TData> ApiComplete { get; set; }
    //}
    [ObjectLiteral]
    public class ApiOptions : AjaxOptions
    {
        //public Action BeforeSend { get; set; }
        public ApiSuccessHandler ApiSuccess { get; set; }
        public ApiFailedHandler ApiFailed { get; set; }
        public ApiCompleteHandler ApiComplete { get; set; }
    }
    //[ObjectLiteral]
    //public class ngApiOptions<TData>
    //{
    //    public ApiSuccessHandler<TData> ApiSuccess { get; set; }
    //    public ApiFailedHandler<TData> ApiFailed { get; set; }
    //    public ApiCompleteHandler<TData> ApiComplete { get; set; }
    //}
}