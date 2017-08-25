using ams.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCID = System.Int32;

namespace ams
{
    public class langHelper
    {
        public const string value_name = nameof(ValueTextPair<object>.value);
        public const string label_name = nameof(ValueTextPair<object>.label);

        public class ValueTextPair<T>
        {
            public T value;
            public string label;
            public ValueTextPair() { }
            public ValueTextPair(T value, string label)
            {
                this.value = value;
                this.label = label;
            }
        }
        public class ValueTextPairs<T> : List<ValueTextPair<T>> { }

        public static ValueTextPairs<string> GetEnums/*  */(string name, /*      */ params string[] exclude) { return GetEnums(name, null, exclude, true); }
        public static ValueTextPairs<string> GetEnumsIn/**/(string name, /*      */ params string[] include) { return GetEnums(name, null, include, false); }
        public static ValueTextPairs<string> GetEnums/*  */(string name, LCID lcid, params string[] exclude) { return GetEnums(name, lcid, exclude, true); }
        public static ValueTextPairs<string> GetEnumsIn/**/(string name, LCID lcid, params string[] include) { return GetEnums(name, lcid, include, false); }
        static ValueTextPairs<string> GetEnums(string name, LCID? lcid, string[] list, bool isExclude)
        {
            LangItem node = LangItem.Cache.Value.GetEnumNode(name);
            list = list ?? _null.strings;
            ValueTextPairs<string> r = new ValueTextPairs<string>();
            foreach (LangItem n1 in node.Childs)
            {
                if ((list.Length > 0) && (isExclude == list.Contains(name))) continue;
                string text = node.GetValue(n1.Name, n1.Name, lcid, false);
                r.Add(new ValueTextPair<string>(n1.Name, text));
            }
            return r;
        }

        public static ValueTextPairs<object> GetEnums/*  */<T>(/*      */ params T[] exclude) where T : struct { return GetEnums<T>(null, exclude, true); }
        public static ValueTextPairs<object> GetEnumsIn/**/<T>(/*      */ params T[] include) where T : struct { return GetEnums<T>(null, include, false); }
        public static ValueTextPairs<object> GetEnums/*  */<T>(LCID lcid, params T[] exclude) where T : struct { return GetEnums<T>(lcid, exclude, true); }
        public static ValueTextPairs<object> GetEnumsIn/**/<T>(LCID lcid, params T[] include) where T : struct { return GetEnums<T>(lcid, include, false); }
        static ValueTextPairs<object> GetEnums<T>(LCID? lcid, T[] list, bool isExclude) where T : struct
        {
            if (typeof(T).IsEnum)
            {
                LangItem node = LangItem.Cache.Value.GetEnumNode<T>();
                list = list ?? _null<T>.array;
                ValueTextPairs<object> r = new ValueTextPairs<object>();
                foreach (T name in Enum.GetValues(typeof(T)))
                {
                    if ((list.Length > 0) && (isExclude == list.Contains(name))) continue;
                    string text = node.GetValue(name.ToString(), name.ToString(), lcid, false);
                    r.Add(new ValueTextPair<object>(name, text));
                }
                return r;
            }
            return null;
        }

        public static List<string> GetEnums2/*  */(string name, /*      */ params string[] exclude) { return GetEnums2(name, null, exclude, true); }
        public static List<string> GetEnums2In/**/(string name, /*      */ params string[] include) { return GetEnums2(name, null, include, false); }
        public static List<string> GetEnums2/*  */(string name, LCID lcid, params string[] exclude) { return GetEnums2(name, lcid, exclude, true); }
        public static List<string> GetEnums2In/**/(string name, LCID lcid, params string[] include) { return GetEnums2(name, lcid, include, false); }
        static List<string> GetEnums2(string name, LCID? lcid, string[] list, bool isExclude)
        {
            LangItem node = LangItem.Cache.Value.GetEnumNode(name);
            list = list ?? _null.strings;
            List<string> r = new List<string>();
            foreach (LangItem n1 in node.Childs)
            {
                if ((list.Length > 0) && (isExclude == list.Contains(name))) continue;
                string text = node.GetValue(n1.Name, n1.Name, lcid, false);
                r.Add(text ?? n1.Name);
            }
            return r;

        }

