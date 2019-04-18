using Bridge;
using System;

#if wasm
namespace Microsoft.AspNetCore.Mvc
{
    public interface IActionResult { }
}
#elif jslib

namespace InnateGlory
{
    [NonScriptable]
    public static class JsonHelper
    {
        [NonScriptable]
        public class StringEnumAttribute : Attribute
        {
            public StringEnumAttribute(bool asString = true) { }
        }
    }

    //public interface IApiResult { }
}

//namespace Microsoft.AspNetCore.Mvc
//{
//    [NonScriptable]
//    public interface IActionResult { }
//}

namespace Newtonsoft.Json
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class JsonObjectAttribute : Attribute
    {
        public JsonObjectAttribute() { }
        public JsonObjectAttribute(MemberSerialization memberSerialization) { }
        public JsonObjectAttribute(string id) { }

        public MemberSerialization MemberSerialization { get; set; }
        public Required ItemRequired { get; set; }
    }
    [NonScriptable]
    public enum MemberSerialization
    {
        OptOut = 0,
        OptIn = 1,
        Fields = 2
    }
}
namespace System.Net.Http { }
namespace Microsoft.AspNetCore.Mvc { }
namespace Microsoft.Extensions.DependencyInjection { }
namespace InnateGlory.Api { }

#else

namespace Bridge
{
    //public sealed class NonScriptableAttribute : Attribute { }
    public sealed class InlineConstAttribute : Attribute { }
    public enum Emit
    {
        Name = 1,
        Value = 2,
        StringName = 3,
        StringNamePreserveCase = 4,
        StringNameLowerCase = 5,
        StringNameUpperCase = 6,
        NamePreserveCase = 7,
        NameLowerCase = 8,
        NameUpperCase = 9
    }
    public class EnumAttribute : Attribute
    {
        public EnumAttribute(Emit emit) { }
    }
}

#endif

//namespace StackExchange.Redis { }
//namespace Microsoft.AspNetCore.Mvc.ModelBinding.Validation { }
//namespace Microsoft.Extensions.DependencyInjection { }
//namespace StackExchange.Redis { }
//namespace System.Net { }
//namespace InnateGlory.Api { }
