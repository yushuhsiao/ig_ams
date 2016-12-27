using ams.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

// 調閱申訴
namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AppealApiController : _ApiController
    {
        [HttpPost, Route("~/Users/Member/Appeal/list")]
        public ListResponse<AppealData1> list(ListRequest<AppealData1> args) => this.Null(args).Validate(this, true).GetResponse(onBuild: (string name, FilterableAttribute item) =>
        {
            switch (name)
            {
                case nameof(AppealData1.RequestTime): args.AddFilter_TimeRange(name, item); break;
                case nameof(AppealData1.PlatformID):
                case nameof(AppealData1.PlatformName): args.AddFilter_PlatformNames(nameof(AppealData1.PlatformID), item); break;
                case nameof(AppealData1.GameName): args.AddFilter_GameIDs(nameof(AppealData1.GameID), item); break;
                case nameof(AppealData1.UserName): args.AddFilter_StringContains(name, item); break;
                case nameof(AppealData1.State): args.AddFilter_Enum<AppealState>(name, item); break;
            }
        });

        #region arguments

        [JsonProperty]
        public UserName CorpName;

        [JsonProperty]
        public UserName UserName;

        [JsonProperty]
        public UserName PlatformName;

        [JsonProperty]
        public string GameName;

        [JsonProperty]
        public long? GroupID;

        [JsonProperty]
        public string Text;

        #endregion

        // 提交申請
        [HttpPost, Route("~/Users/Member/Appeal/appeal")]
        public AppealData1 appeal(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate(nameof(UserName), UserName);
                ModelState.Validate(nameof(PlatformName), PlatformName);
                ModelState.Validate(nameof(GameName), GameName);
                ModelState.Validate(nameof(GroupID), GroupID);
                ModelState.Validate(nameof(Text), Text);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(name: this.CorpName, err: true);
            MemberData m = corp.GetMemberData(name: this.UserName, err: true);
            PlatformInfo p = PlatformInfo.GetPlatformInfo(this.PlatformName, err: true);
            GameInfo g = GameInfo.GetGameInfo(this.GameName, err: true);
            GameLog[] ll = GameLog.GetGameLog(corp, p, g, this.GroupID.Value);
            if (ll.Length == 0)
                throw new _Exception(Status.Appeal_GameLogNotFound);
            GameLog l = (from _l in ll where _l.CorpID == corp.ID && _l.UserID == m.ID && _l.PlatformID == p.ID && _l.GameID == g.ID select _l).First();
            if (l == null)
                throw new _Exception(Status.Appeal_GameLogNotContainsUser);
            SqlBuilder sql = new SqlBuilder();
            sql[" ", nameof(AppealData1.ID), "           "] = (SqlBuilder.str)"@ID";
            sql[" ", nameof(AppealData1.SerialNumber), " "] = (SqlBuilder.str)"@sn";
            sql[" ", nameof(AppealData1.CorpID), "       "] = corp.ID;
            sql["n", nameof(AppealData1.CorpName), "     "] = corp.UserName;
            sql[" ", nameof(AppealData1.UserID), "       "] = m.ID;
            sql["n", nameof(AppealData1.UserName), "     "] = m.UserName;
            sql[" ", nameof(AppealData1.PlatformID), "   "] = p.ID;
            sql["n", nameof(AppealData1.PlatformName), " "] = p.PlatformName;
            sql[" ", nameof(AppealData1.GameID), "       "] = g.ID;
            sql["n", nameof(AppealData1.GameName), "     "] = g.Name;
            sql[" ", nameof(AppealData1.GroupID), "      "] = l.GroupID;
            sql[" ", nameof(AppealData1.State), "        "] = AppealState.New;
            sql[" ", nameof(AppealData1.RequestTime), "  "] = SqlBuilder.str.getdate;
            return corp.DB_User01W().ToObject<AppealData1>(true, $@"declare @sn varchar(16), @ID uniqueidentifier exec alloc_TranID @prefix='0', @group=2, @len=8, @sn=@sn output, @ID=@ID output
insert into {TableName<AppealData1>.Value}{sql._insert()}
insert into {TableName<AppealData2>.Value} (AppealID, Text, CreateTime, CreateUser) values (@ID, N'{SqlCmd.magic_quote(Text)}', getdate(), {m.ID})
select * from {TableName<AppealData1>.Value} nolock where ID=@ID");
        }

        // 變更狀態
        [HttpPost, Route("~/Users/Member/Appeal/SetState")]
        public void SetState()
        {
        }

        // 客服回覆
        [HttpPost, Route("~/Users/Member/Appeal/reply")]
        public void reply()
        {
        }

        // 裁決
        [HttpPost, Route("~/Users/Member/Appeal/adjudicate")]
        public void adjudicate()
        {
        }

        // 調閱
        [HttpPost, Route("~/Users/Member/Appeal/GameReplay")]
        public void GameReplay()
        {
        }
    }
}
namespace ams.Data
{
    public enum AppealDataType : byte
    {
        /// <summary>
        /// 玩家提交
        /// </summary>
        Request,
        /// <summary>
        /// 申訴中心回應
        /// </summary>
        Response,
        /// <summary>
        /// 裁決結果
        /// </summary>
        Result,
    }
    public enum AppealState : byte
    {
        /// <summary>
        /// 新申請
        /// </summary>
        New,
        /// <summary>
        /// 已受理
        /// </summary>
        Accepted,
        /// <summary>
        /// 申訴駁回
        /// </summary>
        Rejected,
        /// <summary>
        /// 
        /// </summary>
        Finished,
    }
    [TableName("Appeal1", SortField = nameof(RequestTime))]
    public class AppealData1
    {
        [DbImport, JsonProperty]
        public Guid ID;
        [DbImport, JsonProperty, Filterable]
        public string SerialNumber;

        [DbImport, JsonProperty, Filterable]
        public UserID CorpID;
        [DbImport, JsonProperty, Filterable]
        public UserName CorpName;
        [DbImport, JsonProperty, Filterable]
        public UserID UserID;
        [DbImport, JsonProperty, Filterable]
        public UserName UserName;

        // 要申訴的遊戲局
        [DbImport, JsonProperty, Filterable]
        public int PlatformID;
        [DbImport, JsonProperty, Filterable]
        public UserName PlatformName;
        [DbImport, JsonProperty, Filterable]
        public int GameID;
        [DbImport, JsonProperty, Filterable]
        public string GameName;
        [DbImport, JsonProperty, Filterable]
        public long GroupID;
        //[DbImport]
        //public string SerialNumber;

        [DbImport, JsonProperty, Filterable]
        public AppealState State;

        [DbImport, JsonProperty, Filterable]
        public DateTime RequestTime;
    }

    [TableName("Appeal2", SortField = nameof(CreateTime))]
    public class AppealData2
    {
        [DbImport, JsonProperty]
        public long sn;
        [DbImport, JsonProperty]
        public Guid AppealID;
        [DbImport, JsonProperty]
        public string Text;
        [DbImport, JsonProperty]
        public DateTime CreateTime;
        [DbImport, JsonProperty]
        public DateTime CreateUser;
    }
    [TableName("Appeal2", SortField = nameof(CreateTime))]
    public class AppealText
    {
        [DbImport]
        public long sn;
        [DbImport]
        public Guid AppealID;
        [DbImport]
        public AppealDataType DataType;
        [DbImport]
        public string Text;
        [DbImport]
        public DateTime CreateTime;
        [DbImport]
        public UserID CreateUser;
    }
}