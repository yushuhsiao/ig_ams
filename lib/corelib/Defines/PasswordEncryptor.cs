using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ams
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>基於安全上的考量, 這個類別不該被序列化</remarks>
    public class PasswordEncryptor
    {
        [DbImport]
        public UserID UserID;
        [DbImport]
        public ActiveState Active;
        [DbImport("ver")]
        public SqlTimeStamp version;
        [DbImport("n")]
        public int Type;
        [DbImport("a")]
        public string Ciphertext
        {
            get;
            private set;
        }
        [DbImport("b")]
        public string Password;
        [DbImport("c")]
        public string Salt;
        [DbImport("TTL")]
        public int? TTL;
        [DbImport]
        public DateTime CreateTime;
        [DbImport]
        public UserID CreateUser;


        public bool IsExpired
        {
            get
            {
                if (this.UserID.IsRoot)
                    return false;
                if (this.TTL.HasValue)
                {
                    int _ttl = (int)(DateTime.Now - CreateTime).TotalSeconds;
                    return _ttl <= this.TTL.Value;
                }
                return false;
            }
        }

        public bool IsActive
        {
            get { return this.UserID.IsRoot || (this.Active == ActiveState.Active); }
        }

        public bool Compare(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            if (string.IsNullOrEmpty(this.Ciphertext)) return false;
            string _ciphertext = this.CreateCiphertext(input);
            if (string.IsNullOrEmpty(_ciphertext)) return false;
            return _ciphertext == this.Ciphertext;
        }

        public PasswordEncryptor() { }
        public PasswordEncryptor(string input) { this.Encrypt(input); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type">Encrypt Type, 0 for random</param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="c"></param>
        public void Encrypt(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (string.IsNullOrEmpty(this.Password))
                this.Password = RandomValue.GetRandomString(32, 50);
            if (string.IsNullOrEmpty(this.Salt))
                this.Salt = RandomValue.GetRandomString(32, 50);
            for (this.Ciphertext = null; ;)
            {
                if (this.Type == 0)
                    this.Type = RandomValue.GetInt32() % 8;
                this.Ciphertext = CreateCiphertext(input);
                if (this.Ciphertext == null)
                    this.Type = 0;
                else
                    break;
            }
        }

        string CreateCiphertext(string input)
        {
            SymmetricAlgorithm provider = null;
            switch (this.Type)
            {
                case 1:
                case 2: provider = Crypto.AES; break;
                case 3:
                case 4: provider = Crypto.DES; break;
                case 5:
                case 6: provider = Crypto.TripleDES; break;
                default: return null;
            }
            byte[] n1, n2;
            n1 = n2 = Crypto.Encrypt(provider, input, this.Password, this.Salt, Encoding.UTF8);
            if ((this.Type % 2) == 1) n2 = Crypto.MD5(n1);
            return Convert.ToBase64String(n2);
        }
    }
}