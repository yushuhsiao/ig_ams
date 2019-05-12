using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public struct Union<T1, T2>
    {
        public object Value { get; set; }

        public static implicit operator Union<T1, T2>(T1 t) => new Union<T1, T2>() { Value = t };
        public static implicit operator Union<T1, T2>(T2 t) => new Union<T1, T2>() { Value = t };
        public static explicit operator T1(Union<T1, T2> value) => value.Value.TryCast<T1>();
        public static explicit operator T2(Union<T1, T2> value) => value.Value.TryCast<T2>();
    }
}
