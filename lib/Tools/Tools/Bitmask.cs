using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
using UInt08 = System.Byte;
using SInt08 = System.SByte;
using SInt16 = System.Int16;
using SInt32 = System.Int32;
using SInt64 = System.Int64;


namespace System.Bits
{
    [_DebuggerStepThrough]
    public static unsafe class Bitmask
    {
        abstract class mask<M, T>
            where M : mask<M, T>, new()
            where T : struct
        {
            public static readonly M n = new M();

            T[][] nn = new T[3][];
            public T[] this[int index] { get { lock (this) return nn[index] = nn[index] ?? create(index); } }

            protected abstract int NumberOfBytes { get; }
            protected abstract T FromUInt64(UInt64 n);

            protected virtual T[] create(int index)
            {
                UInt64[] src = mask_UInt64.n[index];
                T[] dst = new T[this.NumberOfBytes * 8];
                for (int i = dst.Length - 1; i >= 0; i--)
                    dst[i] = this.FromUInt64(src[i]);
                return dst;
            }
        }

        class mask_SInt08 : mask<mask_SInt08, SInt08>
        {
            protected override int NumberOfBytes { get { return sizeof(SInt08); } }
            protected override SInt08 FromUInt64(UInt64 n) { return (SInt08)n; }
        }
        class mask_UInt08 : mask<mask_UInt08, UInt08>
        {
            protected override int NumberOfBytes { get { return sizeof(UInt08); } }
            protected override UInt08 FromUInt64(UInt64 n) { return (UInt08)n; }
        }
        class mask_SInt16 : mask<mask_SInt16, SInt16>
        {
            protected override int NumberOfBytes { get { return sizeof(SInt16); } }
            protected override SInt16 FromUInt64(UInt64 n) { return (SInt16)n; }
        }
        class mask_UInt16 : mask<mask_UInt16, UInt16>
        {
            protected override int NumberOfBytes { get { return sizeof(UInt16); } }
            protected override UInt16 FromUInt64(UInt64 n) { return (UInt16)n; }
        }
        class mask_SInt32 : mask<mask_SInt32, SInt32>
        {
            protected override int NumberOfBytes { get { return sizeof(SInt32); } }
            protected override SInt32 FromUInt64(UInt64 n) { return (SInt32)n; }
        }
        class mask_UInt32 : mask<mask_UInt32, UInt32>
        {
            protected override int NumberOfBytes { get { return sizeof(UInt32); } }
            protected override UInt32 FromUInt64(UInt64 n) { return (UInt32)n; }
        }
        class mask_SInt64 : mask<mask_SInt64, SInt64>
        {
            protected override int NumberOfBytes { get { return sizeof(SInt64); } }
            protected override SInt64 FromUInt64(UInt64 n) { return (SInt64)n; }
        }
        class mask_UInt64 : mask<mask_UInt64, UInt64>
        {
            protected override SInt32 NumberOfBytes { get { throw new NotImplementedException(); } }
            protected override UInt64 FromUInt64(UInt64 n) { throw new NotImplementedException(); }
            protected override UInt64[] create(int index)
            {
                if ((index != 0) && (index != 1) && (index != 2))
                    throw new NotImplementedException();
                UInt64[] dst = new UInt64[sizeof(UInt64) * 8];
                if (index == 0)
                {
                    for (int i = 0; i < dst.Length; i++)
                    {
                        dst[i] = 1;
                        dst[i] <<= i;
                    }
                }
                else if (index == 1)
                {
                    for (int i = 0; i < dst.Length; i++)
                    {
                        dst[i] = 1;
                        dst[i] <<= i;
                        dst[i] ^= 0xffffffffffffffff;
                    }
                }
                else if (index == 2)
                {
                    UInt64 tmp = 0;
                    for (int i = 0; i < dst.Length; i++)
                    {
                        dst[i] = tmp;
                        tmp <<= 1;
                        tmp |= 1;
                    }
                    dst[0] = 0xffffffffffffffff;
                }
                return dst;
            }
        }

        public static SInt08 GetBits(this SInt08 src, int position, int length) { src >>= position; src &= mask_SInt08.n[2][length]; return src; }
        public static UInt08 GetBits(this UInt08 src, int position, int length) { src >>= position; src &= mask_UInt08.n[2][length]; return src; }
        public static SInt16 GetBits(this SInt16 src, int position, int length) { src >>= position; src &= mask_SInt16.n[2][length]; return src; }
        public static UInt16 GetBits(this UInt16 src, int position, int length) { src >>= position; src &= mask_UInt16.n[2][length]; return src; }
        public static SInt32 GetBits(this SInt32 src, int position, int length) { src >>= position; src &= mask_SInt32.n[2][length]; return src; }
        public static UInt32 GetBits(this UInt32 src, int position, int length) { src >>= position; src &= mask_UInt32.n[2][length]; return src; }
        public static SInt64 GetBits(this SInt64 src, int position, int length) { src >>= position; src &= mask_SInt64.n[2][length]; return src; }
        public static UInt64 GetBits(this UInt64 src, int position, int length) { src >>= position; src &= mask_UInt64.n[2][length]; return src; }

