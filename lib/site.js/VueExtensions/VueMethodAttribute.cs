using Bridge;
using System;

namespace Vue
{
    /// <summary>
    /// Only static methods are supported.
    /// </summary>
    //[FileName(InnateGlory._Consts.bridge_vue)]
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class VueMethodAttribute : Attribute
    {
    }
}