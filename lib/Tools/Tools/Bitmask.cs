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

namespace System
{
    [_DebuggerStepThrough]
    public static unsafe class Bitmask
    {
        static UInt64[] UInt64_bits = new UInt64[]
        {
            0x0000000000000001, 0x0000000000000002, 0x0000000000000004, 0x0000000000000008,
            0x0000000000000010, 0x0000000000000020, 0x0000000000000040, 0x0000000000000080,
            0x0000000000000100, 0x0000000000000200, 0x0000000000000400, 0x0000000000000800,
            0x0000000000001000, 0x0000000000002000, 0x0000000000004000, 0x0000000000008000,
            0x0000000000010000, 0x0000000000020000, 0x0000000000040000, 0x0000000000080000,
            0x0000000000100000, 0x0000000000200000, 0x0000000000400000, 0x0000000000800000,
            0x0000000001000000, 0x0000000002000000, 0x0000000004000000, 0x0000000008000000,
            0x0000000010000000, 0x0000000020000000, 0x0000000040000000, 0x0000000080000000,
            0x0000000100000000, 0x0000000200000000, 0x0000000400000000, 0x0000000800000000,
            0x0000001000000000, 0x0000002000000000, 0x0000004000000000, 0x0000008000000000,
            0x0000010000000000, 0x0000020000000000, 0x0000040000000000, 0x0000080000000000,
            0x0000100000000000, 0x0000200000000000, 0x0000400000000000, 0x0000800000000000,
            0x0001000000000000, 0x0002000000000000, 0x0004000000000000, 0x0008000000000000,
            0x0010000000000000, 0x0020000000000000, 0x0040000000000000, 0x0080000000000000,
            0x0100000000000000, 0x0200000000000000, 0x0400000000000000, 0x0800000000000000,
            0x1000000000000000, 0x2000000000000000, 0x4000000000000000, 0x8000000000000000,
        };
        static UInt64[] UInt64_mask = new UInt64[]
        {
            0x0000000000000001, 0x0000000000000003, 0x0000000000000007, 0x000000000000000F,
            0x000000000000001F, 0x000000000000003F, 0x000000000000007F, 0x00000000000000FF,
            0x00000000000001FF, 0x00000000000003FF, 0x00000000000007FF, 0x0000000000000FFF,
            0x0000000000001FFF, 0x0000000000003FFF, 0x0000000000007FFF, 0x000000000000FFFF,
            0x000000000001FFFF, 0x000000000003FFFF, 0x000000000007FFFF, 0x00000000000FFFFF,
            0x00000000001FFFFF, 0x00000000003FFFFF, 0x00000000007FFFFF, 0x0000000000FFFFFF,
            0x0000000001FFFFFF, 0x0000000003FFFFFF, 0x0000000007FFFFFF, 0x000000000FFFFFFF,
            0x000000001FFFFFFF, 0x000000003FFFFFFF, 0x000000007FFFFFFF, 0x00000000FFFFFFFF,
            0x00000001FFFFFFFF, 0x00000003FFFFFFFF, 0x00000007FFFFFFFF, 0x0000000FFFFFFFFF,
            0x0000001FFFFFFFFF, 0x0000003FFFFFFFFF, 0x0000007FFFFFFFFF, 0x000000FFFFFFFFFF,
            0x000001FFFFFFFFFF, 0x000003FFFFFFFFFF, 0x000007FFFFFFFFFF, 0x00000FFFFFFFFFFF,
            0x00001FFFFFFFFFFF, 0x00003FFFFFFFFFFF, 0x00007FFFFFFFFFFF, 0x0000FFFFFFFFFFFF,
            0x0001FFFFFFFFFFFF, 0x0003FFFFFFFFFFFF, 0x0007FFFFFFFFFFFF, 0x000FFFFFFFFFFFFF,
            0x001FFFFFFFFFFFFF, 0x003FFFFFFFFFFFFF, 0x007FFFFFFFFFFFFF, 0x00FFFFFFFFFFFFFF,
            0x01FFFFFFFFFFFFFF, 0x03FFFFFFFFFFFFFF, 0x07FFFFFFFFFFFFFF, 0x0FFFFFFFFFFFFFFF,
            0x1FFFFFFFFFFFFFFF, 0x3FFFFFFFFFFFFFFF, 0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF,
        };
        static UInt32[] UInt32_bits = new UInt32[]
        {
            0x00000001, 0x00000002, 0x00000004, 0x00000008,
            0x00000010, 0x00000020, 0x00000040, 0x00000080,
            0x00000100, 0x00000200, 0x00000400, 0x00000800,
            0x00001000, 0x00002000, 0x00004000, 0x00008000,
            0x00010000, 0x00020000, 0x00040000, 0x00080000,
            0x00100000, 0x00200000, 0x00400000, 0x00800000,
            0x01000000, 0x02000000, 0x04000000, 0x08000000,
            0x10000000, 0x20000000, 0x40000000, 0x80000000,
        };
        static UInt32[] UInt32_mask = new UInt32[]
        {
            0x00000001, 0x00000003, 0x00000007, 0x0000000F,
            0x0000001F, 0x0000003F, 0x0000007F, 0x000000FF,
            0x000001FF, 0x000003FF, 0x000007FF, 0x00000FFF,
            0x00001FFF, 0x00003FFF, 0x00007FFF, 0x0000FFFF,
            0x0001FFFF, 0x0003FFFF, 0x0007FFFF, 0x000FFFFF,
            0x001FFFFF, 0x003FFFFF, 0x007FFFFF, 0x00FFFFFF,
            0x01FFFFFF, 0x03FFFFFF, 0x07FFFFFF, 0x0FFFFFFF,
            0x1FFFFFFF, 0x3FFFFFFF, 0x7FFFFFFF, 0xFFFFFFFF,
        };
        static UInt16[] UInt16_bits = new UInt16[] { 0x0001, 0x0002, 0x0004, 0x0008, 0x0010, 0x0020, 0x0040, 0x0080, 0x0100, 0x0200, 0x0400, 0x0800, 0x1000, 0x2000, 0x4000, 0x8000, };
        static UInt16[] UInt16_mask = new UInt16[] { 0x0001, 0x0003, 0x0007, 0x000F, 0x001F, 0x003F, 0x007F, 0x00FF, 0x01FF, 0x03FF, 0x07FF, 0x0FFF, 0x1FFF, 0x3FFF, 0x7FFF, 0xFFFF, };
        static UInt08[] UInt08_bits = new UInt08[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, };
        static UInt08[] UInt08_mask = new UInt08[] { 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F, 0x7F, 0xFF, };

