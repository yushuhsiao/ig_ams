// $Id: ILookup.cs 71137f497bf2 2012/04/16 20:01:27 azizatif $

namespace System.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines an indexer, size property, and Boolean search method for 
    /// data structures that map keys to <see cref="IEnumerable{T}"/> 
    /// sequences of values.
    /// </summary>

    partial interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>
    {
        bool Contains(TKey key);
        int Count { get; }
        IEnumerable<TElement> this[TKey key] { get; }
    }
}

