using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Data
{
    [_DebuggerStepThrough]
    public class SqlBuilderX : DynamicObject, IDictionary<string, object>
    {
        public static string DateFormat = "yyyy-MM-dd";
        public static string TimeFormat = "HH:mm:ss";
        public static string DateTimeFormat = DateFormat + " " + TimeFormat;
        public static string DateTimeFormatX = DateTimeFormat + ".fff";
        //public const string CreateTime = "CreateTime";
        //public const string ModifyTime = "ModifyTime";
        //public const string CreateUser = "CreateUser";
        //public const string ModifyUser = "ModifyUser";

        public struct str
        {
            public string value;
            public str(string value) { this.value = value.Trim(true); }
            public override string ToString() { return this.value; }

            public static explicit operator string(str value) { return value.value; }
            public static implicit operator str(string value) { return new str(value); }

            //public static bool operator ==(str src, object obj)
            //{
            //    return src.Equals(obj);
            //}
            //public static bool operator !=(str src, object obj)
            //{
            //    return !src.Equals(obj);
            //}
            //public override bool Equals(object obj)
            //{
            //    if (obj is str)
            //        return string.Compare(this.value, ((str)obj).value, true) == 0;
            //    else if (obj is string)
            //        return string.Compare(this.value, (string)obj, true) == 0;
            //    else
            //        return this.value == (obj as string);
            //}
            //public override int GetHashCode()
            //{
            //    return base.GetHashCode();
            //}

            public static str Null = new str("null");
            public static str getdate = new str("getdate()");
            public static str newid = new str("newid()");
            public static object NullValue(object value)
            {
                if (value == null)
                    return Null;
                return value;
            }

            public str GetValueOrDefault(string _default)
            {
                if (string.IsNullOrEmpty(this.value))
                    return new str(_default);
                else
                    return this;
            }
        }
        public struct err : _op
        {
            public string name;
            public int value;
            public string msg;
            public err(int value, string name = "Error", string msg_str = null)
            {
                this.name = name;
                this.value = value;
                this.msg = msg_str;
            }

            void _op.Build(SqlBuilderX src, StringBuilder s)
            {
                s.AppendFormat(" begin select {0} as {1} return end", this.value, this.name);
            }
        }

        //readonly Dictionary<string, object> values = new Dictionary<string, object>();

        readonly List<item> items = new List<item>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag">*wudtN</param>
        /// <param name="field"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public object this[ /*                 */ string flag, string field, string format = null, string exp = null]
        {
            [DebuggerStepThrough]
            set { this.SetValue(null, flag, field, format, exp, value); }
        }
        public object this[SqlSchemaTable schema, string flag, string field, string format = null, string exp = null]
        {
            [DebuggerStepThrough]
            set { this.SetValue(schema, flag, field, format, exp, value); }
        }
        //public object this[ /*                 */ string flag, string field, string format, object oldValue, string fill = null]
        //{
        //    [DebuggerStepThrough]
        //    set { if (!object.Equals(value, oldValue)) this.SetValue(null, flag, field, format, value); }
        //}
        //public object this[SqlSchemaTable schema, string flag, string field, string format, object oldValue, string fill = null]
        //{
        //    [DebuggerStepThrough]
        //    set { if (!object.Equals(value, oldValue)) this.SetValue(schema, flag, field, format, value); }
        //}

        public string[] GetMissingFields(Func<string[], Exception> throwException = null)
        {
            List<string> result = null;
            for (int i = 0, n = items.Count; i < n; i++)
            {
                item item = items[i];
                if (item.require && item.value == null)
                    _null._new(ref result).Add(item.field);
            }
            if (result == null) return null;
            string[] f = result.ToArray();
            if (throwException != null)
            {
                Exception e = throwException(f);
                if (e != null)
                    throw e;
            }
            return f;
        }

        bool get_item(string field, out item result, out int index)
        {
            int count = items.Count;
            for (index = 0, count = items.Count; index < count; index++)
                if ((result = items[index]).field == field)
                    return true;
            result = default(item);
            index = -1;
            return false;
        }
        item set_item(item item)
        {
            item tmp;
            int index;
            if (this.get_item(item.field, out tmp, out index))
                items[index] = item;
            else
                items.Add(item);
            return item;
        }

        public object this[string field]
        {
            get
            {
                item n; int i;
                if (this.get_item(field, out n, out i))
                    return n.value;
                return null;
            }
            set
            {
                field = field.Trim(true);
                if (field == null) return;
                item item;
                int index;
                if (this.get_item(field, out item, out index))
                    item.value = value;
                else
                    item = new item() { is_field = false, field = field, value = value };
                this.set_item(item);
            }
        }

        [DebuggerDisplay("flag:{flag}, field:{field}, format:{format}")]
        protected struct item
        {
            public bool is_field;
            public bool is_where_key;
            public bool is_update_value;
            string _flag;
            public string flag
            {
                get { return _flag; }
                set
                {
                    _flag = value;
                    this.is_where_key = Enumerable.Contains(value, 'w');
                    this.is_update_value = Enumerable.Contains(value, 'u');
                    this.require = Enumerable.Contains(value, '*') || this.is_where_key;
                    this.dateformat = Enumerable.Contains(value, 'd');
                    this.timeformat = Enumerable.Contains(value, 't');
                    this.nvarchar = Enumerable.Contains(value, 'N');
                    this.varchar = Enumerable.Contains(value, 'n');
                }
            }
            public string field;
            public string format;
            object _value;
            public object value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    if (this.value is DateTime)
                    {
                        if (this.format != null) { }
                        else if (this.dateformat) this.format = DateFormat;
                        else if (this.timeformat) this.format = TimeFormat;
                        else this.format = DateTimeFormat;
                    }
                }
            }

            public bool require;

            public string exp;

            /// <summary> apply format : "yyyy-MM-dd" </summary>
            public bool dateformat;
            /// <summary> apply format : "HH:mm:ss" </summary>
            public bool timeformat;
            /// <summary> sql nvarchar (force value as string)</summary>
            public bool nvarchar;
            /// <summary> sql varchar (force value as string)</summary>
            public bool varchar;
        }

        protected item SetValue(SqlSchemaTable schema, string flag, string field, string format, string exp, object value)
        {
            field = field.Trim(true);
            if (schema != null)
                field = schema.GetFieldName(field);
            if (field == null) return default(item);
            return this.set_item(new item()
            {
                is_field = true,
                flag = flag.Trim(true) ?? string.Empty,
                field = field,
                exp = exp,
                format = format.Trim(true),
                value = value,
            });
        }

        public interface _op { void Build(SqlBuilderX src, StringBuilder s); }
        public sealed partial class op : _op
        {
            op() { }
            bool _field = true;
            string _field_l = "[";
            string _field_r = "]";
            bool _value = true;
            string _delimiter1 = null;
            string _delimiter2 = ", ";

            /// <summary> [Field1],[Field2],[Field3] </summary>
            public static readonly op Fields = new op() { _value = false };
            /// <summary> {Field1},{Field2},{Field3} </summary>
            public static readonly op Values = new op() { _field = false };
            /// <summary> @Field1={Field1},@Field2={Field2},@Field3={Field3} </summary>
            public static readonly op AtFieldValue = new op() { _field_l = "@", _field_r = null };
            /// <summary> [Field1]={Field1},[Field2]={Field2},[Field3]={Field3} </summary>
            public static readonly op FieldValue = new op() { };
            /// <summary> [Field1]={Field1} and [Field2]={Field2} and [Field3]={Field3} </summary>
            public static readonly op AndFieldValue = new op() { _delimiter2 = " and " };
            public static readonly op where = new op() { _delimiter1 = " where ", _delimiter2 = " and " };
            public static readonly op where2 = new op() { _delimiter1 = " and ", _delimiter2 = " and " };
            public static readonly op update_set = new op() { _delimiter1 = " set " };
            public static readonly object insert = new object();

            void _op.Build(SqlBuilderX src, StringBuilder s)
            {
                string sep = this._delimiter1;
                for (int _index = 0, _count = src.items.Count; _index < _count; _index++)
                {
                    item n = src.items[_index];
                    if (!n.is_field) continue;
                    if ((object.ReferenceEquals(this, op.where) || object.ReferenceEquals(this, op.where2)) && !n.is_where_key) continue;
                    if (object.ReferenceEquals(this, op.update_set) && !n.is_update_value) continue;
                    if (n.value == null && !n.require)
                        continue;
                    s.Append(sep); sep = this._delimiter2;
                    if (this._field)
                    {
                        s.Append(this._field_l);
                        s.Append(n.field);
                        s.Append(this._field_r);
                        if (this._value)
                            s.Append(n.exp ?? "=");
                    }
                    if (this._value)
                    {
                        string format;
                        if (n.nvarchar)
                            format = StringFormatWith.sql_nvarchar;
                        else if (n.varchar)
                            format = StringFormatWith.sql_varchar;
                        else
                            format = n.format;
                        //if (n.value is string && n.nvarchar)
                        //    format = System.Data.SqlClient.SqlCmd.nvarchar;
                        s.Append('{');
                        s.Append(n.field);
                        if (format != null)
                        {
                            s.Append(':');
                            s.Append(format);
                        }
                        s.Append('}');
                    }
                }
            }
        }

        public int UpdateCount
        {
            get
            {
                int cnt = 0;
                for (int i = 0; i < this.items.Count; i++)
                {
                    item item = this.items[i];
                    if (item.is_update_value && (item.value != null))
                    {
                        cnt++;
                    }
                }
                return cnt;
            }
        }
        public int WhereCount
        {
            get
            {
                int cnt = 0;
                for (int i = 0; i < this.items.Count; i++)
                {
                    item item = this.items[i];
                    if (item.is_where_key && (item.value != null))
                    {
                        cnt++;
                    }
                }
                return cnt;
            }
        }

        /// <summary> build pattern for string.FormatWith </summary>
        public string BuildPattern(params object[] args)
        {
            StringBuilder s = new StringBuilder();
            for (int arg_n = 0; arg_n < args.Length; arg_n++)
            {
                object arg = args[arg_n];
                if (arg == null) continue;
                else if (object.ReferenceEquals(arg, op.insert))
                {
                    s.Append(' ');
                    s.Append('(');
                    ((_op)op.Fields).Build(this, s);
                    s.Append(@") values (");
                    ((_op)op.Values).Build(this, s);
                    s.Append(')');
                }
                else if (arg is _op)
                    ((_op)arg).Build(this, s);
                else if (arg is Action<StringBuilder>)
                    ((Action<StringBuilder>)arg)(s);
                else if (arg is Func<string>)
                    s.Append(((Func<string>)arg)());
                else s.Append(arg);
            }
            return s.ToString();
        }

        public string Build(out string pattern, params object[] args)
        {
            return (pattern = this.BuildPattern(args)).FormatWith(this, true);
        }

        public string Build(params object[] args)
        {
            string pattern;
            return this.Build(out pattern, args);
        }

        string __op(object op, out string pattern)
        {
            return this.Build(out pattern, op);
        }
        string __op(object op, bool pattern_only)
        {
            if (pattern_only)
                return this.BuildPattern(op);
            else
                return this.Build(op);
        }


        public string _insert(out string pattern) { return __op(SqlBuilderX.op.insert, out pattern); }
        public string _insert(bool pattern_only = false) { return __op(op.insert, pattern_only); }

        public string _insert(string tablename, out string pattern)
        {
            return this.Build(out pattern, "insert into ", tablename, op.insert);
        }
        public string _insert(string tablename, bool pattern_only = false)
        {
            if (pattern_only)
                return this.BuildPattern("insert into ", tablename, op.insert);
            else
                return this.Build("insert into ", tablename, op.insert);
        }

        public string _where(out string pattern) { return __op(SqlBuilderX.op.where, out pattern); }
        public string _where(bool pattern_only = false) { return __op(op.where, pattern_only); }

        public string _fields(out string pattern) { return __op(SqlBuilderX.op.Fields, out pattern); }
        public string _fields(bool pattern_only = false) { return __op(op.Fields, pattern_only); }

        public string _values(out string pattern) { return __op(SqlBuilderX.op.Values, out pattern); }
        public string _values(bool pattern_only = false) { return __op(op.Values, pattern_only); }

        public string _err(SqlBuilderX.err err, out string pattern) { return __op(err, out pattern); }
        public string _err(SqlBuilderX.err err, bool pattern_only = false) { return __op(err, pattern_only); }

        public string _update_set(out string pattern) { return __op(SqlBuilderX.op.update_set, out pattern); }
        public string _update_set(bool pattern_only = false) { return __op(op.update_set, pattern_only); }

        //public void SetUser(params string[] fields)
        //{
        //    if (fields.Length == 0)
        //        fields = new string[] { SqlBuilder.CreateTime, SqlBuilder.CreateUser, SqlBuilder.ModifyTime, SqlBuilder.ModifyUser };
        //    for (int i = 0; i < fields.Length; i++)
        //    {
        //        if (fields[i] is string)
        //        {
        //            string field = (string)fields[i];
        //            switch (field)
        //            {
        //                case SqlBuilder.CreateTime:
        //                case SqlBuilder.ModifyTime: this["", field, ""] = SqlBuilder.str.getdate; break;
        //                case SqlBuilder.CreateUser:
        //                case SqlBuilder.ModifyUser: this["", field, ""] = _HttpContext.Current._User.ID; break;
        //            }
        //        }
        //    }
        //}

        public SqlBuilderX() { }

        public SqlBuilderX(object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] pp = t.GetProperties(_TypeExtensions.BindingFlags2);
            FieldInfo[] ff = t.GetFields(_TypeExtensions.BindingFlags3);
            foreach (PropertyInfo p in pp)
                this[null, p.Name, null, null] = p.GetValue(obj, null);
            foreach (FieldInfo f in ff)
                this[null, f.Name, null, null] = f.GetValue(obj);
        }

        #region DynamicObject

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        #endregion

        #region IDictionary<string, object>

        void IDictionary<string, object>.Add(string key, object value)
        {
            this[key] = value;
        }
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            ((IDictionary<string, object>)this).Add(item.Key, item.Value);
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            item n; int i;
            bool r = this.get_item(key, out n, out i);
            if (r) this.items.RemoveAt(i);
            return r;
        }
        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)this).Remove(item.Key);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            item n; int i; return this.get_item(key, out n, out i);
        }

        object IDictionary<string, object>.this[string key]
        {
            get { return this[key]; }
            set { this[key] = value; }
        }
        ICollection<string> IDictionary<string, object>.Keys
        {
            get { throw new NotImplementedException(); }
        }
        ICollection<object> IDictionary<string, object>.Values
        {
            get { throw new NotImplementedException(); }
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            item n; int i; bool r;
            r = this.get_item(key, out n, out i);
            value = n.value;
            return r;
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            this.items.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            item n; int i;
            return this.get_item(item.Key, out n, out i);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get { return this.items.Count; }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return false; }
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}