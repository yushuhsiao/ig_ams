using InnateGlory.Entity.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 遊戲平台
    /// </summary>
    [TableName("GamePlatform", Database = _Consts.db.CoreDB)]
    public class GamePlatform : BaseData
    {
        [DbImport]
        public PlatformId Id { get; set; }

        [DbImport]
        public UserName Name { get; set; }

        [DbImport]
        public PlatformType PlatformType { get; set; }

        [DbImport]
        public CurrencyCode Currency { get; set; }

        [DbImport]
        public PlatformActiveState Active { get; set; }
    }
}
namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlatformInfoModel
    {
        [JsonProperty]
        public PlatformId? PlatformId { get; set; }

        [JsonProperty]
        public UserName PlatformName { get; set; }

        [JsonProperty]
        public PlatformType? PlatformType { get; set; }
    }
}