        //abstract class op<M, T>
        //    where M : new()
        //    where T : struct
        //{
        //    public static readonly M obj = new M();
        //    private readonly T[] _bits;
        //    private readonly T[] _masks;
        //    public T bit(int index) => Value(__bit[index]);
        //    public T mask(int index) => Value(__mask[index]);
        //    protected abstract int Sizeof { get; }
        //    protected abstract T Value(ulong value);

        //    public op()
        //    {
        //        lock (this)
        //        {
        //            int bits = this.Sizeof * 8;
        //            _bits = new T[bits];
        //            _masks = new T[bits];
        //            ulong m1 = 1, m2 = 1;
        //            for (int i = 0; i < bits; i++)
        //            {
        //                _bits[i] = Value(m1);
        //                _masks[i] = Value(m2);
        //                m1 <<= 1;
        //                m2 <<= 1;
        //                m2 |= 1;
        //            }
        //        }
        //    }
        //}
        //class op_UInt08 : op<op_UInt08, UInt08> { protected override int Sizeof => sizeof(UInt08); protected override UInt08 Value(ulong value) => (UInt08)value; }
        //class op_SInt08 : op<op_SInt08, SInt08> { protected override int Sizeof => sizeof(SInt08); protected override SInt08 Value(ulong value) => (SInt08)value; }
        //class op_UInt16 : op<op_UInt16, UInt16> { protected override int Sizeof => sizeof(UInt16); protected override UInt16 Value(ulong value) => (UInt16)value; }
        //class op_SInt16 : op<op_SInt16, SInt16> { protected override int Sizeof => sizeof(SInt16); protected override SInt16 Value(ulong value) => (SInt16)value; }
        //class op_UInt32 : op<op_UInt32, UInt32> { protected override int Sizeof => sizeof(UInt32); protected override UInt32 Value(ulong value) => (UInt32)value; }
        //class op_SInt32 : op<op_SInt32, SInt32> { protected override int Sizeof => sizeof(SInt32); protected override SInt32 Value(ulong value) => (SInt32)value; }
        //class op_UInt64 : op<op_UInt64, UInt64> { protected override int Sizeof => sizeof(UInt64); protected override UInt64 Value(ulong value) => (UInt64)value; }
        //class op_SInt64 : op<op_SInt64, SInt64> { protected override int Sizeof => sizeof(SInt64); protected override SInt64 Value(ulong value) => (SInt64)value; }