        public static List<string> GetEnums2/*  */<T>(/*      */ params T[] exclude) where T : struct { return GetEnums2<T>(null, exclude, true); }
        public static List<string> GetEnums2In/**/<T>(/*      */ params T[] include) where T : struct { return GetEnums2<T>(null, include, false); }
        public static List<string> GetEnums2/*  */<T>(LCID lcid, params T[] exclude) where T : struct { return GetEnums2<T>(lcid, exclude, true); }
        public static List<string> GetEnums2In/**/<T>(LCID lcid, params T[] include) where T : struct { return GetEnums2<T>(lcid, include, false); }
        static List<string> GetEnums2<T>(LCID? lcid, T[] list, bool isExclude) where T : struct
        {
            if (typeof(T).IsEnum)
            {
                LangItem node = LangItem.Cache.Value.GetEnumNode<T>();
                list = list ?? _null<T>.array;
                List<string> r = new List<string>();
                foreach (T name in Enum.GetValues(typeof(T)))
                {
                    if ((list.Length > 0) && (isExclude == list.Contains(name))) continue;
                    string text = node.GetValue(name.ToString(), name.ToString(), lcid, false);
                    r.Add(text);
                }
                return r;
            }
            return null;
        }

        public static ValueTextPairs<int> CorpList(bool include_root = true)
        {
            CorpInfo _default; return CorpList(out _default, include_root);
        }
        public static ValueTextPairs<int> CorpList(out CorpInfo _default, bool include_root = true)
        {
            _User user = _User.Current;
            ValueTextPairs<int> r = new ValueTextPairs<int>();
            if (user.ID.IsRoot)
            {
                _default = null;
                foreach (CorpInfo corp in CorpInfo.Cache.Value)
                {
                    if (corp.ID.IsRoot)
                        if (include_root == false) continue;
                    _default = _default ?? corp;
                    r.Add(new ValueTextPair<int>(corp.ID, corp.UserName));
                }
            }
            else
            {
                _default = user.GetCorpInfo();
                r.Add(new ValueTextPair<int>(_default.ID, _default.UserName));
            }
            return r;
        }

        public static ValueTextPairs<int> PlatformIDs()
        {
            ValueTextPairs<int> r = new ValueTextPairs<int>();
            LangItem lang = LangItem.Cache.Value.GetEnumNode("PlatformName");
            foreach (PlatformInfo p in PlatformInfo.Cache.Value)
                r.Add(new ValueTextPair<int>(p.ID, lang.GetValue(p.PlatformName, p.PlatformName, null, false)));
            return r;
        }

        public static ValueTextPairs<UserName> PlatformNames()
        {
            ValueTextPairs<UserName> r = new ValueTextPairs<UserName>();
            LangItem lang = LangItem.Cache.Value.GetEnumNode("PlatformName");
            foreach (PlatformInfo p in PlatformInfo.Cache.Value)
            {
                r.Add(new ValueTextPair<UserName>(p.PlatformName, lang.GetValue(p.PlatformName, p.PlatformName, null, false)));
            }
            return r;
        }

        public static ValueTextPairs<object> PlatformTypes()
        {
            return GetEnumsIn(PlatformType.InnateGloryA, PlatformType.InnateGloryB, PlatformType.InnateGloryC, PlatformType.InnateGlory_Appeal);
        }

        public static ValueTextPairs<object> GameNames()
        {
            ValueTextPairs<object> r = new ValueTextPairs<object>();
            LangItem lang = LangItem.Cache.Value.GetEnumNode("GameName");
            foreach (GameInfo g in GameInfo.Cache.Value)
            {
                r.Add(new ValueTextPair<object>(g.Name, lang.GetValue(g.Name, g.Name, null, false)));
            }
            return r;
        }

        //public static ValueTextPairs<object> PlatformGames()
        //{
        //    ValueTextPairs<object> r = new ValueTextPairs<object>();
        //    LangItem lang = LangItem.Cache.Value.GetEnumNode("PlatformGames");
        //    foreach (PlatformGameInfo g in PlatformGameInfo.Cache.Value)
        //    {
        //        r.Add(new ValueTextPair<object>(g., lang.GetValue(g.Name, g.Name, null, false)));
        //    }
        //    return r;
        //}

        public static ValueTextPairs<object> LogTypes()
        {
            return GetEnums<LogType>();
        }

        //public static ValueTextPairs<object> GameClass()
        //{
        //    ValueTextPairs<object> r = new ValueTextPairs<object>();
        //    LangItem lang = LangItem.Cache.Value.GetEnumNode<GameClass>();
        //    foreach (GameClass g in langHelper.GetEnums<GameClass>(null, ams.GameClass.Others)
        //    {
        //        r.Add(new ValueTextPair<object>(g.ID, lang.GetValue(g.Name, g.Name, null, false)));
        //    }
        //    return r;
        //}
    }
}
