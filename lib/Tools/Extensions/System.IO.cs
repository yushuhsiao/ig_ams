using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace System.IO
{
    public static class _Extensions
    {
#if NETCORE || NET452 || NET461
        [DebuggerStepThrough]
        public static StreamReader CreateStreamReader(this Stream stream, Encoding encoding = null, bool detectEncodingFromByteOrderMarks = true, int bufferSize = 1024, bool leaveOpen = true)
            => new StreamReader(stream, encoding ?? Encoding.UTF8, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen);
#endif
    }
}
