using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using _DebuggerStepThroughAttribute = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Collections.Generic
{
    [_DebuggerStepThrough, DebuggerDisplay("({ID}) {FullPath}")]
    public class PathList<TValue>
    {
        static int _id_alloc = 1;
        public readonly int ID = Interlocked.Increment(ref _id_alloc);
        public readonly string Name;
        public readonly PathList<TValue> Parent;
        public readonly string FullPath;
        private readonly List<PathList<TValue>> childs = new List<PathList<TValue>>();
        private readonly Dictionary<int, PathList<TValue>> all;
        private readonly PathList<TValue> root;
        public PathList<TValue> RootNode => root;
        public bool IsRoot => object.ReferenceEquals(this, root);

        public TValue Value { get; set; }

        public PathList() : this(null, "~/") { }

        private PathList(PathList<TValue> parent, string name)
        {
            PathList<TValue> _this = (PathList<TValue>)this;
            this.Name = name;
            this.Parent = parent;
            if (object.ReferenceEquals(parent, null))
            {
                this.root = _this;
                this.all = new Dictionary<int, PathList<TValue>>();
                this.all.Add(this.ID, _this);
                if (this.Name == "~" || this.Name == "~/")
                    this.FullPath = "~/";
                else
                    this.FullPath = "/" + this.Name;
            }
            else
            {
                this.root = parent.root;
                StringBuilder s = new StringBuilder(parent.FullPath);
                if (!parent.FullPath.EndsWith("/"))
                    s.Append('/');
                s.Append(this.Name);
                this.FullPath = s.ToString();
            }
        }

        public virtual IEnumerable<PathList<TValue>> Childs
        {
            get
            {
                lock (root.all)
                    for (int n1 = 0, n2 = this.childs.Count; (n1 < n2); n1++)
                        yield return this.childs[n1];
            }
        }

        public bool HasChilds => this.childs.Count > 0;

        public virtual IEnumerable<PathList<TValue>> All
        {
            get
            {
                lock (root.all)
                    foreach (PathList<TValue> n in root.all.Values)
                        yield return n;
            }
        }

        public IEnumerable<PathList<TValue>> GetParents(bool self = true, bool root = true)
        {
            for (PathList<TValue> n = self ? this : this.Parent; n != null; n = n.Parent)
            {
                if (n.IsRoot && (root == false))
                    continue;
                yield return n;
            }
        }

        private bool GetChildNode(string path, out PathList<TValue> result, bool create)
        {
            PathList<TValue> _this = this;
            if (path == null) goto _not_found;
            if (path == "") { result = this; return true; }
            string name, next;// = parse_path(path, out next);
            path.Split("/\\?", out name, out next);

            lock (root.all)
            {
                PathList<TValue> node;
                if (name == "~")
                    node = root;
                else if (name == "")
                    node = _this;
                else
                {
                    node = null;
                    for (int n1 = 0, n2 = _this.childs.Count; node == null && n1 < n2; n1++)
                    {
                        var tmp = _this.childs[n1];
                        if (string.Compare(tmp.Name, name, true) == 0)
                            node = tmp;
                    }
                    if (node == null)
                    {
                        if (create)
                        {
                            node = new PathList<TValue>(_this, name);
                            _this.childs.Add(node);
                            root.all.Add(node.ID, node);
                        }
                        else
                            goto _not_found;
                    }
                }
                return node.GetChildNode(next, out result, create);
            }
            _not_found:
            result = null;
            return false;
        }

        public bool GetChild(string path, out PathList<TValue> result, bool create, CreateValueHandler createValue, IServiceProvider serviceProvider, params object[] parameters)
        {
            bool r = this.GetChildNode(path, out result, create);
            if (result != null)
            {
                lock (result)
                {
                    if (object.ReferenceEquals(result.Value, null))
                    {
                        if (createValue != null)
                            result.Value = createValue(result);
#if NETCORE
                        else if (serviceProvider != null)
                            result.Value = ActivatorUtilities.CreateInstance<TValue>(serviceProvider, parameters);
#endif
                        else
                            result.Value = _ctor.CreateInstance<TValue>();
                    }
                }
            }
            return r;
        }

        #region GetChild

        public PathList<TValue> GetChild(string path, bool create = false)
        {
            GetChild(path, out var result, create);
            return result;
        }

        public bool GetChild(string path, out PathList<TValue> result, bool create = false)
        {
            return GetChild(path, out result, create, null, null);
        }

        #endregion

        #region GetChild with CreateValue

        public delegate TValue CreateValueHandler(PathList<TValue> node);

        public PathList<TValue> GetChild(string path, CreateValueHandler createValue)
        {
            GetChild(path, out var result, createValue);
            return result;
        }

        public bool GetChild(string path, out PathList<TValue> result, CreateValueHandler createValue)
        {
            return this.GetChild(path, out result, createValue != null, createValue, null);
        }

        #endregion

        #region GetChild with IServiceProvider
#if NETCORE
        public PathList<TValue> GetChild(string path, IServiceProvider serviceProvider, params object[] parameters)
        {
            GetChild(path, out var result, serviceProvider, parameters);
            return result;
        }

        public bool GetChild(string path, out PathList<TValue> result, IServiceProvider serviceProvider, params object[] parameters)
        {
            return this.GetChild(path, out result, serviceProvider != null, null, serviceProvider, parameters);
        }
#endif
        #endregion

        public PathList<TValue> SetChildValue(string path, TValue value, bool replace = false)
        {
            if (this.GetChildNode(path, out var result, true))
            {
                TValue v = replace ? default(TValue) : value;
                lock (result)
                    if (object.ReferenceEquals(result.Value, v))
                        result.Value = value;
                return result;
            }
            return null;
        }


        public void Remove(string path, bool dispose = false) => this.GetChild(path, false)?.Remove(dispose);

        public void Remove(bool dispose = false)
        {
            lock (root.all)
            {
                PathList<TValue> _this = this;
                PathList<TValue> parent = this.Parent;
                if (parent != null)
                    parent.childs.RemoveAll(_this);
                root.all.RemoveValue(_this);
                while (this.childs.Count > 0)
                {
                    PathList<TValue> n = this.childs[0];
                    this.childs.RemoveAt(0);
                    if (dispose)
                        using (n.Value as IDisposable)
                            continue;
                    n.Value = default(TValue);
                }
            }
        }



        public static bool operator ==(PathList<TValue> a, PathList<TValue> b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;
            if (string.Compare(a.Name, b.Name, true) == 0)
                return a.Parent == b.Parent;
            return false;
        }
        public static bool operator !=(PathList<TValue> a, PathList<TValue> b) => !(a == b);
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }
}
