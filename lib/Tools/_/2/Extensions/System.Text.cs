using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
    partial class StringExtensions
    {
        public static string ReadTo(this string str, ref int pos, char c)
        {
            int start, n;
            for (start = pos, n = str.Length; pos < n; pos++)
                if (str[pos] == c)
                    return str.Substring(start, pos++ - start);
            return null;
        }
        public static string ReadLine(this string str, ref int pos)
        {
            int start = pos;
            int n = str.Length - 1;
            char c;
            for (; pos < n; pos++)
            {
                c = str[pos];
                if ((pos == start) && (c == ' '))
                    start++;
                if ((c == '\r') && (str[pos + 1] == '\n'))
                {
                    string s = str.Substring(start, pos++ - start);
                    pos++;
                    return s;
                }
            }
            return null;
        }

        static char GetChar(this string s, int n)
        {
            if (n < s.Length)
                return s[n];
            else
                return default(char);
        }

        public static string ToBase64String(this Encoding encoding, string s)
        {
            if (s == null) return null;
            try { return Convert.ToBase64String(encoding.GetBytes(s)); }
            catch { return null; }
        }
        public static string FromBase64String(this Encoding encoding, string s)
        {
            if (s == null) return null;
            for (;;)
            {
                try
                {
                    return encoding.GetString(Convert.FromBase64String(s));
                }
                catch
                {
                    if (s.EndsWith("=="))
                        return null;
                    s += "=";
                }
            }
        }

        //public struct sql_str
        //{
        //    public sql_str(string _string) { this.value = _string; }
        //    public string value;
        //    public override string ToString() { return value; }

        //    public static explicit operator string(sql_str value)
        //    {
        //        return value.value;
        //    }
        //    public static implicit operator sql_str(string value)
        //    {
        //        return new sql_str(value);
        //    }
        //    public override bool Equals(object obj)
        //    {
        //        return base.Equals(obj);
        //    }
        //    public override int GetHashCode()
        //    {
        //        return base.GetHashCode();
        //    }

        //    public static sql_str Null = (sql_str)"null";
        //    public static sql_str getdate = (sql_str)"getdate()";
        //}
    }


    //[_DebuggerStepThrough]
    //class StringExportContractGroup : Dictionary<string, StringExportContract>
    //{
    //    public readonly StringExportContract _default;
    //    public StringExportContractGroup(Type t)
    //    {

    //        foreach (MemberInfo m in t.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    //        {
    //            foreach (StringExportAttribute a in m.GetCustomAttributes(typeof(StringExportAttribute), true))
    //            {
    //                a.p = m as PropertyInfo;
    //                a.f = m as FieldInfo;
    //                if ((a.p != null) || (a.f != null))
    //                {
    //                    a.m = m;
    //                    string name = a.Name ?? m.Name;
    //                    StringExportContract c;
    //                    if (a.ID == null)
    //                        c = this._default;
    //                    else if (!this.ContainsKey(a.ID))
    //                        c = this[a.ID] = new StringExportContract();
    //                    else
    //                        c = this[a.ID];
    //                    if (!c.ContainsKey(name))
    //                        c[name] = new List<StringExportAttribute>();
    //                    c[name].Add(a);
    //                }
    //            }
    //        }
    //    }
    //}


}
namespace System.Text
{
    public static class Extensions
    {
        public static string ReadTo(this StringBuilder sb, ref int pos, char c)
        {
            int start, n;
            for (start = pos, n = sb.Length; pos < n; pos++)
                if (sb[pos] == c)
                    return sb.ToString(start, pos++ - start);
            return null;
        }
        public static string ReadLine(this StringBuilder sb, ref int pos)
        {
            int start = pos;
            int n = sb.Length - 1;
            char c;
            for (; pos < n; pos++)
            {
                c = sb[pos];
                if ((pos == start) && (c == ' '))
                    start++;
                if ((c == '\r') && (sb[pos + 1] == '\n'))
                {
                    string s = sb.ToString(start, pos++ - start);
                    pos++;
                    return s;
                }
            }
            return null;
        }
    }
}
