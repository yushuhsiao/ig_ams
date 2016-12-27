using ams.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AnnounceApiController : _ApiController
    {
        #region arguments

        public class arguments
        {
            public SqlBuilder.str TableName
            {
                get { return TableName<AnnounceItem>._.TableName; }
            }
            public int? sn;
            public UserName CorpName;
            public string Text;
            public bool? Active;
            [JsonIgnore]
            public int? ActiveValue
            {
                get { if (this.Active.HasValue) return this.Active.Value ? 1 : 0; return null; }
            }
            public int? Order;
            public DateTime? ActiveTime;
            public DateTime? ExpireTime;
        }

        [JsonProperty]
        public int? sn;
        public UserID? CorpID;
        [JsonProperty]
        public UserName CorpName;
        [JsonProperty]
        public string Text;
        [JsonProperty]
        public bool? Active;
        [JsonIgnore]
        public int? ActiveValue
        {
            get { if (this.Active.HasValue) return this.Active.Value ? 1 : 0; return null; }
        }
        [JsonProperty]
        public int? Order;
        [JsonProperty]
        public DateTime? ActiveTime;
        [JsonProperty]
        public DateTime? ExpireTime;

        #endregion

        [HttpPost, Route("~/Events/Announces/list")]
        public ListResponse<AnnounceItem> list(ListRequest<AnnounceItem> args)
        {
            return this.Null(args).Validate(this, true).GetResponse(get_sqlcmd: () => args.CorpInfo.DB_User01R());
        }

        [HttpPost, Route("~/Events/Announces/list_current")]
        public ListResponse<AnnounceItem> list_current(ListRequest<AnnounceItem> args)
        {
            this.Null(args).Validate(this, true);
            //this.Validate(args, true);
            return new ListResponse<AnnounceItem>(new List<AnnounceItem>(
                from row in AnnounceItem.Cache.Value
                where row.CorpID == args.CorpInfo.ID && row.Active == ActiveState.Active
                orderby row.Order ascending, row.CreateTime descending
                select row));
        }

        [HttpPost, Route("~/Events/Announces/add")]
        public AnnounceItem add(arguments args)
        {
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.CorpName), args.CorpName, allow_null: true);
                ModelState.Validate(nameof(args.Text), args.Text);
                ModelState.Validate(nameof(args.Active), args.Active, allow_null: true);
                ModelState.Validate(nameof(args.Order), args.Order, allow_null: true);
                ModelState.Validate(nameof(args.ActiveTime), args.ActiveTime, allow_null: true);
                ModelState.Validate(nameof(args.ExpireTime), args.ExpireTime, allow_null: true);
                if (args.Order < 0) args.Order = null;
            });
            SqlBuilder sql1 = new SqlBuilder();
            CorpInfo corp = CorpInfo.GetCorpInfo(name: args.CorpName, err: true);
            SqlCmd userdb = corp.DB_User01W();
            string tableName = TableName<AnnounceItem>._.TableName;
            sql1[" ", "CorpID       "] = corp.ID;
            sql1["N", "Text         "] = args.Text;
            sql1[" ", "Active       "] = args.ActiveValue;
            sql1[" ", "Order        "] = args.Order;
            sql1[" ", "ActiveTime   "] = args.ActiveTime;
            sql1[" ", "ExpireTime   "] = args.ExpireTime;
            sql1.SetUserID(true, true, true, true);
            string sql2 = $@"insert into {tableName}{sql1._insert()}
