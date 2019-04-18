using InnateGlory.Entity.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 支付平台
    /// </summary>
    [TableName("PayPlatform", Database = _Consts.db.CoreDB)]
    public class PayPlatform : BaseData
    {
        [DbImport]
        public int Id { get; set; }

        [DbImport]
        public ActiveState Active { get; set; }
    }
}
