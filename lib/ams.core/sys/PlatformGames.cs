using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;

namespace ams.Data
{
    /// <summary>
    /// 平台遊戲名稱定義
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [TableName("PlatformGames", SortField = nameof(GameID), SortOrder = SortOrder.asc)]
    public class PlatformGameInfo
    {
        //public static readonly SqlBuilder.str TableName = TableName<PlatformGameInfo>._.TableName;

        public static readonly RedisVer<List<PlatformGameInfo>> Cache = new RedisVer<List<PlatformGameInfo>>(TableName<PlatformGameInfo>._.TableName)
        {
            ReadData = (sqlcmd, index) => sqlcmd.ToList<PlatformGameInfo>($"select * from {TableName<PlatformGameInfo>.Value} nolock")
        };

        public static PlatformGameInfo GetItem(Guid id, SqlCmd coredb = null, bool err = false)
        {
            PlatformGameInfo row;
            if (coredb == null)
                row = Cache.Value.Find((n) => n.ID == id);
            else
                row = coredb.ToObject<PlatformGameInfo>($"select * from {TableName<PlatformGameInfo>.Value} nolock where ID='{id}'");
            if (err && row == null)
                throw new _Exception(Status.PlatformGameDefineNotExist);
            return row;
        }

        [DbImport, JsonProperty]
        public Guid ID;
        [DbImport, JsonProperty]
        public SqlTimeStamp ver;
        [DbImport, JsonProperty, Sortable, Filterable]
        public int PlatformID;
        [JsonProperty, Sortable, Filterable]
        public UserName PlatformName;
        [DbImport, JsonProperty, Sortable, Filterable]
        public int GameID;

        [JsonProperty, Sortable]
        public string GameName;
        //[DbImport, JsonProperty, Sortable]
        //public Guid GameRowID;
        [DbImport, JsonProperty]
        public GameClass GameClass;
        [DbImport, JsonProperty]
        public string OriginalID;

        [JsonProperty]
        public int? EprobGroup;
        [JsonProperty]
        public int? ConfigKeys;

        //[DbImport, JsonProperty]
        //public UserName GameName;
        //[DbImport, JsonProperty]
        //public GameType GameType;
        //[DbImport]
        //public ActiveState Active;

        ////[DbImport, JsonProperty]
        ////public UserName PlatformName;
        ////[DbImport, JsonProperty]
        ////public PlatformType PlatformType;
        ////[DbImport, JsonProperty]
        ////public GameClass GameClass;
        //[JsonProperty("Active")]
        //bool _Active
        //{
        //    get { return this.Active == ActiveState.Active; }
        //}
        [DbImport, JsonProperty, Sortable]
        public DateTime CreateTime;
        [DbImport, JsonProperty, Sortable, JsonConverter(typeof(UserNameJsonConverter))]
        public UserID CreateUser;
        [DbImport, JsonProperty, Sortable]
        public DateTime ModifyTime;
        [DbImport, JsonProperty, Sortable, JsonConverter(typeof(UserNameJsonConverter))]
        public UserID ModifyUser;

        public GameInfo GetGameInfo() => GameInfo.GetGameInfo(this.GameID);
        public PlatformInfo GetPlatformInfo() => PlatformInfo.GetPlatformInfo(this.PlatformID);
    }
}