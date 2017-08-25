using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlatformsApiController : _ApiController
    {
        //const string p_url = "~/sys/platforms/";
        //static readonly SqlBuilder.err p_err1 = new SqlBuilder.err(value: (int)Status.PlatformAlreadyExist);
        //static readonly SqlBuilder.err p_err2 = new SqlBuilder.err(value: (int)Status.PlatformNotExist);

        [HttpPost, Route("~/Sys/Platforms/list")]
        public ListResponse<PlatformInfo> list(ListRequest<PlatformInfo> args)
        {
            return this.Null(args).Validate(this, false).GetResponse(onBuild: (name, item) =>
            {
                switch (name)
                {
                    case nameof(PlatformInfo.ID): /*            */ args.AddFilter_Int32(name, item); break;
                    case nameof(PlatformInfo.PlatformType): /*  */ args.AddFilter_Enum<PlatformType>(name, item); break;
                    case nameof(PlatformInfo.PlatformName): /*  */ args.AddFilter_StringContains(name, item); break;
                }
            });
        }

        //public ListResponse<PlatformInfo> list(ListRequest_2<PlatformInfo> args)
        //{
        //    return this.Validate(args, valid_user: false).
        //        GetResponse(_HttpContext.GetSqlCmd(DB.Core01R), create: () => new PlatformInfo());
        //    //if (args == null)
        //    //    throw new _Exception(Status.InvalidParameter);
        //    //string sql = args.GetSqlStr("Platforms");
        //    //dynamic res = new ListResponse<PlatformInfo>();
        //    //res.Rows = _HttpContext.GetSqlCmd(DB.Core01R).ToList<PlatformInfo>(sql);
        //    //res.TableVer = PlatformInfo.Cache.Version;
        //    //return res;
        //}

        #region arguments
        [JsonProperty]
        public int? ID;
        [JsonProperty]
        public PlatformActiveState? Active;
        [JsonProperty]
        public PlatformType? PlatformType;
        [JsonProperty]
        public UserName PlatformName;
        [JsonProperty]
        public CurrencyCode? Currency;
        #endregion

        [HttpPost, Route("~/Sys/Platforms/add")]
        public PlatformInfo add(_empty args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate("ID          ", this.ID);
                ModelState.Validate("PlatformType", this.PlatformType);
                ModelState.Validate("PlatformName", this.PlatformName);
                ModelState.Validate("Currency    ", this.Currency);
                ModelState.Validate("Active      ", this.Active = this.Active ?? PlatformActiveState.Disabled);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1[" w", "ID          "] = this.ID;
            //sql1[" *", "uid         "] = SqlBuilder.str.newid;
            sql1[" *", "PlatformType"] = this.PlatformType;
            sql1["n*", "PlatformName"] = this.PlatformName;
            sql1[" *", "Currency    "] = this.Currency;
            sql1[" *", "Active      "] = this.Active;
            sql1.SetCreateUser();
            sql1.SetModifyUser();
            string TableName = TableName<PlatformInfo>._.TableName;
            string sql = $@"insert into {TableName}{sql1._insert()}
select * from {TableName} nolock{sql1._where()}";
            SqlCmd core01w = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit in core01w.BeginTran())
            {
                PlatformInfo ret;
                try { ret = core01w.ToObject<PlatformInfo>(sql); }
                catch (SqlException ex) when (ex.IsDuplicateKey())
                { throw new _Exception(Status.PlatformAlreadyExist); }
                if (ret == null) break;
                PlatformInfo.Cache.UpdateVersion(core01w);
                commit();
                return ret;
            }
            return null;
        }

        [HttpPost, Route("~/Sys/Platforms/set")]
        public PlatformInfo set(_empty args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate("ID", this.ID);
                ModelState.Validate("Active", this.Active, allow_null: true);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1["TableName"] = (SqlBuilder.str)"Platforms";
            sql1["w", "ID    "] = this.ID;
            sql1["u", "Active"] = this.Active;
            if (sql1.UpdateCount == 0)
                return System.Web._HttpContext.GetSqlCmd(DB.Core01W).ToObject<PlatformInfo>(sql1.Build("select * from {TableName} nolock", SqlBuilder.op.where));
            sql1.SetModifyTime("u");
            sql1.SetModifyUser("u");
            string sql = sql1.Build(@"update {TableName}", SqlBuilder.op.update_set, SqlBuilder.op.where, @"
select * from {TableName} nolock", SqlBuilder.op.where);
            SqlCmd core01w = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit in core01w.BeginTran())
            {
                PlatformInfo ret = core01w.ToObject<PlatformInfo>(sql);
                if (ret == null)
                    throw new _Exception(Status.PlatformNotExist);
                PlatformInfo.Cache.UpdateVersion(core01w);
                commit();
                return ret;
            }
            return null;
        }

        [HttpPost, Route("~/Sys/Platforms/get")]
        public PlatformInfo get(_empty args) { throw new NotImplementedException(); }


        /// <summary>
        /// 遊戲帳號列表
        /// </summary>
        //[HttpPost, Route("~/Users/Member/game/list")]
        //public ListResponse<MemberPlatformData> game_list(ListRequest<MemberPlatformData> args)
        //{
        //    ListRequest_2<MemberPlatformData>.Valid(ModelState, args);
        //    PlatformInfo pp = PlatformInfo.GetPlatformInfo(0);
        //    string sqlstr = args.sql_list("MemberPlatform");
        //    if (string.IsNullOrEmpty(args.SortField))
        //        args.SortField = "PlatformID";
        //    args.SortOrder = args.SortOrder ?? SortOrder.asc;
        //    var result = new ListResponse<MemberPlatformData>() { Rows = new List<MemberPlatformData>() };
        //    foreach (PlatformInfo p in PlatformInfo.Cache.Value)
        //        result.Rows.Add(p.NewMember());
        //    SqlBuilder sql = new SqlBuilder();
        //    sql["TableName"] = "MemberPlatform";
        //    sql["SortField"] = args.SortField;//.GetValueOrDefault("PlatformID");
        //    sql["SortOrder"] = args.SortOrder ?? SortOrder.asc;
        //    sql["w", "MemberID"] = args.MemberData.ID;
        //    SqlCmd sqlcmd = args.CorpInfo.DB_User01R();
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach(sql.Build("select * from {TableName} nolock", SqlBuilder.op.where)))
        //    {
        //        int platformID = sqlcmd.DataReader.GetInt32("PlatformID");
        //        var nn = result.Rows.Find((n) => n.PlatformID == platformID);
        //        if (nn != null)
        //            r.FillObject(nn);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 遊戲帳戶設定
        /// </summary>
        //[HttpPost, Route("~/Users/Member/game/set")]
        //public void m_set() { }

        /// <summary>
        /// 跳轉進入遊戲
        /// </summary>
        [HttpPost, Route("~/Users/Member/game/forward")]
        public ForwardGameArguments m_forward(JObject args)
        {
            if (args == null)
                throw new _Exception(Status.InvalidParameter);
            ForwardGameArguments _args = args.ToObject<ForwardGameArguments>();
            _args.src = args;
            PlatformInfo platform = ModelState.ValidatePlatformName("PlatformName", _args.PlatformName);
            ModelState.IsValid();
            return platform.ForwardGame(this, _args);
        }

        public void KickUser(string username, bool clearSession = true)
        {
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GamesApiController : _ApiController
    {
        [HttpPost, Route("~/Sys/Games/list")]
        public ListResponse<GameInfo> list(ListRequest<GameInfo> args)
        {
            return this.Null(args).Validate(this, false).GetResponse(onBuild: (name, item) =>
            {
                switch (name)
                {
                    case nameof(GameInfo.Name): /*          */ args.AddFilter_StringContains(name, item); break;
                    case nameof(GameInfo.GameClass): /*     */ args.AddFilter_Enum<GameClass>(name, item); break;
                    case nameof(GameInfo.ID): /*            */ args.AddFilter_Int32(name, item); break;
                }
            });
        }

        //static readonly SqlBuilder.err g_err1 = new SqlBuilder.err(value: (int)Status.GameDefineAlreadyExist);
        //static readonly SqlBuilder.err g_err2 = new SqlBuilder.err(value: (int)Status.GameDefineNotExist);

        //public ListResponse<GameInfo> list(ListRequest_2<GameInfo> args)
        //{
        //    return this.Validate(args, valid_user: false).
        //        GetResponse(_HttpContext.GetSqlCmd(DB.Core01R), create: () => new GameInfo());
        //    //if (args == null)
        //    //    throw new _Exception(Status.InvalidParameter);
        //    //args.PageSize = 1000;
        //    //args.SortField = nameof(GameInfo.GameID);
        //    //string sql = args.GetSqlStr(GameInfo.TableName);
        //    //return new ListResponse<GameInfo>() { Rows = _HttpContext.GetSqlCmd(DB.Core01R).ToList<GameInfo>(sql) };
        //}

        #region arguments

        //[JsonProperty]
        //public Guid? ID;

        [JsonProperty]
        public GameClass? GameClass;

        [JsonProperty]
        public int? ID;

        [JsonProperty]
        public string Name;

        [JsonProperty]
        public string Name_en;

        [JsonProperty]
        public string Name_cht;

        [JsonProperty]
        public string Name_chs;

        #endregion

        [HttpPost, Route("~/Sys/Games/add")]
        public GameInfo add(_empty _args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate(nameof(this.GameClass), this.GameClass);
                ModelState.Validate(nameof(this.ID), this.ID, min: (_min) => _min >= 1, max: (_max) => _max <= 0xffff);
                ModelState.Validate(nameof(this.Name), this.Name);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1["w", "ID       "] = this.ID;
            sql1[" ", "GameClass"] = this.GameClass;
            sql1["n", "Name     "] = this.Name;
            sql1.SetCreateUser();
            sql1.SetModifyUser();
            string sql = $@"insert into {TableName<GameInfo>._.TableName}{sql1._insert()}
select * from {TableName<GameInfo>._.TableName} nolock{sql1._where()}";
            SqlCmd core01w = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit in core01w.BeginTran())
            {
                GameInfo ret;
                try { ret = core01w.ToObject<GameInfo>(sql); }
                catch (SqlException ex) when (ex.IsDuplicateKey())
                { throw new _Exception(Status.GameDefineAlreadyExist); }
                GameInfo.Cache.UpdateVersion(core01w);
                commit();
                return ret;
            }
            throw new _Exception(Status.NoResult);
        }

        [HttpPost, Route("~/Sys/Games/set")]
        public GameInfo set(_empty _args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                //ModelState.Validate("ID", this.ID, min: (_min) => _min >= 1, max: (_max) => _max <= 0xffff, allow_null: true);
                ModelState.Validate("ID", this.ID);
                ModelState.Validate("GameClass", this.GameClass, allow_null: true);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1["w ", "ID          "] = this.ID;
            sql1[" u", "GameClass   "] = this.GameClass;
            sql1["nu", "Name        "] = this.Name;
            if (sql1.UpdateCount == 0)
                return System.Web._HttpContext.GetSqlCmd(DB.Core01W).ToObject<GameInfo>($"select * from {TableName<GameInfo>._.TableName} nolock {sql1._where()}");
            sql1.SetModifyTime("u");
            sql1.SetModifyUser("u");
            string sql = $@"update {TableName<GameInfo>._.TableName}{sql1._update_set()}{sql1._where()}
select * from {TableName<GameInfo>._.TableName} nolock{sql1._where()}";
            SqlCmd core01w = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit in core01w.BeginTran())
            {
                GameInfo ret = core01w.ToObject<GameInfo>(sql);
                if (ret == null)
                    throw new _Exception(Status.GameDefineNotExist);
                LangItem lang = ret.GetLangItem();
                lang.SetValue(core01w, ret.Name, 9, Name_en);
                lang.SetValue(core01w, ret.Name, 31748, Name_cht);
                lang.SetValue(core01w, ret.Name, 4, Name_chs);
                LangItem.Cache.UpdateVersion(core01w);
                GameInfo.Cache.UpdateVersion(core01w);
                commit();
                return ret;
            }
            return null;
        }

        [HttpPost, Route("~/Sys/Games/get")]
        public GameInfo get(_empty args) { throw new NotImplementedException(); }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlatformGamesApiController : _ApiController
    {
        //static readonly SqlBuilder.err g_err1 = new SqlBuilder.err(value: (int)Status.GameDefineAlreadyExist);
        //static readonly SqlBuilder.err g_err2 = new SqlBuilder.err(value: (int)Status.GameDefineNotExist);

        //const string base_url = "~/Sys/PlatformGames/";

        //public ListResponse<PlatformGameInfo> list(ListRequest_2<PlatformGameInfo> args)
        //{
        //    return this.Validate(args, valid_user: false).
        //        GetResponse(_HttpContext.GetSqlCmd(DB.Core01R), create: () => new PlatformGameInfo());

        //    //if (args == null)
        //    //    throw new _Exception(Status.InvalidParameter);
        //    //string sql = args.GetSqlStr(PlatformGameInfo.TableName);
        //    //return new ListResponse<PlatformGameInfo>() { Rows = _HttpContext.GetSqlCmd(DB.Core01R).ToList<PlatformGameInfo>(sql) };
        //}
        [HttpPost, Route("~/Sys/PlatformGames/list")]
        public ListResponse<PlatformGameInfo> list(ListRequest<PlatformGameInfo> args)
        {
            var ret = this.Null(args).Validate(this, false).GetResponse(onBuild: (name, item) =>
            {
                switch (name)
                {
                    case nameof(PlatformGameInfo.ID): /*            */ args.AddFilter_Int32(name, item); break;
                    case nameof(PlatformGameInfo.PlatformID):
                    case nameof(PlatformGameInfo.PlatformName): /*  */ args.AddFilter_PlatformNames("PlatformID", item); break;
                    //case nameof(PlatformGameInfo.GameName): /*      */ args.AddFilter_StringContains(name, item); break;
                    case nameof(PlatformGameInfo.GameClass): /*     */ args.AddFilter_Enum<GameClass>(name, item); break;
                }
            });
            foreach (var n1 in ret.Rows)
            {
                n1.GameName = n1.GetGameInfo().Name;
                n1.GameClass = n1.GetGameInfo().GameClass;
                n1.PlatformName = n1.GetPlatformInfo().PlatformName;
            }
            foreach (PlatformInfo p in PlatformInfo.Cache.Value)
                p.ExtraInfo(ret.Rows);
            return ret;
        }

        #region arguments
        [JsonProperty]
        public Guid? ID;
        [JsonProperty]
        public PlatformType? PlatformType;
        [JsonProperty]
        public GameClass? GameClass;
        [JsonProperty]
        public int? GameID;
        [JsonProperty]
        public string OriginalID;
        //[JsonProperty("Active")]
        //public bool? _Active;
        //public ActiveState? Active
        //{
        //    get
        //    {
        //        if (this._Active.HasValue)
        //            return this._Active.Value ? ActiveState.Active : ActiveState.Disabled;
        //        return null;
        //    }
        //}
        #endregion

        [HttpPost, Route("~/Sys/PlatformGames/add")]
        public PlatformGameInfo add(_empty args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate("PlatformType", this.PlatformType);
                ModelState.Validate("GameClass   ", this.GameClass);
                ModelState.Validate("GameID      ", this.GameID);
                ModelState.Validate("OriginalID  ", this.OriginalID);
            });
            GameInfo gameinfo = GameInfo.GetGameInfo(this.GameClass.Value, this.GameID.Value, err: true);
            SqlBuilder sql1 = new SqlBuilder();
            sql1["w", "ID           "] = (SqlBuilder.str)"@id";
            sql1["*", "PlatformType "] = this.PlatformType;
            sql1["*", "GameID       "] = gameinfo.ID;
            sql1["n", "OriginalID   "] = this.OriginalID;
            sql1.SetCreateUser();
            sql1.SetModifyUser();
            string TableName = TableName<PlatformGameInfo>._.TableName;
            string sql = $@"declare @id uniqueidentifier set @id=newid()
