using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Http;

namespace ams.Data
{
    [DebuggerDisplay("{ID}, {FullPath}")]
    public class AclDefine : TreeNode<AclDefine>
    {
        [DbImport]
        public Guid ID;
        [DbImport]
        public int Flag;

        static RedisVer<AclDefine> Cache = new RedisVer<AclDefine>("AclDefine") { ReadData = ReadData };
        public static AclDefine RootNode
        {
            [DebuggerStepThrough]
            get { return Cache.Value; }
        }

        static AclDefine ReadData(SqlCmd sqlcmd1, int index)
        {
            AclDefine root = new AclDefine();
            foreach (var a in _HttpApplication.Configuration.GetApiDescriptions())
                root.GetChild(a.RelativePath, true);
            foreach (SqlDataReader r in sqlcmd1.ExecuteReaderEach("select * from AclDefine nolock"))
                r.ToObject(() => root.GetChild(r.GetString("_Path"), true));

            SqlCmd sqlcmd2 = null;
            try
            {
                foreach (var n in root.All)
                {
                    if (!n.ID.Equals(Guid.Empty)) continue;
                    try
                    {
                        sqlcmd2 = sqlcmd2 ?? new SqlCmd(DB.Core01W);
                        sqlcmd2.FillObject(n, true, $@"declare @id uniqueidentifier, @path varchar(200)
select @id=newid(), @path='{n.FullPath}'
insert into AclDefine (ID, _Path) values (@id, @path)
select * from AclDefine nolock where ID=@id");
                    }
                    catch (SqlException ex) when (ex.IsDuplicateKey()) { }
                }
            }
            finally { using (sqlcmd2) { } }
            return root;
        }
    }
}