select * from {tableName} nolock where sn=@@IDENTITY";
            var ret = userdb.ToObject<AnnounceItem>(true, sql2);
            AnnounceItem.Cache.UpdateVersion();
            return ret;
        }

        [HttpPost, Route("~/Events/Announces/get")]
        public AnnounceItem get(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate("sn", this.sn);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(id: CorpID, name: this.CorpName, err: true);
            return corp.DB_User01R().ToObject<AnnounceItem>($"select * from {TableName<AnnounceItem>.Value} nolock where sn={sn}");
        }

        [HttpPost, Route("~/Events/Announces/set")]
        public AnnounceItem set(arguments args)
        {
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.sn), args.sn);
                ModelState.Validate(nameof(args.Text), args.Text);
                ModelState.Validate(nameof(args.Active), args.ActiveValue, allow_null: true);
                ModelState.Validate(nameof(args.Order), args.Order, allow_null: true);
                ModelState.Validate(nameof(args.ActiveTime), args.ActiveTime, allow_null: true);
                ModelState.Validate(nameof(args.ExpireTime), args.ExpireTime, allow_null: true);
                if (args.Order < 0) args.Order = null;
            });
            SqlBuilder sql1 = new SqlBuilder();
            CorpInfo corp = CorpInfo.GetCorpInfo(name: args.CorpName, err: true);
            SqlCmd userdb = corp.DB_User01W();
            string tableName = TableName<AnnounceItem>._.TableName;
            sql1["Nu", "Text         "] = args.Text;
            sql1[" u", "Active       "] = args.Active == true ? ActiveState.Active : ActiveState.Disabled;
            sql1[" u", "Order        "] = SqlBuilder.str.NullValue(args.Order);
            sql1[" u", "ActiveTime   "] = SqlBuilder.str.NullValue(args.ActiveTime);
            sql1[" u", "ExpireTime   "] = SqlBuilder.str.NullValue(args.ExpireTime);
            sql1[" w", "sn           "] = SqlBuilder.str.NullValue(args.sn);
            sql1.SetModifyTime("u");
            sql1.SetModifyUser("u");
            string sql2 = $@"update {tableName}{sql1._update_set()}{sql1._where()}
select * from {tableName} nolock{sql1._where()}";
            var ret = userdb.ToObject<AnnounceItem>(true, sql2);
            AnnounceItem.Cache.UpdateVersion();
            return ret;
        }
    }
}
namespace ams.Controllers
{
    using System.Web.Mvc;

    public class AnnounceController : _Controller
    {
        [HttpGet, Route("~/Events/Announces"), Acl(typeof(AnnounceApiController), "get")]
        public ActionResult Announces() => View("~/Views/Events/Announces.cshtml");

        [HttpGet, Route("~/Events/Announces" + url_Details), Acl(typeof(AnnounceApiController), "get")]
        public ActionResult AnnouncesDetails() => View_Details(Announces);
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [TableName("Announce")]
    public class AnnounceItem
    {
        public static RedisVer<List<AnnounceItem>> Cache = new RedisVer<List<AnnounceItem>>("Announce") { ReadData = ReadData };

        static List<AnnounceItem> ReadData(SqlCmd sqlcmd, int index)
        {
            List<AnnounceItem> cache = new List<AnnounceItem>();
            var n = CorpInfo.Cache.Value;
            foreach (CorpInfo c in n)
                foreach (SqlDataReader r in c.DB_User01R().ExecuteReaderEach($"select * from {TableName<AnnounceItem>._.TableName} nolock where CorpID={c.ID}"))
                    cache.Add(r.ToObject<AnnounceItem>());
            return cache;
        }

        [DbImport, JsonProperty]
        public long sn;
        [DbImport, JsonProperty]
        public SqlTimeStamp ver;
        [DbImport, JsonProperty, Sortable]
        public UserID CorpID;
        [DbImport, JsonProperty, Sortable, Filterable]
        public string CorpName
        {
            get { return CorpInfo.GetCorpInfo(this.CorpID).UserName; }
        }
        [DbImport, JsonProperty]
        public string Text;
        [DbImport, JsonProperty]
        public ActiveState Active;
        //[JsonProperty("Active")]
        //public bool _Active
        //{
        //    get { return this.Active == ActiveState.Active; }
        //}
        [DbImport, JsonProperty, Sortable]
        public int? Order;
        [DbImport, JsonProperty, Sortable]
        public DateTime? ActiveTime;
        [DbImport, JsonProperty, Sortable]
        public DateTime? ExpireTime;
        [DbImport, JsonProperty, Sortable]
        public DateTime CreateTime;
        [DbImport, JsonProperty, Sortable]
        public UserID CreateUser;
        [DbImport, JsonProperty, Sortable]
        public DateTime ModifyTime;
        [DbImport, JsonProperty, Sortable]
        public UserID ModifyUser;
    }
}