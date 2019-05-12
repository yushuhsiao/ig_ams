using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Linq
{
    public static class _Extensions
    {
        public static Array ToArray(this JArray src, Type elementType, bool all = false)
        {
            Array objs = Array.CreateInstance(elementType, src.Count);
            for (int i = 0; i < src.Count; i++)
            {
                try { objs.SetValue(src[i].ToObject(elementType), i); }
                catch { if (all) throw; }
            }
            return objs;
        }

        public static TElement[] ToArray<TElement>(this JArray src, bool all = false) => (TElement[])src.ToArray(typeof(TElement), all);
    }
}