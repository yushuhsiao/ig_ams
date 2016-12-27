using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StackExchange.Redis
{
    public class RedisLogWriter : System.IO.TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void WriteLine(string value)
        {
            log.message("Redis", value);
            base.WriteLine(value);
        }
    }

    public static class _RedisExtensions
    {
        //public static T HashGet<T>(this IDatabase db, RedisKey key, CommandFlags flags = CommandFlags.None) where T : new()
        //{
        //    HashEntry[] h = db.HashGetAll(key, flags);
        //    return default(T);
        //}
        //public static void HashSet<T>(this IDatabase db, RedisKey key, T obj, CommandFlags flags = CommandFlags.None)
        //{
        //    HashEntry[] h = new HashEntry[0];
        //    db.HashSet(key, h, flags);
        //}

        //public static T ToObject<T>(this HashEntry[] hashes) where T : new()
        //{
        //    return default(T);
        //}
        //public static HashEntry[] FromObject()
        //{
        //}

        //[DebuggerStepThrough]
        //public static bool TryGetValue(this HashEntry[] h, RedisValue key, out RedisValue value)
        //{
        //    for (int i = 0, n = h.Length; i < n; i++)
        //    {
        //        if (h[i].Name.Equals(key))
        //        {
        //            value = h[i].Value;
        //            return true;
        //        }
        //    }
        //    value = default(RedisValue);
        //    return false;
        //}

        //public static bool ContainsKey(this HashEntry[] h, RedisValue key)
        //{
        //    for (int i = 0, n = h.Length; i < n; i++)
        //        if (h[i].Name.Equals(key))
        //            return true;
        //    return false;
        //}

        //[DebuggerStepThrough]
        //public static RedisValue GetValue(this HashEntry[] h, RedisValue key)
        //{
        //    RedisValue value; h.TryGetValue(key, out value); return value;
        //}

        //public static IEnumerable<RedisKey> Keys(this IDatabase db, RedisValue pattern)
        //{
        //    ConnectionMultiplexer m = db.Multiplexer;
        //    foreach (EndPoint e in m.GetEndPoints())
        //    {
        //        IServer s = m.GetServer(e);
        //        if (s.IsSlave) continue;
        //        foreach (var n in s.Keys(db.Database, pattern))
        //            yield return n;
        //    }
        //}

        //public static RedisValue GetValue(this Dictionary<RedisValue, RedisValue> dict, RedisValue key)
        //{
        //    RedisValue value; dict.TryGetValue(key, out value); return value;
        //}

        public static RedisValue GetValue(this HashEntry[] h, RedisValue key)
        {
            for (int i = 0; i < h.Length; i++)
                if (h[i].Name == key)
                    return h[i].Value;
            return default(RedisValue);
        }

        public static int? ToInt32(this RedisValue v)
        {
            int n; if (v.HasValue && v.TryParse(out n)) return n; return null;
        }
        public static bool ToInt32(this RedisValue v, out int value)
        {
            if (v.HasValue) return v.TryParse(out value);
            value = default(int);
            return false;
        }

        public static long? ToInt64(this RedisValue v)
        {
            int n; if (v.HasValue && v.TryParse(out n)) return n; return null;
        }
        public static bool ToInt64(this RedisValue v, out long value)
        {
            if (v.HasValue) return v.TryParse(out value);
            value = default(long);
            return false;
        }

        public static double? ToDouble(this RedisValue v)
        {
            double n; if (v.HasValue && v.TryParse(out n)) return n; return null;
        }
        public static bool ToDouble(this RedisValue v, out double value)
        {
            if (v.HasValue) return v.TryParse(out value);
            value = default(double);
            return false;
        }
    }
}