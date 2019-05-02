using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System.Security.Cryptography
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public unsafe static class RandomValue
    {
        public static readonly RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();

        public static byte[] GetBytes(this RandomNumberGenerator rng, int size)
        {
            byte[] n = new byte[size];
            rng.GetBytes(n);
            return n;
        }
        public static byte[] GetBytes(int size)
        {
            return RNG.GetBytes(size);
        }

        public static Int16 GetInt16(this RandomNumberGenerator rng, Int16 max)
        {
            fixed (void* p = rng.GetBytes(sizeof(Int16)))
            {
                Int16 v = *(Int16*)p;
                if (v < 0) v *= -1;
                v %= max;
                return v;
            }
        }
        public static Int16 GetInt16(this RandomNumberGenerator rand)
        {
            return rand.GetInt16(0);
        }
        public static Int16 GetInt16(Int16 max)
        {
            return RNG.GetInt16(max);
        }
        public static Int16 GetInt16()
        {
            return RNG.GetInt16(Int16.MaxValue);
        }

        public static Int32 GetInt32(this RandomNumberGenerator rng, Int32 max)
        {
            fixed (void* p = rng.GetBytes(sizeof(Int32)))
            {
                Int32 v = *(Int32*)p;
                if (v < 0) v *= -1;
                v %= max;
                return v;
            }
        }
        public static Int32 GetInt32(this RNGCryptoServiceProvider rng)
        {
            return rng.GetInt32(Int32.MaxValue);
        }
        public static Int32 GetInt32(Int32 max)
        {
            return RNG.GetInt32(max);
        }
        public static Int32 GetInt32()
        {
            return RNG.GetInt32(Int32.MaxValue);
        }


        public static Int64 GetInt64(this RandomNumberGenerator rng, Int64 max)
        {
            fixed (void* p = rng.GetBytes(sizeof(Int64)))
            {
                Int64 v = *(Int64*)p;
                if (v < 0) v *= -1;
                v %= max;
                return v;
            }
        }
        public static Int64 GetInt64(this RNGCryptoServiceProvider rng)
        {
            return rng.GetInt64(Int64.MaxValue);
        }
        public static Int64 GetInt64(Int64 max)
        {
            return RNG.GetInt64(max);
        }
        public static Int64 GetInt64()
        {
            return RNG.GetInt64(Int64.MaxValue);
        }
    }

    public static class RandomString
    {
        public const string Number = "0123456789";
        public const string LowerLetter = "abcdefghijklmnopqrstuvwxyz";
        public const string LowerNumber = LowerLetter + Number;
        public const string UpperLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string UpperNumber = UpperLetter + Number;

        public static string GetRandomString(this string pattern, int length)
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
        public static byte[] MD5(this byte[] input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                return md5.ComputeHash(input);
        }
        public static string MD5Hex(this string input)
        {
            return MD5Hex(input, null);
        }
        public static string MD5Hex(this string input, Encoding encoding)
        {
            byte[] data = MD5((encoding ?? Encoding.UTF8).GetBytes(input));
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                s.AppendFormat("{0:x2}", data[i]);
            return s.ToString();
        }

        public static string RSAEncrypt(this string input, string rsa_key)
        {
            return RSAEncrypt(input, rsa_key, null);
        }
        public static string RSAEncrypt(this string input, string rsa_key, Encoding encoding)
        {
            return Convert.ToBase64String(RSAEncrypt((encoding ?? Encoding.UTF8).GetBytes(input), rsa_key));
        }
        public static byte[] RSAEncrypt(this byte[] input, string rsa_key)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            using (MemoryStream ms = new MemoryStream())
            {
                if (rsa_key != null)
                    rsa.FromXmlString(rsa_key);
                int blockSize = rsa.KeySize / 8 - 11;
                for (int offset = 0; offset < input.Length; )
                {
                    int tmp_size = input.Length - offset;
                    if (tmp_size > blockSize)
                        tmp_size = blockSize;
                    byte[] tmp = new byte[tmp_size];
                    Array.Copy(input, offset, tmp, 0, tmp_size);
                    byte[] tmp_enc = rsa.Encrypt(tmp, false);
                    ms.Write(tmp_enc, 0, tmp_enc.Length);
                    offset += tmp_size;
                }
                ms.Flush();
                return ms.ToArray();
            }
        }

        public static string RSADecrypt(this string input, string rsa_key)
        {
            return RSADecrypt(input, rsa_key, null);
        }
        public static string RSADecrypt(this string input, string rsa_key, Encoding encoding)
        {
            return (encoding ?? Encoding.UTF8).GetString(RSADecrypt(Convert.FromBase64String(input), rsa_key));
        }
        public static byte[] RSADecrypt(this byte[] input, string rsa_key)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            using (MemoryStream ms = new MemoryStream())
            {
                if (rsa_key != null)
                    rsa.FromXmlString(rsa_key);
                int keySize = rsa.KeySize / 8;
                for (int offset = 0; offset < input.Length; )
                {
                    int tmp_size = input.Length - offset;
                    if (tmp_size > keySize)
                        tmp_size = keySize;
                    byte[] tmp = new byte[tmp_size];
                    Array.Copy(input, offset, tmp, 0, tmp_size);
                    byte[] tmp_dec = rsa.Decrypt(tmp, false);
                    ms.Write(tmp_dec, 0, tmp_dec.Length);
                    offset += tmp_size;
                }
                ms.Flush();
                return ms.ToArray();
            }
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

        public static byte[] Encrypt<T>(this T provider, string input, string password, string salt, Encoding encoding) where T : SymmetricAlgorithm, new()
        {
            encoding = encoding ?? Encoding.UTF8;
            return provider.Encrypt<T>(encoding.GetBytes(input), password, encoding.GetBytes(salt));
        }
        public static byte[] Encrypt<T>(this T provider, byte[] input, string password, byte[] salt) where T : SymmetricAlgorithm, new()
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

        public static string Decrypt<T>(this T provider, byte[] input, string password, string salt, Encoding encoding) where T : SymmetricAlgorithm, new()
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetString(provider.Decrypt<T>(input, password, encoding.GetBytes(salt)));
        }
        public static byte[] Decrypt<T>(this T provider, byte[] input, string password, byte[] salt) where T : SymmetricAlgorithm, new()
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
}