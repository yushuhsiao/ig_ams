using System.Threading;

namespace System.Collections.Generic
{
    partial class _CollectionsExtensions
    {
        static void _sync_lock(object src, ref object syncLock)
        {
            if (object.Equals(syncLock, true) || (syncLock != null))
                _Monitor.Enter(syncLock = src);
            else if (object.Equals(syncLock, false))
                syncLock = null;
        }
        static void _sync_lock(object syncLock)
        {
            if (syncLock != null)
                _Monitor.Exit(syncLock);
        }

        public static bool RemoveValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value, object syncLock = null)
        {
            if (dict == null) return false;
            try
            {
                _sync_lock(dict, ref syncLock);
                int cnt = 0;
                while (dict.ContainsValue(value))
                {
                    foreach (KeyValuePair<TKey, TValue> p in dict)
                    {
                        if (EqualityComparer<TValue>.Default.Equals(value, p.Value))
                        //if (object.ReferenceEquals(value, p.Value))
                        {
                            dict.Remove(p.Key);
                            cnt++;
                            break;
                        }
                    }
                }
                return cnt > 0;
            }
            finally { _sync_lock(syncLock); }
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, bool remove = false, object syncLock = null)
        {
            TValue result; dict.TryGetValue(key, out result, remove, syncLock); return result;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> create, object syncLock = null)
            => GetValue(dict, key, create, null, syncLock);

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> create, object syncLock = null)
            => GetValue(dict, key, null, create, syncLock);
        //{
        //    try
        //    {
        //        _sync_lock(dict, ref syncLock);
        //        if (dict.TryGetValue(key, out TValue value))
        //            return value;
        //        if (create == null) return default(TValue);
        //        return dict[key] = create();
        //    }
        //    finally { _sync_lock(syncLock); }
        //}

        private static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> create1, Func<TKey, TValue> create2, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                if (dict.TryGetValue(key, out TValue value))
                    return value;
                if (create1 != null)
                    return dict[key] = create1();
                else if (create2 != null)
                    return dict[key] = create2(key);
                else
                    return default(TValue);
            }
            finally { _sync_lock(syncLock); }
        }

        public static bool SetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                bool result = dict.ContainsKey(key);
                dict[key] = value;
                return result;
            }
            finally { _sync_lock(syncLock); }
        }

        public static bool TrySetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                if (dict.ContainsKey(key))
                    return false;
                dict[key] = value;
                return true;
            }
            finally { _sync_lock(syncLock); }
        }

        public static bool TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out TValue value, bool remove = false, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                bool result;
                if (result = dict.TryGetValue(key, out value))
                {
                    if (remove) dict.TryRemove(key);
                }
                return result;
            }
            finally { _sync_lock(syncLock); }
        }

        //public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        //{
        //    if (dict.ContainsKey(key))
        //        return false;
        //    dict[key] = value;
        //    return true;
        //}

        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                bool result = false;
                while (dict.ContainsKey(key))
                    result |= dict.Remove(key);
                return result;
            }
            finally { _sync_lock(syncLock); }
        }

        public static int RemoveWhen<TKey, TValue>(this IDictionary<TKey, TValue> dict, Predicate<KeyValuePair<TKey, TValue>> match, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                int count = 0;
                for (var n = dict.GetEnumerator(); n.MoveNext();)
                {
                    var kvp = n.Current;
                    if (match(kvp))
                    {
                        dict.Remove(kvp);
                        n = dict.GetEnumerator();
                    }
                }
                return count;
            }
            finally { _sync_lock(syncLock); }
        }


        public static bool Clear<TKey, TValue>(this IDictionary<TKey, TValue> dict, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                bool result = dict.Count > 0;
                dict.Clear();
                return result;
            }
            finally { _sync_lock(syncLock); }
        }

        public static bool UpdateValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value, object syncLock = null)
        {
            try
            {
                _sync_lock(dict, ref syncLock);
                if (dict.ContainsKey(key))
                {
                    dict[key] = value;
                    return true;
                }
                return false;
            }
            finally { _sync_lock(syncLock); }
        }

        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> src, bool clear_src = false, object syncLock = null)
        {
            try
            {
                _sync_lock(src, ref syncLock);
                Dictionary<TKey, TValue> copy = new Dictionary<TKey, TValue>();
                foreach (var p in src)
                    copy[p.Key] = p.Value;
                if (clear_src)
                    src.Clear();
                return copy;
            }
            finally { _sync_lock(syncLock); }
        }
    }
}