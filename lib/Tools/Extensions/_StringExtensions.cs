﻿using System.Collections.Generic;
using System.Net;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
    [_DebuggerStepThrough]
    public static partial class _StringExtensions
    {
        public static string Trim(this string s, bool nullOnEmpty = false)
        {
            if (s == null)
                return null;
            s = s.Trim();
            if (s == string.Empty && nullOnEmpty)
                return null;
            return s;
        }

        public static bool Contains(this string s, char c)
        {
            if (s != null)
            {
                for (int i = 0, n = s.Length; i < n; i++)
                    if (s[i] == c)
                        return true;
            }
            return false;
        }
        public static bool Contains(this string[] s, string str, bool ignoreCase = true)
        {
            if (s != null)
            {
                for (int i = 0, n = s.Length; i < n; i++)
                    if (s[i].IsEquals(str, ignoreCase: ignoreCase))
                        return true;
            }
            return false;
        }

        public static int Split(this string src, char separator, out string left, out string right)
        {
            if (src != null)
            {
                int n = src.IndexOf(separator);
                if (n >= 0)
                {
                    left = src.Substring(0, n);
                    right = src.Substring(n + 1);
                    return n;
                }
            }
            left = src;
            right = "";
            return -1;
        }

        public static int Split(this string src, string separator, out string left, out string right)
        {
            if (src != null)
            {
                for (int i = 0; i < separator.Length; i++)
                {
                    int n = src.IndexOf(separator[i]);
                    if (n >= 0)
                    {
                        left = src.Substring(0, n);
                        right = src.Substring(n + 1);
                        return n;
                    }
                }
            }
            left = src;
            right = "";
            return -1;
        }

        public static string Substring(this string src, char l, char r, bool include)
        {
            if (src == null) return null;
            int _l = src.IndexOf(l);
            if (_l < 0) return null;
            int _r = src.IndexOf(r, _l);
            if (_r < 0) return null;
            int len = _r - _l;
            if (include)
            {
                len++;
            }
            else
            {
                len--;
                _l++;
            }
            if (len > 0)
                return src.Substring(_l, len);
            return null;
        }

        public static string ReplaceAll(this string s, string oldValue, string newValue)
        {
            if (s != null)
            {
                while (s.Contains(oldValue))
                    s.Replace(oldValue, newValue);
            }
            return s;
        }

        public static string ToHexString(this byte[] array, string format = "{0:x2}", string prefix = "0x")
        {
            StringBuilder s = new StringBuilder();
            if (!string.IsNullOrEmpty(prefix))
                s.Append(prefix);
            for (int i = 0; i < array.Length; i++)
                s.AppendFormat(format, array[i]);
            return s.ToString();
        }

        public static bool IsEquals(this string strA, string strB, bool ignoreCase = true)
        {
            bool a = strA == null;
            bool b = strB == null;
            if (a | b) return a == b;
            return 0 == string.Compare(strA, strB, ignoreCase);
        }

        public static bool IsNotEquals(this string strA, string strB, bool ignoreCase = true)
        {
            bool a = strA == null;
            bool b = strB == null;
            if (a | b) return a != b;
            return 0 != string.Compare(strA, strB, ignoreCase);
        }

        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        //public static string FormatIf(this string format, object value)
        //{
        //    if (value == null)
        //        return null;
        //    else
        //        return string.Format(format, value);
        //}

        //public static string Append(this string s, string value, bool checkNullOrEmpty = true)
        //{
        //    if (checkNullOrEmpty && string.IsNullOrEmpty(s))
        //        return s;
        //    return $"{s}{value}";
        //}

        //delegate T? GetValueHandler<T>(string s) where T : struct;


        //public static byte[] GetBytes(this string s, Encoding encoding)
        //{
        //    return encoding.GetBytes(s);
        //}
        //public static string ToBase64String(this byte[] b)
        //{
        //    return Convert.ToBase64String(b);
        //}
        //public static byte[] GetBytesFromBase64String(this string s)
        //{
        //    return Convert.FromBase64String(s);
        //}


        //public static bool ToValue(this String s, Type type, out object value)
        //{
        //    if (s != null)
        //    {
        //        if (type == typeof(string))
        //        {
        //            value = s;
        //            return true;
        //        }
        //        else if ((type == typeof(Boolean)) || (type == typeof(Boolean?)))
        //        {
        //            Boolean n; if (s.ToBoolean(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(SByte)) || (type == typeof(SByte?)))
        //        {
        //            SByte n; if (s.ToSByte(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Byte)) || (type == typeof(Byte?)))
        //        {
        //            Byte n; if (s.ToByte(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Int16)) || (type == typeof(Int16?)))
        //        {
        //            Int16 n; if (s.ToInt16(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(UInt16)) || (type == typeof(UInt16?)))
        //        {
        //            UInt16 n; if (s.ToUInt16(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Int32)) || (type == typeof(Int32?)))
        //        {
        //            Int32 n; if (s.ToInt32(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(UInt32)) || (type == typeof(UInt32?)))
        //        {
        //            UInt32 n; if (s.ToUInt32(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Int64)) || (type == typeof(Int64?)))
        //        {
        //            Int64 n; if (s.ToInt64(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(UInt64)) || (type == typeof(UInt64?)))
        //        {
        //            UInt64 n; if (s.ToUInt64(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Single)) || (type == typeof(Single?)))
        //        {
        //            Single n; if (s.ToSingle(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Double)) || (type == typeof(Double?)))
        //        {
        //            Double n; if (s.ToDouble(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Decimal)) || (type == typeof(Decimal?)))
        //        {
        //            Decimal n; if (s.ToDecimal(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(DateTime)) || (type == typeof(DateTime?)))
        //        {
        //            DateTime n; if (s.ToDateTime(out n)) { value = n; return true; }
        //        }
        //        else if (type == typeof(IPAddress))
        //        {
        //            IPAddress n; if (s.ToIPAddress(out n)) { value = n; return true; }
        //        }
        //    }
        //    value = null;
        //    return false;
        //}

        //public static string ToSqlCommandText<T>(this Nullable<T> value) where T : struct
        //{
        //    if (value.HasValue)
        //        return value.ToString();
        //    else
        //        return "null";
        //}

        delegate bool TryParseHandler<T>(string value, out T result) where T : struct;
        static bool _null<T>(string value, out T result) where T : struct { result = default(T); return false; }
        static bool TryParse<T>(this String src, TryParseHandler<T> handler, out T value) where T : struct
        {
            try
            {
                if (!string.IsNullOrEmpty(src))
                    return handler(src, out value);
            }
            catch { }
            return _null<T>(src, out value);
        }
        static T? TryParse<T>(this String src, TryParseHandler<T> handler) where T : struct
        {
            try
            {
                T value;
                if (!string.IsNullOrEmpty(src))
                    if (handler(src, out value))
                        return value;
            }
            catch { }
            return null;
        }

        public static Boolean? ToBoolean(this String s) /*********************/ => s.TryParse<Boolean>(Boolean.TryParse);
        public static bool ToBoolean(this String s, out Boolean result) /*****/ => s.TryParse<Boolean>(Boolean.TryParse, out result);
        public static Byte? ToByte(this String s) /***************************/ => s.TryParse<Byte>(Byte.TryParse);
        public static bool ToByte(this String s, out Byte result) /***********/ => s.TryParse<Byte>(Byte.TryParse, out result);
        public static SByte? ToSByte(this String s) /*************************/ => s.TryParse<SByte>(SByte.TryParse);
        public static bool ToSByte(this String s, out SByte result) /*********/ => s.TryParse<SByte>(SByte.TryParse, out result);
        public static Int16? ToInt16(this String s) /*************************/ => s.TryParse<Int16>(Int16.TryParse);
        public static bool ToInt16(this String s, out Int16 result) /*********/ => s.TryParse<Int16>(Int16.TryParse, out result);
        public static UInt16? ToUInt16(this String s) /***********************/ => s.TryParse<UInt16>(UInt16.TryParse);
        public static bool ToUInt16(this String s, out UInt16 result) /*******/ => s.TryParse<UInt16>(UInt16.TryParse, out result);
        public static Int32? ToInt32(this String s) /*************************/ => s.TryParse<Int32>(Int32.TryParse);
        public static bool ToInt32(this String s, out Int32 result) /*********/ => s.TryParse<Int32>(Int32.TryParse, out result);
        public static UInt32? ToUInt32(this String s) /***********************/ => s.TryParse<UInt32>(UInt32.TryParse);
        public static bool ToUInt32(this String s, out UInt32 result) /*******/ => s.TryParse<UInt32>(UInt32.TryParse, out result);
        public static Int64? ToInt64(this String s) /*************************/ => s.TryParse<Int64>(Int64.TryParse);
        public static bool ToInt64(this String s, out Int64 result) /*********/ => s.TryParse<Int64>(Int64.TryParse, out result);
        public static UInt64? ToUInt64(this String s) /***********************/ => s.TryParse<UInt64>(UInt64.TryParse);
        public static bool ToUInt64(this String s, out UInt64 result) /*******/ => s.TryParse<UInt64>(UInt64.TryParse, out result);
        public static Single? ToSingle(this String s) /***********************/ => s.TryParse<Single>(Single.TryParse);
        public static bool ToSingle(this String s, out Single result) /*******/ => s.TryParse<Single>(Single.TryParse, out result);
        public static Double? ToDouble(this String s) /***********************/ => s.TryParse<Double>(Double.TryParse);
        public static bool ToDouble(this String s, out Double result) /*******/ => s.TryParse<Double>(Double.TryParse, out result);
        public static Decimal? ToDecimal(this String s) /*********************/ => s.TryParse<Decimal>(Decimal.TryParse);
        public static bool ToDecimal(this String s, out Decimal result) /*****/ => s.TryParse<Decimal>(Decimal.TryParse, out result);
        public static DateTime? ToDateTime(this String s) /*******************/ => s.TryParse<DateTime>(DateTime.TryParse);
        public static bool ToDateTime(this String s, out DateTime result) /***/ => s.TryParse<DateTime>(DateTime.TryParse, out result);

        public static List<int> ToInt32(this IList<string> s)
        {
            List<int> ret = null;
            for (int i = 0; i < s.Count; i++)
            {
                int n;
                if (s[i].ToInt32(out n))
                {
                    if (ret == null)
                        ret = new List<int>();
                    ret.Add(n);
                }
            }
            return ret;
        }

        public static List<T> ToEnum<T>(this IList<string> s) where T : struct
        {
            if (s == null) return null;
            List<T> ret = null;
            for (int i = 0; i < s.Count; i++)
            {
                T n;
                if (s[i].ToEnum(out n))
                {
                    if (ret == null)
                        ret = new List<T>();
                    ret.Add(n);
                }
            }
            return ret;
        }

        public static Guid? ToGuid(this String s) /***************************/ { try { return new Guid(s); } catch { return null; } }
        public static bool ToGuid(this String s, out Guid result) /***********/ { try { result = new Guid(s); return true; } catch { result = default(Guid); return false; } }

        public static Int32? HexToInt32(this string s)
        {
            try { return Convert.ToInt32(s, 16); }
            catch { return null; }
        }
        public static Int64? HexToInt64(this string s)
        {
            try { return Convert.ToInt64(s, 16); }
            catch { return null; }
        }

        public static bool ToEnum(this String s, Type type, bool ignoreCase, out object result)
        {
            if ((s != null) && type.IsEnum)
            {
                try
                {
                    result = Enum.Parse(type, s.Trim(), ignoreCase);
                    return Enum.IsDefined(type, result);
                }
                catch { }
            }
            result = null;
            return false;
        }
        public static bool ToEnum(this String s, Type type, out object result) /**********/ { return ToEnum(s, type, true, out result); }
        public static object ToEnum(this String s, Type type, bool ignoreCase) /**********/ { object result; if (ToEnum(s, type, ignoreCase, out result)) return result; return null; }
        public static object ToEnum(this String s, Type type) /***************************/ { object result; if (ToEnum(s, type, true, out result)) return result; return null; }


        public static bool ToEnum<T>(this String s, bool ignoreCase, out T result) where T : struct
        {
            if ((s != null) && typeof(T).IsEnum)
            {
                try
                {
                    result = (T)Enum.Parse(typeof(T), s.Trim(), ignoreCase);
                    return Enum.IsDefined(typeof(T), result);
                }
                catch { }
            }
            result = default(T);
            return false;
        }
        public static bool ToEnum<T>(this String s, out T result) /***********/ where T : struct { return ToEnum<T>(s, true, out result); }
        public static T? ToEnum<T>(this String s, bool ignoreCase) /**********/ where T : struct { T result; if (ToEnum<T>(s, ignoreCase, out result)) return result; return null; }
        public static T? ToEnum<T>(this String s) /***************************/ where T : struct { T result; if (ToEnum<T>(s, true, out result)) return result; return null; }

#if netfx
        //public static IPAddress ToIPAddress(this String s) /******************/ { IPAddress n; if (!string.IsNullOrEmpty(s) && IPAddress.TryParse(s, out n)) return n; return null; }
        //public static bool ToIPAddress(this String s, out IPAddress result)
        //{
        //    if (!string.IsNullOrEmpty(s) && IPAddress.TryParse(s, out result))
        //        return true;
        //    result = default(IPAddress);
        //    return false;
        //}

        //public static IPEndPoint ToIPEndPoint(this String s) { IPEndPoint ip; if (s.ToIPEndPoint(out ip)) return ip; else return null; }
        //public static bool ToIPEndPoint(this String s, out IPEndPoint result)
        //{
        //    if (!string.IsNullOrEmpty(s))
        //    {
        //        string[] ss = s.Split(':');
        //        if (ss.Length >= 2)
        //        {
        //            IPAddress ip;
        //            int port;
        //            if (IPAddress.TryParse(ss[0], out ip) && int.TryParse(ss[1], out port))
        //            {
        //                result = new IPEndPoint(ip, port);
        //                return true;
        //            }
        //        }
        //    }
        //    result = null;
        //    return false;
        //}
#endif
    }
}