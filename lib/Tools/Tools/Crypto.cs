using System.IO;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Security.Cryptography
{
    [_DebuggerStepThrough]
    public static partial class Crypto
    {
        public static string ToBase64String(byte[] input) => Convert.ToBase64String(input);
        public static byte[] FromBase64String(string input) => Convert.FromBase64String(input);

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
            using (var md5 = Cryptography.MD5.Create()) return md5.ComputeHash(input);
            //using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) return md5.ComputeHash(input);
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

        public static byte[] SHA1(this byte[] input)
        {
            using (var sha1 = Cryptography.SHA1.Create()) return sha1.ComputeHash(input);
            //using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider()) return sha1.ComputeHash(input);
        }

        public static AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
        public static TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
        public static DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

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

}
