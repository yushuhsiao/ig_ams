#if NET40
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System.Security.Cryptography
{
    using Threading;
    using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public unsafe static class RandomValue
    {
        public static readonly RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();

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

        public static byte[] MD5(this byte[] input) { using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) return md5.ComputeHash(input); }
        public static byte[] SHA1(this byte[] input) { using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider()) return sha1.ComputeHash(input); }


        //public static string RSAEncrypt(this string input, string rsa_key)
        //{
        //    return RSAEncrypt(input, rsa_key, null);
        //}
        //public static string RSAEncrypt(this string input, string rsa_key, Encoding encoding)
        //{
        //    return Convert.ToBase64String(RSAEncrypt((encoding ?? Encoding.UTF8).GetBytes(input), rsa_key));
        //}
        //public static byte[] RSAEncrypt(this byte[] input, string rsa_key)
        //{
        //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //    {
        //        if (rsa_key != null)
        //            rsa.FromXmlString(rsa_key);
        //        return rsa.RSAEncrypt(input);
        //    }
        //}

        //public static string RSADecrypt(this string input, string rsa_key, byte[] cspblob, RSAParameters? parameter)
        //{
        //    return RSADecrypt(input, rsa_key, cspblob, parameter, null);
        //}
        //public static string RSADecrypt(this string input, string rsa_key, byte[] cspblob, RSAParameters? parameter, Encoding encoding)
        //{
        //    return (encoding ?? Encoding.UTF8).GetString(RSADecrypt(Convert.FromBase64String(input), rsa_key, cspblob, parameter));
        //}
        //public static byte[] RSADecrypt(this byte[] input, string rsa_key, byte[] cspblob, RSAParameters? parameter)
        //{
        //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //    {
        //        if (rsa_key != null)
        //            rsa.FromXmlString(rsa_key);
        //        else if (cspblob != null)
        //            rsa.ImportCspBlob(cspblob);
        //        else if (parameter.HasValue)
        //            rsa.ImportParameters(parameter.Value);
        //        int keySize = rsa.KeySize / 8;
        //        for (int offset = 0; offset < input.Length;)
        //        {
        //            int tmp_size = input.Length - offset;
        //            if (tmp_size > keySize)
        //                tmp_size = keySize;
        //            byte[] tmp = new byte[tmp_size];
        //            Array.Copy(input, offset, tmp, 0, tmp_size);
        //            byte[] tmp_dec = rsa.Decrypt(tmp, false);
        //            ms.Write(tmp_dec, 0, tmp_dec.Length);
        //            offset += tmp_size;
        //        }
        //        ms.Flush();
        //        return ms.ToArray();
        //    }
        //}

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



        //static byte[] salt = Encoding.UTF8.GetBytes("saltValue");

        //#if !NET20
        //        public static string AesEncrypt(this string input, string password)
        //        {
        //            return Encrypt<AesCryptoServiceProvider>(input, password);
        //        }
        //        public static byte[] AesEncrypt(this string input, string password, Encoding encoding)
        //        {
        //            return Encrypt<AesCryptoServiceProvider>(input, password, encoding);
        //        }
        //        public static byte[] AesEncrypt(this byte[] input, string password)
        //        {
        //            return Encrypt<AesCryptoServiceProvider>(input, password);
        //        }

        //        public static string AesDecrypt(this string input, string password)
        //        {
        //            return Decrypt<AesCryptoServiceProvider>(input, password);
        //        }
        //        public static string AesDecrypt(this byte[] input, string password, Encoding encoding)
        //        {
        //            return Decrypt<AesCryptoServiceProvider>(input, password, encoding);
        //        }
        //        public static byte[] AesDecrypt(this byte[] input, string password)
        //        {
        //            return Decrypt<AesCryptoServiceProvider>(input, password);
        //        }
        //#endif
        //        public static string DESEncrypt(this string input, string password)
        //        {
        //            return Encrypt<DESCryptoServiceProvider>(input, password);
        //        }
        //        public static byte[] DESEncrypt(this string input, string password, Encoding encoding)
        //        {
        //            return Encrypt<DESCryptoServiceProvider>(input, password, encoding);
        //        }
        //        public static byte[] DESEncrypt(this byte[] input, string password)
        //        {
        //            return Encrypt<DESCryptoServiceProvider>(input, password);
        //        }

        //        public static string DESDecrypt(this string input, string password)
        //        {
        //            return Decrypt<DESCryptoServiceProvider>(input, password);
        //        }
        //        public static string DESDecrypt(this byte[] input, string password, Encoding encoding)
        //        {
        //            return Decrypt<DESCryptoServiceProvider>(input, password, encoding);
        //        }
        //        public static byte[] DESDecrypt(this byte[] input, string password)
        //        {
        //            return Decrypt<DESCryptoServiceProvider>(input, password);
        //        }

        //        public static string TripleDESEncrypt(this string input, string password)
        //        {
        //            return Encrypt<TripleDESCryptoServiceProvider>(input, password);
        //        }
        //        public static byte[] TripleDESEncrypt(this string input, string password, Encoding encoding)
        //        {
        //            return Encrypt<TripleDESCryptoServiceProvider>(input, password, encoding);
        //        }
        //        public static byte[] TripleDESEncrypt(this byte[] input, string password)
        //        {
        //            return Encrypt<TripleDESCryptoServiceProvider>(input, password);
        //        }

        //        public static string TripleDESDecrypt(this string input, string password)
        //        {
        //            return Decrypt<TripleDESCryptoServiceProvider>(input, password);
        //        }
        //        public static string TripleDESDecrypt(this byte[] input, string password, Encoding encoding)
        //        {
        //            return Decrypt<TripleDESCryptoServiceProvider>(input, password, encoding);
        //        }
        //        public static byte[] TripleDESDecrypt(this byte[] input, string password)
        //        {
        //            return Decrypt<TripleDESCryptoServiceProvider>(input, password);
        //        }

        //        public static string Encrypt<T>(this string input, string password) where T : SymmetricAlgorithm, new()
        //        {
        //            return Convert.ToBase64String(Encrypt<T>(input, password, null));
        //        }
        //        public static byte[] Encrypt<T>(this string input, string password, Encoding encoding) where T : SymmetricAlgorithm, new()
        //        {
        //            return Encrypt<T>((encoding ?? Encoding.UTF8).GetBytes(input), password);
        //        }
        //        public static byte[] Encrypt<T>(this byte[] input, string password) where T : SymmetricAlgorithm, new()
        //        {
        //            using (T aes = new T())
        //            {
        //                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
        //                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
        //                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
        //                aes.Key = rfc.GetBytes(aes.KeySize / 8);
        //                aes.IV = rfc.GetBytes(aes.BlockSize / 8);
        //                using (MemoryStream ms = new MemoryStream())
        //                using (ICryptoTransform transform = aes.CreateEncryptor())
        //                using (CryptoStream encryptor = new CryptoStream(ms, transform, CryptoStreamMode.Write))
        //                {
        //                    encryptor.Write(input, 0, input.Length);
        //                    encryptor.FlushFinalBlock();
        //                    return ms.ToArray();
        //                }
        //            }
        //        }

        //        public static string Decrypt<T>(this string input, string password) where T : SymmetricAlgorithm, new()
        //        {
        //            return Decrypt<T>(Convert.FromBase64String(input), password, null);
        //        }
        //        public static string Decrypt<T>(this byte[] input, string password, Encoding encoding) where T : SymmetricAlgorithm, new()
        //        {
        //            return (encoding ?? Encoding.UTF8).GetString(Decrypt<T>(input, password));
        //        }
        //        public static byte[] Decrypt<T>(this byte[] input, string password) where T : SymmetricAlgorithm, new()
        //        {
        //            using (T aes = new T())
        //            {
        //                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
        //                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
        //                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
        //                aes.Key = rfc.GetBytes(aes.KeySize / 8);
        //                aes.IV = rfc.GetBytes(aes.BlockSize / 8);
        //                using (MemoryStream ms = new MemoryStream())
        //                using (ICryptoTransform transform = aes.CreateDecryptor())
        //                using (CryptoStream decryptor = new CryptoStream(ms, transform, CryptoStreamMode.Write))
        //                {
        //                    decryptor.Write(input, 0, input.Length);
        //                    decryptor.FlushFinalBlock();
        //                    return ms.ToArray();
        //                }
        //            }
        //        }

#if !NET20
        public static AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
#endif
        public static DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        public static TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();

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

        public override void Close()
        {
            using (_s1)
            using (s2)
                base.Close();
        }

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
                    if (this.XmlString != null)
                        rsa.FromXmlString(this.XmlString);
                    else if (this.CspBlob != null)
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
                if (this.XmlString != null)
                    rsa.FromXmlString(this.XmlString);
                else if (this.CspBlob != null)
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
#endif