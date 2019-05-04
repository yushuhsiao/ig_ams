namespace System.Collections.Generic
{
    partial class _CollectionsExtensions
    {
        //public static bool Dequeue<T>(this Queue<T> queue, out T value)
        //{
        //    if ((queue != null) && (queue.Count > 0))
        //    {
        //        value = queue.Dequeue();
        //        return true;
        //    }
        //    return _null.noop(false, out value);
        //}

        public static bool Dequeue<T>(this Queue<T> queue, out T value, object syncLock = null)
        {
            if (queue != null)
            {
                try
                {
                    _sync_lock(queue, ref syncLock);
                    if (queue.Count > 0)
                    {
                        value = queue.Dequeue();
                        return true;
                    }
                }
                finally { _sync_lock(syncLock); }
            }
            return _null.noop(false, out value);
        }
    
        public static bool Enqueue<T>(this Queue<T> queue, T item, object syncLock = null)
        {
            if (queue == null) return false;
            try
            {
                _sync_lock(queue, ref syncLock);
                if (queue.Contains(item))
                    return false;
                queue.Enqueue(item);
                return true;
            }
            finally { _sync_lock(syncLock); }
        }
    }
}