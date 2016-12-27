using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Diagnostics;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
    public static partial class StringExtensions
    {
        public static string format(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }
        public static string format(this string format, object arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }
        public static string format(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(format, arg0, arg1, arg2);
        }

        static bool TryGetValue(object obj, Type objType, string name, out object value)
        {
            try
            {
                if (name == "")
                {
                    value = obj;
                    return value != null;
                }
                string l, r;
                name.Split('.', out l, out r);
                IDictionary<string, object> dict = obj as IDictionary<string, object>;
                if ((dict != null) && (dict.TryGetValue(l, out value)))
                {
                    //if (r == "")
                    //    return true;
                    //if (value == null)
                    //    return false;
                    return TryGetValue(value, value.GetType(), r, out value);
                }

                TypeContract t = TypeContract.GetContract(objType);
                FieldInfo f;
                if (t.GetMember(l, out f))
                {
                    value = f.GetValue(f.IsStatic ? null : obj);
                    return TryGetValue(value, f.FieldType, r, out value);
                }
                PropertyInfo p;
                if (t.GetMember(l, out p))
                {
                    value = p.GetValue(p.IsStatic() ? null : obj, null);
                    return TryGetValue(value, p.PropertyType, r, out value);
                }
            }
            catch { }
            value = null;
            return false;
        }

        //const string _nvarchar = "{0:" + System.Data.SqlClient.SqlCmd.nvarchar + "}";

        [DebuggerStepThrough]
        public static string formatWith(this string format, object obj, bool sql = false) { return StringExtensions.FormatWith(format, obj, sql); }
        public static string FormatWith(this string format, object obj, bool sql = false)
        {
            if (string.IsNullOrEmpty(format)) return format;
            if (obj == null) return format;
            Type objType = obj.GetType();
            StringBuilder str = new StringBuilder();
            char c;
            for (int pos = 0, len = format.Length; -1 < len;)
            {
                if (pos >= len) break;
                c = format[pos++];
                if (c == '{')
                {
                    if (pos < len && format[pos] == '{')
                        pos++;
                    else
                    {
                        for (int bracket = pos, colon = -1; pos < len;)
                        {
                            if (pos >= len) break;
                            c = format[pos++];
                            if (c == '}')
                            {
                                string name, fmt;
                                if (colon == -1)
                                {
                                    name = format.Substring(bracket, pos - bracket - 1);
                                    fmt = null;
                                }
                                else
                                {
                                    name = format.Substring(bracket, colon - bracket - 1);
                                    fmt = format.Substring(colon, pos - colon - 1);
                                }
                                object value;
                                if (TryGetValue(obj, objType, name, out value))
                                {
                                    if (sql)
                                        str.AppendAsSqlString(fmt, value);
                                    else if (fmt == null)
                                        str.Append(value);
                                    else
                                        str.AppendFormat("{0:" + fmt + "}", value);
                                }
                                else
                                {
                                    if (name.ToInt32().HasValue)
                                    {
                                        str.Append('{');
                                        str.Append(name);
                                        str.Append('}');
                                    }
                                    else if (sql)
                                    {
                                        str.Append('@');
                                        str.Append(name);
                                    }
                                }
                                break;
                            }
                            else if (c == ':')
                            {
                                if (colon == -1)
                                    colon = pos;
                            }
                        }
                        continue;
                    }
                }
                else if (c == '}')
                {
                    if (pos < len && format[pos] == '}')
                        pos++;
                }
                str.Append(c);
            }
            return str.ToString();
        }

        static StringBuilder AppendAsSqlString(this StringBuilder str, string fmt, object value)
        {
            bool quote = false;
            if (value == null)
                value = "null";
            else if ((value is IList) && (fmt == System.Data.SqlClient.SqlCmd.array))
            {
                IList list = (IList)value;
                if (list.Count > 0)
                {
                    str.Append('(');
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i > 0) str.Append(',');
                        str.AppendAsSqlString(null, list[i]);
                    }
                    str.Append(')');
                }
                return str;
            }
            else if ((value is string) ||
                (fmt == System.Data.SqlClient.SqlCmd.varchar) ||
                (fmt == System.Data.SqlClient.SqlCmd.nvarchar))
            {
                quote = true;
                string _value;
                if (value is string)
                    _value = value as string;
                else
                    _value = value.ToString();
                value = System.Data.SQLinjection.magic_quote(_value);
                if (fmt == System.Data.SqlClient.SqlCmd.nvarchar) str.Append('N');
            }
            else if (value is Guid)
                quote = true;
            else if (value is DateTime)
                quote = true;
            else if (value is bool)
            {
                quote = false;
                value = ((bool)value) ? 1 : 0;
            }
            else
            {
                Type t = value.GetType();
                if (t.GetTypeInfo().IsEnum)
                    value = Convert.ChangeType(value, Enum.GetUnderlyingType(t));
            }
            if (quote) str.Append('\'');
            if (fmt == null)
                str.Append(value);
            else
                str.AppendFormat("{0:" + fmt + "}", value);
            if (quote) str.Append('\'');
            return str;
        }

        public static string ToSqlString(this IList list)
        {
            StringBuilder s = new StringBuilder();
            StringExtensions.ToSqlString(list, s);
            return s.ToString();
        }
        public static StringBuilder ToSqlString(this IList list, StringBuilder str)
        {
            if (list == null) return str;
            if (list.Count == 0) return str;
            return str.AppendAsSqlString(System.Data.SqlClient.SqlCmd.array, list);
        }
        public static StringBuilder ToString(this IList list, Func<StringBuilder> str)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            return list.ToSqlString(str?.Invoke());
        }

        //static string SqlExport(this string str, object obj)
        //{
        //    return StringExtensions.StringExport(str, null, obj, true);
        //}
        //static string SqlExport(this string str, string id, object obj)
        //{
        //    return StringExtensions.StringExport(str, id, obj, true);
        //}
        //static void SqlExport(this StringBuilder dst, string str, string id, object obj)
        //{
        //    StringExport(str, dst, id, obj, false);
        //}

        //static string StringExport(this string str, object obj)
        //{
        //    return StringExtensions.StringExport(str, null, obj, false);
        //}
        //static string StringExport(this string str, string id, object obj)
        //{
        //    return StringExtensions.StringExport(str, id, obj, false);
        //}

        //static string StringExport(string str, string id, object obj, bool sql)
        //{
        //    StringBuilder dst = new StringBuilder();
        //    StringExport(str, dst, id, obj, sql);
        //    return dst.ToString();
        //}
        //static void StringExport(string str, StringBuilder dst, string id, object obj, bool sql)
        //{
        //    Dictionary<string, object> obj_dict = obj as Dictionary<string, object>;
        //    StringExportContract contract = StringExportContract.GetContract(obj, id);
        //    int? n1 = null, n2 = null;
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        char c = str[i];
        //        if (n1.HasValue)
        //        {
        //            if ((c == ':') && !n2.HasValue)
        //                n2 = i;
        //            else if (c == '}')
        //            {
        //                string field;
        //                string fmt1;
        //                string fmt2;
        //                if (n2.HasValue)
        //                {
        //                    field = str.Substring(n1.Value, n2.Value - n1.Value);
        //                    fmt1 = str.Substring(n2.Value + 1, i - n2.Value - 1);
        //                    fmt2 = string.Format("{{0:{0}}}", fmt1);
        //                }
        //                else
        //                {
        //                    field = str.Substring(n1.Value, i - n1.Value);
        //                    fmt1 = null;
        //                    fmt2 = "{0}";
        //                }
        //                int nnn;
        //                bool hasValue;
        //                object value;
        //                if (obj_dict == null)
        //                {
        //                    hasValue = false;
        //                    value = null;
        //                    if (contract.ContainsKey(field))
        //                    {
        //                        PropertyInfo p = contract[field].p;
        //                        FieldInfo f = contract[field].f;
        //                        if ((p != null) && (p.CanRead))
        //                        {
        //                            value = p.GetValue(p.IsStatic() ? null : obj, null);
        //                            hasValue = true;
        //                        }
        //                        else if (f != null)
        //                        {
        //                            value = f.GetValue(f.IsStatic ? null : obj);
        //                            hasValue = true;
        //                        }
        //                    }
        //                }
        //                else if (hasValue = obj_dict.ContainsKey(field))
        //                    value = obj_dict[field];
        //                else
        //                    value = null;

        //                if (hasValue)
        //                {
        //                    if (sql)
        //                    {
        //                        bool quote = (value is string) || (value is Guid) || (value is DateTime);
        //                        if (value == null)
        //                            value = "null";
        //                        else
        //                        {
        //                            Type t = value.GetType();
        //                            if (t.IsEnum)
        //                                value = Convert.ChangeType(value, Enum.GetUnderlyingType(t));
        //                        }
        //                        if (quote) dst.Append('\'');
        //                        dst.AppendFormat(fmt2, value);
        //                        if (quote) dst.Append('\'');
        //                    }
        //                    else
        //                        dst.AppendFormat(fmt2, value);
        //                }
        //                else if (sql && !int.TryParse(field, out nnn))
        //                {
        //                    dst.Append('@');
        //                    dst.Append(field);
        //                }
        //                else
        //                {
        //                    dst.Append('{');
        //                    dst.AppendFormat(fmt2, field);
        //                    dst.Append('}');
        //                }
        //                n1 = n2 = null;
        //            }
        //        }
        //        else if (c == '{')
        //        {
        //            n1 = i + 1;
        //        }
        //        else
        //            dst.Append(c);
        //    }
        //}


        public static string Trim(this string s, bool nullOnEmpty = false)
        {
            if (s != null)
            {
                s = s.Trim();
                if ((s == string.Empty) && nullOnEmpty)
                    s = null;
            }
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
    }

    //[_DebuggerStepThrough]
    //class StringExportContract : Dictionary<string, StringExportAttribute>
    //{
    //    static readonly StringExportContract _null = new StringExportContract();
    //    static readonly Dictionary<Type, Group> _all = new Dictionary<Type, Group>();

    //    class Group : Dictionary<string, StringExportContract>
    //    {
    //        public StringExportContract _default = new StringExportContract();
    //        public Group(Type t)
    //        {
    //            List<MemberInfo> mm = new List<MemberInfo>();
    //            List<StringExportAttribute> aa = new List<StringExportAttribute>();
    //            for (Type tt = t; tt != null; tt = tt.BaseType)
    //            {
    //                foreach (MemberInfo m in tt.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
    //                {
    //                    PropertyInfo p = m as PropertyInfo;
    //                    FieldInfo f = m as FieldInfo;
    //                    if ((p == null) && (f == null)) continue;
    //                    mm.Add(m);
    //                    foreach (StringExportAttribute a in m.GetCustomAttributes(typeof(StringExportAttribute), true))
    //                    {
    //                        a.m = m;
    //                        a.p = p;
    //                        a.f = f;
    //                        a.Name = a.Name ?? m.Name;
    //                        aa.Add(a);
    //                    }
    //                }
    //            }
    //            bool empty = aa.Count == 0;
    //            if (empty)
    //            {
    //                foreach (MemberInfo m in mm)
    //                    aa.Add(new StringExportAttribute() { m = m, p = m as PropertyInfo, f = m as FieldInfo, Name = m.Name });
    //            }
    //            foreach (StringExportAttribute a in aa)
    //            {
    //                StringExportContract c;
    //                if (a.ID == null)
    //                    c = _default;
    //                else if (!this.TryGetValue(a.ID, out c))
    //                    c = this[a.ID] = new StringExportContract();
    //                if (!c.ContainsKey(a.Name))
    //                    c[a.Name] = a;
    //            }
    //        }
    //    }
    //    public static StringExportContract GetContract(object obj, string id)
    //    {
    //        if (obj == null)
    //            return _null;
    //        if (obj is Dictionary<string, object>)
    //            return _null;
    //        Type t = obj.GetType();

    //        Group g;
    //        lock (_all)
    //        {
    //            if (!_all.TryGetValue(t, out g))
    //                g = _all[t] = new Group(t);
    //        }
    //        if (id == null)
    //            return g._default;
    //        else if (g.ContainsKey(id))
    //            return g[id];
    //        else
    //            return _null;
    //    }
    //}

    //[_DebuggerStepThrough]
    //[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    //public class StringExportAttribute : Attribute
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public StringExportAttribute() { }
    //    public StringExportAttribute(string name) { this.Name = name; }
    //    public StringExportAttribute(string name, string id) : this(name) { this.ID = id; }

    //    internal MemberInfo m;
    //    internal PropertyInfo p;
    //    internal FieldInfo f;

    //}


    [_DebuggerStepThrough]
    public static partial class StringExtensions
    {
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

        public static bool In<T>(this T? n, params T[] args) where T : struct
        {
            if (n.HasValue)
                return args.Contains(n.Value);
            return false;
        }
        public static bool In<T>(this T n, params T[] args) where T : struct
        {
            return args.Contains(n);
        }

        public static bool Contains<T>(this T[] src, T? value) where T : struct
        {
            if (value.HasValue)
                return src.Contains(value.Value);
            return false;
        }
        public static bool Contains<T>(this T[] src, T value)
        {
            if (src != null)
                for (int i = 0, n = src.Length; i < n; i++)
                    if (EqualityComparer<T>.Default.Equals(src[i], value))
                        return true;
            return false;
        }
        public static bool Contains(this Array src, object value)
        {
            if (src != null)
                foreach (object s in src)
                    if (s == value)
                        return true;
            return false;
        }

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

        public static Boolean? ToBoolean(this String s) /*********************/ { return s.TryParse<Boolean>(Boolean.TryParse); }
        public static bool ToBoolean(this String s, out Boolean result) /*****/ { return s.TryParse<Boolean>(Boolean.TryParse, out result); }
        public static Byte? ToByte(this String s) /***************************/ { return s.TryParse<Byte>(Byte.TryParse); }
        public static bool ToByte(this String s, out Byte result) /***********/ { return s.TryParse<Byte>(Byte.TryParse, out result); }
        public static SByte? ToSByte(this String s) /*************************/ { return s.TryParse<SByte>(SByte.TryParse); }
        public static bool ToSByte(this String s, out SByte result) /*********/ { return s.TryParse<SByte>(SByte.TryParse, out result); }
        public static Int16? ToInt16(this String s) /*************************/ { return s.TryParse<Int16>(Int16.TryParse); }
        public static bool ToInt16(this String s, out Int16 result) /*********/ { return s.TryParse<Int16>(Int16.TryParse, out result); }
        public static UInt16? ToUInt16(this String s) /***********************/ { return s.TryParse<UInt16>(UInt16.TryParse); }
        public static bool ToUInt16(this String s, out UInt16 result) /*******/ { return s.TryParse<UInt16>(UInt16.TryParse, out result); }
        public static Int32? ToInt32(this String s) /*************************/ { return s.TryParse<Int32>(Int32.TryParse); }
        public static bool ToInt32(this String s, out Int32 result) /*********/ { return s.TryParse<Int32>(Int32.TryParse, out result); }
        public static UInt32? ToUInt32(this String s) /***********************/ { return s.TryParse<UInt32>(UInt32.TryParse); }
        public static bool ToUInt32(this String s, out UInt32 result) /*******/ { return s.TryParse<UInt32>(UInt32.TryParse, out result); }
        public static Int64? ToInt64(this String s) /*************************/ { return s.TryParse<Int64>(Int64.TryParse); }
        public static bool ToInt64(this String s, out Int64 result) /*********/ { return s.TryParse<Int64>(Int64.TryParse, out result); }
        public static UInt64? ToUInt64(this String s) /***********************/ { return s.TryParse<UInt64>(UInt64.TryParse); }
        public static bool ToUInt64(this String s, out UInt64 result) /*******/ { return s.TryParse<UInt64>(UInt64.TryParse, out result); }
        public static Single? ToSingle(this String s) /***********************/ { return s.TryParse<Single>(Single.TryParse); }
        public static bool ToSingle(this String s, out Single result) /*******/ { return s.TryParse<Single>(Single.TryParse, out result); }
        public static Double? ToDouble(this String s) /***********************/ { return s.TryParse<Double>(Double.TryParse); }
        public static bool ToDouble(this String s, out Double result) /*******/ { return s.TryParse<Double>(Double.TryParse, out result); }
        public static Decimal? ToDecimal(this String s) /*********************/ { return s.TryParse<Decimal>(Decimal.TryParse); }
        public static bool ToDecimal(this String s, out Decimal result) /*****/ { return s.TryParse<Decimal>(Decimal.TryParse, out result); }
        public static DateTime? ToDateTime(this String s) /*******************/ { return s.TryParse<DateTime>(DateTime.TryParse); }
        public static bool ToDateTime(this String s, out DateTime result) /***/ { return s.TryParse<DateTime>(DateTime.TryParse, out result); }

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
            if ((s != null) && type.GetTypeInfo().IsEnum)
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
            if ((s != null) && typeof(T).GetTypeInfo().IsEnum)
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

#if NET40
        public static IPAddress ToIPAddress(this String s) /******************/ { IPAddress n; if (!string.IsNullOrEmpty(s) && IPAddress.TryParse(s, out n)) return n; return null; }
        public static bool ToIPAddress(this String s, out IPAddress result)
        {
            if (!string.IsNullOrEmpty(s) && IPAddress.TryParse(s, out result))
                return true;
            result = default(IPAddress);
            return false;
        }

        public static IPEndPoint ToIPEndPoint(this String s) { IPEndPoint ip; if (s.ToIPEndPoint(out ip)) return ip; else return null; }
        public static bool ToIPEndPoint(this String s, out IPEndPoint result)
        {
            if (!string.IsNullOrEmpty(s))
            {
                string[] ss = s.Split(':');
                if (ss.Length >= 2)
                {
                    IPAddress ip;
                    int port;
                    if (IPAddress.TryParse(ss[0], out ip) && int.TryParse(ss[1], out port))
                    {
                        result = new IPEndPoint(ip, port);
                        return true;
                    }
                }
            }
            result = null;
            return false;
        }
#endif
    }

    public static partial class StringExtensions
    {
        public static byte? Max(this byte? val1, byte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static decimal? Max(this decimal? val1, decimal? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static double? Max(this double? val1, double? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static float? Max(this float? val1, float? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static int? Max(this int? val1, int? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static long? Max(this long? val1, long? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static sbyte? Max(this sbyte? val1, sbyte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static short? Max(this short? val1, short? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static uint? Max(this uint? val1, uint? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static ulong? Max(this ulong? val1, ulong? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static ushort? Max(this ushort? val1, ushort? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static byte? Min(this byte? val1, byte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static decimal? Min(this decimal? val1, decimal? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static double? Min(this double? val1, double? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static float? Min(this float? val1, float? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static int? Min(this int? val1, int? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static long? Min(this long? val1, long? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static sbyte? Min(this sbyte? val1, sbyte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static short? Min(this short? val1, short? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static uint? Min(this uint? val1, uint? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static ulong? Min(this ulong? val1, ulong? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static ushort? Min(this ushort? val1, ushort? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
    }
}