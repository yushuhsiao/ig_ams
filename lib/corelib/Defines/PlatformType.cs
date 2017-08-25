using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ams
{
    /// <summary>
    /// 平台類型
    /// </summary>
    public enum PlatformType : int
    {
        Main = 0,           // 主帳戶
        /// <summary>
        /// slot game, poker, live casino
        /// </summary>
        InnateGloryA = 1,
        /// <summary>
        /// bingo bingo
        /// </summary>
        InnateGloryB = 2,
        /// <summary>
        /// lottery
        /// </summary>
        InnateGloryC = 3,
        InnateGlory_Appeal = 250,
        AG = 11,            // AG_AG, AG_AGIN, AG_DSP
        PT = 12,
        MG = 13,
        BBIN = 14,
        HG = 15,
        EA = 16,
        KENO = 17,          // KENO, KENO_SSC
    }
    public enum PlatformActiveState : byte
    {
        /// <summary>
        /// 永久停用
        /// </summary>
        Disabled = ActiveState.Disabled,
        /// <summary>
        /// 啟用
        /// </summary>
        Active = ActiveState.Active,
        /// <summary>
        /// 維護中
        /// </summary>
        Maintenance = 2,
    }
}
