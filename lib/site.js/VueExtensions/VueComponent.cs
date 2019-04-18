using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bridge;
using Retyped;

namespace Vue
{
    /// <summary>
    /// Vue componenent with a specific Model type.
    /// </summary>
    /// <typeparam name="TModel">Model (data) type</typeparam>
    //[FileName(InnateGlory._Consts.bridge_vue)]
    public class VueComponent<TModel> : vue.ComponentOptions<vue.Vue>
    {
        /// <summary>
        /// Overrides <see cref="vue.ComponentOptions{V}.data"/> with the actual model type.
        /// </summary>
        [Name("data")]
        public new Func<TModel> data { private get; set; }

        /// <summary>
        /// Model instance. Should be used in [VueMethods]/[VueComputed] methods.
        /// </summary>
        protected static extern TModel Model
        {
            [Template("this")]
            get;
        }

        /// <summary>
        /// Creates a Vue componenent with a specific Model type.
        /// </summary>
        /// <param name="registerMembers">If true, automatically registers [VueMethods]/[VueComputed] methods.</param>
        public VueComponent(bool registerMembers = true)
        {
            if (registerMembers)
            {
                methods = RegisterMethods();
                computed = RegisterComputed();
            }
        }

        /// <summary>
        /// Converts component into <see cref="vue.Component"/> type.
        /// </summary>
        [Template("{this}")]
        public extern vue.Component AsComponent();

        /// <summary>
        /// Collects and registers STATIC methods marked with <see cref="VueMethodAttribute"/> attribute.
        /// </summary>
        private methodsConfig RegisterMethods()
        {
            var publicMembers = GetType().GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            var config = new methodsConfig();

            foreach (var member in publicMembers)
            {
                var method = member as MethodInfo;
                if (method == null)
                {
                    continue;
                }

                if (method.IsConstructor || method.IsSpecialName)
                {
                    // Skip constructors and special methods.
                    continue;
                }

                var isVueMethod = method.GetCustomAttributes(typeof(VueMethodAttribute)).Any();
                if (!isVueMethod)
                {
                    continue;
                }

                config[method.ScriptName] = (methodsConfig.keyFn) method.DeclaringType[method.ScriptName];
            }

            return config;
        }

        /// <summary>
        /// Collects and registers STATIC methods (for computed properties) marked with <see cref="VueComputedAttribute"/> attribute.
        /// </summary>
        private computedConfig RegisterComputed()
        {
            var publicMembers = GetType().GetMembers(BindingFlags.Public | BindingFlags.Static);

            var config = new computedConfig();

            foreach (var member in publicMembers)
            {
                var method = member as MethodInfo;
                if (method == null)
                {
                    continue;
                }

                if (method.IsConstructor || method.IsSpecialName)
                {
                    // Skip constructors and special methods.
                    continue;
                }

                var isVueComputed = method.GetCustomAttributes(typeof(VueComputedAttribute)).Any();
                if (!isVueComputed)
                {
                    continue;
                }

                config[method.ScriptName] = (Func<object>)method.DeclaringType[method.ScriptName];
            }

            return config;
        }
    }

    /// <summary>
    /// Vue componenent with a specific Model and Properties type.
    /// </summary>
    /// <typeparam name="TModel">Model (data) type</typeparam>
    /// <typeparam name="TProperties">Properties type</typeparam>
    //[FileName(InnateGlory._Consts.bridge_vue)]
    public class VueComponent<TModel, TProperties> : VueComponent<TModel>
    {
        /// <summary>
        /// Properties instance. Should be used in [VueMethods]/[VueComputed] methods.
        /// </summary>
        protected static extern TProperties Properties
        {
            [Template("this")]
            get;
        }

        /// <summary>
        /// Creates a Vue componenent with a specific Model and Properties type.
        /// </summary>
        /// <param name="registerMembers">If true, automatically registers [VueMethods]/[VueComputed] methods.</param>
        public VueComponent(bool registerMembers = true)
            : base(registerMembers)
        {
            props = GetCmpPropertyNames();
        }

        /// <summary>
        /// Collects component's property names
        /// </summary>
        private static string[] GetCmpPropertyNames()
        {
            var names = new List<string>();

            var propsEntityMembers = typeof(TProperties).GetMembers(BindingFlags.Public | BindingFlags.Instance);
            foreach (var member in propsEntityMembers)
            {
                var field = member as FieldInfo;
                if (field != null && !field.IsSpecialName)
                {
                    names.Add(field.ScriptName);
                    continue;
                }

                var prop = member as PropertyInfo;
                if (prop != null)
                {
                    names.Add(prop.ScriptFieldName ?? prop.Name);
                }
            }

            return names.ToArray();
        }
    }
}