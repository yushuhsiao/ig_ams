// $Id: IOrderedEnumerable.cs 71137f497bf2 2012/04/16 20:01:27 azizatif $

namespace System.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a sorted sequence.
    /// </summary>

    partial interface IOrderedEnumerable<TElement> : IEnumerable<TElement>
    {
        /// <summary>
        /// Performs a subsequent ordering on the elements of an 
        /// <see cref="IOrderedEnumerable{T}"/> according to a key.
        /// </summary>

        IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(
            Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
    }
}

