using ams;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Configuration
{
    [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Property)]
    public class SqlSettingAttribute : DataBaseSettingAttribute
    {
        public int CorpID { get; set; }
        //public int PlatformID { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public SqlSettingAttribute() { }
        public SqlSettingAttribute(string key2) : this(null, key2) { }
        public SqlSettingAttribute(string key1, string key2) { this.Key1 = key1; this.Key2 = key2; }

        protected override bool GetValue(MemberInfo m, out string result, params object[] index)
        {
            int platformID = 0;
            int corpID = this.CorpID;
            string key2 = this.Key2 ?? m.Name;
            var n = SqlConfig.Cache.Value;
            for (int n1 = 0, n2 = index.Length; n1 < n2; n1++)
            {
                if (index[n1] is ams.PlatformID)
                    platformID = ((ams.PlatformID)index[n1]).ID;
                else if (index[n1] is ams.UserID)
                    corpID = ((ams.UserID)index[n1]).ID;
            }
            if (n.GetSetting(corpID, platformID, this.Key1, key2, out result))
                return true;
            if (platformID == 0) return false;
            return n.GetSetting(corpID, 0, this.Key1, key2, out result);
        }
        protected override void SetValue(MemberInfo m, string value)
        {
            throw new NotImplementedException();
        }
    }
}

namespace ams
{
    public class SqlConfig : List<SqlConfig.Row>
    {
        [TableName("Config")]
        public sealed class Row
        {
            [DbImport]
            public int CorpID;
            [DbImport]
            public int PlatformID;
            [DbImport]
            public string Key1;
            [DbImport]
            public string Key2;
            [DbImport]
            public string Value;
            [DbImport]
            public string Description;

            string _Key1() => SqlCmd.magic_quote(Key1);
            string _Key2() => SqlCmd.magic_quote(Key2);

            public bool ReadRow(SqlCmd coredb)
            {
                coredb = coredb ?? _HttpContext.GetSqlCmd(DB.Core01R);
                foreach (SqlDataReader r in coredb.ExecuteReaderEach($"select * from {TableName<Row>.Value} nolock where CorpID={CorpID} and PlatformID={PlatformID} and Key1={_Key1()} and Key2={_Key2()}"))
                {
                    r.FillObject(this);
                    return true;
                }
                return false;
            }
            public string sql_SaveRow(bool reload = true)
            {
                SqlBuilder sql = new SqlBuilder();
                sql[" w", "CorpID     "] = this.CorpID;
                sql[" w", "PlatformID "] = this.PlatformID;
                sql[" w", "Key1       "] = this.Key1 ?? "";
                sql[" w", "Key2       "] = this.Key2 ?? "";
                sql[" u", "Value      "] = this.Value;
                sql["Nu", "Description"] = this.Description;
                string sql_where = sql._where();
                string sql_reload; if (reload) sql_reload = $@"
select * from {TableName<Row>.Value} nolock where ID=@ID"; else sql_reload = null;
                return $@"declare @ID uniqueidentifier select @ID=ID from {TableName<Row>.Value} nolock{sql_where}
if @ID is not null update {TableName<Row>.Value}{sql._update_set()} where ID=@ID
else begin set @ID=newid() insert into {TableName<Row>.Value} ([ID],{sql._fields()}) values (@ID,{sql._values()}) end{sql_reload}";
            }
            public Row SaveRow(SqlCmd coredb, bool reload = true)
            {

                (coredb ?? _HttpContext.GetSqlCmd(DB.Core01W)).FillObject(this, true, sql_SaveRow(reload));
                return this;
            }
        }

        SqlConfig(IEnumerable<Row> collection) : base(collection) { }

        public static readonly RedisVer<SqlConfig> Cache = new RedisVer<SqlConfig>("Config")
        {
            ReadData = (sqlcmd, index) => new SqlConfig(sqlcmd.ToList<Row>($"select * from {TableName<Row>.Value} nolock"))
        };

        public bool GetSetting(int corpID, int platformID, string key1, string key2, out string value)
        {
            value = null;
            foreach (string s in
                from n1 in this
                where (n1.CorpID == corpID) && (n1.PlatformID == platformID) && (n1.Key1 == key1) && (n1.Key2 == key2)
                select n1.Value)
            {
                value = s;
                if (value != null)
                    return true;
            }
            value = null;
            return false;
        }

        public string GetSetting(int corpID, int platformID, string key1, string key2)
        {
            string value; GetSetting(corpID, platformID, key1, key2, out value); return value;
        }
    }

    //[_DebuggerStepThrough, RedisVer("Config")]
    //public class SqlConfig : RedisVerList<SqlConfig, SqlConfig.Row>
    //{
    //    public class Row
    //    {
    //        [DbImport]
    //        public virtual int CorpID
    //        {
    //            get; set;
    //        }
    //        [DbImport]
    //        public virtual int PlatformID
    //        {
    //            get; set;
    //        }
    //        [DbImport]
    //        public virtual string Key1
    //        {
    //            get; set;
    //        }
    //        [DbImport]
    //        public virtual string Key2
    //        {
    //            get; set;
    //        }
    //        [DbImport]
    //        public virtual string Value
    //        {
    //            get; set;
    //        }
    //    }

    //    protected override IEnumerable<Row> ReadItems(SqlCmd sqlcmd, int index)
    //    {
    //        foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach("select * from Config nolock"))
    //            yield return r.ToObject<Row>();
    //    }

    //    public bool GetSetting(int corpID, int platformID, string key1, string key2, out string value)
    //    {
    //        value = null;
    //        foreach (string s in
    //            from n1 in this.Value
    //            where (n1.CorpID == corpID) && (n1.PlatformID == platformID) && (n1.Key1 == key1) && (n1.Key2 == key2)
    //            select n1.Value)
    //        {
    //            value = s;
    //            if (value != null)
    //                return true;
    //        }
    //        value = null;
    //        return false;
    //    }

    //    public string GetSetting(int corpID, int platformID, string key1, string key2)
    //    {
    //        string value; GetSetting(corpID, platformID, key1, key2, out value); return value;
    //    }
    //}
}