using BundleTransformer.Core;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Configuration;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.HttpHandlers;
using BundleTransformer.Core.Minifiers;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using BundleTransformer.Core.Transformers;
using BundleTransformer.Less.Translators;
using BundleTransformer.MicrosoftAjax.Minifiers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;
using Tools;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Diagnostics;
using System.Web.Caching;
using System.Linq;
using BundleTransformer.Core.Translators;
using System.Security.Cryptography;
using BundleTransformer.Core.Resources;
using BundleTransformer.Core.PostProcessors;
using System.Reflection;
using BundleTransformer.Core.Helpers;
using System.Text;

namespace ams
{
    public class LessAssetHandler : _StyleAssetHandlerBase
    {
        protected override bool IsStaticAsset
        {
            get { return false; }
        }

        public LessAssetHandler()
            : this(HttpContext.Current.Cache, BundleTransformerContext.Current.FileSystem.GetVirtualFileSystemWrapper(), BundleTransformerContext.Current.Configuration.GetCoreSettings().AssetHandler)
        {
        }

        public LessAssetHandler(Cache cache, IVirtualFileSystemWrapper virtualFileSystemWrapper, AssetHandlerSettings assetHandlerConfig)
            : base(cache, virtualFileSystemWrapper, assetHandlerConfig)
        {
        }

        protected override IAsset TranslateAsset(IAsset asset, ITransformer transformer, bool isDebugMode)
        {
            return InnerTranslateAsset<LessTranslator>("LessTranslator", asset, transformer, isDebugMode);
        }
    }
    public abstract class _StyleAssetHandlerBase : _AssetHandlerBase
    {
        protected override string ContentType
        {
            get { return "text/css"; }
        }

        protected _StyleAssetHandlerBase(Cache cache, IVirtualFileSystemWrapper virtualFileSystemWrapper, AssetHandlerSettings assetHandlerConfig)
                : base(cache, virtualFileSystemWrapper, assetHandlerConfig)
        {
        }

        protected override Bundle GetBundleByVirtualPath(string virtualPath)
        {
            return base.GetBundleByVirtualPath(virtualPath);
        }

        protected override BundleFile GetBundleFileByVirtualPath(Bundle bundle, string virtualPath)
        {
            return base.GetBundleFileByVirtualPath(bundle, virtualPath);
        }

        protected override string GetCacheKey(string assetVirtualPath, string bundleVirtualPath)
        {
            return base.GetCacheKey(assetVirtualPath, bundleVirtualPath);
        }

        protected override IAsset PostProcessAsset(IAsset asset, ITransformer transformer)
        {
            return base.PostProcessAsset(asset, transformer);
        }

        protected override IAsset TranslateAsset(IAsset asset, ITransformer transformer, bool isDebugMode)
        {
            return base.TranslateAsset(asset, transformer, isDebugMode);
        }

        protected override ITransformer GetTransformer(Bundle bundle)
        {
            IBundleTransform transformer = null;
            if (bundle != null)
            {
                transformer = bundle.Transforms.FirstOrDefault(t => t is StyleTransformer);
            }

            return (ITransformer)transformer;
        }

        protected override T GetTranslatorByName<T>(string translatorName)
        {
            ITranslator translator = BundleTransformerContext.Current.Styles.GetTranslatorInstance(translatorName);

            return (T)translator;
        }
    }
    public abstract class _AssetHandlerBase : IHttpHandler
    {
        protected HttpContextBase _context;
        protected readonly Cache _cache;
        private static readonly object _cacheSynchronizer = new object();
        private readonly IVirtualFileSystemWrapper _virtualFileSystemWrapper;
        private readonly AssetHandlerSettings _assetHandlerConfig;
        private static readonly Lazy<IHttpHandler> _staticFileHandler = new Lazy<IHttpHandler>(CreateStaticFileHandlerInstance);
        protected abstract string ContentType
        {
            get;
        }
        protected abstract bool IsStaticAsset
        {
            get;
        }
        public bool IsReusable
        {
            get { return true; }
        }

