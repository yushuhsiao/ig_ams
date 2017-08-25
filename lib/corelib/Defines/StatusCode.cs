using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;
//using System.Security;

namespace ams
{
    [StringEnum(false)]
    public enum Status : int
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 參數遺失
        /// </summary>
        MissingParameter = 2,
        /// <summary>
        /// 參數無效
        /// </summary>
        InvalidParameter = 3,
        /// <summary>
        /// 參數不允許
        /// </summary>
        ParameterNotAllow = 4,
        /// <summary>
        /// 無回傳
        /// </summary>
        NoResult = 5,
        /// <summary>
        /// 用戶類型不允許
        /// </summary>
        UserTypeNotAllow,
        /// <summary>
        /// 無法判定用戶類型
        /// </summary>
        UnableDecideUserType,
        /// <summary>
        /// 無法分配用戶ID
        /// </summary>
        UnableAllocateUserID,
        /// <summary>
        /// 用戶不存在
        /// </summary>
        UserNotExist,
        /// <summary>
        /// 所屬公司已停用
        /// </summary>
        CorpDisabled,
        /// <summary>
        /// 帳號被鎖定
        /// </summary>
        AccountDisabled,
        /// <summary>
        /// 上級代理被鎖定
        /// </summary>
        ParentDisabled,
        /// <summary>
        /// 所屬公司已存在
        /// </summary>
        CorpAlreadyExist,
        /// <summary>
        /// 所屬公司不存在
        /// </summary>
        CorpNotExist,
        /// <summary>
        /// 代理已存在
        /// </summary>
        AgentAlreadyExist,
        /// <summary>
        /// 代理不存在
        /// </summary>
        AgentNotExist,
        /// <summary>
        /// 管理者已存在
        /// </summary>
        AdminAlreadyExist,
        /// <summary>
        /// 管理者不存在
        /// </summary>
        AdminNotExist,
        /// <summary>
        /// 會員已存在
        /// </summary>
        MemberAlreadyExist,
        /// <summary>
        /// 會員不存在
        /// </summary>
        MemberNotExist,
        /// <summary>
        /// 找不到密碼
        /// </summary>
        PasswordNotFound,
        /// <summary>
        /// 密碼已禁用
        /// </summary>
        PasswordDisabled,
        /// <summary>
        /// 密碼已過期
        /// </summary>
        PasswordExpired,
        /// <summary>
        /// 密碼不匹配
        /// </summary>
        PasswordNotMatch,
        /// <summary>
        /// 下級層數最大限制
        /// </summary>
        MaxDepthLimit,
        /// <summary>
        /// 代理最大限制
        /// </summary>
        MaxAgentLimit,
        /// <summary>
        /// 管理者最大限制
        /// </summary>
        MaxAdminLimit,
        /// <summary>
        /// 會員最大限制
        /// </summary>
        MaxMemberLimit,
        /// <summary>
        /// 平台已存在
        /// </summary>
        PlatformAlreadyExist,
        /// <summary>
        /// 平台不存在
        /// </summary>
        PlatformNotExist,
        /// <summary>
        /// 平台已鎖定
        /// </summary>
        PlatformDisabled,
        /// <summary>
        /// 平台維護中
        /// </summary>
        PlatformMaintenance,
        /// <summary>
        /// 平台API錯誤
        /// </summary>
        PlatformApiFailed,
        /// <summary>
        /// 平台不支援
        /// </summary>
        PlatformNotSupported,
        /// <summary>
        /// 平台不匹配
        /// </summary>
        PlatformTypeNotMatch,
        /// <summary>
        /// 遊戲定義已存在
        /// </summary>
        GameDefineAlreadyExist,
        /// <summary>
        /// 遊戲定義不存在
        /// </summary>
        GameDefineNotExist,
        /// <summary>
        /// 平台遊戲定義已存在
        /// </summary>
        PlatformGameDefineAlreadyExist,
        /// <summary>
        /// 平台遊戲定義不存在
        /// </summary>
        PlatformGameDefineNotExist,
        /// <summary>
        /// 找不到轉帳單
        /// </summary>
        TranNotFound,
        /// <summary>
        /// 用戶餘額不足
        /// </summary>
        UserBalanceNotEnough,
        /// <summary>
        /// 轉點方不存在
        /// </summary>
        ProviderNotExist,
        /// <summary>
        /// 轉點方餘額不足
        /// </summary>
        ProviderBalanceNotEnough,
        /// <summary>
        /// 平台餘額不足
        /// </summary>
        PlatformBalanceNotEnough,
        /// <summary>
        /// 轉帳忙線
        /// </summary>
        TranBusy,
        /// <summary>
        /// 找不到第三方支付訊息
        /// </summary>
        PaymentInfoNotFound,
        /// <summary>
        /// 第三方支付已鎖定
        /// </summary>
        PaymentInfoDisabled,
        /// <summary>
        /// 找不到遊戲申訴紀錄
        /// </summary>
        Appeal_GameLogNotFound,
        /// <summary>
        /// 遊戲申訴紀錄不包含用戶
        /// </summary>
        Appeal_GameLogNotContainsUser,

        PlatformUserNotExist,
        RecogSessionNotExist,
    }
}