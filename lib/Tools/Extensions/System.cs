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
    public static class DateTimeExtensions
    {
        public static string FormatAs(this DateTime dateTime, string format = DateTimeFormat, int ms_digit = 0)
        {
            if (ms_digit <= 0)
                return dateTime.ToString(format);
            if (ms_digit > 6)
                ms_digit = 6;
            return dateTime.ToString(format + "." + new String('f', ms_digit));
        }
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DateTimeFormatEx = DateTimeFormat + ".ffffff";

        //public static readonly DateTime UnixBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //static long unix_time_base_tick = UnixBaseTime.Ticks;

        //public static long ToUnixTime(this DateTime t)
        //{
        //    return (t.Ticks - unix_time_base_tick) / 10000;
        //}
        //public static DateTime FromUnixTime(long unixtime)
        //{
        //    return new DateTime((unixtime + unix_time_base_tick) * 10000);
        //}

        public static DateTime ToACTime(this DateTime datetime)
        {
            return datetime.AddHours(-12).Date;
        }
    }

    [_DebuggerStepThrough]
    public static partial class ArrayExtensions
    {
        public static bool TryGetValueAt<T>(this T[] array, int index, out T result)
        {
            if (array.Length < index)
            {
                result = array[index];
                return true;
            }
            result = default(T);
            return false;
        }
        public static T GetValueAt<T>(this T[] array, int index)
        {
            T result;
            array.TryGetValueAt(index, out result);
            return result;
        }
        public static int indexOf(this byte[] src, byte[] value, int start, int count)
        {
            if ((src == null) || (value == null)) return -1;
            if (src.Length < value.Length) return -1;
            //int end = start + count;
            //if (src.Length < end)
            //    end = src.Length;
            //for (; start < end; start++)
            //{
            //    count = end - start - value.Length + 1;
            //    start = Array.IndexOf<byte>(src, value[0], start, count);
            //    bool f = true;
            //    count = value.Length;
            //    for (int i = start + 1, j = 1; f && (j < count); i++, j++)
            //        f = src[i] == value[j];
            //    if (f) return start;
            //}

            for (int i = start, end = start + count - value.Length; i <= end; i++)
            {
                if (src[i] == value[0])
                {
                    bool f = true;
                    int length = value.Length;
                    for (int j1 = i + 1, j2 = 1; f && (j2 < length); j1++, j2++)
                        f = src[j1] == value[j2];
                    if (f) return i;
                }
            }
            return -1;
        }
        public static bool IsNullOrEmpty(this System.Collections.IList list)
        {
            if (list == null) return true;
            if (list.Count == 0) return true;
            return false;
        }

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
