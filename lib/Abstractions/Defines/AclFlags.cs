using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace InnateGlory
{
    [Flags]
    public enum AclFlags : int
    {
        /// <summary>
        /// 允許
        /// </summary>
        Allow = 0x01,

        /// <summary>
        /// 繼承自上級
        /// </summary>
        Inherited = 0x02,

        /// <summary>
        /// 可見, 當 <see cref="Allow"/> 值為 0 時, 傳回 <see cref="HttpStatusCode.Forbidden"/> 或 <see cref="HttpStatusCode.NotFound"/>
        /// </summary>
        Visible = 0x04,

        /// <summary>
        /// 可被繼承
        /// </summary>
        Inheritable = 0x08,
    }
}