        protected _AssetHandlerBase(Cache cache, IVirtualFileSystemWrapper virtualFileSystemWrapper, AssetHandlerSettings assetHandlerConfig)
        {
            _cache = cache;
            _virtualFileSystemWrapper = virtualFileSystemWrapper;
            _assetHandlerConfig = assetHandlerConfig;
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        public void ProcessRequest(HttpContextBase context)
        {
            _context = context;

            var request = context.Request;
            var response = context.Response;

            Uri assetUri = request.Url;
            if (assetUri == null)
            {
                throw new HttpException(500, Strings.Common_ValueIsNull);
            }

            string assetVirtualPath = assetUri.LocalPath;
            if (string.IsNullOrWhiteSpace(assetVirtualPath))
            {
                throw new HttpException(500, Strings.Common_ValueIsEmpty);
            }

            if (!_virtualFileSystemWrapper.FileExists(assetVirtualPath))
            {
                throw new HttpException(404, string.Format(Strings.Common_FileNotExist, assetVirtualPath));
            }

            string bundleVirtualPath = request.QueryString["bundleVirtualPath"];
            if (string.IsNullOrWhiteSpace(bundleVirtualPath) && IsStaticAsset)
            {
                // Delegate a processing of asset to the instance of `System.Web.StaticFileHandler` type
                ProcessStaticAssetRequest(context);
                return;
            }

            string content;

            try
            {
                content = GetProcessedAssetContent(assetVirtualPath, bundleVirtualPath);
            }
            catch (HttpException)
            {
                throw;
            }
            catch (AssetTranslationException e)
            {
                throw new HttpException(500, e.Message, e);
            }
            catch (AssetPostProcessingException e)
            {
                throw new HttpException(500, e.Message, e);
            }
            catch (FileNotFoundException e)
            {
                throw new HttpException(500, string.Format(Strings.AssetHandler_DependencyNotFound, e.Message, e));
            }
            catch (Exception e)
            {
                throw new HttpException(500, string.Format(Strings.AssetHandler_UnknownError, e.Message, e));
            }

            var clientCache = response.Cache;
            if (_assetHandlerConfig.DisableClientCache)
            {
                response.StatusCode = 200;
                response.StatusDescription = "OK";

                // Configure browser cache
                clientCache.SetCacheability(HttpCacheability.NoCache);
                clientCache.SetExpires(DateTime.UtcNow.AddYears(-1));
                clientCache.SetValidUntilExpires(false);
                clientCache.SetNoStore();
                clientCache.SetNoServerCaching();

                // Output text content of asset
                response.ContentType = ContentType;
                response.Write(content);
            }
            else
            {
                // Generate a ETag value and check it
                string eTag = GenerateAssetETag(content);
                bool eTagChanged = IsETagHeaderChanged(request, eTag);

                // Add a special HTTP-headers to ensure that
                // asset caching in browsers
                if (eTagChanged)
                {
                    response.StatusCode = 200;
                    response.StatusDescription = "OK";
                }
                else
                {
                    response.StatusCode = 304;
                    response.StatusDescription = "Not Modified";

                    // Set to 0 to prevent client waiting for data
                    response.AddHeader("Content-Length", "0");
                }

                // Add a Bundle Transformer's copyright HTTP header
                response.AddHeader("X-Asset-Transformation-Powered-By", "Bundle Transformer");

                clientCache.SetCacheability(HttpCacheability.Public);
                clientCache.SetExpires(DateTime.UtcNow.AddYears(-1));
                clientCache.SetValidUntilExpires(true);
                clientCache.AppendCacheExtension("must-revalidate");
                clientCache.SetNoServerCaching();
                clientCache.VaryByHeaders["If-None-Match"] = true;
                clientCache.SetETag(eTag);

                if (eTagChanged)
                {
                    // Output text content of asset
                    response.ContentType = ContentType;
                    response.Write(content);
                }
            }

            context.ApplicationInstance.CompleteRequest();
        }

        private static void ProcessStaticAssetRequest(HttpContextBase context)
        {
            _staticFileHandler.Value.ProcessRequest(context.ApplicationInstance.Context);
        }

        private static IHttpHandler CreateStaticFileHandlerInstance()
        {
            const string fullTypeName = "System.Web.StaticFileHandler";
            Assembly assembly = typeof(HttpApplication).Assembly;
            Type type = assembly.GetType(fullTypeName, true);

            var handler = (IHttpHandler)Activator.CreateInstance(type, true);
            if (handler == null)
            {
                throw new HttpException(500, string.Format(Strings.Common_InstanceCreationFailed,
                    fullTypeName, assembly.FullName));
            }

            return handler;
        }

        protected virtual string GetCacheKey(string assetVirtualPath, string bundleVirtualPath)
        {
            if (string.IsNullOrWhiteSpace(assetVirtualPath))
            {
                throw new ArgumentException(
                    string.Format(Strings.Common_ArgumentIsEmpty, "assetVirtualPath"), "assetVirtualPath");
            }

            string processedAssetVirtualPath = UrlHelpers.ProcessBackSlashes(assetVirtualPath);
            string key = string.Format("BT:ProcessedAssetContent_{0}", processedAssetVirtualPath.ToLowerInvariant());
            if (!string.IsNullOrWhiteSpace(bundleVirtualPath))
            {
                string processedBundleVirtualPath = UrlHelpers.ProcessBackSlashes(bundleVirtualPath);
                processedBundleVirtualPath = UrlHelpers.RemoveLastSlash(processedBundleVirtualPath);

                key += "_" + processedBundleVirtualPath.ToLowerInvariant();
            }

            return key;
        }

        private string GetProcessedAssetContent(string assetVirtualPath, string bundleVirtualPath)
        {
            if (string.IsNullOrWhiteSpace(assetVirtualPath))
            {
                throw new ArgumentException(
                    string.Format(Strings.Common_ArgumentIsEmpty, "assetVirtualPath"), "assetVirtualPath");
            }

            string content;

            if (_assetHandlerConfig.DisableServerCache)
            {
                IAsset processedAsset = ProcessAsset(assetVirtualPath, bundleVirtualPath);
                content = processedAsset.Content;
            }
            else
            {
                lock (_cacheSynchronizer)
                {
                    string cacheItemKey = GetCacheKey(assetVirtualPath, bundleVirtualPath);
                    object cacheItem = _cache.Get(cacheItemKey);

                    if (cacheItem != null)
                    {
                        content = (string)cacheItem;
                    }
                    else
                    {
                        IAsset processedAsset = ProcessAsset(assetVirtualPath, bundleVirtualPath);
                        content = processedAsset.Content;

                        DateTime utcStart = DateTime.UtcNow;
                        DateTime absoluteExpiration = DateTime.Now.AddMinutes(
                            _assetHandlerConfig.ServerCacheDurationInMinutes);
                        TimeSpan slidingExpiration = Cache.NoSlidingExpiration;

                        var fileDependencies = new List<string> { assetVirtualPath };
                        fileDependencies.AddRange(processedAsset.VirtualPathDependencies);

                        var cacheDep = _virtualFileSystemWrapper.GetCacheDependency(assetVirtualPath,
                            fileDependencies.ToArray(), utcStart);

                        _cache.Insert(cacheItemKey, content, cacheDep,
                            absoluteExpiration, slidingExpiration, CacheItemPriority.Low, null);
                    }
                }
            }

            return content;
        }

        private static string GenerateAssetETag(string assetContent)
        {
            string hash;

            using (var hashAlgorithm = CreateHashAlgorithm())
            {
                hash = HttpServerUtility.UrlTokenEncode(
                    hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(assetContent)));
            }

            string eTag = string.Format("\"{0}\"", hash);

            return eTag;
        }

