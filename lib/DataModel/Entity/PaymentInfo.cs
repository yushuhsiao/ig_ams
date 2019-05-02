using InnateGlory.Entity.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    [TableName("Payments", Database = _Consts.db.UserDB)]
    public class PaymentInfo : BaseData
    {
        public Guid Id { get; set; }
        public CorpId CorpId { get; set; }
        public UserId AgentId { get; set; }
        public string Name { get; set; }
        public PaymentType PaymentType { get; set; }
        public ActiveState Active { get; set; }
        public string MerhantId { get; set; }
        public string extdata { get; set; }
        public string Description { get; set; }
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