insert into {TableName} {sql1._insert()}
select * from {TableName} nolock{sql1._where()}";
            SqlCmd core01w = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit in core01w.BeginTran())
            {
                PlatformGameInfo ret;
                try { ret = core01w.ToObject<PlatformGameInfo>(sql); }
                catch (SqlException ex) when (ex.IsDuplicateKey()) { throw new _Exception(Status.GameDefineAlreadyExist); }
                if (ret == null) break;
                PlatformGameInfo.Cache.UpdateVersion(core01w);
                commit();
                return ret;
            }
            throw new _Exception(Status.NoResult);
        }

        [HttpPost, Route("~/Sys/PlatformGames/set")]
        public PlatformGameInfo set(_empty args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate("ID          ", this.ID);
                ModelState.Validate("PlatformType", this.PlatformType, allow_null: true);
                ModelState.Validate("GameID      ", this.GameID, allow_null: true);
                ModelState.Validate("OriginalID  ", this.OriginalID, allow_null: true);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1["w", "ID          "] = this.ID;
            sql1["u", "PlatformType"] = this.PlatformType;
            sql1["u", "GameID      "] = this.GameID;
            sql1["u", "OriginalID  "] = this.OriginalID;
            //sql1.AddEnum("     u", "Active      ", args.Active);
            GameInfo game = GameInfo.GetGameInfo(this.GameClass.Value, this.GameID.Value);
            if (sql1.UpdateCount == 0)
                return System.Web._HttpContext.GetSqlCmd(DB.Core01R).ToObject<PlatformGameInfo>($"select * from {TableName<PlatformGameInfo>._.TableName} nolock{sql1._where()}");
            else
            {
                sql1.SetModifyTime("u");
                sql1.SetModifyUser("u");
                string TableName = TableName<PlatformGameInfo>._.TableName;
                string sql = $@"update {TableName} {sql1._update_set()}{sql1._where()}
select * from {TableName} nolock{sql1._where()}";
                SqlCmd core01w = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
                foreach (Action commit in core01w.BeginTran())
                {
                    PlatformGameInfo ret = core01w.ToObject<PlatformGameInfo>(sql);
                    if (ret == null) throw new _Exception(Status.GameDefineNotExist);
                    PlatformGameInfo.Cache.UpdateVersion(core01w);
                    commit();
                    return ret;
                }
            }
            return null;
        }

        [HttpPost, Route("~/Sys/PlatformGames/get")]
        public PlatformGameInfo get(_empty args) { throw new NotImplementedException(); }
    }
}