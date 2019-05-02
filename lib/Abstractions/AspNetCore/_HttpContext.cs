using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;

namespace Microsoft.AspNetCore.Http
{
    public class _HttpContext : DefaultHttpContext//, IDisposable
    {
        //private static IHttpContextAccessor _httpContextAccessor;
        //public static _HttpContext Current => Interlocked.CompareExchange(ref _httpContextAccessor, null, null)?.HttpContext as _HttpContext;


        public _HttpContext(/*IHttpContextAccessor httpContextAccessor, */IFeatureCollection features) : base(features)
        {
            //Interlocked.Exchange(ref _httpContextAccessor, httpContextAccessor);
        }

        protected override HttpRequest InitializeHttpRequest() => new _HttpRequest(this);

        protected override void UninitializeHttpRequest(HttpRequest instance)
        {
            base.UninitializeHttpRequest(instance);
        }

        //void IDisposable.Dispose()
        //{
        //    this.Items.RemoveWhen(kvp => (kvp.Key as HttpContextItemKey)?.AutoRemoveOnDispose ?? false);
        //}

        private class _HttpRequest : DefaultHttpRequest
        {
            internal _HttpRequest(_HttpContext context) : base(context) { }

            //public override Stream Body
            //{
            //    get
            //    {
            //        ActionContext actionContext = this.HttpContext.RequestServices.GetService<ActionContext>();
            //        return base.Body;
            //    }
            //    set => base.Body = value;
            //}
        }
    }

    public interface IHttpContextDispose
    {
        void Dispose(HttpContext httpContext);
    }

}