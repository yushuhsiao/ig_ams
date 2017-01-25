using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using ams.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web;
using ams.Controllers;
using ams.Data;

namespace ams.Data
{
    [Flags]
    public enum MemberActiveFlag : byte
    {
        Disabled = ActiveState.Disabled,
        Accounts = AgentActiveFlag.Accounts,
        Game = AgentActiveFlag.Game,
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [ams.TableName("Members", SortField = nameof(CreateTime))]
    [UserData(UserType = UserType.Member, Balance = "MemberBalance")]
    public class MemberData : UserData<MemberData>
    {
        public MemberData(CorpInfo corpInfo) : base(corpInfo) { }

        [DbImport("Active")]
        MemberActiveFlag Active;

        [JsonProperty("Active1")]
        public bool AccountActive
        {
            get { return this.Active.HasFlag(MemberActiveFlag.Accounts); }
        }

        [JsonProperty("Active2")]
        public bool GameDisabled
        {
            get { return !this.Active.HasFlag(MemberActiveFlag.Game); }
        }

        public MemberDetailData GetDetails(SqlCmd userDB = null)
        {
            return (userDB ?? CorpInfo.DB_User01R()).ToObject(() => new MemberDetailData(this), $"select * from {TableName<MemberDetailData>.Value} nolock where ID={ID}");
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [TableName("MemberDetails")]
    public class MemberDetailData
    {
        MemberData member;
        public MemberDetailData() { }
        public MemberDetailData(MemberData member)
        {
            this.ID = member.ID;
            this.member = member;
            this.UserName = member.UserName;
        }

        [JsonProperty]
        public UserID? CorpID;
        [JsonProperty]
        public UserName CorpName;

        [DbImport, JsonProperty]
        public SqlTimeStamp? ver;

        [DbImport, JsonProperty]
        public UserID ID;
        /// <summary>
        /// 帳號
        /// </summary>
        [JsonProperty]
        public UserName UserName;
        /// <summary>
        /// 真實姓名
        /// </summary>
        [DbImport, JsonProperty]
        public string RealName;
        /// <summary>
        /// 電話
        /// </summary>
        [DbImport, JsonProperty]
        public string Tel;
        /// <summary>
        /// 電子信箱
        /// </summary>
        [DbImport, JsonProperty]
        public string E_Mail;
        /// <summary>
        /// 生日
        /// </summary>
        [DbImport, JsonProperty]
        public DateTime? Birthday;
        /// <summary>
        /// 地址:國家
        /// </summary>
        [DbImport, JsonProperty]
        public string Country;
        /// <summary>
        /// 地址:洲
        /// </summary>
        [DbImport, JsonProperty]
        public string State;
        /// <summary>
        /// 地址:城市
        /// </summary>
        [DbImport, JsonProperty]
        public string City;
        /// <summary>
        /// 地址:城市
        /// </summary>
        [DbImport, JsonProperty]
        public string District;
        /// <summary>
        /// 地址1
        /// </summary>
        [DbImport, JsonProperty]
        public string Address1;
        /// <summary>
        /// 地址2
        /// </summary>
        [DbImport, JsonProperty]
        public string Address2;
        /// <summary>
        /// 郵遞區號
        /// </summary>
        [DbImport, JsonProperty]
        public string PostalCode;
    }
}