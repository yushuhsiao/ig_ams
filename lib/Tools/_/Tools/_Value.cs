using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class _Value<T>
    {
        private T _value;
        public T Value
        {
            get => (GetValueHandler ?? DefaultHandler)();
            set => (SetValueHandler ?? DefaultHandler)(value);
        }

        public Func<T> GetValueHandler { get; set; }
        public Action<T> SetValueHandler { get; set; }

        private T DefaultHandler() => _value;
        private void DefaultHandler(T value) => _value = value;
    }
}
