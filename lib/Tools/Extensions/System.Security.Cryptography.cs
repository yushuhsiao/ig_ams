using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Security.Cryptography
{
    [_DebuggerStepThrough]
    public unsafe static class RandomValue
    {
        public static readonly RandomNumberGenerator RNG = RandomNumberGenerator.Create();

        public static byte[] GetBytes(this RandomNumberGenerator rand, int size)
        {
            byte[] n = new byte[size];
            rand.GetBytes(n);
            return n;
        }
        public static byte[] GetBytes(int size)
        {
            return RNG.GetBytes(size);
        }

        public static Int16 GetInt16(this RandomNumberGenerator rand, Int16 max = Int16.MaxValue)
        {
            fixed (void* p = rand.GetBytes(sizeof(Int16)))
            {
                Int16 v = *(Int16*)p;
                if (v < 0) v *= -1;
                v %= max;
                return v;
            }
        }
        public static Int16 GetInt16(Int16 max = Int16.MaxValue)
        {
            return RandomValue.RNG.GetInt16(max);
        }

        public static Int32 GetInt32(this RandomNumberGenerator rng, Int32 max = Int32.MaxValue)
        {
            fixed (void* p = rng.GetBytes(sizeof(Int32)))
            {
                Int32 v = *(Int32*)p;
                if (v < 0) v *= -1;
                v %= max;
                return v;
            }
        }
        public static Int32 GetInt32(Int32 max = Int32.MaxValue)
        {
            return RandomValue.RNG.GetInt32(max);
        }


        public static Int64 GetInt64(this RandomNumberGenerator rng, Int64 max = Int64.MaxValue)
        {
            fixed (void* p = rng.GetBytes(sizeof(Int64)))
            {
                Int64 v = *(Int64*)p;
                if (v < 0) v *= -1;
                v %= max;
                return v;
            }
        }
        public static Int64 GetInt64(Int64 max = Int64.MaxValue)
        {
            return RandomValue.RNG.GetInt64(max);
        }

        public const string Number = "0123456789";
        public const string LowerLetter = "abcdefghijklmnopqrstuvwxyz";
        public const string LowerNumber = LowerLetter + Number;
        public const string UpperLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string UpperNumber = UpperLetter + Number;

        public static string GetRandomString(int minLength, int maxLength, string pattern = RandomValue.LowerNumber)
        {
            int length;
            if (minLength == maxLength)
                length = minLength;
            else
            {
                int range = Math.Abs(maxLength - minLength);
                length = RandomValue.GetInt32() % range;
                length += Math.Min(minLength, maxLength);
            }
            return RandomValue.GetRandomString(length, pattern);
        }
        public static string GetRandomString(int length, string pattern = RandomValue.LowerNumber)
        {
            if (pattern != null)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < length; i++)
                    sb.Append(pattern[RandomValue.GetInt32(pattern.Length)]);
                return sb.ToString();
            }
            return pattern;
        }
    }

    [_DebuggerStepThrough]
    public static class Crypto
    {
        //public static string ToBase64String(this byte[] input)
        //{
        //    return Convert.ToBase64String(input);
        //}
        //public static byte[] Base64StringToArray(this string input)
        //{
        //    return Convert.FromBase64String(input);
        //}

        public static string MD5(this string input)
        {
            return MD5(input, null);
        }
        public static string MD5(this string input, Encoding encoding)
        {
            return Convert.ToBase64String(MD5((encoding ?? Encoding.UTF8).GetBytes(input)));
        }
        public static string MD5Hex(this string input)
        {
            return MD5Hex(input, null);
        }
        public static string MD5Hex(this string input, Encoding encoding)
        {
            return MD5((encoding ?? Encoding.UTF8).GetBytes(input)).ToHex();
        }
        public static string SHA1Hex(this string input)
        {
            return SHA1Hex(input, null);
        }
        public static string SHA1Hex(this string input, Encoding encoding)
        {
            return SHA1((encoding ?? Encoding.UTF8).GetBytes(input)).ToHex();
        }
        static string ToHex(this byte[] data)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                s.AppendFormat("{0:X2}", data[i]);
            return s.ToString();
        }

        public static byte[] MD5(this byte[] input)
        {
            using (var md5 = Cryptography.MD5.Create()) return md5.ComputeHash(input);
            //using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) return md5.ComputeHash(input);
        }
        public static byte[] SHA1(this byte[] input)
        {
            using (var sha1 = Cryptography.SHA1.Create()) return sha1.ComputeHash(input);
            //using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider()) return sha1.ComputeHash(input);
        }


        public static void Encrypt(this RSACryptoServiceProvider rsa, byte[] input, Stream output)
        {
            int blockSize = rsa.KeySize / 8 - 11;
            int offset = 0;
            while (offset < input.Length)
            {
                int tmp_size = input.Length - offset;
                if (tmp_size > blockSize)
                    tmp_size = blockSize;
                byte[] tmp = new byte[tmp_size];
                Array.Copy(input, offset, tmp, 0, tmp_size);
                byte[] tmp_enc = rsa.Encrypt(tmp, false);
                output.Write(tmp_enc, 0, tmp_enc.Length);
                offset += tmp_size;
            }
            output.Flush();
        }
        public static void Decrypt(this RSACryptoServiceProvider rsa, byte[] input, Stream stream)
        {
            int keySize = rsa.KeySize / 8;
            int offset = 0;
            while (offset < input.Length)
            {
                int tmp_size = input.Length - offset;
                if (tmp_size > keySize)
                    tmp_size = keySize;
                byte[] tmp = new byte[tmp_size];
                Array.Copy(input, offset, tmp, 0, tmp_size);
                byte[] tmp_dec = rsa.Decrypt(tmp, false);
                stream.Write(tmp_dec, 0, tmp_dec.Length);
                offset += tmp_size;
            }
            stream.Flush();
        }

        public static void Encrypt(this RSACryptoServiceProvider rsa, Stream input, Stream output)
        {
            int blockSize = rsa.KeySize / 8 - 11;
            while (input.Position < input.Length)
            {
                int tmp_size = (int)(input.Length - input.Position);
                if (tmp_size > blockSize)
                    tmp_size = blockSize;
                byte[] tmp = new byte[tmp_size];
                input.Read(tmp, 0, tmp_size);
                byte[] tmp_enc = rsa.Encrypt(tmp, false);
                output.Write(tmp_enc, 0, tmp_enc.Length);
            }
            output.Flush();
        }
        public static void Decrypt(this RSACryptoServiceProvider rsa, Stream input, Stream stream)
        {
            int keySize = rsa.KeySize / 8;
            for (;;)
            {
                byte[] tmp = new byte[keySize];
                int n = input.Read(tmp, 0, keySize);
                if (n == 0) break;
                if (n != keySize) Array.Resize(ref tmp, n);
                byte[] tmp_dec = rsa.Decrypt(tmp, false);
                stream.Write(tmp_dec, 0, tmp_dec.Length);
            }
            stream.Flush();
            //while (input.Position < input.Length)
            //{
            //    int tmp_size = (int)(input.Length - input.Position);
            //    if (tmp_size > keySize)
            //        tmp_size = keySize;
            //    byte[] tmp = new byte[tmp_size];
            //    input.Read(tmp, 0, tmp.Length);
            //    byte[] tmp_dec = rsa.Decrypt(tmp, false);
            //    stream.Write(tmp_dec, 0, tmp_dec.Length);
            //}
            //stream.Flush();
        }



