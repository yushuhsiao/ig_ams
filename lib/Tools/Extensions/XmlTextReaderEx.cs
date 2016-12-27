#if NET40
using System;
using System.Collections.Generic;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Xml
{
    [_DebuggerStepThrough]
    public static partial class XmlTextReaderEx
    {
        public static bool IsTextNode(this XmlTextReader xr, int depth)
        {
            return (xr.Depth == depth) && (xr.NodeType == XmlNodeType.Text);
        }
        public static bool IsStartElement(this XmlTextReader xr, int depth, string name)
        {
            return (xr.Depth == depth) && (xr.NodeType == XmlNodeType.Element) && (string.Compare(xr.Name, name ?? xr.Name, true) == 0);
        }
        public static bool Read(this XmlTextReader xr, int depth)
        {
            if (xr.Read())
                return xr.Depth >= depth;
            return false;
        }
        public static IEnumerable<XmlTextReader> ReadElement(this XmlTextReader xr, int depth, string name)
        {
            while (xr.Read(depth))
            {
                if (xr.IsStartElement(depth, name))
                {
                    yield return xr;
                }
            }
        }
    }
}
#endif