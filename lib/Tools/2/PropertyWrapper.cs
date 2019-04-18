using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools
{
    public class PropertyWrapper<T>
    {
        object _value;
        public T Value
        {
            get { return (GetValueHandler ?? getValueHandler_default)(); }
            set { (SetValueHandler ?? setValueHandler_default)(value); }
        }

        T getValueHandler_default()
        {
            object n = Interlocked.CompareExchange(ref _value, null, null);
            if (n == null) return default(T);
            return (T)n;
        }
        void setValueHandler_default(T value) => Interlocked.Exchange(ref _value, value);

        Func<T> getValueHandler;
        Action<T> setValueHandler;

        public Func<T> GetValueHandler
        {
            get { return Interlocked.CompareExchange(ref getValueHandler, null, null); }
            set { Interlocked.Exchange(ref getValueHandler, value); }
        }
        public Action<T> SetValueHandler
        {
            get { return Interlocked.CompareExchange(ref setValueHandler, null, null); }
            set { Interlocked.Exchange(ref setValueHandler, value); }
        }
    }
}