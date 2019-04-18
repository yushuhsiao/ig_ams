namespace System.Linq
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IGrouping<out TKey, TElement> : IEnumerable<TElement>, IEnumerable
    {
        TKey Key { get; }
    }
}

