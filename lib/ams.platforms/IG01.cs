using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ams.Data
{
    [PlatformInfo(PlatformType = PlatformType.InnateGloryA)]
    public partial class IG01PlatformInfo : PlatformInfo<IG01PlatformInfo, IG01MemberPlatformData>
    {
        const int err_retry = 3;
        const string Key1 = "GeniusBull";
        //public string ApiUrl() => GetConfig(0, this.ID, Key1, "ApiUrl");
        public SqlCmd GameDB() => _HttpContext.GetSqlCmd(this.ApiUrl);
        //public string LobbyUrl() => GetConfig(0, this.ID, Key1, "LobbyUrl");

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "ApiUrl")]
        public string ApiUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "LobbyUrl")]
        public string LobbyUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "EnrtyLobby")]
        public GeniusBull.EntryLobby EnrtyLobby
        {
            get { return app.config.GetValue<GeniusBull.EntryLobby>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "NoticeApi"), DefaultValue("http://hub.betis73168.com:880/Api/Notice")]
        public string NoticeApi
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "AssetServerUrl"), DefaultValue("http://asset.betis73168.com:81")]
        public string AssetServerUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "RecognitionApiUrl1"), DefaultValue("http://gs1.betis73168.com:9080/recognitionservice/rest")]
        public string RecognitionApiUrl1
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "RecognitionApiUrl2"), DefaultValue("http://192.168.5.32:9080/recognitionservice/rest")]
        public string RecognitionApiUrl2
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "SlotServerRest"), DefaultValue("http://192.168.5.32:14000/")]
        public string SlotServerRest
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "MjServerRest"), DefaultValue("http://192.168.5.32:8078/twmjserver/rest")]
        public string MjServerRest
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "TexasHoldemRest"), DefaultValue("http://192.168.5.32:9080/texesholdemserver-zero/services")]
        public string TexasHoldemRest
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "DoudizhuRest"), DefaultValue("http://192.168.5.32:9080/fightthelandlordserver/rest")]
        public string DoudizhuRest
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "VideoReplay"), DefaultValue("ftp://viewer@218.32.1.235:21")]
        public string VideoReplay
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "MahjongReplay"), DefaultValue("http://game1.betis73168.com:880/Replay/TaiwanMahjong?serialNumber={SerialNumber}")]
        public string MahjongReplay
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "DoudizhuReplay"), DefaultValue("http://game1.betis73168.com:880/Replay/DouDizhu?id={ID}")]
        public string DoudizhuReplay
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "TexasHoldemReplay"), DefaultValue("http://game1.betis73168.com:880/Replay/TexasHoldem?id={ID}")]
        public string TexasHoldemReplay
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }
        
        public static IG01PlatformInfo GetImageInstance()
        {
            foreach (PlatformInfo p1 in PlatformInfo.Cache.Value)
            {
                if (p1 is IG01PlatformInfo)
                {
                    IG01PlatformInfo p2 = (IG01PlatformInfo)p1;
                    if (p2.RecognitionApiUrl1 != null)
                        return p2;
                }
            }
            return null;
        }

        public HttpStatusCode InvokeRecogApi(string url, out string response_text, string method = "GET")
        {
            string api_url = $"{this.RecognitionApiUrl2}{url}";
            DateTime t1 = DateTime.Now;
            HttpWebRequest request = WebRequest.Create(api_url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "text/plain";
            HttpWebResponse response = null;
            log.message("recog", api_url);
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex) { response = (HttpWebResponse)ex.Response; }
            using (response)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    response_text = sr.ReadToEnd();
                TimeSpan t2 = DateTime.Now - t1;
                log.message("recog", $@"{response.StatusCode}, {t2.TotalMilliseconds}ms, {response_text}");
                return response.StatusCode;
            }
        }

        protected override IG01MemberPlatformData CreateMember(MemberData member)
        {
            SqlBuilder sql0 = new SqlBuilder();
            sql0["TableName         "] = (SqlBuilder.str)TableName<IG01MemberPlatformData>.Value;
            sql0["w", "MemberID     "] = member.ID;
            sql0["w", "PlatformID   "] = this.ID;
            sql0["w", "n            "] = 0;
            sql0[" ", "Active       "] = MemberPlatformActiveState.Init;
            sql0["State_Delete      "] = MemberPlatformActiveState.Delete;
            for (int i = 0; ; i++)
            {
                string account = this.AllocAccountName(member);
                sql0[" ", "Account"] = account;

                SqlCmd sqlcmd = member.CorpInfo.DB_User01W();
                IG01MemberPlatformData m1 = sqlcmd.ToObject<IG01MemberPlatformData>(
                    () => new IG01MemberPlatformData() { Member = member },
                    true, sql0.Build(@"update {TableName} set n=n+1 where MemberID={MemberID} and PlatformID={PlatformID} and Active={State_Delete}
insert into {TableName}", SqlBuilder.op.insert, @"
select * from {TableName} nolock", SqlBuilder.op.where));
                try
                {
                    GeniusBull.Member m2 = api_CreateUser(m1, member);
                    if (m2 == null)
                    {
                        if (i < 10)
                            sqlcmd.ExecuteNonQuery(true, sql0.Build("delete from {TableName}", SqlBuilder.op.where));
                        else
                            throw new _Exception(Status.PlatformApiFailed);
                    }
                    else
                    {
                        sql0["u", "Active"] = MemberPlatformActiveState.Active;
                        sql0["u", "destID"] = m2.Id;
                        return sqlcmd.ToObject<IG01MemberPlatformData>(true, sql0.Build("update {TableName}", SqlBuilder.op.update_set, SqlBuilder.op.where, @"
select * from {TableName} nolock", SqlBuilder.op.where));
                    }
                }
                catch
                {
                    sqlcmd.ExecuteNonQuery(true, sql0.Build("delete from {TableName}", SqlBuilder.op.where));
                    throw;
                }
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class _ForwardGameModel : ForwardGameArguments
        {
            [JsonProperty]
            public string Lang;
            [JsonProperty]
            public GeniusBull.EntryLobby? Lobby;
        }

        internal override ForwardGameArguments ForwardGame(_ApiController controller, ForwardGameArguments _args)
        {
            _ForwardGameModel args = _args.src.ToObject<_ForwardGameModel>();
            CorpInfo corp = CorpInfo.GetCorpInfo(_User.Current.CorpID);
            controller.ModelState.Validate("MemberName", args.MemberName);
            controller.ModelState.IsValid();
            if (this.Active == PlatformActiveState.Disabled)
                new HttpResponseException(HttpStatusCode.Forbidden);
            if (this.Active == PlatformActiveState.Maintenance)
                throw new _Exception(Status.PlatformMaintenance);
            if (corp.Active != ActiveState.Active)
                new HttpResponseException(HttpStatusCode.Forbidden);

            MemberData member = corp.GetMemberData(args.MemberName);
            if (member == null)
                throw new _Exception(Status.MemberNotExist);

            args.Url = this.LobbyUrl;// GeniusBull._Config.LobbyUrl(this.ID);

            GeniusBull.Member m2 = api_SetToken(member, null, args);
            args.Lobby = this.EnrtyLobby;// s1.Length > 10 ? GeniusBull.EntryLobby.VideoArcade : GeniusBull.EntryLobby.TabletopGames;

            args.ForwardType = ForwardType.Url;
            args.Url = args.Url.formatWith(m2);
            if (args.Lobby.HasValue)
                args.Url = $"{args.Url}&Lobby={args.Lobby}";
            if (!string.IsNullOrEmpty(args.Lang))
                args.Url = $"{args.Url}&Lang={args.Lang}";
            return args;


            //for (int retry = 0; ; retry++)
            //{
            //    IG01MemberPlatformData m1 = this.GetMember(member, true);
            //    if (m1 == null) throw new _Exception(Status.PlatformApiFailed);
            //    GeniusBull.Member m2;
            //    try
            //    {
            //        m2 = api_SetToken(m1, member, null, args);
            //        args.Lobby = this.EnrtyLobby;// s1.Length > 10 ? GeniusBull.EntryLobby.VideoArcade : GeniusBull.EntryLobby.TabletopGames;

            //        args.ForwardType = ForwardType.Url;
            //        args.Url = args.Url.formatWith(m2);
            //        if (args.Lobby.HasValue)
            //            args.Url = $"{args.Url}&Lobby={args.Lobby}";
            //        if (!string.IsNullOrEmpty(args.Lang))
            //            args.Url = $"{args.Url}&Lang={args.Lang}";
            //        return args;
            //    }
            //    catch (_Exception ex) when (ex.StatusCode == Status.PlatformUserNotExist) { _retry(retry, member, m1); }
            //}
        }

        internal override bool GetBalance(MemberData member, out decimal balance)
        {
            IG01MemberPlatformData m1 = this.GetMember(member, false);
            if (m1 == null) balance = 0;
            else return api_GetBalance(m1, member, out balance);
            return true;
        }

        void _retry(int retry, MemberData m1, IG01MemberPlatformData m2)
        {
            if (retry >= err_retry) throw new _Exception(Status.PlatformApiFailed);
            this.DeleteMember(m1);
            this.UnAllocAccountName(m2.Account);
        }

        internal override bool Deposit(MemberData member, decimal amount, out decimal balance)
        {
            for (int retry = 0; ; retry++)
            {
                IG01MemberPlatformData m1 = this.GetMember(member, true);
                if (m1 == null) throw new _Exception(Status.PlatformApiFailed);
                try { return api_UpdateBalance(m1, member, amount, out balance); }
                catch (_Exception ex) when (ex.StatusCode == Status.PlatformUserNotExist) { _retry(retry, member, m1); }
            }
        }

        internal override bool Withdrawal(MemberData member, decimal amount, out decimal balance)
        {
            for (int retry = 0; ; retry++)
            {
                IG01MemberPlatformData m1 = this.GetMember(member, false);
                if (m1 == null) return _null.noop(false, out balance);
                try { return api_UpdateBalance(m1, member, -amount, out balance); }
                catch (_Exception ex) when (ex.StatusCode == Status.PlatformUserNotExist) { _retry(retry, member, m1); }
            }
        }

        // use for LogService
        public IG01MemberPlatformData GetMemberByDestID(SqlCmd userDB, int destID)
        {
            return userDB.ToObject<IG01MemberPlatformData>($"select * from {TableName<IG01MemberPlatformData>.Value} nolock where destID={destID} and PlatformID={this.ID}");
        }

        public IG01MemberPlatformData GetMemberByDestID(int destID, bool getMemberData = false, bool getMemberData_err=false)
        {
            var corps = CorpInfo.Cache.Value;
            foreach (CorpInfo corp1 in corps)
            {
                if (corp1.ID.IsRoot) continue;
                SqlCmd userDB1 = corp1.DB_User01R();
                var m2 = this.GetMemberByDestID(userDB1, destID);
                if (m2 != null)
                {
                    if (getMemberData)
                    {
                        foreach (CorpInfo corp2 in corps)
                        {
                            if (corp2.ID.IsRoot) continue;
                            SqlCmd userDB2 = corp2.DB_User01R();
                            m2.Member = corp2.GetMemberData(m2.MemberID, userDB2, getMemberData_err);
                            if (m2.Member != null)
                                break;
                        }
                    }
                    return m2;
                }
            }
            return null;
        }




        readonly object _games_sync = new object();
        RedisVer<List<GeniusBull.Game>> _games;
        public GeniusBull.Game GetGame(int gameID)
        {
            RedisVer<List<GeniusBull.Game>> _games;
            lock (_games_sync)
            {
                if (this._games == null)
                    this._games = new RedisVer<List<GeniusBull.Game>>("GeniusBull_Game", index: base.ID) { ReadData = ReadGames };
                _games = this._games;
            }
            return _games.Value.Find((g) => g.Id == gameID);
        }

        List<GeniusBull.Game> ReadGames(SqlCmd sqlcmd, int index)
        {
            IG01PlatformInfo p = IG01PlatformInfo.GetPlatformInfo<IG01PlatformInfo>(index);
            SqlCmd gamedb;
            using (_HttpContext.GetSqlCmd(out gamedb, null, p.ApiUrl))
                return gamedb.ToList(
                    () => new GeniusBull.Game(p, gamedb.DataReader.GetInt32(nameof(GeniusBull.Game.Id))),
                    $"select * from {TableName<GeniusBull.Game>.Value} nolock")
                    ?? new List<GeniusBull.Game>();
        }



        public GeniusBull.Member api_GetUser(string accessToken)
        {
            return this.GameDB().ToObject<GeniusBull.Member>($"select * from {TableName<GeniusBull.Member>.Value} nolock where AccessToken='{SqlCmd.magic_quote(accessToken)}'");
        }

        public GeniusBull.Member api_GetUser(MemberData member)
        {
            return api_GetUser(this.GetMember(member));
        }

        public GeniusBull.Member api_GetUser(IG01MemberPlatformData p_member)
        {
            if (p_member == null) return null;
            return this.GameDB().ToObject<GeniusBull.Member>($"select * from {TableName<GeniusBull.Member>.Value} nolock where Id={p_member.destID}");
        }

        public GeniusBull.Member api_SetToken(MemberData member, string accessToken, ForwardGameArguments args, bool forceSetToken = true)
        {
            for (int retry = 0; ; retry++)
            {
                IG01MemberPlatformData m1 = this.GetMember(member, true);
                if (m1 == null) throw new _Exception(Status.PlatformApiFailed);

                SqlBuilder sql0 = new SqlBuilder();
                sql0[" w", "Id           "] = m1.destID;
                if (args != null)
                {
                    sql0[" u", "LastLoginIp  "] = args.RequestIP.Trim(true) ?? _HttpContext.Current.RequestIP;
                    sql0[" u", "LastLoginTime"] = SqlBuilder.str.getdate;
                    args.NickName = args.NickName.Trim(true);
                    if (args.NickName != null)
                        sql0["Nu", "Nickname     "] = args.NickName ?? member.NickName;
                }
                string _force = null;
                if (forceSetToken == false)
                    _force = " and AccessToken is null";
                sql0[" u", "AccessToken  "] = accessToken ?? Guid.NewGuid().ToString("N");
                string sql = $@"update {TableName<GeniusBull.Member>.Value}{sql0._update_set()} where Id={m1.destID}{_force}
select Id, Account, AccessToken from {TableName<GeniusBull.Member>.Value} nolock where Id={m1.destID}";
                GeniusBull.Member m2 = this.GameDB().ToObject<GeniusBull.Member>(true, sql);
                if (m2 == null)
                    _retry(retry, member, m1);
                else
                    return m2;
            }
        }

//        public GeniusBull.Member api_SetToken(IG01MemberPlatformData p_member, MemberData member, string accessToken, ForwardGameArguments args)
//        {
//            SqlBuilder sql0 = new SqlBuilder();
//            sql0[" w", "Id           "] = p_member.destID;
//            if (args != null)
//            {
//                sql0[" u", "LastLoginIp  "] = args.RequestIP.Trim(true) ?? _HttpContext.Current.RequestIP;
//                sql0[" u", "LastLoginTime"] = SqlBuilder.str.getdate;
//                sql0["Nu", "Nickname     "] = args.NickName ?? member.NickName;
//            }
//            sql0[" u", "AccessToken  "] = accessToken ?? Guid.NewGuid().ToString("N");
//            string sql = $@"update {TableName<GeniusBull.Member>.Value}{sql0._update_set()}{sql0._where()}
//select Id, Account, AccessToken from {TableName<GeniusBull.Member>.Value} nolock{sql0._where()}";
//            GeniusBull.Member m = this.GameDB().ToObject<GeniusBull.Member>(true, sql);
//            if (m==null) throw new _Exception(Status.PlatformUserNotExist);
//            return m;
//            #region ...
//            //GeniusBull.Member m2 = 
//            //if (m2 == null)
//            //{
//            //    m2 = api_CreateUser(m1, member, accessToken);
//            //    SqlBuilder sql1 = new SqlBuilder();
//            //    sql1["TableName         "] = (SqlBuilder.str)TableName<IG01MemberPlatformData>.Value;
//            //    sql1["w", "MemberID     "] = m1.MemberID;
//            //    sql1["w", "PlatformID   "] = this.ID;
//            //    sql1["w", "n            "] = 0;
//            //    sql1["u", "destID       "] = m2.Id;
//            //    member.CorpInfo.DB_User01W().ExecuteNonQuery(sql1.Build("update {TableName}", SqlBuilder.op.update_set, SqlBuilder.op.where));
//            //}
//            //return m2;
//            #endregion
//        }

        public GeniusBull.Member api_CreateUser(IG01MemberPlatformData p_member, MemberData member, string accessToken = null)
        {
            SqlBuilder sql0 = new SqlBuilder();
            sql0[" ", "ParentId     "] = 0;
            sql0["w", "Account      "] = p_member.Account;
            sql0[" ", "Password     "] = "";
            sql0["N", "Nickname     "] = member.NickName;
            sql0[" ", "Balance      "] = 0;
            sql0[" ", "Stock        "] = 0;
            sql0[" ", "Role         "] = GeniusBull.MemberRole.Player;
            sql0[" ", "Type         "] = GeniusBull.MemberType.Cash;
            sql0[" ", "Status       "] = GeniusBull.MemberStatus.Active;
            sql0[" ", "RegisterTime "] = SqlBuilder.str.getdate;
            if (!string.IsNullOrEmpty(accessToken))
            {
                sql0[" ", "LastLoginIp"] = _HttpContext.Current.RequestIP;
                sql0[" ", "LastLoginTime"] = SqlBuilder.str.getdate;
                sql0[" ", "AccessToken"] = accessToken;
            }
            string sql1 = $@"
insert into {TableName<GeniusBull.Member>.Value}{sql0._insert()}
select Id, Account, AccessToken from {TableName<GeniusBull.Member>.Value} nolock {sql0._where()}";
            try { return this.GameDB().ToObject<GeniusBull.Member>(true, sql1); }
            catch (SqlException ex) when (ex.IsDuplicateKey()) { return null; }
        }

        bool api_UpdateBalance(IG01MemberPlatformData m1, MemberData member, decimal amount, out decimal balance)
        {
            balance = 0;
            string sql = $@"update {GeniusBull.Member.TableName} set Balance=Balance+({amount}) where Id={m1.destID}
select Balance from {GeniusBull.Member.TableName} nolock where Id={m1.destID}";

            SqlCmd sqlcmd = this.GameDB();
            foreach (Action commit in sqlcmd.BeginTran())
            {
                decimal? b = null;
                foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach(sql))
                    b = r.GetDecimalN("Balance");
                if (b.HasValue)
                {
                    if (b.Value < 0) return false;
                    balance = b.Value;
                    commit();
                    return true;
                }
                else throw new _Exception(Status.PlatformUserNotExist);
            }
            return false;
        }

        bool api_GetBalance(IG01MemberPlatformData m1, MemberData member, out decimal balance)
        {
            decimal? b = null;
            foreach (SqlDataReader r in this.GameDB().ExecuteReaderEach($"select Balance from {GeniusBull.Member.TableName} nolock where Id={m1.destID}"))
                b = r.GetDecimalN("Balance");
            balance = b ?? 0;
            return b.HasValue;
        }
    }

    [TableName("MemberPlatform_IG01")]
    public class IG01MemberPlatformData : MemberPlatformData
    {
        [DbImport]
        public int destID;

        //public override object GetBlackList()
        //{
        //    IG01PlatformInfo p = this.Platform as IG01PlatformInfo;
        //    if (p != null)
        //    {
        //        List<MemberPlatformBlackListItem> items = p.GameDB().ToList<MemberPlatformBlackListItem>($"select * from {}");
        //    }
        //}
    }
}
//namespace ams.Controllers
//{
//    public class GeniusBullSysApiController : _ApiController
//    {
//        IG01PlatformInfo platform;
//        GameInfo gameinfo;

//        public class _ListRequest<T> : ListRequest<T>
//        {
//            IG01PlatformInfo platform;
//            UserName? platformName;
//            public UserName GetPlatformName()
//            {
//                if (this.platformName.HasValue)
//                    return this.platformName.Value;
//                string platformName;
//                this.GetFilter("PlatformName").GetValue(out platformName);
//                this.platformName = platformName;
//                return this.platformName.Value;
//            }
//            public IG01PlatformInfo GetPlatform(bool err = false)
//            {
//                return platform = platform ?? (IG01PlatformInfo)PlatformInfo.GetPlatformInfo(this.GetPlatformName(), err: err);
//            }
//        }

//        [HttpPost, Route("~/Sys/GeniusBull/EprobTable/list")]
//        public ListResponse<EprobTableRow> list(_ListRequest<EprobTableRow> args)
//        {
//            return this.Null(args).Validate(this, false).GetResponse(
//                get_sqlcmd: () => args.GetPlatform().GameDB(),
//                onBuild: (name, item) =>
//            {
//            });
//        }

//        [HttpPost, Route("~/Sys/GeniusBull/EprobTable/ListGroup")]
//        public ListResponse<EprobTableGroup> EprobTableGroup(_ListRequest<EprobTableGroup> args)
//        {
//            this.Null(args).Validate(this, false, () =>
//            {
//                ModelState.Validate("PlatformName", args.GetPlatformName(), allow_null: false);
//            });
//            var ret = new ListResponse<EprobTableGroup>();
//            ret.Rows = new List<EprobTableGroup>();
//            foreach (var n in PlatformGameInfo.Cache.Value)
//            {
//                ret.Rows.Add(new Data.EprobTableGroup()
//                {
//                    GameId = n.OriginalID.ToInt32() ?? 0,
//                    GameName = n.GetGameInfo().Name
//                });
//            }
//            //SqlCmd sqlcmd = args.GetPlatform(true).GameDB();
//            //ret.Rows = ams.Data.EprobTableGroup.GetAll(args.GetPlatform());
//            //foreach (var row in ret.Rows)
//            //{
//            //    try
//            //    {
//            //        row.GameName = args.GetPlatform()?.GetPlatformGameInfo(row.GameId.ToString())?.GetGameInfo()?.Name;
//            //    }
//            //    catch { }
//            //    //row.GameName = GameInfo.GetGameInfo()
//            //}
//            return ret;
//        }
//    }
//}
//namespace ams.Data
//{
//    [TableName("EprobTable", SortField = nameof(Id), SortOrder = SortOrder.asc)]
//    public class EprobTableRow
//    {
//        //[Filterable]
//        //internal UserName PlatformName;

//        [Filterable]
//        public string GameName;

//        //[JsonProperty]
//        //public int? GameID
//        //{
//        //    get { return gameinfo?.GameID; }
//        //}

//        [DbImport, JsonProperty]
//        public long Id;
//        [DbImport, JsonProperty]
//        public int GameId;
//        [DbImport, JsonProperty]
//        public int Eprob;       // 分組標籤
//        [DbImport, JsonProperty]
//        public bool Selected;   // 是否啟用
//        [DbImport, JsonProperty]
//        public int Symbol;
//        [DbImport, JsonProperty]
//        public int Reel_1;
//        [DbImport, JsonProperty]
//        public int Reel_2;
//        [DbImport, JsonProperty]
//        public int Reel_3;
//        [DbImport, JsonProperty]
//        public int Reel_4;
//        [DbImport, JsonProperty]
//        public int Reel_5;
//    }

//    [TableName("EprobTable", SortField = nameof(GameId), SortOrder = SortOrder.asc)]
//    public class EprobTableGroup
//    {
//        [Filterable]
//        internal UserName PlatformName;

//        [JsonProperty]
//        public string GameName;

//        [DbImport, JsonProperty]
//        public int GameId;
//        [DbImport, JsonProperty]
//        public int Eprob;
//        [DbImport, JsonProperty]
//        public int ReelSum1;
//        [DbImport, JsonProperty]
//        public int ReelSum2;
//        [DbImport, JsonProperty]
//        public int ReelSum3;
//        [DbImport, JsonProperty]
//        public int ReelSum4;
//        [DbImport, JsonProperty]
//        public int ReelSum5;

//        public static List<EprobTableGroup> GetAll(IG01PlatformInfo ig01) => ig01.GameDB().ToList<EprobTableGroup>(sqlstr);

//        static string sqlstr = $@"select GameId, Eprob, SUM(Reel_1) as {nameof(ReelSum1)}, SUM(Reel_2) as {nameof(ReelSum2)}, SUM(Reel_3) as {nameof(ReelSum3)}, SUM(Reel_4) as {nameof(ReelSum4)}, SUM(Reel_5) as {nameof(ReelSum5)} from {TableName<EprobTableGroup>.Value} group by GameId, Eprob";

//        List<EprobTableRow> rows;
//        public List<EprobTableRow> GetRows(IG01PlatformInfo ig01) => rows = rows ?? ig01.GameDB().ToList<EprobTableRow>($"select * from {TableName<EprobTableRow>.Value} nolock where GameId={GameId} and Eprob={Eprob}");
//    }
//}



// Global Config

// Game Config
//  德州撲克
/*
        CREATE TABLE [dbo].[TexasConfig](
            [TableName_EN] [nvarchar](20) NOT NULL,     -- display name
            [TableName_CHS] [nvarchar](20) NOT NULL,    -- display name
            [TableName_CHT] [nvarchar](20) NOT NULL,    -- display name
            [BigBlind] [int] NOT NULL,					-- 大盲
            [SmallBlind] [int] NOT NULL,				-- 小盲
            [SeatMax] [int] NOT NULL,					-- 最大人數
            [TableMax] [int] NOT NULL,					-- 最大桌數
            [SecondsToCountdown] [int] NOT NULL,		-- 讀秒時間
        )
*/
//  鬥地主
/*
        CREATE TABLE [dbo].[DouDizhuConfig](
            [BaseValue] [int] NOT NULL,				-- 底分
            [SnatchLord] [bit] NOT NULL,			-- 搶地主模式
            [Fine] [bit] NOT NULL,					-- 罰款
            [MissionMode] [bit] NOT NULL,			-- 任務模式
            [Ai] [bit] NOT NULL,					-- 
            [LuckyHand] [int] NOT NULL,				-- 好牌率
            [FakePlayerNum] [int] NOT NULL,			-- 玩家人數
        )
*/

/*
    jackpot :
     遊戲資訊設定  - [Game]
     Jackpot 設定  - [JackpotConfig]
     機率表設定 - [EprobSymbol]
     機率表設定(編輯) - [EprobTable]

SELECT a.Id,a.GameId, b.Name as GameName, a.Eprob, a.Selected, a.Symbol, c.Name as SymbolName, a.Reel_1, a.Reel_2, a.Reel_3, a.Reel_4, a.Reel_5  FROM EprobTable a
left join Game b on a.GameId=b.Id
left join EprobSymbol c on a.Symbol = c.Symbol 
     
     
     
     */
/* poker defines
數值 "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A"
加上
花色 'd', 'c', 'h', 's'
黑桃 > Spade
紅心 > Heart
方塊 > Diamond
梅花 > Club

如: Ts > 黑桃十
*/
