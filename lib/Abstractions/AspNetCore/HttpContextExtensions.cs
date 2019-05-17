using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http
{
    public static partial class HttpContextExtensions
    {
        public static HttpRequest EnableRewind(this HttpRequest request, IOptions<FormOptions> formOptions)
        {
            var _options = (formOptions ?? request.HttpContext.RequestServices.GetService<IOptions<FormOptions>>())?.Value;
            return request.EnableRewind(
                _options?.MemoryBufferThreshold ?? FormOptions.DefaultMemoryBufferThreshold,
                _options?.BufferBodyLengthLimit ?? FormOptions.DefaultBufferBodyLengthLimit);
        }

        public static bool GetItem<T>(this HttpContext context, out T item, bool remove = false)
        {
            foreach (var n in context.Items)
            {
                if (n.Value is T)
                {
                    item = (T)n.Value;
                    if (remove) context.Items.Remove(n.Key);
                    return true;
                }
            }
            return _null.noop(false, out item);
        }

        public static T GetItem<T>(this HttpContext context, bool create = false)
        {
            if (GetItem(context, out T item))
                return item;
            if (create)
                return SetItem(context, context.RequestServices.CreateInstance<T>());
            return default(T);
        }

        public static T GetItem<T>(this HttpContext context, Func<T> create)
        {
            if (GetItem(context, out T item))
                return item;
            if (create != null)
                return SetItem(context, create());
            return default(T);
        }

        public static T GetItem<T>(this HttpContext context, Func<HttpContext, T> create)
        {
            if (GetItem(context, out T item))
                return item;
            if (create != null)
                return SetItem(context, create(context));
            return default(T);
        }

        public static T SetItem<T>(this HttpContext context, T value)
        {
            context.Items[typeof(T)] = value;
            return value;
        }

        public static void RemoveItems<T>(this HttpContext context, Action<T> cb)
        {
            context.Items.RemoveWhen(x =>
            {
                if (x.Value is T)
                {
                    try { cb((T)x.Value); }
                    catch { }
                    return true;
                }
                return false;
            });

        }

        public static HttpContext GetHttpContext(this IServiceProvider services) => services.GetService<IHttpContextAccessor>()?.HttpContext;
    }
}
