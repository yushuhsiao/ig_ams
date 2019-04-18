using System;

namespace InnateGlory
{
    public enum LoginMode : SByte
    {
        /// <summary>
        /// 使用 cookie 驗證, 適用於同站台登入
        /// </summary>
        Cookie,

        /// <summary>
        /// 只檢查帳號密碼
        /// </summary>
        AuthOnly,

        /// <summary>
        /// 登入成功時會產生 AccessToken, 叫用端在 Http Header 指定 IG-AUTH-TOKEN = &lt;AccessToken&gt; 進行身分驗證
        /// </summary>
        UserToken
    }
}