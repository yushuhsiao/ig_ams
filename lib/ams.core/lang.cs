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

namespace ams
{
    public sealed class LangItem : TreeNode<LangItem>
    {
        abstract class Row
        {
            [JsonProperty, DbImport("_Path")]
            public virtual string Path { get; set; }
            protected abstract string field { get; }

            [JsonProperty, DbImport]
            public string Name;

            [JsonProperty, DbImport]
            public int LCID;

            [JsonProperty, DbImport]
            public string Text;

            public bool Save(SqlCmd coredb)
            {
                Path = SqlCmd.magic_quote(Path);
                Name = SqlCmd.magic_quote(Name);
                Text = SqlCmd.magic_quote(Text);
                string tableName = TableNameAttribute.GetInstance(this).TableName;
                string sql = $@"delete from {tableName} where {field}='{Path}' and Name='{Name}' and LCID={LCID}
insert into {tableName} ({field},Name,LCID,Text) values ('{Path}','{Name}',{LCID},N'{Text}')";
                int cnt = coredb.ExecuteNonQuery(sql);
                return cnt == 1 || cnt == 2;
            }
        }
        abstract class Row<T> : Row where T : Row<T>, new() { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn), TableName("Enums")]
        class EnumRow : Row<EnumRow>
        {
            [JsonProperty, DbImport("Type")]
            public override string Path
            {
                get { return base.Path; }
                set { base.Path = value; }
            }

