using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Collections.Generic
{
    public static class Extensions
    {
        [_DebuggerStepThrough]
        public static void RemoveValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
        {
            if (dict != null)
            {
                while (dict.ContainsValue(value))
                {
                    foreach (KeyValuePair<TKey, TValue> p in dict)
                    {
                        if (EqualityComparer<TValue>.Default.Equals(value, p.Value))
                        //if (object.ReferenceEquals(value, p.Value))
                        {
                            dict.Remove(p.Key);
                            break;
                        }
                    }
                }
            }
        }

        //[_DebuggerStepThrough]
        //public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, bool remove = false)
        //{
        //    if (dict == null) return default(TValue);
        //    TValue result;
        //    if (dict.ContainsKey(key))
        //    {
        //        result = dict[key];
        //        if (remove) dict.Remove(key);
        //        return result;
        //    }
        //    return default(TValue);
        //}

        [_DebuggerStepThrough]
        public static bool TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, out TValue value, bool remove = false)
        {
            bool result;
            if (result = dict.TryGetValue(key, out value))
            {
                if (remove) dict.TryRemove(key);
            }
            return result;
        }


        [_DebuggerStepThrough]
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                return false;
            dict[key] = value;
            return true;
        }

        [_DebuggerStepThrough]
        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            bool result = false;
            while (dict.ContainsKey(key))
                result |= dict.Remove(key);
            return result;
        }

        [_DebuggerStepThrough]
        public static int RemoveAll<T>(this List<T> list, T value)
        {
            int result = 0;
            while (list.Remove(value))
                result++;
            return result;
        }

        [_DebuggerStepThrough]
        public static bool AddOnce<T>(this List<T> list, T item, bool sync_lock = false)
        {
            if (sync_lock) Monitor.Enter(list);
            try
            {
                if (list.Contains(item))
                    return false;
                list.Add(item);
                return true;
            }
            finally
            {
                if (sync_lock)
                    Monitor.Exit(list);
            }
        }


        [_DebuggerStepThrough]
        public static int RemoveWhen<T>(this List<T> list, Predicate<T> match)
        {
            int count = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    list.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }


        [_DebuggerStepThrough]
        public static List<T> Trim<T>(this List<T> list, bool nullOnEmpty = false)
        {
            if (list == null) return null;
            if (list.Count == 0 && nullOnEmpty)
                return null;
            return list;
        }

        [_DebuggerStepThrough]
        public static IEnumerable<T> ForEach<T>(this List<T> users, bool sync_lock = true)
        {
            if (sync_lock)
                Monitor.Enter(users);
            try
            {
                for (int i = users.Count - 1; i >= 0; i--)
                    yield return users[i];
            }
            finally
            {
                if (sync_lock)
                    Monitor.Exit(users);
            }
        }

        [_DebuggerStepThrough]
        public static int AddOnce<T>(this List<T> list, IEnumerable<T> items)
        {
            int cnt = 0;
            foreach (T item in items)
            {
                if (list.Contains(item)) continue;
                list.Add(item);
                cnt++;
            }
            return cnt;
        }

        [_DebuggerStepThrough]
        public static bool Contains(this List<string> list, string item, bool ignoreCase)
        {
            if (ignoreCase)
            {
                for (int i = 0, n = list.Count; i < n; i++)
                    if (string.Compare(list[i], item, true) == 0)
                        return true;
                return false;
            }
            else
                return list.Contains(item);
        }

        [_DebuggerStepThrough]
        public static IEnumerable<T> FindEach<T>(this List<T> list, Predicate<T> match)
        {
            if ((list != null) && (match != null))
            {
                for (int i = 0, n = list.Count; i < n; i++)
                    if (match(list[i]))
                        yield return list[i];
            }
        }
    }

    [_DebuggerStepThrough]
    public class ObjectDictionary : Dictionary<string, object>
    {
        public T GetValue<T>(string key)
        {
            object result;
            if (base.TryGetValue(key, out result))
                if (result is T)
                    return (T)result;
            return default(T);
        }
    }
    //    [_DebuggerStepThrough]
    //    public static class Extensions
    //    {
    //        public static void AddNoDuplicate<T>(this List<T> list, T item)
    //        {
    //            if (list != null)
    //                if (!list.Contains(item))
    //                    list.Add(item);
    //        }

    //        public static void RemoveAt<T>(this List<T> list, T item)
    //        {
    //            if (list != null)
    //                while (list.Remove(item)) { }
    //        }
    //    }
}