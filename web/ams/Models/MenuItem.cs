using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ams.Models
{
    //[RedisVer("Menu")]
    public class MenuItem : TreeNode<MenuItem>
    {
        static readonly RedisVer<MenuItem> Cache = new RedisVer<MenuItem>("Menu")
        {
            ReadData = (sqlcmd, index) =>
            {
                MenuItem root = new MenuItem();
                foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach("select * from [Menu] nolock"))
                    r.FillObject(root.GetChild(r.GetString("_Path"), true));
                root.Sort((x, y) =>
                {
                    int xx = x.Order ?? int.MaxValue;
                    int yy = y.Order ?? int.MaxValue;
                    if (xx == yy)
                        return string.Compare(x.Name, y.Name);
                    return xx - yy;
                });
                return root;
            }
        };

        public static MenuItem Current { get { return Cache.Value; } }

        //public MenuItem() : base() { } public MenuItem(string name, MenuItem parent) : base(name, parent) { }

        [DbImport]
        public string Text { get; set; }

        [DbImport]
        public string Url { get; set; }

        [DbImport("_Order")]
        public int? Order { get; set; }
    }
}