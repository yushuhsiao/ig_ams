using InnateGlory.Entity.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 系統支付帳號
    /// </summary>
    [TableName("PayAccount", Database = _Consts.db.CoreDB)]
    public class SysPayAccount : BaseData
    {
        [DbImport]
        public int Id { get; set; }

        [DbImport]
        public int PayPlatformId { get; set; }
        internal PayPlatform PayPlatform;

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
    [TableName("PayAccount", Database = _Consts.db.UserDB)]
    public class UserPayAccount : BaseData
    {
        [DbImport]
        public int Id { get; set; }

        [DbImport]
        public int PayAccountId { get; set; }
        internal SysPayAccount PayAccount;

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