        private static SHA256 CreateHashAlgorithm()
        {
            bool isMonoRuntime = (Type.GetType("Mono.Runtime") != null);
            SHA256 hashAlgorithm;

            if (!isMonoRuntime)
            {
                hashAlgorithm = new SHA256CryptoServiceProvider();
            }
            else
            {
                hashAlgorithm = new SHA256Managed();
            }

            return hashAlgorithm;
        }

        private static bool IsETagHeaderChanged(HttpRequestBase request, string eTag)
        {
            bool eTagChanged = true;
            string ifNoneMatch = request.Headers["If-None-Match"];

            if (!string.IsNullOrWhiteSpace(ifNoneMatch))
            {
                eTagChanged = (ifNoneMatch != eTag);
            }

            return eTagChanged;
        }

        private IAsset ProcessAsset(string assetVirtualPath, string bundleVirtualPath)
        {
            BundleFile bundleFile = null;
            ITransformer transformer = null;

            if (!string.IsNullOrWhiteSpace(bundleVirtualPath))
            {
                Bundle bundle = GetBundleByVirtualPath(bundleVirtualPath);
                if (bundle == null)
                {
                    throw new HttpException(500, string.Format(Strings.AssetHandler_BundleNotFound, bundleVirtualPath));
                }

                bundleFile = GetBundleFileByVirtualPath(bundle, assetVirtualPath);
                if (bundleFile == null)
                {
                    throw new HttpException(500, string.Format(Strings.AssetHandler_BundleFileNotFound,
                        assetVirtualPath, bundleVirtualPath));
                }

                transformer = GetTransformer(bundle);
                if (transformer == null)
                {
                    throw new HttpException(500, string.Format(Strings.AssetHandler_TransformerNotFound, bundleVirtualPath));
                }
            }

            IAsset asset = new Asset(assetVirtualPath, bundleFile);

            if (!IsStaticAsset)
            {
                asset = TranslateAsset(asset, transformer, BundleTransformerContext.Current.IsDebugMode);
            }

            if (transformer != null)
            {
                asset = PostProcessAsset(asset, transformer);
            }

            return asset;
        }

