using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace InnateGlory.TagHelpers
{
    internal class PageBundleFileProvider : IFileProvider
    {
        public IFileProvider Provider1 { get; }
        public IFileProvider Provider2 { get; }
        private Dictionary<string, string> maps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public PageBundleFileProvider(IHostingEnvironment env, IFileVersionProvider fileVersionProvider)
        {
            Provider1 = env.WebRootFileProvider;
            Provider2 = new PhysicalFileProvider(env.ContentRootPath);
        }

        internal void AddMap(string url, string path)
        {
            maps.TrySetValue(url, path, syncLock: true);
        }

        IDirectoryContents IFileProvider.GetDirectoryContents(string subpath)
        {
            return Provider1.GetDirectoryContents(subpath);
        }

        IFileInfo IFileProvider.GetFileInfo(string subpath)
        {
            var r = Provider1.GetFileInfo(subpath);
            if (r.Exists)
                return r;
            if (maps.TryGetValue(subpath, out string path, syncLock: true))
                r = Provider2.GetFileInfo(path);
            return r;
        }

        IChangeToken IFileProvider.Watch(string filter)
        {
            return Provider1.Watch(filter);
        }

        internal bool Process(IPageBundleTagHelpers _src, TagHelperContext context, TagHelperOutput output)
        {
            var viewContext = _src.ViewContext;
            IServiceProvider _services = viewContext.HttpContext.RequestServices;
            //PageBundleFileProvider map = this;
            bool _auto = context.AllAttributes.ContainsName(_src.Key_Auto);
            bool _inline = context.AllAttributes.ContainsName("inline");
            if (_auto || _inline)
            {
                var actionDescriptor1 = viewContext.ActionDescriptor.TryCast<ControllerActionDescriptor>();
                var actionDescriptor2 = viewContext.ActionDescriptor.TryCast<PageActionDescriptor>();
                //var pathA = viewContext.ExecutingFilePath;
                var pathA = viewContext.View.Path;
                var pathB = actionDescriptor2?.RelativePath;

                if (_inline)
                {
                    string path = null;
                    if (context.AllAttributes.TryGetAttribute(_src.Key_Src, out var src_attr))
                    {
                        string url = src_attr.Value as string ?? (src_attr.Value as HtmlString)?.ToString();
                        if (_src.TryResolveUrl(url, out string tmp))
                            path = tmp;
                    }
                    if (string.IsNullOrEmpty(path) && _auto)
                    {
                        path = $"{pathB ?? pathA}.{_src.Ext}";
                        //if (actionDescriptor1 != null)
                        //    path = $"{viewContext.ExecutingFilePath}.{_src.Ext}";
                        //else
                        //    path = $"{actionDescriptor2.RelativePath}.{_src.Ext}";
                    }
                    if (!string.IsNullOrEmpty(path))
                    {
                        IFileInfo f = this.Provider1.GetFileInfo(path);
                        if (!f.Exists)
                            f = this.Provider2.GetFileInfo(path);
                        if (f.Exists)
                        {
                            using (var s1 = f.CreateReadStream())
                            using (StreamReader s2 = new StreamReader(s1))
                            {
                                string str = s2.ReadToEnd();
                                output.TagMode = TagMode.StartTagAndEndTag;
                                output.TagName = _src.Inline_TagName;
                                output.Attributes.Clear();
                                output.Attributes.Add("type", _src.Inline_Type);
                                output.Content.SetHtmlContent("\r\n");
                                output.Content.AppendHtmlLine(str);
                                context.Reinitialize(_src.Inline_TagName, context.Items, context.UniqueId);
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    string url1 = $"~{pathB ?? pathA}.{_src.Ext}";
                    string path = $"{pathB ?? pathA}.{_src.Ext}";
                    //if (actionDescriptor1 != null)
                    //{
                    //    url1 = $"~{viewContext.ExecutingFilePath}.{_src.Ext}";
                    //    path = $"{viewContext.ExecutingFilePath}.{_src.Ext}";
                    //}
                    //else
                    //{
                    //    url1 = $"~{actionDescriptor2.RelativePath}.{_src.Ext}";
                    //    path = $"{actionDescriptor2.RelativePath}.{_src.Ext}";
                    //}

                    if (_src.TryResolveUrl(url1, out string url2))
                        this.AddMap(url2, path);
                    else
                        this.AddMap(url1, path);

                    TagHelperAttribute a;
                    if (_src.AppendVersion == true)
                    {
                        path = trimQueryString(path);
                        url1 = trimQueryString(url1);

                        var path2 = _src.FileVersionProvider.AddFileVersionToPath(viewContext.HttpContext.Request.PathBase, path);
                        string q = path2.Substring(path.Length);

                        a = new TagHelperAttribute(_src.Key_Src, url1 + q, HtmlAttributeValueStyle.DoubleQuotes);
                    }
                    else
                    {
                        a = new TagHelperAttribute(_src.Key_Src, url1 ?? path, HtmlAttributeValueStyle.DoubleQuotes);
                    }

                    if (IndexOfName(output.Attributes, _src.Key_Src, out int n))
                    {
                        output.Attributes[n] = a;
                        Remove(output.Attributes, _src.Key_Auto);
                    }
                    else if (IndexOfName(output.Attributes, _src.Key_Auto, out n))
                        output.Attributes[n] = a;
                    else
                        output.Attributes.Add(a);
                }
            }
            return true;
        }



        private static void Remove(TagHelperAttributeList list, string name)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var tmp = list[i];
                if (tmp.Name.IsEquals(name))
                    list.RemoveAt(i);
            }
        }

        private static bool IndexOfName(IList<TagHelperAttribute> list, string name, out int index)
        {
            for (int i = 0, n = list.Count; i < n; i++)
            {
                var tmp = list[i];
                if (tmp.Name.IsEquals(name))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        private static readonly char[] QueryStringAndFragmentTokens = new[] { '?', '#' };

        private static string trimQueryString(string path)
        {
            var queryStringOrFragmentStartIndex = path.IndexOfAny(QueryStringAndFragmentTokens);
            if (queryStringOrFragmentStartIndex != -1)
            {
                return path.Substring(0, queryStringOrFragmentStartIndex);
            }
            return path;
        }
    }
}
