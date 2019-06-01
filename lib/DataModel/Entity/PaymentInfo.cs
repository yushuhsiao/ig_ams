using InnateGlory.Entity.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 支付平台
    /// </summary>
    [TableName("Payments", Database = _Consts.db.CoreDB)]
    public class PaymentInfo : BaseData
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ActiveState Active { get; set; }
    }

    /// <summary>
    /// 支付帳號
    /// </summary>
    [TableName("PaymentAccount", Database = _Consts.db.CoreDB)]
    public class PaymentAccount : BaseData
    {
        public int Id { get; set; }

        /// <summary>
        /// <see cref="PaymentInfo.Id"/>
        /// </summary>
        public int PaymentId { get; set; }

        public string Name { get; set; }

        public ActiveState Active { get; set; }

        public string MerhantId { get; set; }

        public string extdata { get; set; }

        public string Description { get; set; }
    }

    /// <summary>
    /// 代理商支付帳號
    /// </summary>
    [TableName("PaymentAccount", Database = _Consts.db.UserDB)]
    public class AgentPaymentAccount : BaseData
    {
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="PaymentAccount.Id"/>
        public int PaymentAccountId { get; set; }

        public CorpId CorpId { get; set; }

        public UserId AgentId { get; set; }

        public string Name { get; set; }

        public ActiveState Active { get; set; }
    }
}
namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PaymentInfoModel
    {
        [JsonProperty]
        public PaymentType? PaymentType { get; set; }
    }
}