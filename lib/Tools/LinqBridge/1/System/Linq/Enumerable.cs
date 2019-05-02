namespace System.Linq
{
    using LinqBridge;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Enumerable
    {
        public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException();
                }
                return enumerator.Renumerable<TSource>().Skip<TSource>(1).Aggregate<TSource, TSource>(enumerator.Current, func);
            }
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            return source.Aggregate<TSource, TAccumulate, TAccumulate>(seed, func, r => r);
        }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            TAccumulate local = seed;
            foreach (TSource local2 in source)
            {
                local = func(local, local2);
            }
            return resultSelector(local);
        }

        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            foreach (TSource local in source)
            {
                if (!predicate(local))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                return enumerator.MoveNext();
            }
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).Any<TSource>();
        }

        public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            return source;
        }

        public static decimal? Average(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            decimal num = 0M;
            long num2 = 0;
            foreach (decimal? nullable in from n in source
                where n.HasValue
                select n)
            {
                num += nullable.Value;
                num2++;
            }
            if (num2 == 0)
            {
                return null;
            }
            decimal? nullable3 = new decimal?(num);
            decimal num3 = num2;
            if (!nullable3.HasValue)
            {
                return null;
            }
            return new decimal?(nullable3.GetValueOrDefault() / num3);
        }

        public static double? Average(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            double num = 0.0;
            long num2 = 0;
            foreach (double? nullable in from n in source
                where n.HasValue
                select n)
            {
                num += nullable.Value;
                num2++;
            }
            if (num2 == 0)
            {
                return null;
            }
            double? nullable3 = new double?(num);
            double num3 = num2;
            if (!nullable3.HasValue)
            {
                return null;
            }
            return new double?(nullable3.GetValueOrDefault() / num3);
        }

        public static double? Average(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long num = 0;
            long num2 = 0;
            foreach (int? nullable in from n in source
                where n.HasValue
                select n)
            {
                num += (long) nullable.Value;
                num2++;
            }
            if (num2 == 0)
            {
                return null;
            }
            double? nullable3 = new double?((double) num);
            double num3 = num2;
            if (!nullable3.HasValue)
            {
                return null;
            }
            return new double?(nullable3.GetValueOrDefault() / num3);
        }

        public static double? Average(this IEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long num = 0;
            long num2 = 0;
            foreach (long? nullable in from n in source
                where n.HasValue
                select n)
            {
                num += nullable.Value;
                num2++;
            }
            if (num2 == 0)
            {
                return null;
            }
            double? nullable3 = new double?((double) num);
            double num3 = num2;
            if (!nullable3.HasValue)
            {
                return null;
            }
            return new double?(nullable3.GetValueOrDefault() / num3);
        }

        public static float? Average(this IEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            float num = 0f;
            long num2 = 0;
            foreach (float? nullable in from n in source
                where n.HasValue
                select n)
            {
                num += nullable.Value;
                num2++;
            }
            if (num2 == 0)
            {
                return null;
            }
            float? nullable3 = new float?(num);
            float num3 = num2;
            if (!nullable3.HasValue)
            {
                return null;
            }
            return new float?(nullable3.GetValueOrDefault() / num3);
        }

        public static decimal Average(this IEnumerable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            decimal num = 0M;
            long num2 = 0;
            foreach (decimal num3 in source)
            {
                num += num3;
                num2++;
            }
            if (num2 == 0)
            {
                throw new InvalidOperationException();
            }
            return (num / num2);
        }

        public static double Average(this IEnumerable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            double num = 0.0;
            long num2 = 0;
            foreach (double num3 in source)
            {
                num += num3;
                num2++;
            }
            if (num2 == 0)
            {
                throw new InvalidOperationException();
            }
            return (num / ((double) num2));
        }

        public static double Average(this IEnumerable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long num = 0;
            long num2 = 0;
            foreach (int num3 in source)
            {
                num += num3;
                num2++;
            }
            if (num2 == 0)
            {
                throw new InvalidOperationException();
            }
            return (((double) num) / ((double) num2));
        }

        public static double Average(this IEnumerable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long num = 0;
            long num2 = 0;
            foreach (long num3 in source)
            {
                num += num3;
                num2++;
            }
            if (num2 == 0)
            {
                throw new InvalidOperationException();
            }
            return (((double) num) / ((double) num2));
        }

        public static float Average(this IEnumerable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            float num = 0f;
            long num2 = 0;
            foreach (float num3 in source)
            {
                num += num3;
                num2++;
            }
            if (num2 == 0)
            {
                throw new InvalidOperationException();
            }
            return (num / ((float) num2));
        }

        public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select<TSource, decimal>(selector).Average();
        }

        public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select<TSource, decimal?>(selector).Average();
        }

        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select<TSource, double>(selector).Average();
        }

        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select<TSource, double?>(selector).Average();
        }

        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select<TSource, long?>(selector).Average();
        }

        public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select<TSource, float?>(selector).Average();
        }

        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select<TSource, int>(selector).Average();
        }

        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select<TSource, long>(selector).Average();
        }

        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select<TSource, int?>(selector).Average();
        }

        public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select<TSource, float>(selector).Average();
        }

        public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return CastYield<TResult>(source);
        }

        private static IEnumerable<TResult> CastYield<TResult>(IEnumerable source)
        {
            IEnumerator enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                yield return (TResult) current;
            }
        }

        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            return ConcatYield<TSource>(first, second);
        }

        private static IEnumerable<TSource> ConcatYield<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            foreach (TSource iteratorVariable0 in first)
            {
                yield return iteratorVariable0;
            }
            foreach (TSource iteratorVariable1 in second)
            {
                yield return iteratorVariable1;
            }
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            return source.Contains<TSource>(value, null);
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (comparer == null)
            {
                ICollection<TSource> is2 = source as ICollection<TSource>;
                if (is2 != null)
                {
                    return is2.Contains(value);
                }
            }
            comparer = comparer ?? EqualityComparer<TSource>.Default;
            return source.Any<TSource>(item => comparer.Equals(item, value));
        }

        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            ICollection is2 = source as ICollection;
            if (is2 == null)
            {
                return source.Aggregate<TSource, int>(0, (count, item) => (count + 1));
            }
            return is2.Count;
        }

        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).Count<TSource>();
        }

        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source.DefaultIfEmpty<TSource>(default(TSource));
        }

        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return DefaultIfEmptyYield<TSource>(source, defaultValue);
        }

        private static IEnumerable<TSource> DefaultIfEmptyYield<TSource>(IEnumerable<TSource> source, TSource defaultValue)
        {
            using (IEnumerator<TSource> iteratorVariable0 = source.GetEnumerator())
            {
                if (!iteratorVariable0.MoveNext())
                {
                    yield return defaultValue;
                }
                else
                {
                    do
                    {
                        yield return iteratorVariable0.Current;
                    }
                    while (iteratorVariable0.MoveNext());
                }
            }
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
        {
            return source.Distinct<TSource>(null);
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return DistinctYield<TSource>(source, comparer);
        }

        private static IEnumerable<TSource> DistinctYield<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            Dictionary<TSource, object> iteratorVariable0 = new Dictionary<TSource, object>(comparer);
            bool iteratorVariable1 = false;
            foreach (TSource iteratorVariable2 in source)
            {
                if (iteratorVariable2 == null)
                {
                    if (iteratorVariable1)
                    {
                        continue;
                    }
                    iteratorVariable1 = true;
                }
                else
                {
                    if (iteratorVariable0.ContainsKey(iteratorVariable2))
                    {
                        continue;
                    }
                    iteratorVariable0.Add(iteratorVariable2, null);
                }
                yield return iteratorVariable2;
            }
        }

        public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
        {
            Func<TSource, int, bool> predicate = null;
            TSource local;
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, null);
            }
            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                return list[index];
            }
            try
            {
                if (predicate == null)
                {
                    predicate = (item, i) => i < index;
                }
                local = source.SkipWhile<TSource>(predicate).First<TSource>();
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentOutOfRangeException("index", index, null);
            }
            return local;
        }

        public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (index < 0)
            {
                return default(TSource);
            }
            IList<TSource> list = source as IList<TSource>;
            if (list == null)
            {
                return source.SkipWhile<TSource>(((Func<TSource, int, bool>) ((item, i) => (i < index)))).FirstOrDefault<TSource>();
            }
            if (index >= list.Count)
            {
                return default(TSource);
            }
            return list[index];
        }

        public static IEnumerable<TResult> Empty<TResult>()
        {
            return Sequence<TResult>.Empty;
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return first.Except<TSource>(second, null);
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return first.IntersectExceptImpl<TSource>(second, comparer, false);
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            return source.FirstImpl<TSource>(Futures<TSource>.Undefined);
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).First<TSource>();
        }

        private static TSource FirstImpl<TSource>(this IEnumerable<TSource> source, Func<TSource> empty)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count <= 0)
                {
                    return empty();
                }
                return list[0];
            }
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                return (enumerator.MoveNext() ? enumerator.Current : empty());
            }
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            return source.FirstImpl<TSource>(Futures<TSource>.Default);
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).FirstOrDefault<TSource>();
        }

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy<TSource, TKey>(keySelector, null);
        }

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.GroupBy<TSource, TKey, TSource>(keySelector, ((Func<TSource, TSource>) (e => e)), comparer);
        }

        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, null);
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        {
            return source.GroupBy<TSource, TKey, TResult>(keySelector, resultSelector, null);
        }

        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }
            return source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, comparer);
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            return source.GroupBy<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, null);
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return (from g in source.ToLookup<TSource, TKey>(keySelector, comparer) select resultSelector(g.Key, g));
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return (from g in source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, comparer) select resultSelector(g.Key, g));
        }

        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            return outer.GroupJoin<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
            {
                throw new ArgumentNullException("outer");
            }
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            if (outerKeySelector == null)
            {
                throw new ArgumentNullException("outerKeySelector");
            }
            if (innerKeySelector == null)
            {
                throw new ArgumentNullException("innerKeySelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            ILookup<TKey, TInner> lookup = inner.ToLookup<TInner, TKey>(innerKeySelector, comparer);
            return (from o in outer select resultSelector(o, lookup[outerKeySelector(o)]));
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return first.Intersect<TSource>(second, null);
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return first.IntersectExceptImpl<TSource>(second, comparer, true);
        }

        private static IEnumerable<TSource> IntersectExceptImpl<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer, bool flag)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            List<Key<TSource>> list = new List<Key<TSource>>();
            Dictionary<Key<TSource>, bool> flags = new Dictionary<Key<TSource>, bool>(new KeyComparer<TSource>(comparer));
            foreach (Key<TSource> key in from item in first
                select new Key<TSource>(item) into item
                where !flags.ContainsKey(item)
                select item)
            {
                flags.Add(key, !flag);
                list.Add(key);
            }
            foreach (Key<TSource> key2 in from item in second
                select new Key<TSource>(item) into item
                where flags.ContainsKey(item)
                select item)
            {
                flags[key2] = flag;
            }
            return (from item in list
                where flags[item]
                select item.Value);
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Join<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
            {
                throw new ArgumentNullException("outer");
            }
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            if (outerKeySelector == null)
            {
                throw new ArgumentNullException("outerKeySelector");
            }
            if (innerKeySelector == null)
            {
                throw new ArgumentNullException("innerKeySelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            ILookup<TKey, TInner> lookup = inner.ToLookup<TInner, TKey>(innerKeySelector, comparer);
            return (from o in outer
                from i in lookup[outerKeySelector(o)]
                select resultSelector(o, i));
        }

        public static TSource Last<TSource>(this IEnumerable<TSource> source)
        {
            return source.LastImpl<TSource>(Futures<TSource>.Undefined);
        }

        public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).Last<TSource>();
        }

        private static TSource LastImpl<TSource>(this IEnumerable<TSource> source, Func<TSource> empty)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count <= 0)
                {
                    return empty();
                }
                return list[list.Count - 1];
            }
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return empty();
                }
                TSource current = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                }
                return current;
            }
        }

        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            return source.LastImpl<TSource>(Futures<TSource>.Default);
        }

        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).LastOrDefault<TSource>();
        }

        public static long LongCount<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            Array array = source as Array;
            if (array == null)
            {
                return source.Aggregate<TSource, long>(0, (count, item) => (count + 1));
            }
            return array.LongLength;
        }

        public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).LongCount<TSource>();
        }

        public static decimal? Max(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<decimal>(null, (max, x) => (!x.HasValue || (max.HasValue && (x.Value < max.Value))));
        }

        public static double? Max(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<double>(null, (max, x) => (!x.HasValue || (max.HasValue && (x.Value < max.Value))));
        }

        public static int? Max(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<int>(null, (max, x) => (!x.HasValue || (max.HasValue && (x.Value < max.Value))));
        }

        public static long? Max(this IEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<long>(null, (max, x) => (!x.HasValue || (max.HasValue && (x.Value < max.Value))));
        }

        public static float? Max(this IEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<float>(null, (max, x) => (!x.HasValue || (max.HasValue && (x.Value < max.Value))));
        }

        public static TSource Max<TSource>(this IEnumerable<TSource> source)
        {
            Comparer<TSource> comparer = Comparer<TSource>.Default;
            return source.MinMaxImpl<TSource>((x, y) => (comparer.Compare(x, y) > 0));
        }

        public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select<TSource, decimal?>(selector).Max();
        }

        public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select<TSource, TResult>(selector).Max<TResult>();
        }

        public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select<TSource, double?>(selector).Max();
        }

        public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select<TSource, int?>(selector).Max();
        }

        public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select<TSource, long?>(selector).Max();
        }

        public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select<TSource, float?>(selector).Max();
        }

        public static decimal? Min(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<decimal>(null, delegate (decimal? min, decimal? x) {
                decimal? nullable = min;
                decimal? nullable2 = x;
                return ((nullable.GetValueOrDefault() < nullable2.GetValueOrDefault()) && (nullable.HasValue & nullable2.HasValue));
            });
        }

        public static int? Min(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<int>(null, delegate (int? min, int? x) {
                int? nullable = min;
                int? nullable2 = x;
                return ((nullable.GetValueOrDefault() < nullable2.GetValueOrDefault()) && (nullable.HasValue & nullable2.HasValue));
            });
        }

        public static float? Min(this IEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<float>(null, delegate (float? min, float? x) {
                float? nullable = min;
                float? nullable2 = x;
                return ((nullable.GetValueOrDefault() < nullable2.GetValueOrDefault()) && (nullable.HasValue & nullable2.HasValue));
            });
        }

        public static TSource Min<TSource>(this IEnumerable<TSource> source)
        {
            Comparer<TSource> comparer = Comparer<TSource>.Default;
            return source.MinMaxImpl<TSource>((x, y) => (comparer.Compare(x, y) < 0));
        }

        public static double? Min(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<double>(null, delegate (double? min, double? x) {
                double? nullable = min;
                double? nullable2 = x;
                return ((nullable.GetValueOrDefault() < nullable2.GetValueOrDefault()) && (nullable.HasValue & nullable2.HasValue));
            });
        }

        public static long? Min(this IEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (from x in source
                where x.HasValue
                select x).MinMaxImpl<long>(null, delegate (long? min, long? x) {
                long? nullable = min;
                long? nullable2 = x;
                return ((nullable.GetValueOrDefault() < nullable2.GetValueOrDefault()) && (nullable.HasValue & nullable2.HasValue));
            });
        }

        public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select<TSource, decimal?>(selector).Min();
        }

        public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select<TSource, double?>(selector).Min();
        }

        public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select<TSource, int?>(selector).Min();
        }

        public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select<TSource, long?>(selector).Min();
        }

        public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select<TSource, float?>(selector).Min();
        }

        public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select<TSource, TResult>(selector).Min<TResult>();
        }

        private static TSource MinMaxImpl<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> lesser)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (typeof(TSource).IsClass)
            {
                source = (from e in source
                    where e != null
                    select e).DefaultIfEmpty<TSource>();
            }
            return source.Aggregate<TSource>(delegate (TSource a, TSource item) {
                if (!lesser(a, item))
                {
                    return item;
                }
                return a;
            });
        }

        private static TSource? MinMaxImpl<TSource>(this IEnumerable<TSource?> source, TSource? seed, Func<TSource?, TSource?, bool> lesser) where TSource: struct
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Aggregate<TSource?, TSource?>(seed, delegate (TSource? a, TSource? item) {
                if (!lesser(a, item))
                {
                    return item;
                }
                return a;
            });
        }

        public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return OfTypeYield<TResult>(source);
        }

        private static IEnumerable<TResult> OfTypeYield<TResult>(IEnumerable source)
        {
            IEnumerator enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                if (current is TResult)
                {
                    yield return (TResult) current;
                }
            }
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.OrderBy<TSource, TKey>(keySelector, null);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, false);
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.OrderByDescending<TSource, TKey>(keySelector, null);
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (source == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, true);
        }

        public static IEnumerable<int> Range(int start, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, null);
            }
            long end = start + count;
            if ((end - 1) >= 0x7fffffff)
            {
                throw new ArgumentOutOfRangeException("count", count, null);
            }
            return RangeYield(start, end);
        }

        private static IEnumerable<int> RangeYield(int start, long end)
        {
            int iteratorVariable0 = start;
            while (true)
            {
                if (iteratorVariable0 >= end)
                {
                    yield break;
                }
                yield return iteratorVariable0;
                iteratorVariable0++;
            }
        }

        private static IEnumerable<T> Renumerable<T>(this IEnumerator<T> e)
        {
            do
            {
                yield return e.Current;
            }
            while (e.MoveNext());
        }

        public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, null);
            }
            return RepeatYield<TResult>(element, count);
        }

        private static IEnumerable<TResult> RepeatYield<TResult>(TResult element, int count)
        {
            int iteratorVariable0 = 0;
            while (true)
            {
                if (iteratorVariable0 >= count)
                {
                    yield break;
                }
                yield return element;
                iteratorVariable0++;
            }
        }

        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return ReverseYield<TSource>(source);
        }

        private static IEnumerable<TSource> ReverseYield<TSource>(IEnumerable<TSource> source)
        {
            Stack<TSource> iteratorVariable0 = new Stack<TSource>();
            foreach (TSource local in source)
            {
                iteratorVariable0.Push(local);
            }
            foreach (TSource iteratorVariable1 in iteratorVariable0)
            {
                yield return iteratorVariable1;
            }
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            return source.Select<TSource, TResult>(((Func<TSource, int, TResult>) ((item, i) => selector(item))));
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            return SelectYield<TSource, TResult>(source, selector);
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            return source.SelectMany<TSource, TResult>(((Func<TSource, int, IEnumerable<TResult>>) ((item, i) => selector(item))));
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            return source.SelectMany<TSource, TResult, TResult>(selector, (item, subitem) => subitem);
        }

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (collectionSelector == null)
            {
                throw new ArgumentNullException("collectionSelector");
            }
            return source.SelectMany<TSource, TCollection, TResult>(((Func<TSource, int, IEnumerable<TCollection>>) ((item, i) => collectionSelector(item))), resultSelector);
        }

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (collectionSelector == null)
            {
                throw new ArgumentNullException("collectionSelector");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            return source.SelectManyYield<TSource, TCollection, TResult>(collectionSelector, resultSelector);
        }

        private static IEnumerable<TResult> SelectManyYield<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            int iteratorVariable0 = 0;
            foreach (TSource iteratorVariable1 in source)
            {
                foreach (TCollection iteratorVariable2 in collectionSelector(iteratorVariable1, iteratorVariable0++))
                {
                    yield return resultSelector(iteratorVariable1, iteratorVariable2);
                }
            }
        }

        private static IEnumerable<TResult> SelectYield<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            int iteratorVariable0 = 0;
            foreach (TSource iteratorVariable1 in source)
            {
                yield return selector(iteratorVariable1, iteratorVariable0++);
            }
        }

        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return first.SequenceEqual<TSource>(second, null);
        }

        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException("frist");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            comparer = comparer ?? EqualityComparer<TSource>.Default;
            using (IEnumerator<TSource> enumerator = first.GetEnumerator())
            {
                using (IEnumerator<TSource> enumerator2 = second.GetEnumerator())
                {
                    do
                    {
                        if (!enumerator.MoveNext())
                        {
                            return !enumerator2.MoveNext();
                        }
                        if (!enumerator2.MoveNext())
                        {
                            return false;
                        }
                    }
                    while (comparer.Equals(enumerator.Current, enumerator2.Current));
                }
            }
            return false;
        }

        public static TSource Single<TSource>(this IEnumerable<TSource> source)
        {
            return source.SingleImpl<TSource>(Futures<TSource>.Undefined);
        }

        public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).Single<TSource>();
        }

        private static TSource SingleImpl<TSource>(this IEnumerable<TSource> source, Func<TSource> empty)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    TSource current = enumerator.Current;
                    if (enumerator.MoveNext())
                    {
                        throw new InvalidOperationException();
                    }
                    return current;
                }
                return empty();
            }
        }

        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            return source.SingleImpl<TSource>(Futures<TSource>.Default);
        }

        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate).SingleOrDefault<TSource>();
        }

        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
        {
            return source.SkipWhile<TSource>(((Func<TSource, int, bool>) ((item, i) => (i < count))));
        }

        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return source.SkipWhile<TSource>(((Func<TSource, int, bool>) ((item, i) => predicate(item))));
        }

        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return SkipWhileYield<TSource>(source, predicate);
        }

        private static IEnumerable<TSource> SkipWhileYield<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            using (IEnumerator<TSource> iteratorVariable0 = source.GetEnumerator())
            {
                int num = 0;
            Label_003F:
                if (iteratorVariable0.MoveNext())
                {
                    if (predicate(iteratorVariable0.Current, num))
                    {
                        num++;
                        goto Label_003F;
                    }
                    do
                    {
                        yield return iteratorVariable0.Current;
                    }
                    while (iteratorVariable0.MoveNext());
                }
            }
        }

        public static decimal Sum(this IEnumerable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            decimal num = 0M;
            foreach (decimal num2 in source)
            {
                num += num2;
            }
            return num;
        }

        public static double Sum(this IEnumerable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            double num = 0.0;
            foreach (double num2 in source)
            {
                num += num2;
            }
            return num;
        }

        public static int Sum(this IEnumerable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            int num = 0;
            foreach (int num2 in source)
            {
                num += num2;
            }
            return num;
        }

        public static int? Sum(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            int num = 0;
            foreach (int? nullable in source)
            {
                int? nullable2 = nullable;
                num += nullable2.HasValue ? nullable2.GetValueOrDefault() : 0;
            }
            return new int?(num);
        }

        public static long? Sum(this IEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long num = 0;
            foreach (long? nullable in source)
            {
                long? nullable2 = nullable;
                num += nullable2.HasValue ? nullable2.GetValueOrDefault() : 0;
            }
            return new long?(num);
        }

        public static long Sum(this IEnumerable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            long num = 0;
            foreach (long num2 in source)
            {
                num += num2;
            }
            return num;
        }

        public static decimal? Sum(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            decimal num = 0M;
            foreach (decimal? nullable in source)
            {
                decimal? nullable2 = nullable;
                num += nullable2.HasValue ? nullable2.GetValueOrDefault() : 0M;
            }
            return new decimal?(num);
        }

        public static double? Sum(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            double num = 0.0;
            foreach (double? nullable in source)
            {
                double? nullable2 = nullable;
                num += nullable2.HasValue ? nullable2.GetValueOrDefault() : 0.0;
            }
            return new double?(num);
        }

        public static float? Sum(this IEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            float num = 0f;
            foreach (float? nullable in source)
            {
                float? nullable2 = nullable;
                num += nullable2.HasValue ? nullable2.GetValueOrDefault() : 0f;
            }
            return new float?(num);
        }

        public static float Sum(this IEnumerable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            float num = 0f;
            foreach (float num2 in source)
            {
                num += num2;
            }
            return num;
        }

        public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select<TSource, decimal>(selector).Sum();
        }

        public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select<TSource, double>(selector).Sum();
        }

        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select<TSource, int>(selector).Sum();
        }

        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select<TSource, long>(selector).Sum();
        }

        public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select<TSource, decimal?>(selector).Sum();
        }

        public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select<TSource, double?>(selector).Sum();
        }

        public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select<TSource, int?>(selector).Sum();
        }

        public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select<TSource, long?>(selector).Sum();
        }

        public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select<TSource, float?>(selector).Sum();
        }

        public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select<TSource, float>(selector).Sum();
        }

        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
        {
            return source.TakeWhile<TSource>(((Func<TSource, int, bool>) ((item, i) => (i < count))));
        }

        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return source.TakeWhile<TSource>(((Func<TSource, int, bool>) ((item, i) => predicate(item))));
        }

        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return source.TakeWhileYield<TSource>(predicate);
        }

        private static IEnumerable<TSource> TakeWhileYield<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            int iteratorVariable0 = 0;
            foreach (TSource iteratorVariable1 in source)
            {
                if (!predicate(iteratorVariable1, iteratorVariable0++))
                {
                    break;
                }
                yield return iteratorVariable1;
            }
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ThenBy<TSource, TKey>(keySelector, null);
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
        }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ThenByDescending<TSource, TKey>(keySelector, null);
        }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
        }

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            return source.ToList<TSource>().ToArray();
        }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ToDictionary<TSource, TKey>(keySelector, null);
        }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.ToDictionary<TSource, TKey, TSource>(keySelector, e => e, comparer);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return source.ToDictionary<TSource, TKey, TElement>(keySelector, elementSelector, null);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }
            Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
            foreach (TSource local in source)
            {
                dictionary.Add(keySelector(local), elementSelector(local));
            }
            return dictionary;
        }

        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return new List<TSource>(source);
        }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ToLookup<TSource, TKey, TSource>(keySelector, e => e, null);
        }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.ToLookup<TSource, TKey, TSource>(keySelector, e => e, comparer);
        }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, null);
        }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }
            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TSource local in source)
            {
                TKey key = keySelector(local);
                Grouping<TKey, TElement> item = (Grouping<TKey, TElement>) lookup.Find(key);
                if (item == null)
                {
                    item = new Grouping<TKey, TElement>(key);
                    lookup.Add(item);
                }
                item.Add(elementSelector(local));
            }
            return lookup;
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return first.Union<TSource>(second, null);
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return first.Concat<TSource>(second).Distinct<TSource>(comparer);
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return source.Where<TSource>(((Func<TSource, int, bool>) ((item, i) => predicate(item))));
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return WhereYield<TSource>(source, predicate);
        }

        private static IEnumerable<TSource> WhereYield<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            int iteratorVariable0 = 0;
            foreach (TSource iteratorVariable1 in source)
            {
                if (predicate(iteratorVariable1, iteratorVariable0++))
                {
                    yield return iteratorVariable1;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <CastYield>d__0<TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public IEnumerable <>3__source;
            public IEnumerator <>7__wrap2;
            public IDisposable <>7__wrap3;
            private int <>l__initialThreadId;
            public object <item>5__1;
            public IEnumerable source;

            [DebuggerHidden]
            public <CastYield>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                this.<>7__wrap3 = this.<>7__wrap2 as IDisposable;
                if (this.<>7__wrap3 != null)
                {
                    this.<>7__wrap3.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap2 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_0070;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_0070;

                        default:
                            goto Label_0083;
                    }
                Label_003C:
                    this.<item>5__1 = this.<>7__wrap2.Current;
                    this.<>2__current = (TResult) this.<item>5__1;
                    this.<>1__state = 2;
                    return true;
                Label_0070:
                    if (this.<>7__wrap2.MoveNext())
                    {
                        goto Label_003C;
                    }
                    this.<>m__Finally4();
                Label_0083:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                Enumerable.<CastYield>d__0<TResult> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<CastYield>d__0<TResult>) this;
                }
                else
                {
                    d__ = new Enumerable.<CastYield>d__0<TResult>(0);
                }
                d__.source = this.<>3__source;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally4();
                        }
                        return;
                }
            }

            TResult IEnumerator<TResult>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ConcatYield>d__63<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public IEnumerable<TSource> <>3__first;
            public IEnumerable<TSource> <>3__second;
            public IEnumerator<TSource> <>7__wrap66;
            public IEnumerator<TSource> <>7__wrap68;
            private int <>l__initialThreadId;
            public TSource <item>5__64;
            public TSource <item>5__65;
            public IEnumerable<TSource> first;
            public IEnumerable<TSource> second;

            [DebuggerHidden]
            public <ConcatYield>d__63(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally67()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap66 != null)
                {
                    this.<>7__wrap66.Dispose();
                }
            }

            private void <>m__Finally69()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap68 != null)
                {
                    this.<>7__wrap68.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap66 = this.first.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_0079;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_0079;

                        case 4:
                            goto Label_00CE;

                        default:
                            goto Label_00E8;
                    }
                Label_0047:
                    this.<item>5__64 = this.<>7__wrap66.Current;
                    this.<>2__current = this.<item>5__64;
                    this.<>1__state = 2;
                    return true;
                Label_0079:
                    if (this.<>7__wrap66.MoveNext())
                    {
                        goto Label_0047;
                    }
                    this.<>m__Finally67();
                    this.<>7__wrap68 = this.second.GetEnumerator();
                    this.<>1__state = 3;
                    while (this.<>7__wrap68.MoveNext())
                    {
                        this.<item>5__65 = this.<>7__wrap68.Current;
                        this.<>2__current = this.<item>5__65;
                        this.<>1__state = 4;
                        return true;
                    Label_00CE:
                        this.<>1__state = 3;
                    }
                    this.<>m__Finally69();
                Label_00E8:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                Enumerable.<ConcatYield>d__63<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<ConcatYield>d__63<TSource>) this;
                }
                else
                {
                    d__ = new Enumerable.<ConcatYield>d__63<TSource>(0);
                }
                d__.first = this.<>3__first;
                d__.second = this.<>3__second;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally67();
                        }
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally69();
                        }
                        return;
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <DefaultIfEmptyYield>d__7e<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public TSource <>3__defaultValue;
            public IEnumerable<TSource> <>3__source;
            private int <>l__initialThreadId;
            public IEnumerator<TSource> <e>5__7f;
            public TSource defaultValue;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <DefaultIfEmptyYield>d__7e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally80()
            {
                this.<>1__state = -1;
                if (this.<e>5__7f != null)
                {
                    this.<e>5__7f.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<e>5__7f = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            if (this.<e>5__7f.MoveNext())
                            {
                                break;
                            }
                            this.<>2__current = this.defaultValue;
                            this.<>1__state = 2;
                            return true;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_009E;

                        case 3:
                            this.<>1__state = 1;
                            if (this.<e>5__7f.MoveNext())
                            {
                                break;
                            }
                            goto Label_009E;

                        default:
                            goto Label_00A4;
                    }
                    this.<>2__current = this.<e>5__7f.Current;
                    this.<>1__state = 3;
                    return true;
                Label_009E:
                    this.<>m__Finally80();
                Label_00A4:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                Enumerable.<DefaultIfEmptyYield>d__7e<TSource> d__e;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__e = (Enumerable.<DefaultIfEmptyYield>d__7e<TSource>) this;
                }
                else
                {
                    d__e = new Enumerable.<DefaultIfEmptyYield>d__7e<TSource>(0);
                }
                d__e.source = this.<>3__source;
                d__e.defaultValue = this.<>3__defaultValue;
                return d__e;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally80();
                        }
                        return;
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <DistinctYield>d__6c<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public IEqualityComparer<TSource> <>3__comparer;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap70;
            private int <>l__initialThreadId;
            public bool <gotNull>5__6e;
            public TSource <item>5__6f;
            public Dictionary<TSource, object> <set>5__6d;
            public IEqualityComparer<TSource> comparer;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <DistinctYield>d__6c(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally71()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap70 != null)
                {
                    this.<>7__wrap70.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<set>5__6d = new Dictionary<TSource, object>(this.comparer);
                            this.<gotNull>5__6e = false;
                            this.<>7__wrap70 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_00C9;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_00C9;

                        default:
                            goto Label_00DC;
                    }
                Label_0057:
                    this.<item>5__6f = this.<>7__wrap70.Current;
                    if (this.<item>5__6f == null)
                    {
                        if (this.<gotNull>5__6e)
                        {
                            goto Label_00C9;
                        }
                        this.<gotNull>5__6e = true;
                    }
                    else
                    {
                        if (this.<set>5__6d.ContainsKey(this.<item>5__6f))
                        {
                            goto Label_00C9;
                        }
                        this.<set>5__6d.Add(this.<item>5__6f, null);
                    }
                    this.<>2__current = this.<item>5__6f;
                    this.<>1__state = 2;
                    return true;
                Label_00C9:
                    if (this.<>7__wrap70.MoveNext())
                    {
                        goto Label_0057;
                    }
                    this.<>m__Finally71();
                Label_00DC:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                Enumerable.<DistinctYield>d__6c<TSource> d__c;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__c = (Enumerable.<DistinctYield>d__6c<TSource>) this;
                }
                else
                {
                    d__c = new Enumerable.<DistinctYield>d__6c<TSource>(0);
                }
                d__c.source = this.<>3__source;
                d__c.comparer = this.<>3__comparer;
                return d__c;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally71();
                        }
                        return;
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OfTypeYield>d__7<TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public IEnumerable <>3__source;
            public IEnumerator <>7__wrap9;
            public IDisposable <>7__wrapa;
            private int <>l__initialThreadId;
            public object <item>5__8;
            public IEnumerable source;

            [DebuggerHidden]
            public <OfTypeYield>d__7(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallyb()
            {
                this.<>1__state = -1;
                this.<>7__wrapa = this.<>7__wrap9 as IDisposable;
                if (this.<>7__wrapa != null)
                {
                    this.<>7__wrapa.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap9 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_007D;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_007D;

                        default:
                            goto Label_0090;
                    }
                Label_003C:
                    this.<item>5__8 = this.<>7__wrap9.Current;
                    if (this.<item>5__8 is TResult)
                    {
                        this.<>2__current = (TResult) this.<item>5__8;
                        this.<>1__state = 2;
                        return true;
                    }
                Label_007D:
                    if (this.<>7__wrap9.MoveNext())
                    {
                        goto Label_003C;
                    }
                    this.<>m__Finallyb();
                Label_0090:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                Enumerable.<OfTypeYield>d__7<TResult> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<OfTypeYield>d__7<TResult>) this;
                }
                else
                {
                    d__ = new Enumerable.<OfTypeYield>d__7<TResult>(0);
                }
                d__.source = this.<>3__source;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallyb();
                        }
                        return;
                }
            }

            TResult IEnumerator<TResult>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RangeYield>d__e : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private int <>2__current;
            public long <>3__end;
            public int <>3__start;
            private int <>l__initialThreadId;
            public int <i>5__f;
            public long end;
            public int start;

            [DebuggerHidden]
            public <RangeYield>d__e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<i>5__f = this.start;
                        break;

                    case 1:
                        this.<>1__state = -1;
                        this.<i>5__f++;
                        break;

                    default:
                        goto Label_0065;
                }
                if (this.<i>5__f < this.end)
                {
                    this.<>2__current = this.<i>5__f;
                    this.<>1__state = 1;
                    return true;
                }
            Label_0065:
                return false;
            }

            [DebuggerHidden]
            IEnumerator<int> IEnumerable<int>.GetEnumerator()
            {
                Enumerable.<RangeYield>d__e _e;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    _e = this;
                }
                else
                {
                    _e = new Enumerable.<RangeYield>d__e(0);
                }
                _e.start = this.<>3__start;
                _e.end = this.<>3__end;
                return _e;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Int32>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            int IEnumerator<int>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Renumerable>d__93<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private T <>2__current;
            public IEnumerator<T> <>3__e;
            private int <>l__initialThreadId;
            public IEnumerator<T> e;

            [DebuggerHidden]
            public <Renumerable>d__93(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        break;

                    case 1:
                        this.<>1__state = -1;
                        if (this.e.MoveNext())
                        {
                            break;
                        }
                        goto Label_004C;

                    default:
                        goto Label_004C;
                }
                this.<>2__current = this.e.Current;
                this.<>1__state = 1;
                return true;
            Label_004C:
                return false;
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                Enumerable.<Renumerable>d__93<T> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<Renumerable>d__93<T>) this;
                }
                else
                {
                    d__ = new Enumerable.<Renumerable>d__93<T>(0);
                }
                d__.e = this.<>3__e;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            T IEnumerator<T>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RepeatYield>d__12<TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public int <>3__count;
            public TResult <>3__element;
            private int <>l__initialThreadId;
            public int <i>5__13;
            public int count;
            public TResult element;

            [DebuggerHidden]
            public <RepeatYield>d__12(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<i>5__13 = 0;
                        break;

                    case 1:
                        this.<>1__state = -1;
                        this.<i>5__13++;
                        break;

                    default:
                        goto Label_005F;
                }
                if (this.<i>5__13 < this.count)
                {
                    this.<>2__current = this.element;
                    this.<>1__state = 1;
                    return true;
                }
            Label_005F:
                return false;
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                Enumerable.<RepeatYield>d__12<TResult> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<RepeatYield>d__12<TResult>) this;
                }
                else
                {
                    d__ = new Enumerable.<RepeatYield>d__12<TResult>(0);
                }
                d__.element = this.<>3__element;
                d__.count = this.<>3__count;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            TResult IEnumerator<TResult>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ReverseYield>d__4f<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public IEnumerable<TSource> <>3__source;
            public Stack<TSource>.Enumerator <>7__wrap52;
            private int <>l__initialThreadId;
            public TSource <item>5__51;
            public Stack<TSource> <stack>5__50;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <ReverseYield>d__4f(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally53()
            {
                this.<>1__state = -1;
                this.<>7__wrap52.Dispose();
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<stack>5__50 = new Stack<TSource>();
                            foreach (TSource local in this.source)
                            {
                                this.<stack>5__50.Push(local);
                            }
                            this.<>7__wrap52 = this.<stack>5__50.GetEnumerator();
                            this.<>1__state = 2;
                            while (this.<>7__wrap52.MoveNext())
                            {
                                this.<item>5__51 = this.<>7__wrap52.Current;
                                this.<>2__current = this.<item>5__51;
                                this.<>1__state = 3;
                                return true;
                            Label_00A0:
                                this.<>1__state = 2;
                            }
                            this.<>m__Finally53();
                            break;

                        case 3:
                            goto Label_00A0;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                Enumerable.<ReverseYield>d__4f<TSource> d__f;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__f = (Enumerable.<ReverseYield>d__4f<TSource>) this;
                }
                else
                {
                    d__f = new Enumerable.<ReverseYield>d__4f<TSource>(0);
                }
                d__f.source = this.<>3__source;
                return d__f;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally53();
                        }
                        return;
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <SelectManyYield>d__31<TSource, TCollection, TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public Func<TSource, int, IEnumerable<TCollection>> <>3__collectionSelector;
            public Func<TSource, TCollection, TResult> <>3__resultSelector;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap35;
            public IEnumerator<TCollection> <>7__wrap37;
            private int <>l__initialThreadId;
            public int <i>5__32;
            public TSource <item>5__33;
            public TCollection <subitem>5__34;
            public Func<TSource, int, IEnumerable<TCollection>> collectionSelector;
            public Func<TSource, TCollection, TResult> resultSelector;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <SelectManyYield>d__31(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally36()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap35 != null)
                {
                    this.<>7__wrap35.Dispose();
                }
            }

            private void <>m__Finally38()
            {
                this.<>1__state = 1;
                if (this.<>7__wrap37 != null)
                {
                    this.<>7__wrap37.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<i>5__32 = 0;
                            this.<>7__wrap35 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap35.MoveNext())
                            {
                                this.<item>5__33 = this.<>7__wrap35.Current;
                                this.<>7__wrap37 = this.collectionSelector(this.<item>5__33, this.<i>5__32++).GetEnumerator();
                                this.<>1__state = 2;
                                while (this.<>7__wrap37.MoveNext())
                                {
                                    this.<subitem>5__34 = this.<>7__wrap37.Current;
                                    this.<>2__current = this.resultSelector(this.<item>5__33, this.<subitem>5__34);
                                    this.<>1__state = 3;
                                    return true;
                                Label_00C2:
                                    this.<>1__state = 2;
                                }
                                this.<>m__Finally38();
                            }
                            this.<>m__Finally36();
                            break;

                        case 3:
                            goto Label_00C2;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                Enumerable.<SelectManyYield>d__31<TSource, TCollection, TResult> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<SelectManyYield>d__31<TSource, TCollection, TResult>) this;
                }
                else
                {
                    d__ = new Enumerable.<SelectManyYield>d__31<TSource, TCollection, TResult>(0);
                }
                d__.source = this.<>3__source;
                d__.collectionSelector = this.<>3__collectionSelector;
                d__.resultSelector = this.<>3__resultSelector;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                    case 3:
                        try
                        {
                            switch (this.<>1__state)
                            {
                                case 2:
                                case 3:
                                    try
                                    {
                                    }
                                    finally
                                    {
                                        this.<>m__Finally38();
                                    }
                                    break;
                            }
                        }
                        finally
                        {
                            this.<>m__Finally36();
                        }
                        break;

                    default:
                        return;
                }
            }

            TResult IEnumerator<TResult>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <SelectYield>d__23<TSource, TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public Func<TSource, int, TResult> <>3__selector;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap26;
            private int <>l__initialThreadId;
            public int <i>5__24;
            public TSource <item>5__25;
            public Func<TSource, int, TResult> selector;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <SelectYield>d__23(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally27()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap26 != null)
                {
                    this.<>7__wrap26.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<i>5__24 = 0;
                            this.<>7__wrap26 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_0091;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_0091;

                        default:
                            goto Label_00A4;
                    }
                Label_0046:
                    this.<item>5__25 = this.<>7__wrap26.Current;
                    this.<>2__current = this.selector(this.<item>5__25, this.<i>5__24++);
                    this.<>1__state = 2;
                    return true;
                Label_0091:
                    if (this.<>7__wrap26.MoveNext())
                    {
                        goto Label_0046;
                    }
                    this.<>m__Finally27();
                Label_00A4:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                Enumerable.<SelectYield>d__23<TSource, TResult> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<SelectYield>d__23<TSource, TResult>) this;
                }
                else
                {
                    d__ = new Enumerable.<SelectYield>d__23<TSource, TResult>(0);
                }
                d__.source = this.<>3__source;
                d__.selector = this.<>3__selector;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally27();
                        }
                        return;
                }
            }

            TResult IEnumerator<TResult>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <SkipWhileYield>d__59<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public Func<TSource, int, bool> <>3__predicate;
            public IEnumerable<TSource> <>3__source;
            private int <>l__initialThreadId;
            public IEnumerator<TSource> <e>5__5a;
            public Func<TSource, int, bool> predicate;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <SkipWhileYield>d__59(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5b()
            {
                this.<>1__state = -1;
                if (this.<e>5__5a != null)
                {
                    this.<e>5__5a.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            this.<>1__state = 1;
                            if (this.<e>5__5a.MoveNext())
                            {
                                goto Label_0073;
                            }
                            this.<>m__Finally5b();
                            goto Label_00A9;

                        default:
                            goto Label_00A9;
                    }
                    this.<>1__state = -1;
                    this.<e>5__5a = this.source.GetEnumerator();
                    this.<>1__state = 1;
                    int num = 0;
                Label_003F:
                    if (!this.<e>5__5a.MoveNext())
                    {
                        this.System.IDisposable.Dispose();
                        goto Label_00A9;
                    }
                    if (this.predicate(this.<e>5__5a.Current, num))
                    {
                        num++;
                        goto Label_003F;
                    }
                Label_0073:
                    this.<>2__current = this.<e>5__5a.Current;
                    this.<>1__state = 2;
                    return true;
                Label_00A9:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                Enumerable.<SkipWhileYield>d__59<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<SkipWhileYield>d__59<TSource>) this;
                }
                else
                {
                    d__ = new Enumerable.<SkipWhileYield>d__59<TSource>(0);
                }
                d__.source = this.<>3__source;
                d__.predicate = this.<>3__predicate;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally5b();
                        }
                        return;
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <TakeWhileYield>d__3e<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public Func<TSource, int, bool> <>3__predicate;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap41;
            private int <>l__initialThreadId;
            public int <i>5__3f;
            public TSource <item>5__40;
            public Func<TSource, int, bool> predicate;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <TakeWhileYield>d__3e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally42()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap41 != null)
                {
                    this.<>7__wrap41.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<i>5__3f = 0;
                            this.<>7__wrap41 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_0099;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_0099;

                        default:
                            goto Label_00AC;
                    }
                Label_0046:
                    this.<item>5__40 = this.<>7__wrap41.Current;
                    if (!this.predicate(this.<item>5__40, this.<i>5__3f++))
                    {
                        goto Label_00A6;
                    }
                    this.<>2__current = this.<item>5__40;
                    this.<>1__state = 2;
                    return true;
                Label_0099:
                    if (this.<>7__wrap41.MoveNext())
                    {
                        goto Label_0046;
                    }
                Label_00A6:
                    this.<>m__Finally42();
                Label_00AC:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                Enumerable.<TakeWhileYield>d__3e<TSource> d__e;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__e = (Enumerable.<TakeWhileYield>d__3e<TSource>) this;
                }
                else
                {
                    d__e = new Enumerable.<TakeWhileYield>d__3e<TSource>(0);
                }
                d__e.source = this.<>3__source;
                d__e.predicate = this.<>3__predicate;
                return d__e;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally42();
                        }
                        return;
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <WhereYield>d__19<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public Func<TSource, int, bool> <>3__predicate;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap1c;
            private int <>l__initialThreadId;
            public int <i>5__1a;
            public TSource <item>5__1b;
            public Func<TSource, int, bool> predicate;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <WhereYield>d__19(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally1d()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap1c != null)
                {
                    this.<>7__wrap1c.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<i>5__1a = 0;
                            this.<>7__wrap1c = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_0099;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_0099;

                        default:
                            goto Label_00AC;
                    }
                Label_0046:
                    this.<item>5__1b = this.<>7__wrap1c.Current;
                    if (this.predicate(this.<item>5__1b, this.<i>5__1a++))
                    {
                        this.<>2__current = this.<item>5__1b;
                        this.<>1__state = 2;
                        return true;
                    }
                Label_0099:
                    if (this.<>7__wrap1c.MoveNext())
                    {
                        goto Label_0046;
                    }
                    this.<>m__Finally1d();
                Label_00AC:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                Enumerable.<WhereYield>d__19<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Enumerable.<WhereYield>d__19<TSource>) this;
                }
                else
                {
                    d__ = new Enumerable.<WhereYield>d__19<TSource>(0);
                }
                d__.source = this.<>3__source;
                d__.predicate = this.<>3__predicate;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally1d();
                        }
                        return;
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        private static class Futures<T>
        {
            public static readonly Func<T> Default;
            public static readonly Func<T> Undefined;

            static Futures()
            {
                Enumerable.Futures<T>.Default = () => default(T);
                Enumerable.Futures<T>.Undefined = delegate {
                    throw new InvalidOperationException();
                };
            }
        }

        private sealed class Grouping<K, V> : List<V>, IGrouping<K, V>, IEnumerable<V>, IEnumerable
        {
            internal Grouping(K key)
            {
                this.Key = key;
            }

            public K Key
            {
                [CompilerGenerated]
                get
                {
                    return this.<Key>k__BackingField;
                }
                [CompilerGenerated]
                private set
                {
                    this.<Key>k__BackingField = value;
                }
            }
        }

        private static class Sequence<T>
        {
            public static readonly IEnumerable<T> Empty;

            static Sequence()
            {
                Enumerable.Sequence<T>.Empty = new T[0];
            }
        }
    }
}

