using System;
using System.Diagnostics;

namespace InnateGlory
{
    public enum LogType : int
    {
        CorpBalanceIn /*                */ = 0x000 << 0 | LogTypeFlag.In,
        CorpBalanceOut /*               */ = 0x000 << 0 | LogTypeFlag.Out,
        //CorpBalanceOutRollback /*     */ = 0x000 << 0 | LogTypeFlag.Both,

        BalanceIn /*                    */ = 0x001 << 2 | LogTypeFlag.In,
        BalanceOut /*                   */ = 0x001 << 2 | LogTypeFlag.Out,
        BalanceOutRollback /*           */ = 0x001 << 2 | LogTypeFlag.None,

        AgentBalanceIn /*               */ = 0x002 << 2 | LogTypeFlag.In,
        AgentBalanceOut /*              */ = 0x002 << 2 | LogTypeFlag.Out,
        AgentBalanceRollback /*         */ = 0x002 << 2 | LogTypeFlag.Both,

        MemberBalanceIn /*              */ = 0x003 << 2 | LogTypeFlag.In,
        MemberBalanceOut /*             */ = 0x003 << 2 | LogTypeFlag.Out,
        MemberBalanceRollback /*        */ = 0x003 << 2 | LogTypeFlag.Both,

        MemberExchange /*               */ = 0x013 << 2 | LogTypeFlag.None,
        MemberExchangeIn /*             */ = 0x013 << 2 | LogTypeFlag.In,
        MemberExchangeOut /*            */ = 0x013 << 2 | LogTypeFlag.Out,
        MemberExchangeRollback /*       */ = 0x013 << 2 | LogTypeFlag.Both,

        //LoadTo /*                     */ = 0x002 << 2 | LogTypeFlag.Out,  // 代理額度下放
        //LoadFrom /*                   */ = 0x002 << 2 | LogTypeFlag.In,   // 子代理上分/子會員上分
        //UnloadFrom /*                 */ = 0x003 << 2 | LogTypeFlag.In,   // 代理額度收回
        //UnloadTo /*                   */ = 0x003 << 2 | LogTypeFlag.Out,  // 子代理卸分/子會員卸分

        /// <summary>
        /// 遊戲轉點-轉入
        /// </summary>
        PlatformWithdrawal /*           */ = 0x004 << 2 | LogTypeFlag.In,
        /// <summary>
        /// 遊戲轉點-轉出(預扣)
        /// </summary>
        PlatformDeposit /*              */ = 0x004 << 2 | LogTypeFlag.Out,
        /// <summary>
        /// 遊戲轉點-退還預扣
        /// </summary>
        PlatformRollback /*             */ = 0x004 << 2 | LogTypeFlag.None,

        /// <summary>
        /// 遊戲內回存 (callback)
        /// </summary>
        InPlatformDeposit /*            */ = 0x005 << 2 | LogTypeFlag.In,
        /// <summary>
        /// 遊戲內提領-預扣 (callback)
        /// </summary>
        InPlatformWithdrawal /*         */ = 0x005 << 2 | LogTypeFlag.Out,
        /// <summary>
        /// 遊戲內提領-退還預扣 (callback)
        /// </summary>
        InPlatformRollback /*           */ = 0x005 << 2 | LogTypeFlag.None, 

        /// <summary>
        /// 第三方支付
        /// </summary>
        PaymentAPI /*                   */ = 0x010 << 2 | LogTypeFlag.In,
        //CashDeposit /*                */ = 0x011 << 2 | LogTypeFlag.In,   // 現金存款
        //CashWithdrawal /*             */ = 0x011 << 2 | LogTypeFlag.Out,  // 現金提款(預扣)
        //CashWithdrawalRollback /*     */ = 0x011 << 2 | LogTypeFlag.None, // 現金提款-退還預扣
        //GiftCard /*                   */ = 0x012 << 2 | LogTypeFlag.In,   // 儲值卡

        Promotion /*                    */ = 0x011 << 2 | LogTypeFlag.In,
        Penalty /*                      */ = 0x011 << 2 | LogTypeFlag.Out,
        PenaltyRollback /*              */ = 0x011 << 2 | LogTypeFlag.None,
    }
    [Flags]
    public enum LogTypeFlag : int
    {
        Out = 1 << 0,
        In = 1 << 1,
        Both = In | Out,
        None = 0
    }
    public static partial class LogTypeExtensions
    {
        [DebuggerStepThrough]
        public static bool HasFlag(this LogType logType, LogTypeFlag flag) => logType.GetFlag().HasFlag(flag);

        [DebuggerStepThrough]
        public static LogTypeFlag GetFlag(this LogType logType)
        {
            int n = (int)logType;
            n &= 0x0f;
            return (LogTypeFlag)n;
        }
    }
}