using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace casino
{
    partial class api
    {
		internal const BindingFlags _BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

        [_DebuggerStepThrough]
        public class router : RouteBase
        {
            private static readonly List<Assembly> _asm = new List<Assembly>();
            internal static readonly Dictionary<string, RouteData> Items = new Dictionary<string, RouteData>();
            static router instance = new router();

            [DebuggerStepThrough]
            public static void Initialize()
            {
                lock (Items)
                {
                    if (!RouteTable.Routes.Contains(instance))
                        RouteTable.Routes.Add(instance);
                }
            }

            router() { }

            void InitHandler(object sender, AssemblyLoadEventArgs args)
            {
                lock (Items)
                {
                    foreach (Assembly asm in ((AppDomain)sender).GetAssemblies())
                    {
                        if (_asm.Contains(asm)) continue;
                        _asm.Add(asm);
                        if (asm.FullName.StartsWith("mscorlib")) continue;
                        if (asm.FullName.StartsWith("System")) continue;
                        if (asm.FullName.StartsWith("Microsoft")) continue;
                        foreach (Type t in asm.GetTypes())
                        {
                            foreach (MethodInfo m in t.GetMethods(api._BindingFlags))
                            {
                                foreach (api.httpAttribute a in m.GetCustomAttributes<api.httpAttribute>(false))
                                {
                                    string p = a.Name.Trim(true);
                                    if (p == null) continue;
                                    if (!p.StartsWith("~/")) continue;
                                    while (p.Contains("//"))
                                        p = p.Replace("//", "/");
                                    if (Items.ContainsKey(p)) continue;
                                    if (PathIsAllow(p))
                                        Items[p] = new apiRoute(this, m, a, p).RouteData;
                                }
                            }
                            #region //
                            //string p1 = null;
                            //foreach (apiAttribute a in t.GetCustomAttributes<apiAttribute>(false))
                            //{
                            //    p1 = a.Name.Trim(true);
                            //    if (p1 == null) continue;
                            //    if (t.HasInterface<IRouteHandler>())
                            //    {
                            //        if (p1.StartsWith("~/"))
                            //        {
                            //            if (_comet_handlers.ContainsKey(p1)) continue;
                            //            IRouteHandler handler = _comet_handlers[p1] = (IRouteHandler)Activator.CreateInstance(t);
                            //            RouteTable.Routes.Add(new Route(p1.Substring(2), handler) { RouteExistingFiles = false });
                            //            p1 = null;
                            //            continue;
                            //        }
                            //    }
                            //    if (p1.StartsWith("~/") == false)
                            //        p1 = string.Format("~/{0}", p1);
                            //}
                            //foreach (MethodInfo m in t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                            //{
                            //    foreach (apiAttribute a in m.GetCustomAttributes<apiAttribute>(false))
                            //    {
                            //        string path;
                            //        string p2 = a.Name.Trim(true);
                            //        if (p2 == null)
                            //        {
                            //            if (p1 == null)
                            //                continue;
                            //            else
                            //                path = string.Format("{0}/{1}", p1, m.Name);
                            //        }
                            //        else
                            //        {
                            //            if (p2.StartsWith("~/"))
                            //                path = p2;
                            //            else if (p1 == null)
                            //                continue;
                            //            else
                            //                path = string.Format("{0}/{1}", p1, m.Name);
                            //        }
                            //        while (path.Contains("//"))
                            //            path = path.Replace("//", "/");

                            //        if (_api_handlers.ContainsKey(path)) break;
                            //        if (PathIsAllow(path))
                            //            RouteTable.Routes.Add(new Route(path.Substring(2), _api_handlers[path] = new Handler(m, a, path)) { RouteExistingFiles = false });
                            //    }
                            //}
                            #endregion
                        }
                    }
                }
            }

            static bool PathIsAllow(string path)
            {
                var c1 = ConfigurationManager.GetSection("apiPath") as apiPathSection;
                if (c1 == null) return false;
                apiPathElement c2 = null;
                foreach (apiPathElement e in c1.Instances)
                {
                    if (!e.Path.StartsWith("~/")) continue;
                    if (e.Path == path)
                    {
                        c2 = e;
                        break;
                    }
                    if (path.StartsWith(e.Path))
                    {
                        if ((c2 != null) && (c2.Path.Length < e.Path.Length))
                            continue;
                        c2 = e;
                    }
                }
                if (c2 == null) return false;
                if (c2.Type == apiPathType.deny) return false;
                return true;
            }

            public override RouteData GetRouteData(HttpContextBase httpContext)
            {
                string url = httpContext.Request.AppRelativeCurrentExecutionFilePath;
                RouteData r;
                lock (router.Items)
                {
                    if (router._asm.Count == 0)
                    {
                        InitHandler(AppDomain.CurrentDomain, new AssemblyLoadEventArgs(Assembly.GetExecutingAssembly()));
                        AppDomain.CurrentDomain.AssemblyLoad += InitHandler;
                    }
                    router.Items.TryGetValue(url, out r);
                }
                return r;
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
            {
                return null;
            }
        }
    }
}