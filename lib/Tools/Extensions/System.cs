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
        public static string DateTimeText(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

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
    public static class _TypeExtensions
    {
        public const BindingFlags BindingFlags0 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        public const BindingFlags BindingFlags1 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        public const BindingFlags BindingFlags2 = BindingFlags.Public | BindingFlags.Instance |  BindingFlags.GetProperty | BindingFlags.DeclaredOnly;
        public const BindingFlags BindingFlags3 = BindingFlags.Public | BindingFlags.Instance |  BindingFlags.GetField | BindingFlags.DeclaredOnly;
        public const BindingFlags BindingFlags4 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static bool IsSubclassOf(this Type type, Type c, bool include_self = false)
        {
            if (type == null) return false;
            bool n = type.IsSubclassOf(c);
            if (include_self) n |= type == c;
            return n;
        }
        public static bool IsSubclassOf<T>(this Type type, bool include_self = false)
        {
            return IsSubclassOf(type, typeof(T), include_self);
        }

        public static bool IsEquals<T>(this Type type)
        {
            return type == typeof(T);
        }

        /// <summary>
        /// Get FieldType or PropertyType
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Type ValueType(this MemberInfo m)
        {
            FieldInfo f = m as FieldInfo;
            if (f != null) return f.FieldType;
            PropertyInfo p = m as PropertyInfo;
            if (p != null) return p.PropertyType;
            throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }
        /// <summary>
        /// Get Value for FieldInfo or PropertyInfo
        /// </summary>
        /// <param name="m"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetValue(this MemberInfo m, object obj)
        {
            FieldInfo f = m as FieldInfo;
            if (f != null) return f.GetValue(obj);
            PropertyInfo p = m as PropertyInfo;
            if (p != null) return p.GetValue(obj, null);
            throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }

        public static void SetValue(this MemberInfo m, object obj, object value)
        {
            if (m is FieldInfo)
                ((FieldInfo)m).SetValue(obj, value);
            else if (m is PropertyInfo)
                ((PropertyInfo)m).SetValue(obj, value, null);
            else
                throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }

#if NET40
        public static Type GetTypeInfo(this Type type) => type;
#endif
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

    internal class TypeContract : Dictionary<string, MemberInfo>
    {
        static Dictionary<Type, TypeContract> all = new Dictionary<Type, TypeContract>();

        private readonly TypeContract _base;
        private TypeContract(Type type)
        {
            foreach (MemberInfo m in type.GetMembers(_TypeExtensions.BindingFlags4))
            {
                if (this.ContainsKey(m.Name)) continue;
                this[m.Name] = m;
            }
            all[type] = this;
            Type b = type.GetTypeInfo().BaseType;
            if (b != null)
                if (!all.TryGetValue(b, out this._base))
                    this._base = new TypeContract(b);
        }

        public bool GetMember(string name, out MemberInfo value)
        {
            if (this.TryGetValue(name, out value))
                return true;
            if (this._base != null)
                return this._base.GetMember(name, out value);
            return false;
        }

        public bool GetMember<T>(string name, out T value) where T : MemberInfo
        {
            MemberInfo m;
            if (this.TryGetValue(name, out m))
                if ((value = m as T) != null)
                    return true;
            if (this._base != null)
                return this._base.GetMember(name, out value);
            value = null;
            return false;
        }

        public static TypeContract GetContract(Type t)
        {
            TypeContract result;
            lock (all)
            {
                if (all.TryGetValue(t, out result))
                    return result;
                else
                    return new TypeContract(t);
            }
        }
    }
}