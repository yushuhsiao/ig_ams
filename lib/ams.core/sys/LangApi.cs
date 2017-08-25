using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using LCID = System.Int32;

namespace ams.Controllers
{
    public abstract class _LangApiController : _ApiController
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class tree : TreeNode<tree>
        {
            //public tree() : base() { } tree(string name, tree parent) : base(name, parent) { }

            [JsonProperty("label")]
            public override string Name
            {
                get { return base.Name; }
            }

            [JsonProperty("value")]
            public EncodingPath value
            {
                get { return base.FullPath; }
            }

            [JsonProperty("items")]
            public List<tree> items
            {
                get { if (this.HasChilds) return new List<tree>(base.Childs); return null; }
            }

            public void get_files(VirtualPath path)
            {
                DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(path.FullPath));
                foreach (DirectoryInfo d2 in dir.GetDirectories())
                {
                    VirtualPath p2 = path.GetChild(d2.Name, true);
                    if (p2 == VirtualPath.GetPath("~/Areas/HelpPage")) continue;
                    if (p2 == VirtualPath.GetPath("~/Areas/_")) continue;
                    get_files(p2);
                }
                foreach (FileInfo f2 in dir.GetFiles("*.cshtml"))
                    this.GetChild(path.FullPath, true).GetChild(f2.Name, true);
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class item
        {
            [JsonProperty]
            public EncodingPath rowid;
            [JsonProperty]
            public EncodingPath path;
            [JsonProperty]
            public string name;
            [JsonProperty]
            public Dictionary<LCID, string> values;

            public void get_sqlstr(LangItem n1, SqlBuilder sql, StringBuilder s)
            {
                sql["", "LCID"] = null;
                sql["", "Text"] = null;
                LangItem n2 = this.rowid.GetTreeNode(n1, true);
                if (this.values == null)
                {
                    if (n2 == null) return;
                    if (n2.Parent != n1) return;
                    sql["*w", "Name "] = n2.Name;
                    s.AppendLine(sql.Build("delete from {TableName}", SqlBuilder.op.where));
                    return;
                }
                if (n2 == null)
                    n2 = n1.GetChild(this.name, true);
                if (n2 == null) return;
                if (n2.Parent != n1) return;
                sql["*w", "Name "] = n2.Name;
                foreach (var n in this.values)
                {
                    sql["*w", "LCID"] = n.Key;
                    string sql_where = sql.Build(SqlBuilder.op.where);
                    if (this.name != n2.Name)
                        sql["Nu", "Name"] = this.name;
                    sql["Nu", "Text"] = n.Value.Trim(false);
                    if (sql.GetMissingFields() == null)
                        s.AppendLine(sql.Build(@"if exists (select [LCID] from {TableName} nolock", sql_where, @")
     update {TableName}", SqlBuilder.op.update_set, sql_where, @"
else insert into {TableName}", SqlBuilder.op.insert));
                }
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class page_args
        {
            [JsonProperty]
            public EncodingPath path { get; set; }
            [JsonProperty]
            public item[] values { get; set; }

            public ListResponse<item> page_proc(_ApiController controller)
            {
                LangItem n1 = this.path.GetTreeNode(new LangItem(), true);
                if (n1 == null) return null;
                SqlCmd sqlcmd = null;

                if (this.values != null)
                {
                    SqlBuilder sql = new SqlBuilder();
                    StringBuilder s = new StringBuilder();
                    sql["TableName"] = (SqlBuilder.str)"Lang";
                    sql["*w", "_Path"] = n1.FullPath;
                    foreach (item row in this.values)
                        row.get_sqlstr(n1, sql, s);
                    string sqlstr = s.ToString();
                    sqlcmd = _HttpContext.GetSqlCmd(DB.Core01W);
                    LangItem.WriteData(sqlcmd, sqlstr);
                    n1 = this.path.GetTreeNode(new LangItem(), true);
                }

                n1.ReadData(sqlcmd ?? _HttpContext.GetSqlCmd(DB.Core01R));
                ListResponse<item> r1 = new ListResponse<item>() { Rows = new List<item>() };
                dynamic r2 = r1;
                foreach (LangItem n3 in n1.All)
                {
                    if (n3.Parent == n1)
                    {
                        r1.Rows.Add(new item()
                        {
                            path = n3.FullPath,
                            name = n3.Name,
                            values = n3.values
                        });
                    }
                }
                r2.ver = LangItem.Cache.GetVersion(sqlcmd);
                return r1;
            }
        }
    }
}