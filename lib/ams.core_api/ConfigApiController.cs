using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ConfigApiController : _ApiController
    {
        [HttpPost, Route("~/sys/config/list")]
        public void list(_empty args)
        {
            this.Validate(true, args, null);
        }

        [JsonProperty]
        UserName? CorpName;

        [JsonProperty]
        UserName? PlatformName;

        [JsonProperty]
        List<SqlConfig.Row> Rows;

        void _platform_op(_empty args, bool rows, out CorpInfo c, out PlatformInfo p)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate(nameof(PlatformName), PlatformName, allow_null: true);
                if (rows && Rows == null) ModelState.AddModelError(nameof(Rows), Status.InvalidParameter);
            });
            if (CorpName.HasValue)
                c = CorpInfo.GetCorpInfo(CorpName.Value);
            else
                c = null;
            if (PlatformName.HasValue)
                p = PlatformInfo.GetPlatformInfo(PlatformName.Value, err: true);
            else
                p = PlatformInfo.GetPlatformInfo(0);
        }

        [HttpPost, Route("~/sys/config/platform_get")]
        public List<SqlConfig.Row> platform_get(_empty args)
        {
            CorpInfo c; PlatformInfo p;
            _platform_op(args, false, out c, out p);
            string sql = $"select * from {TableName<SqlConfig.Row>.Value} nolock where CorpID={c?.ID ?? 0} and PlatformID={p.ID}";
            var n1 = p.GetDefaultConfigSettings();
            var n2 = new List<SqlConfig.Row>();
            foreach (var r in System.Web._HttpContext.GetSqlCmd(DB.Core01R).ExecuteReaderEach(sql))
            {
                SqlConfig.Row row = r.ToObject<SqlConfig.Row>();
                for (int i = 0; i < n1.Count; i++)
                {
                    if ((n1[i].Key1 == row.Key1) && (n1[i].Key2 == row.Key2))
                    {
                        n1.RemoveAt(i);
                        n1.Insert(i, row);
                        break;
                    }
                }
            }
            return n1;
            //return _HttpContext.GetSqlCmd(DB.Core01R).ToList<SqlConfig.Row>(sql) ?? _null<SqlConfig.Row>.list;
        }

        [HttpPost, Route("~/sys/config/platform_set")]
        public List<SqlConfig.Row> platform_set(_empty args)
        {
            CorpInfo c; PlatformInfo p;
            _platform_op(args, false, out c, out p);
            StringBuilder sql1 = new StringBuilder();
            SqlBuilder sql2 = new SqlBuilder();
            var n1 = p.GetDefaultConfigSettings();
            foreach (var row in this.Rows)
            {
                if (string.IsNullOrEmpty(row.Key1) ||
                    string.IsNullOrEmpty(row.Key2) ||
                    string.IsNullOrEmpty(row.Value))
                    continue;
                SqlConfig.Row row2 = n1.Find((_row2) =>
                row.CorpID == _row2.CorpID &&
                row.PlatformID == _row2.PlatformID &&
                row.Key1 == _row2.Key1 &&
                row.Key2 == _row2.Key2);
                if (row2 == null) continue;
                sql2[" w", "CorpID    "] = c?.ID ?? 0;
                sql2[" w", "PlatformID"] = p.ID;
                sql2[" w", "Key1      "] = row.Key1;
                sql2[" w", "Key2      "] = row.Key2;
                sql2["nu", "Value     "] = row.Value;
                sql1.Append($@"if exists (select ID from {TableName<SqlConfig.Row>.Value}{sql2._where()})
     update {TableName<SqlConfig.Row>.Value} {sql2._update_set()}{sql2._where()}
else insert into {TableName<SqlConfig.Row>.Value}{sql2._insert()}");
            }
            sql1.Append($"select * from {TableName<SqlConfig.Row>.Value} nolock where CorpID={c?.ID ?? 0} and PlatformID={p.ID}");
            SqlCmd sqlcmd = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit in sqlcmd.BeginTran())
            {
                var ret = sqlcmd.ToList<SqlConfig.Row>(sql1.ToString());
                SqlConfig.Cache.UpdateVersion(sqlcmd);
                commit();
                return ret;
            }
            return null;
        }



        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class arguments
        {
            [JsonProperty]
            public UserID? CorpID;
            [JsonProperty]
            public int? PlatformID;
            [JsonProperty]
            public string Key1;
            [JsonProperty]
            public string Key2;
            [JsonProperty]
            public string Value;
            [JsonProperty]
            public string Description;
        }

        [HttpPost, Route("~/sys/config/get")]
        public SqlConfig.Row get(arguments args)
        {
            this.Validate(true, args, ()=>
            {
                ModelState.Validate(nameof(arguments.Key1), args.Key1 = args.Key1.Trim(true));
                ModelState.Validate(nameof(arguments.Key2), args.Key2 = args.Key2.Trim(true));
            });
            var row = new SqlConfig.Row() { CorpID = args.CorpID ?? 0, PlatformID = args.PlatformID ?? 0, Key1 = args.Key1, Key2 = args.Key2 };
            if (row.ReadRow(null))
                return row;
            return null;
        }

        [HttpPost, Route("~/sys/config/set")]
        public SqlConfig.Row set(arguments args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(arguments.CorpID), args.CorpID);
                ModelState.Validate(nameof(arguments.PlatformID), args.PlatformID);
                ModelState.Validate(nameof(arguments.Key1), args.Key1 = args.Key1.Trim(true));
                ModelState.Validate(nameof(arguments.Key2), args.Key2 = args.Key2.Trim(true));
            });
            var row = new SqlConfig.Row() { CorpID = args.CorpID ?? 0, PlatformID = args.PlatformID ?? 0, Key1 = args.Key1, Key2 = args.Key2 };
            row.SaveRow(null, true);
            return row;
            //return this.setall(null, args).FirstOrDefault();
        }

