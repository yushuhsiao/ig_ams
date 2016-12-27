using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [TableName("Games", SortField = nameof(ID), SortOrder = SortOrder.asc)]
    public class GameInfo
    {
        //public static readonly SqlBuilder.str TableName = "Games";

        public static readonly RedisVer<List<GameInfo>> Cache = new RedisVer<List<GameInfo>>(TableName<GameInfo>._.TableName)
        {
            ReadData = (sqlcmd, index) => sqlcmd.ToList<GameInfo>($"select * from {TableName<GameInfo>._.TableName} nolock")
        };

        static GameInfo _GetGameInfo(GameInfo obj, bool err)
        {
            if (err && (obj == null))
                throw new _Exception(Status.GameDefineNotExist);
            return obj;
        }
        public static GameInfo GetGameInfo(int id, SqlCmd coreDB = null, bool err = false)
        {
            GameInfo obj;
            if (coreDB == null)
                obj = Cache.Value.Find((n) => n.ID == id);
            else
                obj = coreDB.ToObject<GameInfo>($"select * from {TableName<GameInfo>._.TableName} nolock where ID='{id}'");
            return _GetGameInfo(obj, err);
        }
        public static GameInfo GetGameInfo(GameClass gameClass, int gameID, SqlCmd coreDB = null, bool err = false)
        {
            GameInfo obj;
            if (coreDB == null)
                obj = Cache.Value.Find((n) => n.GameClass == gameClass && n.ID == gameID);
            else
                obj = coreDB.ToObject<GameInfo>($"select * from {TableName<GameInfo>._.TableName} nolock where GameClass={(int)gameClass} and GameID={gameID}");
            return _GetGameInfo(obj, err);
        }
        public static GameInfo GetGameInfo(string name, SqlCmd coreDB = null, bool err = false)
        {
            GameInfo obj;
            if (coreDB == null)
                obj = Cache.Value.Find((n) => string.Compare(n.Name, name, true) == 0);
            else
                obj = coreDB.ToObject<GameInfo>($"select * from {TableName<GameInfo>._.TableName} nolock where Name='{SQLinjection.magic_quote(name)}'");
            return _GetGameInfo(obj, err);
        }

        [DbImport, JsonProperty, Sortable, Filterable]
        public int ID;
        //[DbImport, JsonProperty, Sortable]
        //public Guid ID;
        [DbImport, JsonProperty, Sortable, Filterable]
        public string Name;
        [DbImport, JsonProperty, Sortable, Filterable]
        public GameClass GameClass;
        [DbImport, JsonProperty]
        public SqlTimeStamp ver;
        [DbImport, JsonProperty, Sortable]
        public DateTime CreateTime;
        [DbImport, JsonProperty, Sortable, JsonConverter(typeof(UserNameJsonConverter))]
        public UserID CreateUser;
        [DbImport, JsonProperty, Sortable]
        public DateTime ModifyTime;
        [DbImport, JsonProperty, Sortable, JsonConverter(typeof(UserNameJsonConverter))]
        public UserID ModifyUser;



        LangItem _lang;
        LangItem lang { get { return _lang = _lang ?? GetLangItem(); } }
        public LangItem GetLangItem() => LangItem.EnumsRoot.GetEnumNode("GameName");

        [JsonProperty]
        public string Name_en
        {
            get { return GetLangItem().GetValue(this.Name, 9); }
        }
        [JsonProperty]
        public string Name_cht
        {
            get { return GetLangItem().GetValue(this.Name, 31748); }
        }
        [JsonProperty]
        public string Name_chs
        {
            get { return GetLangItem().GetValue(this.Name, 4); }
        }
    }
}