namespace LinqBridge
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct Key<T>
    {
        public Key(T value)
        {
            this = (Key) new Key<T>();
            this.Value = value;
        }

        public T Value
        {
            [CompilerGenerated]
            get
            {
                return this.<Value>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Value>k__BackingField = value;
            }
        }
    }
}

