using Newtonsoft.Json;
using redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace casino
{
    public static class vpath
    {
        [DebuggerDisplay("{FullPath}")]
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class node
        {
            const int MAX_DEPTH = 100;

            internal Dictionary<Guid, node> dict;

            [JsonProperty, DbImport("id")]
            public Guid ID
            {
                get;
                internal set;
            }

            [JsonProperty, DbImport("name")]
            public string Name
            {
                get;
                internal set;
            }

            [JsonProperty, DbImport("dir")]
            internal Guid dir
            {
                get;
                private set;
            }

            public node this[string name]
            {
                get
                {
                    foreach (var n in
                        from n1 in dict.Values
                        where (n1.dir == this.ID) && (string.Compare(n1.Name, name, true) == 0)
                        select n1)
                        return n;
                    return null;
                }
            }

            public node Parent
            {
                get
                {
                    if (this.ID == this.dir || this.ID == Guid.Empty) return null;
                    node result;
                    dict.TryGetValue(this.dir, out result);
                    return result;
                }
            }

            [JsonProperty]
            public string FullPath
            {
                get
                {
                    StringBuilder s = new StringBuilder();
                    int d = 0;
                    for (node n = this; n != null && d < MAX_DEPTH; n = n.Parent, d++)
                    {
                        s.Insert(0, '/');
                        s.Insert(0, n.Name);
                    }
                    return s.ToString();
                }
            }

            bool GetFullPath(StringBuilder s, int depth)
            {
                if (depth >= MAX_DEPTH) return false;
                node parent = this.Parent;
                if (parent != null && !parent.GetFullPath(s, depth + 1))
                    return false;
                s.Append(this.Name);
                s.Append('/');
                return true;
            }
        }

        class cache : WebTools.TableVer<cache, node>
        {
            public cache() : base("path") { }

            protected override node ReadData(SqlCmd sqlcmd)
            {
                Dictionary<Guid, node> dict = new Dictionary<Guid, node>();
                node root = dict[Guid.Empty] = new node() { dict = dict, Name = "~", ID = Guid.Empty };

                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from VPath nolock"))
                {
                    node n = r.ToObject<node>();
                    n.dict = dict;
                    dict[n.ID] = n;
                }
                //foreach (node n in dict.Values)
                //{
                //    if (n.ID == Guid.Empty) continue;
                //    node parent;
                //    if (dict.TryGetValue(n.dir, out parent))
                //    {
                //        parent[n.Name] = n;
                //        n.Parent = parent; 
                //    }
                //}
                return root;
            }
        }

        [api("~/sys/path/add")]
        static dynamic add(string path)
        {
            if (vpath.SetNode(path))
            {
                vpath.node node;
                if (vpath.GetNode(path, out node))
                    return new { successed = true, value = node };
            }
            return new { successed = false };
        }

        [api("~/sys/path/get")]
        static node get(string path)
        {
            node result; vpath.GetNode(path, out result); return result;
        }

        internal delegate void SetNodeHandler(vpath.node node);

        internal static bool SetNode(string path, SetNodeHandler cb = null)
        {
            string[] p;
            if (!vpath.split_path(path, out p)) return false;
            SqlCmd sqlcmd = null;
            try
            {
                sqlcmd = DB.GetSqlCmd(DB.DB01W);
                cache c = cache.GetInstance(sqlcmd);
                vpath.node root = c.Value;
                vpath.node node = root;
                for (int i = 1, n = p.Length - 1; i < n; i++)
                {
                    string name = SqlBuilder.Force(p[i]);
                    vpath.node parent = node;
                    SqlBuilder s = new SqlBuilder();
                    s["*", "dir ", ""] = parent.ID;
                    s["*", "name", ""] = name;
                    node = sqlcmd.ToObject<vpath.node>(s.BuildF("select * from VPath nolock where ", SqlBuilder._AndFieldValue));
                    if (node == null)
                    {
                        if (sqlcmd.Transaction == null)
                            sqlcmd.BeginTransaction();
                        node = sqlcmd.ToObject<vpath.node>(s.BuildF("exec path_add ", SqlBuilder._AtFieldValue));
                        if (node == null)
                            throw new Exception("值不該為 null");
                    }
                }
                if (cb != null)
                    cb(node);
                if (sqlcmd.Transaction != null)
                {
                    sqlcmd.Commit();
                    c.UpdateVersion();
                    return true;
                }
                return false;
            }
            catch
            {
                if (sqlcmd != null && sqlcmd.Transaction != null)
                    sqlcmd.Rollback();
                throw;
            }
        }

        public static bool GetNode(string path, out node result)
        {
            string[] p;
            if (split_path(path, out p))
            {
                result = cache.GetInstance().Value;
                for (int i = 1, n = p.Length - 1; i < n; i++)
                {
                    string name = SqlBuilder.Force(p[i]);
                    result = result[name];
                    if (result == null)
                        return false;
                }
                return true;
            }
            result = null;
            return false;
        }

        //public static string Normalize(string path)
        //{
        //    path = path.Trim(true);
        //    if (path == null) return null;
        //    if (VirtualPathUtility.IsAppRelative())
        //    return path;
        //}

        [DebuggerStepThrough]
        public static bool split_path(string path, out string[] result)
        {
            try
            {
                if (path != null)
                {
                    path = path.Trim().ToLower();
                    path = VirtualPathUtility.AppendTrailingSlash(path);
                    path = VirtualPathUtility.ToAppRelative(path);
                    result = path.Split('/');
                    if (result.Length > 0)
                        if (result[0] == "~")
                            return true;
                }
            }
            catch { }
            result = null;
            return false;
        }

        [DebuggerStepThrough]
        public static string[] split_path(string path)
        {
            string[] result; split_path(path, out result); return result;
        }
    }
}