        public static SInt08 SetBits(this SInt08 src, int position, int length, SInt08 value) { return (SInt08)SetBits((UInt64)src, position, length, (UInt64)value); }
        public static UInt08 SetBits(this UInt08 src, int position, int length, UInt08 value)
        {
            UInt08[] _2 = mask_UInt08.n[2];
            UInt08 mask2 = _2[length];
            value &= mask2;
            mask2 <<= position;
            value <<= position;
            mask2 ^= _2[0];
            src &= mask2;
            src |= value;
            return src;
        }
        public static SInt16 SetBits(this SInt16 src, int position, int length, SInt16 value) { return (SInt16)SetBits((UInt64)src, position, length, (UInt64)value); }
        public static UInt16 SetBits(this UInt16 src, int position, int length, UInt16 value)
        {
            UInt16[] _2 = mask_UInt16.n[2];
            UInt16 mask2 = _2[length];
            value &= mask2;
            mask2 <<= position;
            value <<= position;
            mask2 ^= _2[0];
            src &= mask2;
            src |= value;
            return src;
        }
        public static SInt32 SetBits(this SInt32 src, int position, int length, SInt32 value) { return (SInt32)SetBits((UInt64)src, position, length, (UInt64)value); }
        public static UInt32 SetBits(this UInt32 src, int position, int length, UInt32 value)
        {
            UInt32[] _2 = mask_UInt32.n[2];
            UInt32 mask2 = _2[length];
            value &= mask2;
            mask2 <<= position;
            value <<= position;
            mask2 ^= _2[0];
            src &= mask2;
            src |= value;
            return src;
        }
        public static SInt64 SetBits(this SInt64 src, int position, int length, SInt64 value) { return (SInt64)SetBits((UInt64)src, position, length, (UInt64)value); }
        public static UInt64 SetBits(this UInt64 src, int position, int length, UInt64 value)
        {
            UInt64[] _2 = mask_UInt64.n[2];
            UInt64 mask2 = _2[length];
            value &= mask2;
            mask2 <<= position;
            value <<= position;
            mask2 ^= _2[0];
            src &= mask2;
            src |= value;
            return src;
        }

        public static bool GetBit(this SInt08 n, int position) { n &= mask_SInt08.n[0][position]; return n != 0; }
        public static bool GetBit(this UInt08 n, int position) { n &= mask_UInt08.n[0][position]; return n != 0; }
        public static bool GetBit(this SInt16 n, int position) { n &= mask_SInt16.n[0][position]; return n != 0; }
        public static bool GetBit(this UInt16 n, int position) { n &= mask_UInt16.n[0][position]; return n != 0; }
        public static bool GetBit(this SInt32 n, int position) { n &= mask_SInt32.n[0][position]; return n != 0; }
        public static bool GetBit(this UInt32 n, int position) { n &= mask_UInt32.n[0][position]; return n != 0; }
        public static bool GetBit(this SInt64 n, int position) { n &= mask_SInt64.n[0][position]; return n != 0; }
        public static bool GetBit(this UInt64 n, int position) { n &= mask_UInt64.n[0][position]; return n != 0; }

#pragma warning disable 675
        public static SInt08 SetBit(this SInt08 n, int position, bool value) { if (value) n |= mask_SInt08.n[0][position]; else n &= mask_SInt08.n[1][position]; return n; }
        public static UInt08 SetBit(this UInt08 n, int position, bool value) { if (value) n |= mask_UInt08.n[0][position]; else n &= mask_UInt08.n[1][position]; return n; }
        public static SInt16 SetBit(this SInt16 n, int position, bool value) { if (value) n |= mask_SInt16.n[0][position]; else n &= mask_SInt16.n[1][position]; return n; }
        public static UInt16 SetBit(this UInt16 n, int position, bool value) { if (value) n |= mask_UInt16.n[0][position]; else n &= mask_UInt16.n[1][position]; return n; }
        public static SInt32 SetBit(this SInt32 n, int position, bool value) { if (value) n |= mask_SInt32.n[0][position]; else n &= mask_SInt32.n[1][position]; return n; }
        public static UInt32 SetBit(this UInt32 n, int position, bool value) { if (value) n |= mask_UInt32.n[0][position]; else n &= mask_UInt32.n[1][position]; return n; }
        public static SInt64 SetBit(this SInt64 n, int position, bool value) { if (value) n |= mask_SInt64.n[0][position]; else n &= mask_SInt64.n[1][position]; return n; }
        public static UInt64 SetBit(this UInt64 n, int position, bool value) { if (value) n |= mask_UInt64.n[0][position]; else n &= mask_UInt64.n[1][position]; return n; }
#pragma warning restore 675
    }
}