            protected override string field { get { return "Type"; } }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn), TableName("Lang")]
        class LangRow : Row<LangRow>
        {
            [JsonProperty, DbImport("_Path")]
            public override string Path
            {
                get { return base.Path; }
                set { base.Path = value; }
            }

            protected override string field { get { return "_Path"; } }
        }

        #region //

        //#region IHttpModule

        ////internal class Module : IHttpModule
        ////{
        ////    void IHttpModule.Dispose() { }

        ////    void IHttpModule.Init(HttpApplication app)
        ////    {
        ////        app.BeginRequest += _BeginRequest;
        ////    }

        ////    private void _BeginRequest(object sender, EventArgs e)
        ////    {
        ////        Lang.SetCurrentUICulture(_HttpContext.Current);
        ////    }
        ////}

        //#endregion

        //#region 語系切換

        //static CultureInfo GetCultureInfo(string name)
        //{
        //    try { return CultureInfo.GetCultureInfo(name); }
        //    catch { }
        //    int lcid;
        //    if (name.ToInt32(out lcid))
        //    {
        //        try { return CultureInfo.GetCultureInfo(lcid); }
        //        catch { }
        //    }
        //    return null;
        //}

        //[DebuggerStepThrough]
        //static void SetCurrentUICulture(_HttpContext context)
        //{
        //    try
        //    {
        //        int lcid;
        //        CultureInfo c = null;
        //        if (context == null) return;
        //        HttpCookie cookie = context.Request.Cookies["lcid"];
        //        if (cookie != null)
        //            if (cookie.Value.ToInt32(out lcid))
        //                try { c = CultureInfo.GetCultureInfo(lcid); }
        //                catch { }
        //            else
        //                try { c = CultureInfo.GetCultureInfo(cookie.Value); }
        //                catch { }
        //        if (c == null && context.Request.UserLanguages != null)
        //            foreach (string s in context.Request.UserLanguages)
        //                try { c = CultureInfo.GetCultureInfo(s); break; }
        //                catch { }
        //        if (c == null) return;
        //        Thread t = Thread.CurrentThread;
        //        t.CurrentCulture = t.CurrentUICulture = c;
        //    }
        //    catch { }
        //}

        //#endregion

        #endregion

        public static readonly RedisVer<LangItem> Cache = new RedisVer<LangItem>("Lang") { ReadData = (sqlcmd, index) => new LangItem().ReadData(sqlcmd) };

        internal LangItem ReadData(SqlCmd sqlcmd)
        {
            string sqlstr;
            if (this.IsRoot)
                sqlstr = $"select * from Lang nolock";
            else
                sqlstr = $"select * from Lang nolock where _Path={FullPath}";
            foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach(sqlstr))
            {
                LangRow row = r.ToObject<LangRow>();
                this.GetChild(row.Path, true)
                    .GetChild(row.Name, true).values[row.LCID] = row.Text;
            }
            if (!this.IsRoot) return this;
            foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach("select * from Enums nolock"))
            {
                EnumRow row = r.ToObject<EnumRow>();
                this.GetEnumNode(row.Path)
                    .GetChild(row.Name, true).values[row.LCID] = row.Text;
            }
            return this;
        }

        internal static void WriteData(SqlCmd sqlcmd, string sqlstr)
        {
            if (string.IsNullOrEmpty(sqlstr)) return;
            using (_HttpContext.GetSqlCmd(out sqlcmd, sqlcmd, DB.Core01W))
            {
                sqlcmd.BeginTransaction();
                try
                {
                    sqlcmd.ExecuteNonQuery(sqlstr);
                    LangItem.Cache.UpdateVersion(sqlcmd);
                    sqlcmd.Commit();
                }
                catch
                {
                    sqlcmd.Rollback();
                    throw;
                }
            }
        }

        public static readonly LangItem Null = CreateItem("", null);

        public override LangItem GetChild(string path, bool create = false)
        {
            return base.GetChild(path, create);
        }

        protected override LangItem CreateChild(string name)
        {
            if (string.Compare(this.FullPath, ENUMS2, true) == 0)
                return CreateItem(name, this);
            return base.CreateChild(name);
        }

        //public LangItem() : base() { } internal LangItem(string name, LangItem parent) : base(name, parent) { }

        internal Dictionary<LCID, string> values = new Dictionary<LCID, string>();

        //public string this[object name, LCID? lcid = null]
        //{
        //    get { return this[name as string, name, lcid] ?? Convert.ToString(name); }
        //}

        //public string this[string text, object name, LCID? lcid = null]
        //{
        //    get
        //    {
        //        text = text.Trim(true);
        //        if (name == null) return text;
        //        string _name = (name as string).Trim(true);
        //        string result;
        //        GetValue(_name, lcid, out result, true);
        //        if (result == null)
        //        {
        //            Type t = name.GetType();
        //            if (t.IsEnum)
        //            {
        //                text = name.ToString();
        //                LangItem n1 = LangItem.GetEnums(this);
        //                LangItem n2 = n1.GetChild(t.Name) ?? n1.GetChild(t.FullName);
        //                if (n2 != null)
        //                    n2.GetValue(text, lcid, out result, false);
        //            }
        //        }
        //        #region ?init_lang=true
        //        if (result == null)
        //        {
        //            _User user = _User.Current;
        //            _HttpContext context = _HttpContext.Current;
        //            if (user.ID.IsRoot && (context != null))
        //            {
        //                bool? x = context.Request.QueryString["init_lang"].ToBoolean();
        //                if (x == true)
        //                {
        //                    SqlBuilder sql = new SqlBuilder();
        //                    StringBuilder s = new StringBuilder();
        //                    sql["TableName  "] = (SqlBuilder.str)"Lang";
        //                    sql["*w", "_Path"] = this.FullPath;
        //                    new Controllers.LangApiController.item()
        //                    {
        //                        name = _name,
        //                        values = new Dictionary<LCID, string>() { { 0, text ?? _name } }
        //                    }
        //                    .get_sqlstr(this, sql, s);
        //                    LangItem.WriteData(null, s.ToString());
        //                }
        //            }
        //        }
        //        #endregion
        //        return result ?? text;
        //    }
        //}

        public bool GetValue(string name, LCID? lcid, out string result, bool _parent = true)
        {
            name = name.Trim(false);
            CultureInfo c = null;
            if (lcid.HasValue && (lcid.Value != 0))
            {
                try { c = CultureInfo.GetCultureInfo(lcid.Value); }
                catch { }
            }
            c = c ?? Thread.CurrentThread.CurrentUICulture;
            LangItem item = this.GetChild(name, false);
            if ((item == null) && _User.Current.ID.IsRoot)
            {
                _HttpContext context = _HttpContext.Current;
                bool xx;
                if ((context != null) && context.Request.QueryString["lang_init"].ToBoolean(out xx) && xx)
                {
                    SqlBuilder sql = new SqlBuilder();
                    StringBuilder s = new StringBuilder();
                    sql["TableName  "] = (SqlBuilder.str)"Lang";
                    sql["*w", "_Path"] = this.FullPath;
                    new Controllers._LangApiController.item()
                    {
                        name = name,
                        values = new Dictionary<LCID, string>() { { 0, name } }
                    }
                    .get_sqlstr(this, sql, s);
                    LangItem.WriteData(null, s.ToString());
                }
            }
            if (item != null)
            {
                for (int i = 0; i < 100; c = c.Parent, i++)
                {
                    if (item.values.TryGetValue(c.LCID, out result))
                        return true;
                    if (c.Equals(c.Parent))
                        break;
                }
                if (item.values.TryGetValue(0, out result))
                    return true;
            }
            if (_parent && (base.Parent != null))
                return base.Parent.GetValue(name, lcid, out result, _parent);
            return _null.noop(false, out result);
        }
        public bool SetValue(SqlCmd coredb, string name, LCID lcid, string text)
        {
            name = name.Trim(false);
            LangItem item = this.GetChild(name, true);
            Row row;
            if (item.IsEnumNode)
                row = new EnumRow() { Path = this.Name };
            else
                row = new LangRow() { Path = this.FullPath };
            row.Name = name;
            row.LCID = lcid;
            row.Text = text;
            if (row.Save(coredb))
            {
                item.values[lcid] = row.Text;
                return true;
            }
            return false;
        }

        public string GetValue(string name, LCID? lcid = null, bool _parent = true)
        {
            return this.GetValue(name, name, lcid, _parent);
        }
        public string GetValue(string text, string name, LCID? lcid = null, bool _parent = true)
        {
            string result;
            if (this.GetValue(name, lcid, out result, _parent))
                return result;
            return text ?? name;
        }

        bool IsEnumNode
        {
            get
            {
                if (this.Parent == null) return false;
                if (this.Parent.IsRoot && this.Name == ENUMS1) return true;
                return this.Parent.IsEnumNode;
                for (LangItem n = this; n != null; n = n.Parent)
                {
                    if (n.Parent == null) break;
                    if (n.Parent.IsRoot && n.Name == ENUMS1)
                        return true;
                }
                return false;
            }
        }


        //public override LangItem GetChild(string path, bool create = false)
        //{
        //    if (this.FullPath == ENUMS2)
        //        create = true;
        //    return base.GetChild(path, create);
        //}



        // enums
        const string ENUMS1 = "Enums";
        const string ENUMS2 = "~/" + ENUMS1;
        public static LangItem EnumsRoot
        {
            [DebuggerStepThrough]
            get { return GetEnumsRoot(null); }
        }
        public static LangItem GetEnumsRoot(LangItem instance)
        {
            return (instance ?? LangItem.Cache.Value).GetChild(ENUMS2, true);
        }
        public LangItem GetEnumNode(string name)
        {
            return LangItem.GetEnumsRoot(this).GetChild(name, true);
        }
        [DebuggerStepThrough]
        public LangItem GetEnumNode<T>() { return this.GetEnumNode(typeof(T)); }
        public LangItem GetEnumNode(Type type)
        {
            LangItem n0 = LangItem.GetEnumsRoot(this);
            LangItem n1 = n0.GetChild(type.Name, false);
            LangItem n2 = n0.GetChild(type.FullName, false);
            return n1 ?? n2 ?? n0.GetChild(type.Name, true);
        }

        //static object GetEnum_CreateObject<T>(T value, string text)
        //{
        //    return new { value = value, text = text };
        //}
        //public delegate object GetEnum_CreateObjectHandler<T>(T value, string text);

        //public static List<ValueTextPair<string>> GetEnums/*  */(string name, LCID? lcid = null, params string[] exclude) { return GetEnums(name, lcid, null, exclude); }
        //public static List<ValueTextPair<string>> GetEnumsIn/**/(string name, LCID? lcid = null, params string[] include) { return GetEnums(name, lcid, include, null); }
        //static List<ValueTextPair<string>> GetEnums(string name, LCID? lcid, string[] include, string[] exclude)
        //{
        //    LangItem node = LangItem.Cache.Value.GetEnumNode(name);
        //    include = include ?? _null.strings;
        //    exclude = exclude ?? _null.strings;
        //    List<ValueTextPair<string>> r = new List<ValueTextPair<string>>();
        //    foreach (LangItem n1 in node.Childs)
        //    {
        //        if ((include.Length > 0) && !include.Contains(n1.Name)) continue;
        //        if ((exclude.Length > 0) && exclude.Contains(n1.Name)) continue;
        //        string text = node.GetValue(n1.Name, n1.Name, lcid, false);
        //        r.Add(new ValueTextPair<string>(n1.Name, text));
        //    }
        //    return r;
        //}

        //public static List<ValueTextPair<object>> GetEnums/*  */<T>(LCID? lcid = null, params T[] exclude) where T : struct { return GetEnums(lcid, null, exclude); }
        //public static List<ValueTextPair<object>> GetEnumsIn/**/<T>(LCID? lcid = null, params T[] include) where T : struct { return GetEnums(lcid, include, null); }
        //static List<ValueTextPair<object>> GetEnums<T>(LCID? lcid = null, T[] include = null, T[] exclude = null) where T : struct
        //{
        //    if (typeof(T).IsEnum)
        //    {
        //        LangItem node = LangItem.Cache.Value.GetEnumNode<T>();
        //        include = include ?? _null<T>.array;
        //        exclude = exclude ?? _null<T>.array;
        //        List<ValueTextPair<object>> r = new List<ValueTextPair<object>>();
        //        foreach (T name in Enum.GetValues(typeof(T)))
        //        {
        //            if ((include.Length > 0) && !include.Contains(name)) continue;
        //            if ((exclude.Length > 0) && exclude.Contains(name)) continue;
        //            string text = node.GetValue(name.ToString(), name.ToString(), lcid, false);
        //            r.Add(new ValueTextPair<object>(name, text));
        //        }
        //        return r;
        //    }
        //    return null;
        //}
    }
}
//namespace ams.Models
//{
//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public class LangModel
//    {
//        [JsonProperty]
//        public string path { get; set; }
//        [JsonProperty]
//        public string name { get; set; }
//        [JsonProperty]
//        public Dictionary<LCID, string> values { get; set; }
//    }

//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public class LangListModel : LangModel
//    {
//        [JsonProperty]
//        public List<LangListModel> items;
//    }
//}
