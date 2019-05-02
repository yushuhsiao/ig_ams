// $Id: IGrouping.cs 71137f497bf2 2012/04/16 20:01:27 azizatif $

namespace System.Linq
{
    #region Imports

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a collection of objects that have a common key.
    /// </summary>

    partial interface IGrouping<out TKey, TElement> : IEnumerable<TElement>
    {
        /// <summary>
        /// Gets the key of the <see cref="IGrouping{TKey,TElement}" />.
        /// </summary>

        TKey Key { get; }
    }
}