//        [HttpPost, Route("~/sys/config/setall")]
//        public SqlConfig.Row[] setall(params args[] args)
//        {
//            StringBuilder sql1 = new StringBuilder();
//            SqlBuilder sql2 = new SqlBuilder();
//            sql2["TableName"] = (SqlBuilder.str)"Config";
//            foreach (args row in args)
//            {
//                sql2["  ", "ID         "] = SqlBuilder.str.newid;
//                sql2[" w", "CorpID     "] = row.CorpID;
//                sql2[" w", "PlatformID "] = row.PlatformID;
//                sql2[" w", "Key1       "] = row.Key1 ?? "";
//                sql2[" w", "Key2       "] = row.Key2 ?? "";
//                sql2[" u", "Value      "] = row.Value;
//                sql2["Nu", "Description"] = row.Description;
//                sql1.AppendLine(sql2.Build("if exists (select ID from {TableName} nolock", SqlBuilder.op.where, @")
//     update {TableName} ", SqlBuilder.op.update_set, SqlBuilder.op.where, @"
//else insert into {TableName} ", SqlBuilder.op.insert, @"
//select * from {TableName} nolock", SqlBuilder.op.where));
//            }
//            string sqlstr = sql1.ToString();
//            SqlCmd core01w = _HttpContext.GetSqlCmd(DB.Core01W);
//            List<SqlConfig.Row> result = null;
//            bool transaction = core01w.Transaction == null;
//            try
//            {
//                if (transaction) core01w.BeginTransaction();
//                result = core01w.ToList<SqlConfig.Row>(sqlstr);
//                if (result.Count > 0)
//                    SqlConfig.Cache.UpdateVersion(core01w);
//                if (transaction) core01w.Commit();
//            }
//            catch
//            {
//                if (transaction) core01w.Rollback();
//                throw;
//            }
//            return result.ToArray();
//        }
    }
}
