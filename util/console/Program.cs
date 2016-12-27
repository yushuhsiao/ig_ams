using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace console
{
    static partial class Program
    {
        static void Main(string[] args)
        {
            CultureInfo tw = new CultureInfo("zh-tw");
            CultureInfo cn = new CultureInfo("zh-cn");
            CultureInfo en = new CultureInfo("en-us");
            Debugger.Break();

            string a, b;
            "123\\".Split("/\\", out a, out b);
        }

    }

    class item
    {
        public string defaultValue;
        public string value;
        public string id;
        public string text;
        public string type;

        public string get_id()
        {
            return "@" + this.id;
            return "@" + this.text.Replace(' ', '_').ToLower();
        }
    }
    partial class Program
    {
        static void Main2(string[] args)
        {
            JSParser js = new JSParser();
            Block b = js.Parse("jqx-all.js");
            Debugger.Break();
        }

        static void Main1(string[] args)
        {
            StringBuilder src = new StringBuilder(File.ReadAllText("jqx.custom1.less"));
            JObject d1 = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(File.ReadAllText("jqx.custom.json"));
            Dictionary<string, item> items2 = new Dictionary<string, item>();
            using (StreamWriter sw = new StreamWriter("jqx.custom.less", false, Encoding.UTF8))
            {
                foreach (var d2 in d1)
                {
                    if (d2.Value.Type == JTokenType.Array)
                    {
                        foreach (var d3 in d2.Value)
                        {
                            item item = d3.First.First.ToObject<item>();
                            if (string.IsNullOrEmpty(item.defaultValue))
                                Debugger.Break();
                            if (string.IsNullOrEmpty(item.value))
                                Debugger.Break();
                            if (string.IsNullOrEmpty(item.id))
                                Debugger.Break();
                            if (string.IsNullOrEmpty(item.text))
                                Debugger.Break();
                            if (string.IsNullOrEmpty(item.type))
                                Debugger.Break();
                            string _id = item.get_id();
                            if (item.type == "color") items2.Add(_id, item);
                            sw.Write((item.get_id() + new string(' ', 40)).Substring(0, 40));
                            sw.WriteLine(@":{defaultValue}; // {value}".FormatWith(item));

                            if (item.type != "color") continue;
                            string v1 = item.value;
                            src.Replace(v1.ToUpper(), _id);
                            src.Replace(v1.ToLower(), _id);
                        }
                    }
                }
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.Write(src);
                //bool f1 = true;
                //StringBuilder s2 = new StringBuilder();
                //for (int i = 0, n = src.Length; i < n; i++)
                //{
                //    char c = src[i];
                //    if (f1)
                //    {
                //        if (c == '#')
                //        {
                //            f1 = false;
                //            s2.Clear();
                //            s2.Append(c);
                //        }
                //        else sw.Write(c);
                //    }
                //    else
                //    {
                //        if (c == ';')
                //        {
                //            f1 = true;
                //            string s3 = s2.ToString();
                //            item item1 = items.Find((_item) => _item.value.Equals(s3, StringComparison.OrdinalIgnoreCase));
                //            if (item1 == null)
                //                sw.Write(s3);
                //            else
                //                sw.Write("@{id}".FormatWith(item1));
                //            sw.Write(';');
                //        }
                //        else s2.Append(c);
                //    }
                //}
            }
        }
    }
}