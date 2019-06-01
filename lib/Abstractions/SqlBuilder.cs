using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory
{
    /// <summary>
    /// Sql CommandText pattern builder
    /// </summary>
    public struct SqlBuilder : IEnumerable<object>, System.Data.ITableName
    {
        IEnumerator<object> IEnumerable<object>.GetEnumerator() => null;
        IEnumerator IEnumerable.GetEnumerator() => null;

        private const string comma = ", ";
        private const string eq = " = ";
        public const string DateFormat = "yyyy-MM-dd";
        public const string TimeFormat = "HH:mm:ss";
        public const string DateTimeFormat = DateFormat + " " + TimeFormat;
        public const string PreciseDateTimeFormat = DateTimeFormat + ".fff";
        public const string TableName = "[{:TableName}]";

        public static object IsNull(object value) => value ?? SqlBuilder.raw_null;

        [Flags]
        public enum Flags : int
        {
            nvarchar = 0x01,
            where_key = 0x04,
            update = 0x08,
        }

        private struct _Item
        {
            public Flags Flags;
            public string Name;
            public object Value;
            public string format;
        }
        private object _model;
        private Type _modelType;
        private _Item[] _items;
        private UserId? _createUser;
        private UserId? _modifyUser;

        public SqlBuilder(Type modelType)
            : this(null, modelType) { }

        public SqlBuilder(object model)
            : this(model, model?.GetType()) { }

        private SqlBuilder(object model, Type modelType)
        {
            _model = model;
            _modelType = modelType;
            _items = null;
            _createUser = _modifyUser = null;
        }

        public object this[string name]
        {
            set => AddImpl(name, value, null, null);
        }

        private bool IndexOf(string name, out int index)
        {
            if (_items != null)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    if (_items[i].Name == name)
                    {
                        index = i;
                        return true;
                    }
                }
            }
            index = -1;
            return false;
        }

        //public void Add(string name, object value = null) => Add(null, name, value);
        public void Add(string flags, string name, object value = null, string format = null)
        {
            Flags f = 0;
            if (flags.Contains('w'))
                f |= Flags.where_key;
            if (flags.Contains('u'))
                f |= Flags.update;
            if (flags.Contains('N'))
                f |= Flags.nvarchar;
            if (value is DateTime)
                format = format ?? DateTimeFormat;
            AddImpl(name, value, f, format);
        }
        //public void Add(string name, Flags flags = 0) => Add(name, default(object), flags);
        private void AddImpl(string name, object value, Flags? flags, string format)
        {
            if (name == null) return;
            if (_items == null)
                _items = new _Item[0];
            _start:
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Name == null || _items[i].Name == name)
                {
                    _items[i] = new _Item()
                    {
                        Name = name,
                        Value = value,
                        Flags = flags ?? _items[i].Flags,
                        format = format ?? _items[i].format
                    };
                    return;
                }
            }
            Array.Resize(ref _items, _items.Length + 1);
            goto _start;
        }

        public void Add(UserId? modifyUser)
        {
            _modifyUser = modifyUser;
        }
        public void Add(UserId? createUser, UserId? modifyUser)
        {
            _createUser = createUser;
            _modifyUser = modifyUser;
        }

        ITableName _TableName =>
            System.Data.TableName.GetInstance(_modelType) ??
            System.Data.TableName.GetInstance(_model);

        string ITableName.TableName => _TableName?.TableName;

        string ITableName.Database => _TableName?.Database;

        string ITableName.SortKey => _TableName?.SortKey;


        public string FormatWith(string pattern) => pattern.FormatWith(this, sql: true, getValue: TryGetValue);

        private bool TryGetValue(object obj, string name, out object value)
        {
            if (name == null)
                return _null.noop(false, out value);
            else if (name == "ModifyTime" && _modifyUser.HasValue)
                value = SqlBuilder.raw_getdate;
            else if (name == "ModifyUser" && _modifyUser.HasValue)
                value = _modifyUser.Value;
            else if (name == "CreateTime" && _createUser.HasValue)
                value = SqlBuilder.raw_getdate;
            else if (name == "CreateUser" && _createUser.HasValue)
                value = _createUser.Value;
            else if (IndexOf(name, out int index))
                return TryGetValue(index, out value);
            else
                return _null.noop(false, out value);
            return true;
        }
        private bool TryGetValue(int index, out object value)
        {
            value = null;
            _modelType?.TryGetValue(_model, _items[index].Name, out value);
            value = value ?? _items[index].Value;
            return value != null;
        }



        private static StringBuilder AppendAtField(StringBuilder s, string delimiter, string name)
        {
            if (s == null)
                s = new StringBuilder();
            else
                s.Append(delimiter);
            s.Append('@').Append(name);
            return s;
        }
        private static StringBuilder AppendField(StringBuilder s, string delimiter, string name)
        {
            if (s == null)
                s = new StringBuilder();
            else
                s.Append(delimiter);
            s.Append('[').Append(name).Append(']');
            return s;
        }
        private static StringBuilder AppendValue(StringBuilder s, string delimiter, string name, Flags flags, string format)
        {
            if (s == null)
                s = new StringBuilder();
            else
                s.Append(delimiter);
            //if (flags.HasFlag(Flags.nvarchar))
            //s.Append('N');
            s.Append('{').Append(name);
            if (flags.HasFlag(Flags.nvarchar))
                s.Append(':').Append(StringFormatWith.sql_nvarchar);
            else if (format != null)
                s.Append(':').Append(format);
            s.Append('}');
            return s;
        }
        private static StringBuilder AppendFieldValue(StringBuilder s, string delimiter, string name, Flags flags, string format)
        {
            s = AppendField(s, delimiter, name);
            s.Append(eq);
            s = AppendValue(s, null, name, flags, format);
            return s;
        }
        private static StringBuilder AppendAtFieldValue(StringBuilder s, string delimiter, string name, Flags flags, string format)
        {
            s = AppendAtField(s, delimiter, name);
            s.Append(eq);
            s = AppendValue(s, null, name, flags, format);
            return s;
        }


        //public static string select_all_from(bool with_nolock = true)
        //{
        //    if (with_nolock)
        //        return $"select * from {SqlBuilder.TableName} nolock";
        //    else
        //        return $"select * from {SqlBuilder.TableName}";
        //}

        //public static string select_all_from<T>(bool with_nolock = true, string orderBy = null, int offset = 0, int next = 0)
        //{
        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("select * from ");
        //    sql.Append(TableName<T>.Value);
        //    if (with_nolock)
        //        sql.Append(" nolock");
        //    if (next > 0)
        //    {
        //        orderBy = TableName<T>._.SortKey;
        //        sql.AppendFormat(" order by {0} offset {1} rows fetch next {2} rows only", orderBy, offset, next);
        //    }
        //    return sql.ToString();
        //}

        //public static string paging(string orderBy = null, int offset = 0, int next = 0)
        //{
        //    return $"order by {orderBy} offset {offset} rows fetch next {next} rows only";
        //}
        //public static string paging<T>(string orderBy = null, int offset = 0, int next = 0)
        //{
        //    return $"order by {orderBy ?? TableName<T>._.SortKey} offset {offset} rows fetch next {next} rows only";
        //}

        //        public string getPage<T>(string orderBy, int pageIndex, int pageSize)
        //        {
        //            return $@"SELECT * FROM {TableName<T>.Value} nolock
        //ORDER BY {orderBy}
        //{Sql_OFFSET_NEXT(pageSize * pageIndex, pageSize)}";
        //        }

        //public static string Sql_OFFSET_NEXT(int offset, int next) => $"OFFSET {offset} ROWS FETCH NEXT {next} ROWS ONLY";
        //public static string Sql_OFFSET_NEXT(string orderBy, int offset, int next) => $"ORDER BY {orderBy} OFFSET {offset} ROWS FETCH NEXT {next} ROWS ONLY";


        /// <summary> @Field1 = {Field1}, @Field2 = {Field2}, @Field3 = {Field3} </summary>
        public string exec(string sp_name = null, bool formatWith = false)
        {
            StringBuilder s = null;
            if (_items != null)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    s = AppendAtFieldValue(s, comma, _items[i].Name, _items[i].Flags, _items[i].format);
                }
                if (_createUser.HasValue)
                    s = AppendAtFieldValue(s, comma, "CreateUser", 0, null);
                if (_modifyUser.HasValue)
                    s = AppendAtFieldValue(s, comma, "ModifyUser", 0, null);

                if (s != null && sp_name != null)
                {
                    int n = 0;
                    s.Insert(ref n, "exec ");
                    s.Insert(ref n, sp_name);
                    s.Insert(ref n, " ");
                }
            }
            string p = s?.ToString();
            if (formatWith)
                return this.FormatWith(p);
            else
                return p;
        }

        public string update(bool update_set = false)
        {
            if (_items != null)
            {
                StringBuilder s = null;
                for (int i = 0; i < _items.Length; i++)
                {
                    if (_items[i].Flags.HasFlag(Flags.update))
                    {
                        if (_model != null && !TryGetValue(i, out object value))
                            continue;
                        s = AppendFieldValue(s, comma, _items[i].Name, _items[i].Flags, _items[i].format);
                    }
                }
                if (s != null)
                {
                    if (_modifyUser.HasValue)
                    {
                        s = AppendFieldValue(s, comma, "ModifyTime", 0, null);
                        s = AppendFieldValue(s, comma, "ModifyUser", 0, null);
                    }
                    if (update_set)
                    {
                        int n = 0;
                        s.Insert(ref n, "update ");
                        s.Insert(ref n, SqlBuilder.TableName);
                        s.Insert(ref n, " set ");
                    }
                    return s.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// [Field1] = {Field1}, [Field2] = {Field2}, [Field3] = {Field3},
        /// <see cref="Flags.update"/> && value != null
        /// </summary>
        public bool update(out string sql, bool update_set = false)
        {
            sql = update(update_set);
            return sql != null;
        }

        public bool update_set(out string sql) => update(out sql, true);
        public string update_set() => update(true);

        /// <summary>
        /// [Field1] = {Field1} and [Field2] = {Field2} and [Field3] = {Field3},
        /// <see cref="Flags.where_key"/>
        /// </summary>
        public string where(string prefix = "where ")
        {
            if (_items != null)
            {
                StringBuilder s = null;
                for (int i = 0; i < _items.Length; i++)
                {
                    if (_items[i].Flags.HasFlag(Flags.where_key))
                    {
                        s = AppendFieldValue(s, " and ", _items[i].Name, _items[i].Flags, _items[i].format);
                    }
                }
                if (s != null)
                {
                    if (prefix != null)
                        s.Insert(0, prefix);
                    return s.ToString();
                }
            }
            return null;
        }

        /// <param name="fields">[Field1], [Field2], [Field3</param>
        /// <param name="values">{Field1}, {Field2}, {Field3}</param>
        public void build(out string fields, out string values)
        {
            StringBuilder s1 = null, s2 = null;
            if (_items != null)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    s1 = AppendField(s1, comma, _items[i].Name);
                    s2 = AppendValue(s2, comma, _items[i].Name, _items[i].Flags, _items[i].format);
                }
                if (_createUser.HasValue)
                {
                    s1.Append(comma).Append("[CreateUser]");
                    s2.Append(comma).Append("{CreateUser}");
                }
                if (_modifyUser.HasValue)
                {
                    s1.Append(comma).Append("[ModifyUser]");
                    s2.Append(comma).Append("{ModifyUser}");
                }
            }
            fields = s1?.ToString();
            values = s2?.ToString();
        }

        public string insert_into()
        {
            build(out string fields, out string values);
            return $@"insert into {SqlBuilder.TableName} ({fields})
values ({values})";
        }

        public string select_where() => $"select * from {SqlBuilder.TableName} {this.where()}";

        //public string build(Func<SqlBuilder, string> cb) => cb(this);

        public sealed class Raw : StringFormatWith.RawSqlString
        {
            private string value;
            public override string ToString() => value;

            public static explicit operator Raw(string value) => new Raw() { value = value };
        }

        public static readonly Raw raw_getdate = (Raw)"getdate()";
        public static readonly Raw raw_null = (Raw)"null";
        public static readonly Raw raw_newid = (Raw)"newid()";
    }
}