// 實作遊戲平台相關的 api 時, 可能因為差異性太大, 實際上是不一樣的 Action
// 但是會有需要使用相同 route 的情況
// 這時候會如果 request body 包含 "PlatformType", "PlatformId", "PlatformName" 其中之一
// 將會根據這些參數的值決定最適合的 action
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace InnateGlory
{
    public class ActionSelectorOptions
    {
        public Func<RouteContext, PageActionDescriptor, bool> SelectCandidate { get; set; }
    }
    /// <summary>
    /// ActionSelector with <see cref="PlatformType"/> & <see cref="PaymentType"/>
    /// </summary>
    class ExtraActionSelector : IActionSelector //ActionSelector
    {
        private ActionSelector _actionSelector;
        private DataService _dataServices;
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<ActionSelectorOptions> _options;

        //public ExtraActionSelector(
        //    IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
        //    ActionConstraintCache actionConstraintCache,
        //    ILoggerFactory loggerFactory,
        //    IHttpContextAccessor httpContextAccessor,
        //    DataService dataServices,
        //    IOptions<ActionSelectorOptions> options)
        //    : base(actionDescriptorCollectionProvider, actionConstraintCache, loggerFactory)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //    _dataServices = dataServices;
        //}

        public ExtraActionSelector(IHttpContextAccessor httpContextAccessor, DataService dataServices, IOptions<ActionSelectorOptions> options)
        {
            _actionSelector = dataServices.CreateInstance<ActionSelector>();
            _httpContextAccessor = httpContextAccessor;
            _dataServices = dataServices;
            _options = options;
        }

        //protected override IReadOnlyList<ActionDescriptor> SelectBestActions(IReadOnlyList<ActionDescriptor> actions)
        //{
        //    var _actions = base.SelectBestActions(actions);
        //    ActionDescriptor _best = null;
        //    Models.PlatformInfo m1 = null;
        //    Models.PaymentInfo m2 = null;
        //    if (_actions != null && _actions.Count > 1)
        //    {
        //        foreach (var action in actions)
        //        {
        //            if (action is PageActionDescriptor)
        //            {
        //                ;
        //            }
        //            else
        //            {
        //                ApiAttribute a0 = null;
        //                PlatformInfoAttribute a1 = null;
        //                PaymentInfoAttribute a2 = null;
        //                foreach (var f in action.FilterDescriptors)
        //                {
        //                    a0 = a0 ?? f.Filter as ApiAttribute;
        //                    a1 = a1 ?? f.Filter as PlatformInfoAttribute;
        //                    a2 = a2 ?? f.Filter as PaymentInfoAttribute;
        //                }
        //                if (a0 == null) continue;
        //                if (IsMatch(_httpContextAccessor.HttpContext, a1, ref m1) ||
        //                    IsMatch(_httpContextAccessor.HttpContext, a2, ref m2))
        //                {
        //                    if (_best == null)
        //                        _best = action;
        //                    else
        //                        return _actions;
        //                }
        //            }
        //        }
        //    }
        //    if (_best != null)
        //        return new ActionDescriptor[] { _best };
        //    return _actions;
        //}

        private bool IsMatch(HttpContext httpContext, PlatformInfoAttribute attr, ref Models.PlatformInfoModel model)
        {
            if (attr == null) return false;
            if (model == null)
                httpContext.RequestServices.GetService<RequestBody>().GetBodyJson(out model);
            //httpContext.Request.GetBodyJson(out model);
            if (model == null) return false;

            if (attr.PlatformType == model.PlatformType)
                return true;

            var ps = _dataServices.GetService<PlatformInfoProvider>();

            if (model.PlatformId.HasValue)
            {
                var platform = ps[model.PlatformId.Value];
                if (attr.PlatformType == platform?.PlatformType)
                    return true;
            }

            if (model.PlatformName.IsValid)
            {
                var platform = ps[model.PlatformName];
                if (attr.PlatformType == platform?.PlatformType)
                    return true;
            }
            return false;
        }

        private bool IsMatch(HttpContext httpContext, PaymentInfoAttribute attr, ref Models.PaymentInfoModel model)
        {
            if (attr == null) return false;
            if (model == null)
                httpContext.RequestServices.GetService<RequestBody>().GetBodyJson(out model);
            //httpContext.Request.GetBodyJson(out model);
            if (model == null) return false;

            if (attr.PaymentType == model.PaymentType)
                return true;

            return false;
        }

        ActionDescriptor IActionSelector.SelectBestCandidate(RouteContext context, IReadOnlyList<ActionDescriptor> candidates)
        {
            if (candidates != null && candidates.Count > 1)
            {
                bool is_pages = true;
                foreach (var _action in candidates)
                {
                    is_pages &= _action is PageActionDescriptor;
                    if (is_pages == false)
                        break;
                }
                if (is_pages)
                {
                    foreach (PageActionDescriptor action in candidates)
                    {
                        if (true == _options.Value.SelectCandidate?.Invoke(context, action))
                            return action;
                    }
                }
                else
                {
                    ActionDescriptor _best = null;
                    Models.PlatformInfoModel m1 = null;
                    Models.PaymentInfoModel m2 = null;

                    foreach (var action in candidates)
                    {
                        ApiAttribute a0 = null;
                        PlatformInfoAttribute a1 = null;
                        PaymentInfoAttribute a2 = null;
                        foreach (var f in action.FilterDescriptors)
                        {
                            a0 = a0 ?? f.Filter as ApiAttribute;
                            a1 = a1 ?? f.Filter as PlatformInfoAttribute;
                            a2 = a2 ?? f.Filter as PaymentInfoAttribute;
                        }
                        if (a0 == null) continue;
                        if (IsMatch(_httpContextAccessor.HttpContext, a1, ref m1) ||
                            IsMatch(_httpContextAccessor.HttpContext, a2, ref m2))
                        {
                            if (_best == null)
                                _best = action;
                            else
                                break;
                        }
                    }
                    if (_best != null)
                        return _best;
                }
            }
            return _actionSelector.SelectBestCandidate(context, candidates);
        }

        IReadOnlyList<ActionDescriptor> IActionSelector.SelectCandidates(RouteContext context)
        {
            return _actionSelector.SelectCandidates(context);
        }
    }
}