        public static SInt08 GetBits(this SInt08 src, int position, int length) { SInt08 m = (SInt08)UInt08_mask[length - 1]; src >>= position; src &= m; return src; }
        public static UInt08 GetBits(this UInt08 src, int position, int length) { UInt08 m = (UInt08)UInt08_mask[length - 1]; src >>= position; src &= m; return src; }
        public static SInt16 GetBits(this SInt16 src, int position, int length) { SInt16 m = (SInt16)UInt16_mask[length - 1]; src >>= position; src &= m; return src; }
        public static UInt16 GetBits(this UInt16 src, int position, int length) { UInt16 m = (UInt16)UInt16_mask[length - 1]; src >>= position; src &= m; return src; }
        public static SInt32 GetBits(this SInt32 src, int position, int length) { SInt32 m = (SInt32)UInt32_mask[length - 1]; src >>= position; src &= m; return src; }
        public static UInt32 GetBits(this UInt32 src, int position, int length) { UInt32 m = (UInt32)UInt32_mask[length - 1]; src >>= position; src &= m; return src; }
        public static SInt64 GetBits(this SInt64 src, int position, int length) { SInt64 m = (SInt64)UInt64_mask[length - 1]; src >>= position; src &= m; return src; }
        public static UInt64 GetBits(this UInt64 src, int position, int length) { UInt64 m = (UInt64)UInt64_mask[length - 1]; src >>= position; src &= m; return src; }

        public static SInt08 SetBits(this SInt08 src, int position, int length, SInt08 value) { SInt08 m = (SInt08)UInt08_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }
        public static UInt08 SetBits(this UInt08 src, int position, int length, UInt08 value) { UInt08 m = (UInt08)UInt08_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }
        public static SInt16 SetBits(this SInt16 src, int position, int length, SInt16 value) { SInt16 m = (SInt16)UInt16_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }
        public static UInt16 SetBits(this UInt16 src, int position, int length, UInt16 value) { UInt16 m = (UInt16)UInt16_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }
        public static SInt32 SetBits(this SInt32 src, int position, int length, SInt32 value) { SInt32 m = (SInt32)UInt32_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }
        public static UInt32 SetBits(this UInt32 src, int position, int length, UInt32 value) { UInt32 m = (UInt32)UInt32_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }
        public static SInt64 SetBits(this SInt64 src, int position, int length, SInt64 value) { SInt64 m = (SInt64)UInt64_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }
        public static UInt64 SetBits(this UInt64 src, int position, int length, UInt64 value) { UInt64 m = (UInt64)UInt64_mask[length - 1]; value &= m; value <<= position; m <<= position; src |= m; src ^= m; src |= value; return src; }

        public static bool GetBit(this SInt08 n, int position) { SInt08 m = (SInt08)UInt08_bits[position]; n &= m; return n != 0; }
        public static bool GetBit(this UInt08 n, int position) { UInt08 m = (UInt08)UInt08_bits[position]; n &= m; return n != 0; }
        public static bool GetBit(this SInt16 n, int position) { SInt16 m = (SInt16)UInt16_bits[position]; n &= m; return n != 0; }
        public static bool GetBit(this UInt16 n, int position) { UInt16 m = (UInt16)UInt16_bits[position]; n &= m; return n != 0; }
        public static bool GetBit(this SInt32 n, int position) { SInt32 m = (SInt32)UInt32_bits[position]; n &= m; return n != 0; }
        public static bool GetBit(this UInt32 n, int position) { UInt32 m = (UInt32)UInt32_bits[position]; n &= m; return n != 0; }
        public static bool GetBit(this SInt64 n, int position) { SInt64 m = (SInt64)UInt64_bits[position]; n &= m; return n != 0; }
        public static bool GetBit(this UInt64 n, int position) { UInt64 m = (UInt64)UInt64_bits[position]; n &= m; return n != 0; }

        public static SInt08 SetBit(this SInt08 n, int position, bool value) { SInt08 m = (SInt08)UInt08_bits[position]; if (value) n |= m; else n &= m; return n; }
        public static UInt08 SetBit(this UInt08 n, int position, bool value) { UInt08 m = (UInt08)UInt08_bits[position]; if (value) n |= m; else n &= m; return n; }
        public static SInt16 SetBit(this SInt16 n, int position, bool value) { SInt16 m = (SInt16)UInt16_bits[position]; if (value) n |= m; else n &= m; return n; }
        public static UInt16 SetBit(this UInt16 n, int position, bool value) { UInt16 m = (UInt16)UInt16_bits[position]; if (value) n |= m; else n &= m; return n; }
        public static SInt32 SetBit(this SInt32 n, int position, bool value) { SInt32 m = (SInt32)UInt32_bits[position]; if (value) n |= m; else n &= m; return n; }
        public static UInt32 SetBit(this UInt32 n, int position, bool value) { UInt32 m = (UInt32)UInt32_bits[position]; if (value) n |= m; else n &= m; return n; }
        public static SInt64 SetBit(this SInt64 n, int position, bool value) { SInt64 m = (SInt64)UInt64_bits[position]; if (value) n |= m; else n &= m; return n; }
        public static UInt64 SetBit(this UInt64 n, int position, bool value) { UInt64 m = (UInt64)UInt64_bits[position]; if (value) n |= m; else n &= m; return n; }
    }
}