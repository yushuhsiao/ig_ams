using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Globalization
{
    public static class _Extensions
    {
        public static CultureInfo GetParent(this CultureInfo cultureInfo)
        {
            if (cultureInfo == null) return null;
            if (cultureInfo.LCID == cultureInfo.Parent.LCID) return null;
            return cultureInfo.Parent;
        }

        public static IEnumerable<CultureInfo> GetParents(this CultureInfo cultureInfo, bool self = true)
        {
            if (cultureInfo != null)
            {
                if (self)
                    yield return cultureInfo;
                for (; ;)
                {
                    CultureInfo p = cultureInfo.Parent;
                    if (cultureInfo.LCID == p.LCID)
                        break;
                    yield return p;
                    cultureInfo = p;
                }
            }
        }
    }
}