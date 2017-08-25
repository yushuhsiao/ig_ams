using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace ams
{
    //[DebuggerDisplay("{FullPath} ({_id})")]
    //public abstract class VirtualPath<T> where T : VirtualPath<T>
    //{
    //    public readonly Guid _id;
    //    public readonly string Name;
    //    public readonly T Parent;
    //    public readonly string FullPath;
    //    private readonly List<T> childs;

    //    readonly T root;
    //    protected VirtualPath() : this("~", null) { }
    //    protected VirtualPath(string name, T parent)
    //    {
    //        this._id = Guid.NewGuid();
    //        this.Name = name;
    //        this.Parent = parent;
    //        this.childs = new List<T>();
    //        if (this.Parent == null)
    //        {
    //            this.root = (T)this;
    //            if (this.Name == "~")
    //                this.FullPath = "~/";
    //            else
    //                this.FullPath = "/" + this.Name;
    //        }
    //        else
    //        {
    //            this.root = parent.root;
    //            StringBuilder s = new StringBuilder(parent.FullPath);
    //            if (!parent.FullPath.EndsWith("/"))
    //                s.Append('/');
    //            s.Append(this.Name);
    //            this.FullPath = s.ToString();
    //        }
    //    }
    //    protected virtual T CreateChild(string name)
    //    {
    //        ConstructorInfo ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new[] { typeof(string), typeof(T) }, null);
    //        return (T)ctor.Invoke(new object[] { name, (T)this });
    //    }
    //    //protected abstract T GetRoot();

    //    public virtual T GetChild(string path, bool create = false)
    //    {
    //        T _this = (T)this;
    //        if (path == null) return null;
    //        if (path == "") return _this;
    //        int n1 = 0, n2, _len = path.Length;
    //        string name = path, next = "";
    //        for (; n1 < _len; n1++)
    //        {
    //            char c = path[n1];
    //            if ((c == '/') || (c == '\\') || (c == '?'))
    //            {
    //                name = path.Substring(0, n1).Trim();
    //                if (c != '?')
    //                    next = path.Substring(n1 + 1);
    //                break;
    //            }
    //        }
    //        //int n = path.IndexOf('/');
    //        //if (n == -1) n = path.IndexOf('\\');
    //        //string name, next;
    //        //if (n == -1)
    //        //{
    //        //    name = path;
    //        //    next = "";
    //        //}
    //        //else
    //        //{
    //        //    name = path.Substring(0, n);
    //        //    next = path.Substring(n + 1);
    //        //}
    //        lock (this.childs)
    //        {
    //            T node;
    //            if (name == "~")
    //                node = root;
    //            else
    //            {
    //                node = null;
    //                for (n1 = 0, n2 = _this.childs.Count; (node == null) && (n1 < n2); n1++)
    //                    if (string.Compare(_this.childs[n1].Name, name, true) == 0)
    //                        node = (T)_this.childs[n1];
    //            }
    //            if (node == null)
    //            {
    //                if (create) _this.childs.Add(node = this.CreateChild(name));
    //                else return null;
    //            }
    //            return node.GetChild(next, create);
    //        }
    //    }
    //    public bool GetChild(string path, bool create, out T result)
    //    {
    //        return null != (result = GetChild(path, create));
    //    }


    //    public static bool operator ==(VirtualPath<T> a, VirtualPath<T> b)
    //    {
    //        if (object.ReferenceEquals(a, b)) return true;
    //        if (object.ReferenceEquals(a, null)) return false;
    //        if (object.ReferenceEquals(b, null)) return false;
    //        if (string.Compare(a.Name, b.Name, true) == 0)
    //            return a.Parent == b.Parent;
    //        return false;
    //    }
    //    public static bool operator !=(VirtualPath<T> a, VirtualPath<T> b)
    //    {
    //        return !(a == b);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return base.Equals(obj);
    //    }
    //    public override int GetHashCode()
    //    {
    //        return base.GetHashCode();
    //    }
    //}

    [JsonConverter(typeof(VirtualPath.JsonConverter))]
    public sealed class VirtualPath : TreeNode<VirtualPath>
    {
        class JsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                string s = serializer.Deserialize<string>(reader);
                return VirtualPath.GetPath(s);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        public static readonly VirtualPath root = new VirtualPath();

        //public VirtualPath() : base() { }
        //private VirtualPath(string name, VirtualPath parent) : base(name, parent) { }
        //protected override VirtualPath CreateChild(string name) { return new VirtualPath(name, this); }

        public static VirtualPath GetPath(string path, bool create = true)
        {
            return root.GetChild(path, create);
        }
        public static bool GetPath(string path, out VirtualPath result)
        {
            return root.GetChild(path, true, out result);
        }
        public static bool GetPath(string path, bool create, out VirtualPath result)
        {
            return root.GetChild(path, create, out result);
        }
    }


    //[TypeConverter(typeof(VirtualPathTypeConverter))]
    //public struct VirtualPath
    //{
    //    string _path;
    //    public string Path
    //    {
    //        get { return _path; }
    //        set
    //        {
    //            this._path = value;
    //            value = value.Trim(true);
    //            if (value != null && VirtualPathUtility.IsAppRelative(value))
    //            {
    //                if (value != "~/")
    //                    value = VirtualPathUtility.RemoveTrailingSlash(value);
    //                try { this.Value = VirtualPathUtility.ToAppRelative(value); return; }
    //                catch { }
    //            }
    //            this.Value = null;
    //        }
    //    }
    //    public string Value { get; private set; }

    //    public VirtualPath Parent
    //    {
    //        get
    //        {
    //            if ((this.Value != null) && (this.Value != "~/"))
    //            {
    //                int n = this.Value.LastIndexOf('/');
    //                if (n >= 0)
    //                    return new VirtualPath() { Path = this.Value.Substring(0, n + 1) };
    //            }
    //            return default(VirtualPath);
    //        }
    //    }

    //    public static explicit operator VirtualPath(string value)
    //    {
    //        return new VirtualPath() { Path = value };
    //    }
    //    public static explicit operator string(VirtualPath n)
    //    {
    //        return n.Value;
    //    }
    //    public override bool Equals(object obj)
    //    {
    //        return base.Equals(obj);
    //    }
    //    public override int GetHashCode()
    //    {
    //        return base.GetHashCode();
    //    }

    //    public static bool operator ==(VirtualPath a, VirtualPath b)
    //    {
    //        //bool n1 = object.ReferenceEquals(a, null);
    //        //bool n2 = object.ReferenceEquals(b, null);
    //        //if (n1 && n2) return true;
    //        //else if (n1 || n2) return false;
    //        return a.Value == b.Value;
    //    }
    //    public static bool operator !=(VirtualPath a, VirtualPath b)
    //    {
    //        return !(a == b);
    //    }

    //    public override string ToString()
    //    {
    //        return this.Value;
    //    }
    //}
    //class VirtualPathTypeConverter : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return sourceType == typeof(string);
    //    }
    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //    {
    //        return destinationType == typeof(string);
    //    }
    //    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    //    {
    //        return (VirtualPath)(value as string);
    //    }
    //    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (value is VirtualPath)
    //            return ((VirtualPath)value).Value;
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }
    //}
}
