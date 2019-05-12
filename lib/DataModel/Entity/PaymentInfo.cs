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
        [DbImport]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DbImport]
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DbImport]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DbImport]
        public ActiveState Active { get; set; }
    }

    /// <summary>
    /// 支付帳號
    /// </summary>
    [TableName("PaymentAccount", Database = _Consts.db.CoreDB)]
    public class PaymentAccount : BaseData
    {
        [DbImport]
        public int Id { get; set; }

        /// <summary>
        /// <see cref="PaymentInfo.Id"/>
        /// </summary>
        [DbImport]
        public int PaymentId { get; set; }

        [DbImport]
        public string Name { get; set; }

        [DbImport]
        public ActiveState Active { get; set; }

        [DbImport]
        public string MerhantId { get; set; }

        [DbImport]
        public string extdata { get; set; }

        [DbImport]
        public string Description { get; set; }
    }

    /// <summary>
    /// 代理商支付帳號
    /// </summary>
    [TableName("PaymentAccount", Database = _Consts.db.UserDB)]
    public class AgentPaymentAccount : BaseData
    {
        [DbImport]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="PaymentAccount.Id"/>
        [DbImport]
        public int PaymentAccountId { get; set; }

        [DbImport]
        public CorpId CorpId { get; set; }

        [DbImport]
        public UserId AgentId { get; set; }

        [DbImport]
        public string Name { get; set; }

        [DbImport]
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