        protected virtual Bundle GetBundleByVirtualPath(string virtualPath)
        {
            Bundle bundle = BundleTable.Bundles.GetBundleFor(virtualPath);

            return bundle;
        }

        protected virtual BundleFile GetBundleFileByVirtualPath(Bundle bundle, string virtualPath)
        {
            BundleFile file = null;
            string url = _virtualFileSystemWrapper.ToAbsolutePath(virtualPath);
            url = UrlHelpers.ProcessBackSlashes(url);
            url = RemoveAdditionalFileExtension(url);

            var bundleContext = new BundleContext(_context, BundleTable.Bundles, bundle.Path);
            IEnumerable<BundleFile> bundleFiles = bundle.EnumerateFiles(bundleContext);

            foreach (BundleFile bundleFile in bundleFiles)
            {
                string bundleFileUrl = _virtualFileSystemWrapper.ToAbsolutePath(bundleFile.VirtualFile.VirtualPath);
                bundleFileUrl = UrlHelpers.ProcessBackSlashes(bundleFileUrl);
                bundleFileUrl = RemoveAdditionalFileExtension(bundleFileUrl);

                if (string.Equals(bundleFileUrl, url, StringComparison.OrdinalIgnoreCase))
                {
                    file = bundleFile;
                    break;
                }
            }

            return file;
        }

        protected virtual string RemoveAdditionalFileExtension(string assetPath)
        {
            return assetPath;
        }

        protected abstract ITransformer GetTransformer(Bundle bundle);

        protected virtual IAsset TranslateAsset(IAsset asset, ITransformer transformer, bool isDebugMode)
        {
            return asset;
        }

        protected IAsset InnerTranslateAsset<T>(string translatorName, IAsset asset, ITransformer transformer, bool isDebugMode) where T : class, ITranslator
        {
            IAsset processedAsset = asset;
            T translator;

            if (transformer != null)
            {
                translator = GetTranslatorByType<T>(transformer);
                if (translator == null)
                {
                    throw new HttpException(500, string.Format(Strings.AssetHandler_TranslatorNotFound,
                        typeof(T).FullName, asset.Url));
                }
            }
            else
            {
                translator = GetTranslatorByName<T>(translatorName);
            }

            if (translator != null)
            {
                processedAsset = translator.Translate(processedAsset);
                translator.IsDebugMode = isDebugMode;
            }

            return processedAsset;
        }

        protected abstract T GetTranslatorByName<T>(string translatorName) where T : class, ITranslator;

        protected T GetTranslatorByType<T>(ITransformer transformer) where T : class, ITranslator
        {
            ITranslator translator = transformer.Translators.FirstOrDefault(t => t is T);

            return (T)translator;
        }

        protected virtual IAsset PostProcessAsset(IAsset asset, ITransformer transformer)
        {
            IList<IPostProcessor> availablePostProcessors = transformer.PostProcessors
                .Where(p => p.UseInDebugMode)
                .ToList()
                ;

            foreach (IPostProcessor postProcessor in availablePostProcessors)
            {
                postProcessor.PostProcess(asset);
            }

            return asset;
        }
    }
}