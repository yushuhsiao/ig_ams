using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Text
{
	[System.Diagnostics.DebuggerStepThrough]
    public static class _Extensions
    {
        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, char[] value, int startIndex, int charCount)
        {
            int n = s.Length;
            var r = s.Insert(index, value, startIndex, charCount);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, char[] value, int startIndex, int charCount) => s.Insert(ref index, out int len, value, startIndex, charCount);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, ushort value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, ushort value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, object value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, object value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, ulong value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, ulong value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, uint value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, uint value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, decimal value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, decimal value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, bool value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, bool value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, float value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, float value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, string value, int count)
        {
            int n = s.Length;
            var r = s.Insert(index, value, count);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, string value, int count) => s.Insert(ref index, out int len, value, count);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, double value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, double value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, sbyte value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, sbyte value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, byte value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, byte value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, short value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, short value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, string value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, string value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, char[] value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, char[] value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, int value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, int value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, long value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, long value) => s.Insert(ref index, out int len, value);

        public static StringBuilder Insert(this StringBuilder s, ref int index, out int len, char value)
        {
            int n = s.Length;
            var r = s.Insert(index, value);
            len = s.Length - n;
            index += len;
            return r;
        }
        public static StringBuilder Insert(this StringBuilder s, ref int index, char value) => s.Insert(ref index, out int len, value);
    }
}
