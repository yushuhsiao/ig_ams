using InnateGlory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InnateGlory.Messages
{
    public sealed class MessageInvoker<TAttribute>
        where TAttribute : MessageInvokerAttribute
    {
        private ILogger<MessageInvoker<TAttribute>> _logger;
        private readonly _Action[] actions;

        public MessageInvoker(IServiceProvider services, ILogger<MessageInvoker<TAttribute>> logger)
        {
            this._logger = logger;
            //IEnumerable<ApplicationPart> parts =
            //    provider.GetService<ApplicationPartManager>()?.ApplicationParts ??
            //    DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(
            //        //serviceProvider.GetRequiredService<IHostingEnvironment>().ApplicationName
            //        Assembly.GetEntryAssembly().GetName().Name);
            //IEnumerable<ApplicationPart> parts = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Assembly.GetEntryAssembly().GetName().Name);
            IEnumerable<ApplicationPart> parts = services.GetService<ApplicationPartManager>().ApplicationParts;
            List<_Action> list = new List<_Action>();
            foreach (var p in parts.OfType<AssemblyPart>())
            {
                foreach (var t in p.Types)
                {
                    foreach (var m in t.GetMethods(_Consts.BindingAttrs))
                    {
                        foreach (TAttribute attr in m.GetCustomAttributes<TAttribute>())
                        {
                            list.Add(new _Action(this, attr, m, t));
                        }
                    }
                }
            }
            this.actions = list.ToArray();
        }

        public IEnumerable<_Action> GetActions(Predicate<_Action> match)
        {
            for (int i = 0, n = actions.Length; i < n; i++)
            {
                var action = actions[i];
                if (match(action))
                    yield return action;
            }
        }

        public IEnumerable<_Action> GetActions<TDeclaringType>(Predicate<_Action> match)
        {
            for (int i = 0, n = actions.Length; i < n; i++)
            {
                var action = actions[i];
                if (action.MethodInfo.DeclaringType != typeof(TDeclaringType))
                    continue;
                if (match(action))
                    yield return action;
            }
        }

        public sealed class _Action
        {
            private MessageInvoker<TAttribute> _owner;
            private ObjectMethodExecutor _executor;
            public TAttribute Attribute { get; }
            public string Name => Attribute.Name ?? _executor.MethodInfo.Name;
            private _Parameter[] Parameters { get; }
            public MethodInfo MethodInfo => _executor.MethodInfo;
            public Type AsyncResultType => _executor.AsyncResultType;
            public Type ReturnType => _executor.MethodReturnType;
            public bool IsAsync => _executor.IsMethodAsync;

            private object target_instance; // use for InstanceFlags.CreateOnce

            internal _Action(MessageInvoker<TAttribute> owner, TAttribute attr, MethodInfo methodInfo, TypeInfo targetTypeInfo)
            {
                this._owner = owner;
                this._executor = ObjectMethodExecutor.Create(methodInfo, targetTypeInfo);
                this.Attribute = attr;
                this.Parameters = new _Parameter[_executor.MethodParameters.Length];
                for (int i = 0; i < _executor.MethodParameters.Length; i++)
                    this.Parameters[i] = new _Parameter(_executor.MethodParameters[i]);
            }

            private bool GetTargetInstance(IServiceProvider services, out object target)
            {
                target = null;
                if (Attribute.Instance == InstanceFlags.CreateOnce)
                {
                    lock (this)
                    {
                        if (this.target_instance == null)
                            this.target_instance = services.CreateInstance(MethodInfo.DeclaringType);
                        target = this.target_instance;
                    }
                }
                else if (Attribute.Instance == InstanceFlags.FromService)
                    target = services.GetService(MethodInfo.DeclaringType);
                else if (Attribute.Instance == InstanceFlags.FromRequiredService)
                {
                    target = services.GetService(MethodInfo.DeclaringType);
                    if (target == null) return false;
                }

                if (target == null)
                    target = services.CreateInstance(MethodInfo.DeclaringType);

                return true;
            }

            public object[] CreateParameters(IServiceProvider services, object data)
            {
                object[] args = new object[this.Parameters.Length];
                // get value from services or default value
                for (int i = 0; i < args.Length; i++)
                {
                    var p = this.Parameters[i];
                    if (p.FromServices)
                        args[i] = services.GetService(p.ParameterType);
                    else if (p.Parameter.HasDefaultValue)
                        args[i] = p.Parameter.DefaultValue;
                }

                if (data is JArray)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (this.Parameters[i].GetValue((JArray)data, out object value))
                        {
                            args[i] = value;
                            break;
                        }
                    }
                }
                else if (data is JToken)
                {
                    JToken token = (JToken)data;
                    for (int i = 0; i < args.Length; i++)
                    {
                        var p = this.Parameters[i];
                        if (p.GetValue(token[p.Name], out object value))
                            args[i] = value;
                    }
                }
                else if (data != null)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        var p = this.Parameters[i];
                        if (p.GetValue(data, out object value))
                        {
                            args[i] = value;
                            break;
                        }
                    }
                }
                return args;
            }

            public object Execute(IServiceProvider services, object data)
            {
                if (GetTargetInstance(services, out object instance))
                    return Execute(instance, services, data);
                return null;
            }

            public object Execute(object target, IServiceProvider services, object data)
            {
                object[] args = CreateParameters(services, data);
                return this._executor.Execute(target, args);
                //try
                //{
                //    object[] args = CreateParameters(provider, data);
                //    return this.Method.Invoke(instance, args);
                //}
                //catch (TargetInvocationException ex)
                //{
                //    throw ex.InnerException;
                //}
            }

            public async Task<object> ExecuteAsync(IServiceProvider services, object data)
            {
                if (GetTargetInstance(services, out object instance))
                    return await ExecuteAsync(instance, services, data);
                return Task.FromCanceled(System.Threading.CancellationToken.None);
            }

            public async Task<object> ExecuteAsync(object target, IServiceProvider services, object data)
            {
                object[] args = CreateParameters(services, data);
                return await this._executor.ExecuteAsync(target, args);
            }
        }
        internal class _Parameter
        {
            public string Name => this.Parameter.Name;
            public Type ParameterType => this.Parameter.ParameterType;
            public ParameterInfo Parameter { get; }
            public bool FromServices { get; }

            public _Parameter(ParameterInfo parameter)
            {
                this.Parameter = parameter;
                this.FromServices = parameter.GetCustomAttribute<FromServicesAttribute>() != null;
            }

            public bool GetValue(object data, out object result)
            {
                if (data == null || this.FromServices)
                    return _null.noop(false, out result);
                return _Convert.ConvertTo(data.GetType(), data, this.ParameterType, out result);
            }

            public bool GetValue(JToken token, out object result)
            {
                if (token == null || this.FromServices)
                    return _null.noop(false, out result);
                try
                {
                    if (this.ParameterType.IsArray && token is JArray)
                        result = ((JArray)token).ToArray(this.ParameterType.GetElementType());
                    else
                        result = token.ToObject(this.ParameterType);
                    return true;
                }
                catch { }
                return _null.noop(false, out result);
            }
        }
    }

    public class MessageInvokerAttribute : Attribute
    {
        public string Name { get; set; }
        public InstanceFlags Instance { get; set; } = InstanceFlags.CreateOnce;
    }
}