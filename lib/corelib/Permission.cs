using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace casino
{
    //[DebuggerDisplay("{FullPath}")]
    //class PathNode
    //{
    //    static readonly PathNode root = new PathNode() { Name = "~" };
    //    public string Name;
    //    public PathNode Parent;
    //    List<PathNode> childs = new List<PathNode>();
    //    PathNode GetChild(string name)
    //    {
    //        lock (this)
    //        {
    //            foreach (PathNode n1 in this.childs)
    //                if (n1.Name == name)
    //                    return n1;
    //            PathNode n2 = new PathNode() { Name = name, Parent = this };
    //            this.childs.Add(n2);
    //            return n2;
    //        }
    //    }

    //    public string FullPath
    //    {
    //        get { return _FullPath(new StringBuilder()); }
    //    }

    //    string _FullPath(StringBuilder s)
    //    {
    //        s.Insert(0, this.Name);
    //        if (Parent == null)
    //            return s.ToString();
    //        s.Insert(0, '/');
    //        return Parent._FullPath(s);
    //    }

    //    public static PathNode GetNodes(string relative_path)
    //    {
    //        using (SqlCmd sqlcmd = DB.Open(DB.DB01))
    //        {
    //        }
    //        relative_path = VirtualPathUtility.AppendTrailingSlash(relative_path);
    //        relative_path = VirtualPathUtility.ToAppRelative(relative_path);
    //        string[] p1 = relative_path.Split('/');
    //        PathNode p2 = root;
    //        for (int i = 1; i < p1.Length - 1; i++)
    //            p2 = p2.GetChild(p1[i]);
    //        return p2;
    //    }
    //}

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Permission1
    {
        public Permission1 Parent
        {
            get;
            private set;
        }
        Dictionary<string, Permission1> childs = new Dictionary<string, Permission1>();

        [DbImport]
        public int ID;
        [DbImport]
        public int ParentID;
        [DbImport]
        public string Name;
        [DbImport]
        public int UserLevel;
        [DbImport]
        public string Text;
        [DbImport]
        public int? Alias;

        public override string ToString()
        {
            return ToString(new StringBuilder());
        }

        string ToString(StringBuilder s)
        {
            s.Insert(0, this.Name);
            if (Parent == null)
                return s.ToString();
            s.Insert(0, '/');
            return Parent.ToString(s);
        }

        //[api("~/sys/permission/add")]
        //Permission1 permission_add()
        //{
        //    return null;
        //}

        //[api("~/sys/permission/get")]
        //Permission1 permission_get()
        //{
        //    return null;
        //}

        //[api("~/sys/permission/set")]
        //Permission1 permission_set()
        //{
        //    return null;
        //}

        //[api("~/sys/permission/del")]
        //Permission1 permission_del()
        //{
        //    return null;
        //}

        //public static Permission1 FromRelativePath(SqlCmd sqlcmd, string relative_path)
        //{
        //    using (RedisConnection redis = RedisConnection.GetConnection(null, DB.Redis_General))
        //    {
        //        relative_path = VirtualPathUtility.AppendTrailingSlash(relative_path);
        //        relative_path = VirtualPathUtility.ToAppRelative(relative_path);
        //        string[] p1 = relative_path.Split('/');
        //        if (p1[0] != "~") return null;      // path not start with "~/"
        //        Permission1 p2 = Permission1.Cache.Instance.Value;
        //        for (int i = 1; (p2 != null) && (i < p1.Length - 1); i++)
        //            p2.childs.TryGetValue(p1[i], out p2);
        //        return p2;
        //    }
        //}

        //public class Cache : WebTools.TableVer_Cache<Cache, Permission1>
        //{
        //    const string _table_name = "Sec1";
        //    public Cache() : base(_table_name) { }

        //    internal static void Init(SqlCmd sqlcmd)
        //    {
        //        sqlcmd.ExecuteNonQuery(true, @"if not exists (select ID from {0} nolock where ID=1) insert into {0} (ID,ParentID,Name,UserLevel,[Text]) VALUES (1,0,'~',0,'root')", _table_name);
        //    }

        //    protected override Permission1 ReadData(SqlCmd sqlcmd, string key, params object[] args)
        //    {
        //        using (DB.Open(DB.DB01R, ref sqlcmd))
        //        {
        //            Permission1 p1;
        //            Dictionary<int, Permission1> rows = new Dictionary<int, Permission1>();
        //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from {0} nolock", this.TableName))
        //            {
        //                p1 = r.ToObject<Permission1>();
        //                rows[p1.ID] = p1;
        //            }
        //            Permission1 root;
        //            if (rows.TryGetValue(1, out root))
        //            {
        //                foreach (Permission1 p2 in rows.Values)
        //                {
        //                    if (rows.TryGetValue(p2.ParentID, out p1))
        //                    {
        //                        p2.Parent = p1;
        //                        p1.childs[p2.Name] = p2;
        //                    }
        //                }
        //                return root;
        //            }
        //            return null;
        //        }
        //    }
        //}
    }
    public class Permission2
    {
    }




    //public class SecurityDefine
    //{
    //    public static readonly SecurityDefine Root = new SecurityDefine() { ID = 0 };
    //    static readonly Dictionary<int, SecurityDefine> all = new Dictionary<int, SecurityDefine>();

    //    public static SecurityDefine GetItem(int id)
    //    {
    //        return null;
    //    }

    //    public SecurityDefine Parent
    //    {
    //        get { SecurityDefine result; lock (all) all.TryGetValue(this.ParentID, out result); return result; }
    //    }

    //    [DbImport]
    //    public int ID;
    //    [DbImport]
    //    public int ParentID;
    //    [DbImport]
    //    public string Name;
    //    [DbImport]
    //    public int UserLevel;
    //    [DbImport]
    //    public string Text;
    //    public string Url;
    //}
    //class SecurityControl
    //{
    //}
    //class SecurityMenu
    //{
    //}
}
