using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ams.Data;
using System.Web.Http.ModelBinding;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace ams
{
    public enum filter_types { stringfilter, numericfilter, booleanfilter, datefilter }
    public enum filter_operators { and = 0, or = 1 }
    #region public enum filter_conditions
    public enum filter_conditions
    {
        // possible conditions for string filter:
        EMPTY, NOT_EMPTY, CONTAINS, CONTAINS_CASE_SENSITIVE,
        DOES_NOT_CONTAIN, DOES_NOT_CONTAIN_CASE_SENSITIVE, STARTS_WITH, STARTS_WITH_CASE_SENSITIVE,
        ENDS_WITH, ENDS_WITH_CASE_SENSITIVE, EQUAL, EQUAL_CASE_SENSITIVE, NULL, NOT_NULL,
        //possible conditions for numeric filter: 
        /*EQUAL,*/
        NOT_EQUAL, LESS_THAN, LESS_THAN_OR_EQUAL, GREATER_THAN, GREATER_THAN_OR_EQUAL, /*NULL, NOT_NULL,*/
    }
    #endregion
    #region public class jqx_filter
    public class jqx_filter
    {
        [JsonProperty("condition")]
        public filter_conditions? Condition;
        [JsonProperty("id")]
        public virtual string Id { get; set; }
        [JsonProperty("operator")]
        public filter_operators? Operator;
        [JsonProperty("type")]
        public filter_types? Type;
        [JsonProperty("value")]
        public virtual string Value { get; set; }

        public string GetValue(bool id_first = true)
        {
            if (id_first) return Id ?? Value;
            else return Value ?? Id;
        }
    }
    #endregion
    #region public class jqx_filter_list
    public class jqx_filter_list : List<jqx_filter>
    {
        public jqx_filter_list() { }
        public jqx_filter_list(params jqx_filter[] init) : base(init) { }

        public List<T> ToEnumList<T>() where T : struct
        {
            List<T> ret = null;
            foreach (var n in this)
            {
                T? nn = n.Id.ToEnum<T>() ?? n.Value.ToEnum<T>();
                if (nn.HasValue)
                    _null._new(ref ret).Add(nn.Value);
            }
            return ret;
        }

        public List<int> ToInt32List()
        {
            List<int> ret = null;
            foreach (var n in this)
            {
                int nn;
                if (n.Id.ToInt32(out nn) || n.Value.ToInt32(out nn))
                    _null._new(ref ret).Add(nn);
            }
            return ret;
        }

        public List<string> ToStringList()
        {
            List<string> ret = null;
            foreach (var n in this)
            {
                string s = n.Id ?? n.Value;
                if (!string.IsNullOrEmpty(s))
                    _null._new(ref ret).Add(s);
            }
            return ret;
        }

        internal static bool TryParse(object value, out object result)
        {
            if (value is JObject)
            {
                try
                {
                    JObject obj = (JObject)value;
                    jqx_filter item = obj.ToObject<jqx_filter>(json.GetJsonSerializer());
                    result = new jqx_filter_list(item);
                    return true;
                }
                catch { }
            }
            else if (value is JArray)
            {
                JArray obj = (JArray)value;
                try
                {
                    jqx_filter_list list = obj.ToObject<jqx_filter_list>(json.GetJsonSerializer());
                    result = list;
                    return list.Count > 0;
                }
                catch { }
                try
                {
                    string[] str = obj.ToObject<string[]>(json.GetJsonSerializer());
                    result = str;
                    if (str.Length == 0)
                        return false;
                    else if (str.Length == 1)
                        result = str[0];
                    return true;
                }
                catch { }
            }
            return _null.noop(false, out result);
        }
    }
    #endregion

    partial class amsHelpers
    {
        public static List<T> GetEnums<T>(this FilterableAttribute attr) where T : struct
        {
            if (attr.Value is string)
                attr.Value = new string[] { attr.Value };
            if (attr.Value is string[])
            {
                List<T> ret = null;
                foreach (var n in (string[])attr.Value)
                {
                    T? nn = n.ToEnum<T>();
                    if (nn.HasValue)
                        _null._new(ref ret).Add(nn.Value);
                }
                return ret;
            }
            else if (attr.Value is jqx_filter_list)
                return ((jqx_filter_list)attr.Value).ToEnumList<T>();
            return null;
        }

        public static bool GetValue(this FilterableAttribute attr, out string result, bool jqx_filter_id_first = true)
        {
            if (attr != null)
            {
                if (attr.Value is string)
                {
                    result = attr.Value;
                    return true;
                }
                else if (attr.Value is string[])
                {
                    string[] s = attr.Value;
                    if (s.Length > 0)
                    {
                        result = s[0];
                        return true;
                    }
                }
                else if (attr.Value is jqx_filter_list)
                {
                    jqx_filter_list n = attr.Value;
                    if (n.Count > 0)
                    {
                        result = n[0].GetValue(jqx_filter_id_first);
                        return true;
                    }
                }
            }
            return _null.noop(false, out result);
        }

        public static bool GetValue(this FilterableAttribute attr, out string[] result, bool jqx_filter_id_first = true)
        {
            result = null;
            if (attr == null) return false;
            else if (attr.Value is string[])
                result = attr.Value;
            else if (attr.Value is jqx_filter_list)
            {
                jqx_filter_list n = attr.Value;
                result = new string[n.Count];
                for (int i = 0; i < n.Count; i++)
                    result[i] = n[i].GetValue(jqx_filter_id_first);
            }
            return !result.IsNullOrEmpty();
        }
    }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ListRequest<T> : DynamicObject
    {
        //#region class filter_item
        //class filter_item
        //{
        //    // Possible values :
        //    //  "str"
        //    //  ["str1", "str2", "str3"]
        //    //  { a:1, b:2 }
        //    //  [ { a:1, b:2 }, { a:3, b:4 }, { a:5, b:6 }]
        //    public dynamic Value;
        //    public FilterableAttribute Filterable;
        //}
        //#endregion

        #region Options

        [JsonProperty]
        public int? PageNumber;
        [JsonProperty]
        public int? PageSize;
        int? BeginRowIndex;
        int? EndRowIndex;

        [JsonProperty]
        public string SortField;
        [JsonProperty]
        public SortOrder? SortOrder;
        [JsonProperty]
        public DateTime? BeginTime;
        [JsonProperty]
        public DateTime? EndTime;
        [JsonIgnore]
        UserName CorpName;

        #endregion

        Dictionary<string, FilterableAttribute> filters = new Dictionary<string, FilterableAttribute>(StringComparer.OrdinalIgnoreCase);

        [JsonProperty]
        JObject jqx;

        public CorpInfo CorpInfo;

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string name = binder.Name;
            FilterableAttribute item = TableName<T>._.GetFilterable2(name);
            if ((item == null) && (name == nameof(CorpName)))
            {
                item = new FilterableAttribute(true) { };
            }
            if (item != null)
            {
                object obj;
                if (jqx_filter_list.TryParse(value, out obj))
                    item.Value = obj;
                else
                    item.Value = value;
                filters[name] = item;
            }
            return true;
            //return base.TrySetMember(binder, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = filters.GetValue(binder.Name, false)?.Value;
            return true;
        }

        public FilterableAttribute GetFilter(string name)=> filters.GetValue(name, false);



        List<string> _sql_where;
        public List<string> sql_where_add(string s)
        {
            if (_sql_where == null)
                _sql_where = new List<string>();
            _sql_where.Add(s);
            return _sql_where;
        }

        public void sql_where_add_list(string name, IList list)
        {
            if (list.IsNullOrEmpty()) return;
            if (list.Count == 1)
            {
                object value = list[0];
                if (value.GetType().IsEnum)
                    value = Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));
                sql_where_add($"{name} = {value}");
            }
            else
                sql_where_add($"{name} in {list.ToSqlString()}");
        }



        void AddFilter_CorpName(string name, FilterableAttribute item)
        {
            string corpName;
            if (item.GetValue(out corpName, false))
            {
                this.CorpInfo = CorpInfo.GetCorpInfo(name: corpName, err: false);
                if (this.CorpInfo != null)
                    sql_where_add($"{name} = {CorpInfo.ID}");
            }

        }

        public void AddFilter_TimeRange(string name, FilterableAttribute item)
        {
            string[] n;
            if (item.GetValue(out n))
            {
                if (n.Length < 2) return;
                DateTime time1, time2;
                if (n[0].ToDateTime(out time1) && n[1].ToDateTime(out time2))
                    AddTimes(name, time1, time2);
                //{
                //    time1 = time1.ToLocalTime();
                //    time2 = time2.ToLocalTime();
                //    DateTime time_start = time1;
                //    DateTime time_end = time2;
                //    if (time1 > time2)
                //    {
                //        time_start = time2;
                //        time_end = time1;
                //    }
                //    sql_where_add($"{name} >= '{time_start.ToString(SqlBuilder.DateTimeFormat)}'");
                //    sql_where_add($"{name} < '{time_end.ToString(SqlBuilder.DateTimeFormat)}'");
                //}
            }
        }

        void AddTimes(string name, DateTime? time_start, DateTime? time_end)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (time_start.HasValue)
                time_start = time_start.Value.ToLocalTime();
            if (time_end.HasValue)
                time_end = time_end.Value.ToLocalTime();
            if (time_start.HasValue && time_end.HasValue)
            {
                if (time_start.Value >= time_end.Value)
                {
                    DateTime tmp = time_start.Value;
                    time_start = time_end;
                    time_end = tmp;
                }
            }
            if (time_start.HasValue)
                sql_where_add($"{name} >= '{time_start.Value.ToString(SqlBuilder.DateTimeFormat)}'");
            if (time_end.HasValue)
                sql_where_add($"{name} < '{time_end.Value.ToString(SqlBuilder.DateTimeFormat)}'");
        }

        public void AddFilter_PlatformNames(string name, FilterableAttribute item)
        {
            string[] n1;
            if (item.GetValue(out n1))
            {
                List<int> n2 = new List<int>();
                for (int i = 0; i < n1.Length; i++)
                {
                    int n3;
                    PlatformInfo p;
                    if (n1[i].ToInt32(out n3))
                        p = PlatformInfo.GetPlatformInfo(n3);
                    else
                        p = PlatformInfo.GetPlatformInfo(n1[i]);
                    if (p != null)
                        n2.Add(p.ID);
                }
                sql_where_add_list(name, n2);
            }
        }

        public void AddFilter_GameIDs(string name, FilterableAttribute item)
        {
            string[] n1;
            if (item.Value is string)
                n1 = new string[] { item.Value };
            else if (item.GetValue(out n1))
            { }
            else return;
            List<int> n2 = new List<int>();
            for (int i = 0; i < n1.Length; i++)
            {
                GameInfo g = GameInfo.GetGameInfo(n1[i]);
                if (g != null)
                    n2.Add(g.ID);
            }
            sql_where_add_list(name, n2);
        }

        public void AddFilter_Int32(string name, FilterableAttribute item)
        {
            string n1; int n2;
            if (item.GetValue(out n1) && n1.ToInt32(out n2))
                this.sql_where_add($"{name} = {n2}");
        }

        public void AddFilter_StringContains(string name, FilterableAttribute item)
        {
            string n;
            if (item.GetValue(out n))
            {
                n = n.Replace("%", "");
                this.sql_where_add($"{name} like '%{SqlCmd.magic_quote(n)}%'");
            }
        }

        public void AddFilter_Enum<TEnum>(string name, FilterableAttribute item) where TEnum : struct => sql_where_add_list(name, item.GetEnums<TEnum>());



        public virtual ListRequest<T> Validate(_ApiController controller, bool valid_user, Action valid = null)
        {
            if (valid_user)
            {
                FilterableAttribute item;
                string corpName = null;
                if (this.filters.TryGetValue(nameof(CorpName), out item))
                    item.GetValue(out corpName, false);
                this.CorpInfo = CorpInfo.GetCorpInfo(name: corpName, err: true);
                sql_where_add($"CorpID = {CorpInfo.ID}");
            }
            if (this.PageNumber.HasValue && (this.PageNumber.Value < 0))
                this.PageNumber = null;
            if (this.PageNumber.HasValue && this.PageSize.HasValue)
            {
                if (this.PageSize.Value < 0) this.PageSize = null;
                else if (this.PageSize.Value > 1000) this.PageSize = 1000;
                if (this.PageSize.HasValue)
                {
                    this.BeginRowIndex = this.PageNumber.Value * this.PageSize.Value + 1;
                    this.EndRowIndex = this.BeginRowIndex + this.PageSize.Value;
                }
            }
            if (string.IsNullOrEmpty(this.SortField))
                this.SortField = TableName<T>._.SortField;
            else if (!TableName<T>._.IsSortable(this.SortField))
                this.SortField = TableName<T>._.SortField;
            this.SortOrder = this.SortOrder ?? TableName<T>._.SortOrder;
            valid?.Invoke();
            controller.ModelState.IsValid(true);
            return this;
        }

        public delegate void OnBuildHandler(string name, FilterableAttribute item);
        public void OnBuild(string name, FilterableAttribute item) { }
        SqlCmd GetSqlCmd()
        {
            if (this.CorpInfo == null)
                return _HttpContext.GetSqlCmd(DB.Core01R);
            return this.CorpInfo.DB_User01R();
        }
        public ListResponse<T> GetResponse(
            Func<SqlCmd> get_sqlcmd = null,
            string tableName = null,
            bool row_count = true,
            Func<T> create = null,
            OnBuildHandler onBuild = null,
            string time_field = null)
        {
            get_sqlcmd = get_sqlcmd ?? this.GetSqlCmd;
            tableName = tableName ?? TableName<T>._.TableName;
            create = create ?? (() => (T)Activator.CreateInstance(typeof(T)));
            onBuild = onBuild ?? this.OnBuild;
            time_field = time_field ?? this.SortField;

            foreach (var p in filters)
                if (p.Value.Value != null)
                    onBuild(p.Key, p.Value);

            AddTimes(time_field, this.BeginTime, this.EndTime);

            string sql_where_string = "";
            if (!_sql_where.IsNullOrEmpty())
                sql_where_string = $"where {string.Join(" and ", _sql_where)}";

            string rowid = "";
            if (this.BeginRowIndex.HasValue && this.EndRowIndex.HasValue)
            {
                rowid = $@"
where _rowid>={this.BeginRowIndex} and _rowid<{this.EndRowIndex}";
            }
            string sql1 = $@"select * from({sql_rows(tableName, sql_where_string)}) a {rowid}";
            string sql2 = sql_count(tableName, sql_where_string);
            ListResponse<T> ret = new ListResponse<T>();
            SqlCmd sqlcmd = get_sqlcmd();
            ret.Rows = sqlcmd.ToList(create, sql1);
            if (row_count)
                ret.RowsCount = sqlcmd.ExecuteScalar(sql2) as int?;
            return ret;
        }

        protected virtual string sql_rows(string tableName, string sql_where_string) => $@"
    select row_number() over (order by {this.SortField} {this.SortOrder}) as _rowid, * from {tableName} nolock
    {sql_where_string}";
        protected virtual string sql_count(string tableName, string sql_where_string) => $@"select count(*) from {tableName} nolock
{sql_where_string}";
    }

    [JsonConverter(typeof(_JsonConverter))]
    public abstract class ListResponse : DynamicObject
    {
        internal Dictionary<string, object> data = new Dictionary<string, object>();

        protected abstract IList _rows { get; }
        public int? RowsCount;

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.data[binder.Name] = value;
            return true;
            //return base.TrySetMember(binder, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.data.TryGetValue(binder.Name, out result);
            //return base.TryGetMember(binder, out result);
        }

        class _JsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType.IsAssignableFrom(typeof(ListResponse));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                ListResponse obj = value as ListResponse;
                if (obj == null)
                    serializer.Serialize(writer, value);
                else
                {
                    IList rows = obj._rows ?? _null.objects;
                    obj.data["records"] = rows;
                    obj.data["totalrecords"] = obj.RowsCount ?? rows.Count;
                    serializer.Serialize(writer, obj.data);
                }
            }
        }
    }

    [DebuggerStepThrough]
    public class ListResponse<T> : ListResponse
    {
        public List<T> Rows;

        protected override IList _rows { get { return this.Rows; } }

        public ListResponse(bool create_list = false) { if (create_list) this.Rows = new List<T>(); }

        public ListResponse(IEnumerable<T> collection) { this.Rows = new List<T>(collection); }
    }

    //partial class amsHelpers
    //{
    //    public static jqx_filter_list ToFilterList(this JArray src)
    //    {
    //        try { return src.ToObject<jqx_filter_list>(json.GetJsonSerializer()); }
    //        catch { return null; }
    //    }
    //    public static jqx_filter_list<T> ToFilterList<T>(this JArray src)
    //    {
    //        try { return src.ToObject<jqx_filter_list<T>>(json.GetJsonSerializer()); }
    //        catch { return null; }
    //    }
    //    public static List<T> ToFilterEnumList<T>(this JArray src) where T : struct
    //    {
    //        jqx_filter_list<T?> t1 = src.ToFilterList<T?>();
    //        List<T> list = null;
    //        foreach (var t2 in t1)
    //        {
    //            T? value = t2.Id ?? t2.Value.ToEnum<T>();
    //            if (value.HasValue)
    //            {
    //                if (list == null)
    //                    list = new List<T>();
    //                list.AddOnce(value.Value);
    //            }
    //        }
    //        if (list == null) return null;
    //        if (list.Count == 0) return null;
    //        return list;
    //    }
    //    public static List<T> ToEnumList<T>(this JArray src) where T : struct
    //    {
    //        try
    //        {
    //            List<string> tmp1 = src.ToObject<List<string>>(json.GetJsonSerializer());
    //            List<T> list = null;
    //            foreach (string tmp2 in tmp1)
    //            {
    //                T? value = tmp2.ToEnum<T>();
    //                if (value.HasValue)
    //                {
    //                    if (list == null)
    //                        list = new List<T>();
    //                    list.AddOnce(value.Value);
    //                }
    //            }
    //            if (list == null) return null;
    //            if (list.Count == 0) return null;
    //            return list;
    //        }
    //        catch { }
    //        return null;
    //    }
    //}
}