#if NET40
        public static AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
        public static TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
        public static DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
#else
        public static Aes AES = Aes.Create();
        public static TripleDES TripleDES = TripleDES.Create();
        public static DES DES = DES.Create();
#endif

        public static byte[] Encrypt<T>(this T provider, string input, string password, string salt, Encoding encoding) where T : SymmetricAlgorithm
        {
            encoding = encoding ?? Encoding.UTF8;
            return provider.Encrypt<T>(encoding.GetBytes(input), password, encoding.GetBytes(salt));
        }
        public static byte[] Encrypt<T>(this T provider, byte[] input, string password, byte[] salt) where T : SymmetricAlgorithm
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
            using (MemoryStream ms = new MemoryStream())
            using (ICryptoTransform transform = provider.CreateEncryptor(rfc.GetBytes(provider.KeySize / 8), rfc.GetBytes(provider.BlockSize / 8)))
            using (CryptoStream encryptor = new CryptoStream(ms, transform, CryptoStreamMode.Write))
            {
                encryptor.Write(input, 0, input.Length);
                encryptor.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        public static string Decrypt<T>(this T provider, byte[] input, string password, string salt, Encoding encoding) where T : SymmetricAlgorithm
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetString(provider.Decrypt<T>(input, password, encoding.GetBytes(salt)));
        }
        public static byte[] Decrypt<T>(this T provider, byte[] input, string password, byte[] salt) where T : SymmetricAlgorithm
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
            using (MemoryStream ms = new MemoryStream())
            using (ICryptoTransform transform = provider.CreateDecryptor(rfc.GetBytes(provider.KeySize / 8), rfc.GetBytes(provider.BlockSize / 8)))
            using (CryptoStream decryptor = new CryptoStream(ms, transform, CryptoStreamMode.Write))
            {
                decryptor.Write(input, 0, input.Length);
                decryptor.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }

    public abstract class RSAStream : Stream
    {
        protected Stream _s1;
        protected Stream s1;
        protected MemoryStream s2;
        protected RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        public string XmlString { get; set; }
        public byte[] CspBlob { get; set; }
        public string Base64CspBlob
        {
            get { if (this.CspBlob == null) return null; return Convert.ToBase64String(this.CspBlob); }
            set { if (value == null) this.CspBlob = null; else this.CspBlob = Convert.FromBase64String(value); }
        }
        public RSAParameters? Parameter { get; set; }

        protected RSAStream(Stream stream, bool leaveOpen = false)
        {
            this.s1 = stream;
            this.s2 = new MemoryStream();
            if (!leaveOpen)
                _s1 = s1;
        }

        public override bool CanRead
        {
            get { return s1.CanRead; }
        }

        public override bool CanSeek
        {
            get { return s1.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return s1.CanWrite; }
        }

        public override long Length
        {
            get { return s1.Length; }
        }

        public override long Position
        {
            get { return s1.Position; }
            set { s1.Position = value; }
        }

        protected override void Dispose(bool disposing)
        {
            using (_s1)
            using (s2)
                base.Dispose(disposing);
        }

#if NET40
        public override void Close()
        {
            using (_s1)
            using (s2)
                base.Close();
        }
#endif

        public override void Flush()
        {
            s1.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return s1.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            s1.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return s1.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            s1.Write(buffer, offset, count);
        }
    }

    public class RSADecryptStream : RSAStream
    {
        public RSADecryptStream(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            using (RSACryptoServiceProvider rsa = Interlocked.Exchange(ref base.rsa, null))
            {
                if (rsa != null)
                {
#if NET40
                    if (this.XmlString != null)
                        rsa.FromXmlString(this.XmlString);
                    else
#endif
                    if (this.CspBlob != null)
                        rsa.ImportCspBlob(this.CspBlob);
                    else if (this.Parameter.HasValue)
                        rsa.ImportParameters(this.Parameter.Value);
                    rsa.Decrypt(s1, s2);
                    s2.Position = 0;
                }
            }
            return s2.Read(buffer, offset, count);
        }
    }

    public class RSAEncryptStream : RSAStream
    {
        public RSAEncryptStream(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        public override void Write(byte[] buffer, int offset, int count)
        {
            s2.Write(buffer, offset, count);
        }

        public override void Flush()
        {
            using (RSACryptoServiceProvider rsa = Interlocked.Exchange(ref base.rsa, null))
            {
#if NET40
                if (this.XmlString != null)
                    rsa.FromXmlString(this.XmlString);
                else
#endif
                if (this.CspBlob != null)
                    rsa.ImportCspBlob(this.CspBlob);
                else if (this.Parameter.HasValue)
                    rsa.ImportParameters(this.Parameter.Value);
                s2.Flush();
                s2.Position = 0;
                rsa.Encrypt(s2, s1);
            }
        }
    }
}
