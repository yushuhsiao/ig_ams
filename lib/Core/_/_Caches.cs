using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory
{
    class _Caches<T> : List<T>
    {
        public _Caches() { }
        public _Caches(IEnumerable<T> collection) : base(collection) { }
    }
}
