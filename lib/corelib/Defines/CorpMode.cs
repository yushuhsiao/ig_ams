using System;

namespace ams
{
    /// <summary>
    /// 站台代理模式
    /// </summary>
    public enum CorpMode : int
    {
        Root = 0,
        /// <summary>
        /// 直接加扣點, 代理占成另外計算
        /// </summary>
        Mode1 = 1,
        /// <summary>
        /// 撥分制
        /// </summary>
        Mode2 = 2,
    }
}