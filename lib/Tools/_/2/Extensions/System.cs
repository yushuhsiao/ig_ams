using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
    [_DebuggerStepThrough]
    public static partial class ArrayExtensions
    {

        //public static long indexOf(this byte[] src, byte[] value, long start, long count)
        //{
        //    if ((src == null) || (value == null)) return -1;
        //    if ((src.Length == 0) || (value.Length == 0)) return -1;
        //    if (src.Length < value.Length) return -1;
        //    long cnt2 = src.LongLength - start;
        //    if (count > cnt2) count = cnt2;
        //    for (long i = start, end = start + count - value.Length; i <= end; i++)
        //    {
        //        if (src[i] == value[0])
        //        {
        //            bool f = true;
        //            for (int j = 1; f && (j < value.Length); j++)
        //                f &= src[i + j] == value[j];
        //            if (f) return i;
        //        }
        //    }
        //    return 0;
        //}
    }    
}
#if NET40
namespace System.Reflection
{
    internal static class _IntrospectionExtensions
    {
        public static Type GetTypeInfo(this Type type) => type;
    }
}
#endif
