using System;
using System.Collections.Generic;
using System.Text;

namespace System.Xml
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
    [_DebuggerStepThrough]
    public static partial class Extensions
    {
        public static XmlElement AppendElement(this XmlNode e, string name)
        {
            return (XmlElement)e.AppendChild(e.OwnerDocument.CreateElement(name));
        }

        public static string GetInnerText(this XmlNode node)
        {
            if (node != null)
                return node.InnerText;
            return null;
        }

        public static string GetValue(this XmlNode node)
        {
            if (node != null)
                return node.Value;
            return null;
        }
    }
}