using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools
{
    public class PropertyHandler<T>
    {
        object _value;
        public T Value
        {
            get { return (GetValue ?? defaultGetValue)(); }
            set { (SetValue ?? defaultSetValue)(value); }
        }

        T defaultGetValue()
        {
            return (T)Interlocked.CompareExchange(ref _value, null, null);
        }
        Func<T> _getValue;
        public Func<T> GetValue
        {
            get { return Interlocked.CompareExchange(ref _getValue, null, null); }
            set { Interlocked.Exchange(ref _getValue, value); }
        }

        void defaultSetValue(T value)
        {
            Interlocked.Exchange(ref _value, value);
        }
        Action<T> _setValue;
        public Action<T> SetValue
        {
            get { return Interlocked.CompareExchange(ref _setValue, null, null); }
            set { Interlocked.Exchange(ref _setValue, value); }
        }
    }
}
