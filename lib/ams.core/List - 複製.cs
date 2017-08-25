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
    public class ListRequest_2<T> : DynamicObject
    {
        class filter_item
        {
            // Possible values :
            //  "str"
            //  ["str1", "str2", "str3"]
            //  { a:1, b:2 }
            //  [ { a:1, b:2 }, { a:3, b:4 }, { a:5, b:6 }]
            public dynamic Value;
            public FilterableAttribute Filterable;
        }

        #region Options

        [JsonProperty]
        public int? PageNumber;
        [JsonProperty]
        public int? PageSize;
        [JsonProperty]
        public string SortField;
        [JsonProperty]
        public SortOrder? SortOrder;
        [JsonProperty]
        public DateTime? BeginTime;
        [JsonProperty]
        public DateTime? EndTime;

        #endregion

        #region User

        [JsonProperty]
        UserName CorpName;
        [JsonProperty]
        UserName AgentName;
        [JsonProperty]
        UserName AdminName;
        [JsonProperty]
        UserName MemberName;

        [JsonProperty]
        UserID? CorpID;
        [JsonProperty]
        UserID? AgentID;
        [JsonProperty]
        UserID? AdminID;
        [JsonProperty]
        UserID? MemberID;

        [JsonIgnore]
        public CorpInfo CorpInfo;
        [JsonIgnore]
        public AgentData AgentData;
        [JsonIgnore]
        public AdminData AdminData;
        [JsonIgnore]
        public MemberData MemberData;

        #endregion

        public dynamic jqx { get; set; }
        Dictionary<string, filter_item> filters = new Dictionary<string, filter_item>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null) return true;
            if (value == null) return true;
            string name = binder.Name;
            if (name == nameof(CorpID)) return true;
            if (name == nameof(AgentID)) return true;
            if (name == nameof(AdminID)) return true;
            if (name == nameof(MemberID)) return true;
            if (name == nameof(CorpName)) return true;
            if (name == nameof(AgentName)) return true;
            if (name == nameof(AdminName)) return true;
            if (name == nameof(MemberName)) return true;
            FilterableAttribute filterable = TableName<T>._.GetFilterable(name);
            if (filterable == null) return true;
            filters[name] = new filter_item() { Filterable = filterable, Value = value };
            return true;
        }



        public string sql_list(string tableName = null, OnBuild onBuild = null)
        {
            string rowid = "";
            if (this.PageNumber.HasValue && this.PageSize.HasValue)
            {
                int beginRowIndex = this.PageNumber.Value * this.PageSize.Value + 1;
                int endRowIndex = beginRowIndex + this.PageSize.Value;
                rowid = $@"
 where _rowid>={beginRowIndex} and _rowid<{endRowIndex}";
            }
            string select = $@"select row_number() over (order by {this.SortField} {this.SortOrder}) as _rowid, * from {tableName ?? TableName<T>._.TableName} nolock
{this.sql_where(onBuild)}";
            return $@"select * from
({select}) a {rowid}";
        }

        public string sql_count(string tableName = null, OnBuild onBuild = null)
        {
            return $@"select count(*) from {tableName ?? TableName<T>._.TableName} nolock
{this.sql_where(onBuild)}";
        }

        public delegate void OnBuild(SqlBuilder sql, string name, Type valueType, jqx_filter_list value1, string[] value2, object value);

        public SqlBuilder sql_builder;
        public List<object> sql_build_op;

        string _sql_where;
        public string sql_where(OnBuild onBuild)
        {
            if (_sql_where != null) return _sql_where;
            _sql_where = "";
            foreach (string name in filters.Keys)
            {
                filter_item _item = filters[name];
                if (_item.Value == null) continue;
                jqx_filter_list value1 = null;
                string[] value2 = null;
                if (_item.Value is JObject)
                #region value is jqx_filter ?
                {
                    JObject obj = _item.Value;
                    try
                    {
                        jqx_filter item = obj.ToObject<jqx_filter>(json.GetJsonSerializer());
                        value1 = new jqx_filter_list();
                        value1.Add(item);
                    }
                    catch { value1 = null; }
                }
                #endregion

                if (value1 == null && _item.Value is JArray)
                #region value is jqx_filter_list?
                {
                    JArray obj = _item.Value;
                    try
                    {
                        value1 = obj.ToObject<jqx_filter_list>(json.GetJsonSerializer());
                        if (value1.Count == 0)
                            value1 = null;
                    }
                    catch { value1 = null; }

                    if (value1 == null)
                    {
                        try { value2 = obj.ToObject<string[]>(json.GetJsonSerializer()); }
                        catch { value2 = null; }
                    }
                }
                #endregion

                if (_item.Filterable == null)
                {
                    if (_item.Value is IList)
                        AddList(sql_builder, name, _item.Value);
                    else
                        sql_builder["w", name] = _item.Value;
                }
                else
                {
                    if (value2 == null && _item.Value is string)
                        value2 = new string[] { (string)_item.Value };
                    Type ftype = _item.Filterable.ValueType;
                    if (ftype == null) continue;
                    else if (ftype == typeof(PlatformType))
                        this.AddList(sql_builder, name, value1?.ToEnumList<PlatformType>() ?? value2?.ToEnum<PlatformType>());
                    else if (ftype == typeof(GameClass))
                        this.AddList(sql_builder, name, value1?.ToEnumList<GameClass>() ?? value2?.ToEnum<GameClass>());
                    else if (ftype == typeof(LogType))
                        this.AddList(sql_builder, name, value1?.ToEnumList<LogType>() ?? value2?.ToEnum<LogType>());
                    else
                        onBuild?.Invoke(sql_builder, name, ftype, value1, value2, _item.Value);
                }
            }
            return _sql_where = sql_builder.Build(sql_build_op.ToArray());
        }

        public void AddList(SqlBuilder sql, string name, IList list)
        {
            if (list == null) return;
            if (list.Count == 0) return;
            if (list.Count == 1)
                sql["w", name] = list[0];
            else
                sql["w", name, SqlCmd.array, " in "] = list;
        }

        public void AddTimeRange(SqlBuilder sql, string name, Type valueType, jqx_filter_list value1, string[] value2)
        {
            value2 = value2 ?? value1?.ToStringList()?.ToArray();
            if (value2 == null) return;
            if (value2.Length < 2) return;
            DateTime time1, time2;
            if (value2[0].ToDateTime(out time1) && value2[1].ToDateTime(out time2))
            {
                time1 = time1.ToLocalTime();
                time2 = time2.ToLocalTime();
                DateTime time_start = time1;
                DateTime time_end = time2;
                if (time1 > time2)
                {
                    time_start = time2;
                    time_end = time1;
                }
                sql[name + "_begin"] = time_start;
                sql[name + "_end"] = time_end;
                this.sql_build_op.Add($" and {name} >= {{{name}_begin:{SqlBuilder.DateTimeFormat}}} and {name} < {{{name}_end:{SqlBuilder.DateTimeFormat}}}");
            }
        }

        public void AddPlatforms(SqlBuilder sql, string name, Type valueType, jqx_filter_list value1, string[] value2)
        {
            value2 = value2 ?? value1?.ToStringList()?.ToArray();
            if (value2 == null) return;

            List<int> list = null;
            //if (value is jqx_filter_list && valueType == typeof(int))
            //    list = ((jqx_filter_list)value).ToInt32List();
            //else if (value is string[])
            //    list = ((string[])value).ToInt32();

            //if (list == null) return;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                PlatformInfo p = PlatformInfo.GetPlatformInfo(list[i]);
                if (p == null)
                    list.RemoveAt(i);
            }
            AddList(sql, name, list);
        }

        public void AddGameIDs(SqlBuilder sql, string name, Type valueType, jqx_filter_list value1, string[] value2)
        {
            value2 = value2 ?? value1?.ToStringList()?.ToArray();
            if (value2 == null) return;

            List<int> list2 = null;
            foreach (string s in value2)
            {
                GameInfo g = GameInfo.GetGameInfo(s);
                if (g != null)
                    _null._new(ref list2).Add(g.GameID);
            }
            if (list2 == null) return;
            if (list2.Count == 0) return;
            AddList(sql, name, list2);
        }

        public ListResponse<T> GetResponse(SqlCmd sqlcmd, string tableName = null, bool row_count = true, Func<T> create = null, OnBuild onBuild = null)
        {
            if (create == null)
                create = () => (T)Activator.CreateInstance(typeof(T));
            ListResponse<T> ret = new ListResponse<T>();

            string sql1 = this.sql_list(tableName, onBuild);
            ret.Rows = sqlcmd.ToList(create, sql1) ?? _null<List<T>>.value;
            if (row_count)
            {
                string sql2 = this.sql_count(tableName);
                ret.RowsCount = sqlcmd.ExecuteScalar(sql2) as int?;
            }
            return ret;
        }

        public static ListRequest_2<T> Valid(ModelStateDictionary modelstate, ListRequest_2<T> args, bool valid_user = true)
        {
            string _json = _HttpContext.Current.Arguments;
            if (args == null)
                throw new _Exception(Status.InvalidParameter);

            //_null._new(ref args.Filters).ValidFilter(TableName<T>._);

            if (valid_user)
            {
                modelstate.Validate(nameof(CorpName), args.CorpName, allow_null: true);
                bool n1 = modelstate.Validate(nameof(AgentName), args.AgentName, allow_null: true);
                bool n2 = modelstate.Validate(nameof(AdminName), args.AdminName, allow_null: true);
                bool n3 = modelstate.Validate(nameof(MemberName), args.MemberName, allow_null: true);
                args.CorpInfo = CorpInfo.GetCorpInfo(corpname: args.CorpName, err: true);
                //args.SetFilter(nameof(CorpID), args.CorpInfo.ID);
                if (n1) args.AgentData = args.CorpInfo.GetAgentData(args.AgentName, err: true);
                if (n2) args.AdminData = args.CorpInfo.GetAdminData(args.AdminName, err: true);
                if (n3) args.MemberData = args.CorpInfo.GetMemberData(args.MemberName, err: true);
                modelstate.IsValid(true);
            }
            args.Valid(modelstate, valid_user);
            args.sql_builder = new SqlBuilder();
            args.sql_build_op = new List<object>();
            args.sql_build_op.Add(SqlBuilder.op.where);
            if (args.CorpInfo != null)
            {
                args.sql_builder["w", "CorpID"] = args.CorpInfo.ID;
            }
            return args;
        }

        protected virtual void Valid(ModelStateDictionary modelstate, bool valid_user)
        {
            if (this.PageNumber.HasValue && (this.PageNumber.Value < 0))
                this.PageNumber = null;
            if (this.PageSize.HasValue)
            {
                if (this.PageSize.Value < 0) this.PageSize = null;
                if (this.PageSize.Value > 1000) this.PageSize = 1000;
            }
            //this.SortField = TableName<T>.GetSortable()
            //TableName<T>.Attr.
            if (string.IsNullOrEmpty(this.SortField))
                this.SortField = TableName<T>._.SortField;
            else if (!TableName<T>._.IsSortable(this.SortField))
                this.SortField = TableName<T>._.SortField;
            this.SortOrder = this.SortOrder ?? TableName<T>._.SortOrder;
        }

        public void TimeFilter(string field_name)
        {
            if (this.BeginTime.HasValue && this.EndTime.HasValue)
            {
                DateTime t1 = this.BeginTime.Value.ToLocalTime(), t2 = this.EndTime.Value.ToLocalTime();
                if (this.BeginTime.Value >= this.EndTime.Value)
                {
                    t2 = this.BeginTime.Value;
                    t1 = this.EndTime.Value;
                }
                this.sql_builder["BeginTime"] = t1;
                this.sql_builder["EndTime"] = t2;
                this.sql_build_op.Add($" and {field_name} >= {{BeginTime:{SqlBuilder.DateTimeFormat}}} and {field_name} < {{EndTime:{SqlBuilder.DateTimeFormat}}}");
            }
            else if (this.BeginTime.HasValue)
                this.sql_builder["w", field_name, null, ">="] = this.BeginTime;
            else if (this.EndTime.HasValue)
                this.sql_builder["w", field_name, null, "<"] = this.EndTime;
        }
    }
}