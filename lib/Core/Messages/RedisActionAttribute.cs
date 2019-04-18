using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory.Messages
{
    public class RedisActionAttribute : MessageInvokerAttribute
    {
        public string Channel { get; set; }
    }
}
