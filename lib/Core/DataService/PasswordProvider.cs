using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace InnateGlory
{
    public class PasswordProvider : IDataService
    {
        private readonly DataService _dataService;

        public PasswordProvider(DataService dataService)
        {
            this._dataService = dataService;
        }

        public Entity.Password Get(UserId userId)
        {
            using (IDbConnection userdb = _dataService.DbConnections.UserDB_R(userId.CorpId))
            {
                var sql = $"select * from {TableName<Entity.Password>.Value} where UserId={userId}";
                var p = userdb.QuerySingleOrDefault<Entity.Password>(sql);
                if (p != null)
                    return p;
            }
            if (userId.IsRoot)
                return Set(userId, UserName.root);
            return default(Entity.Password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <param name="old_value">在變更密碼時, 如果有要求輸入舊密碼, 可以把舊密碼記錄在這個欄位</param>
        /// <returns></returns>
        public Entity.Password Set(UserId userId, string input, string old_value = null)
        {
            Entity.Password p = Create(input);
            //SqlBuilder _sql = new SqlBuilder(typeof(Entity.Password))
            //{
            //    { "w", "UserId"     , userId        },
            //    { " ", "Encrypt"    , p.Type        },
            //    { " ", "a"          , p.Ciphertext  },
            //    { " ", "b"          , p.Password    },
            //    { " ", "c"          , p.Salt        },
            //    { userId, null }
            //};
            old_value = old_value.Trim(true);
            //if (old_value != null)
            //    _sql.Add(" ", "x", old_value);
            object param = new
            {
                UserId = (int)userId,
                Encrypt = p.Type,
                a = p.Ciphertext,
                b = p.Password,
                c = p.Salt,
                CreateUser = (int)userId,
                Expiry = default(int?),
                x = old_value
            };
            string sql = $@"
insert into [{TableName<Entity.PasswordHist>.Value}] ([UserId], [ver], [Encrypt], [a], [b], [c], [Expiry], [CreateTime], [CreateUser], [x])
select [UserId], convert(bigint, [ver]),[Encrypt], [a], [b], [c], [Expiry], [CreateTime], [CreateUser], @x
from [{TableName<Entity.Password>.Value}]
where UserId = @UserId

delete from [{TableName<Entity.Password>.Value}]
where UserId = @UserId

insert into [{TableName<Entity.Password>.Value}] ([UserId], [Encrypt], [a], [b], [c], [Expiry], [CreateTime], [CreateUser])
values (@UserId, @Encrypt, @a, @b, @c, @Expiry, getdate(), @CreateUser)

select * from [{TableName<Entity.Password>.Value}]
where UserId = @UserId
";
            using (IDbConnection userdb = _dataService.DbConnections.UserDB_W(userId.CorpId))
            using (IDbTransaction tran = userdb.BeginTransaction())
            {
                var result = userdb.QuerySingle<Entity.Password>(sql, param, tran);
                tran.Commit();
                return result;
            }
//            string sql = $@"{_sql.exec("UpdatePassword")}
//{_sql.select_where()}";
//            sql = _sql.FormatWith(sql);
//            using (SqlCmd userdb = _dataService.SqlCmds.UserDB_W(userId.CorpId))
//                return userdb.ToObject<Entity.Password>(sql, transaction: true);
        }

        public bool IsMatch(Entity.Password data, string input)
        {
            string ciphertext = CreateCiphertext(data, input);
            return data.Ciphertext == ciphertext;
        }

        private Entity.Password Create(string input, string password = null, string salt = null)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            Entity.Password data = new Entity.Password()
            {
                Password = password ?? RandomValue.GetRandomString(32, 50),
                Salt = salt ?? RandomValue.GetRandomString(32, 50)
            };
            while (string.IsNullOrEmpty(data.Ciphertext))
            {
                data.Type = RandomValue.GetInt32() % 8;
                data.Ciphertext = CreateCiphertext(data, input);
            }
            return data;
        }

        private string CreateCiphertext(Entity.Password data, string input)
        {
            SymmetricAlgorithm provider;
            switch (data.Type)
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
            n1 = n2 = Crypto.Encrypt(provider, input, data.Password, data.Salt, Encoding.UTF8);
            if ((data.Type % 2) == 1) n2 = Crypto.MD5(n1);
            return Convert.ToBase64String(n2);
        }

    }
}