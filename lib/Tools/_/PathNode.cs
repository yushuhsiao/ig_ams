using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using _DebuggerStepThroughAttribute = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Collections.Generic
{
    [_DebuggerStepThrough, DebuggerDisplay("({ID}) {FullPath}")]
    public abstract class PathNode<T> : IDisposable where T : PathNode<T>
    {
        private PathList<T> _path;
        public int ID => _path.ID;
        public string Name => _path.Name;
        public T Parent => _path.Parent?.Value;
        public string FullPath => _path.FullPath;

        public PathNode() : this(_ctor.GetValue<PathList<T>>()) { }
        protected PathNode(PathList<T> _path)
        {
            if (_path == null)
                this._path = new PathList<T>() { Value = (T)this };
            else
                this._path = _path;
        }

        public T RootNode => _path.RootNode.Value;

        public bool IsRoot => _path.IsRoot;

        public IEnumerable<T> Childs
        {
            get
            {
                foreach (var n in _path.Childs)
                    yield return n.Value;
            }
        }

        public bool HasChilds => _path.HasChilds;

        public IEnumerable<T> All
        {
            get
            {
                foreach (var n in _path.All)
                    yield return n.Value;
            }
        }

        public IEnumerable<T> GetParents(bool self = true, bool root = true)
        {
            foreach (var n in _path.GetParents(self, root))
                yield return n.Value;
        }

        private bool GetChild(string path, out T result, bool create, Func<T> createChild, IServiceProvider serviceProvider, params object[] parameters)
        {
            var r = _path.GetChild(path, out var tmp, create, _path =>
            {
                try
                {
                    _ctor.SetValue(_path);
                    if (createChild != null)
                        return createChild();
#if NETCORE
                    else if (serviceProvider != null)
                        return ActivatorUtilities.CreateInstance<T>(serviceProvider, parameters);
#endif
                    else
                        return _ctor.CreateInstance<T>();
                }
                finally
                {
                    _ctor.Clear();
                }
            }, null);

            result = tmp?.Value;
            return r;
        }

        public T GetChild(string path, bool create = false)
        {
            GetChild(path, out var result, create, null, null);
            return result;
        }

        public bool GetChild(string path, out T result, bool create = false)
        {
            return GetChild(path, out result, create, null, null);
        }

        public T GetChild(string path, Func<T> createChild)
        {
            GetChild(path, out var result, createChild != null, createChild, null);
            return result;
        }

        public bool GetChild(string path, out T result, Func<T> createChild)
        {
            return GetChild(path, out result, createChild != null, createChild, null);
        }

#if NETCORE

        public T GetChild(string path, IServiceProvider serviceProvider, params object[] parameters)
        {
            GetChild(path, out var result, serviceProvider != null, null, serviceProvider, parameters);
            return result;
        }

        public bool GetChild(string path, out T result, IServiceProvider serviceProvider, params object[] parameters)
        {
            return GetChild(path, out result, serviceProvider != null, null, serviceProvider, parameters);
        }

#endif

        public void Remove(string path, bool dispose = false) => this.GetChild(path, false)?.Remove();

        public void Remove(bool dispose = false) => _path.Remove(dispose);

        void IDisposable.Dispose() => this.Remove();

        public static bool operator ==(PathNode<T> a, PathNode<T> b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;
            if (a._path == b._path)
                return a.Parent == b.Parent;
            return false;
        }
        public static bool operator !=(PathNode<T> a, PathNode<T> b) => !(a == b);

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }

    //[_DebuggerStepThrough, DebuggerDisplay("({ID}) {FullPath}")]
    //public abstract class TreeNode2<T> : IDisposable where T : TreeNode2<T>//, new()
    //{
    //    static int _id_alloc = 1;
    //    public readonly int ID;
    //    public virtual string Name { get; private set; }
    //    public readonly T Parent;
    //    public readonly string FullPath;
    //    private readonly List<T> childs;
    //    private readonly Dictionary<int, T> all;



    //    private readonly T root;
    //    public T RootNode => root;

    //    public bool IsRoot => object.ReferenceEquals(this, root);

    //    //static Dictionary<Thread, string> ctor_name = new Dictionary<Thread, string>();
    //    //static Dictionary<Thread, T> ctor_parent = new Dictionary<Thread, T>();

    //    //protected static T CreateItem(string name, T parent)
    //    //{
    //    //    Thread t = Thread.CurrentThread;
    //    //    lock (ctor_name) ctor_name[t] = name;
    //    //    lock (ctor_parent) ctor_parent[t] = parent;
    //    //    return new T();
    //    //}

    //    //public TreeNode()
    //    //{
    //    //    Thread t = Thread.CurrentThread;
    //    //    string name;
    //    //    lock (ctor_name)
    //    //    {
    //    //        ctor_name.TryGetValue(t, out name, true);
    //    //        name = name ?? "~";
    //    //    }
    //    //    T parent;
    //    //    lock (ctor_parent)
    //    //        ctor_parent.TryGetValue(t, out parent, true);

    //    //    T _this = (T)this;
    //    //    this.ID = Interlocked.Increment(ref _id_alloc);
    //    //    this.Name = name;
    //    //    this.Parent = parent;
    //    //    this.childs = new List<T>();
    //    //    if (object.ReferenceEquals(parent, null))
    //    //    {
    //    //        this.root = _this;
    //    //        this.all = new Dictionary<int, T>();
    //    //        if (this.Name == "~")
    //    //            this.FullPath = "~/";
    //    //        else
    //    //            this.FullPath = "/" + this.Name;
    //    //    }
    //    //    else
    //    //    {
    //    //        this.root = parent.root;
    //    //        this.all = root.all;
    //    //        parent.childs.Add(_this);
    //    //        StringBuilder s = new StringBuilder(parent.FullPath);
    //    //        if (!parent.FullPath.EndsWith("/"))
    //    //            s.Append('/');
    //    //        s.Append(this.Name);
    //    //        this.FullPath = s.ToString();
    //    //    }
    //    //    root.all.Add(_this.ID, _this);
    //    //}

    //    //protected virtual T CreateChild(string name) => CreateItem(name, (T)this); //_ctor.Create<T>((T)this, name);

    //    public TreeNode2() : this(_ctor.GetValue<T>(), _ctor.GetValue<string>()) { }

    //    protected TreeNode2(T parent, string name)
    //    {
    //        T _this = (T)this;
    //        this.ID = Interlocked.Increment(ref _id_alloc);
    //        this.Name = name;
    //        this.Parent = parent;
    //        this.childs = new List<T>();
    //        if (object.ReferenceEquals(parent, null))
    //        {
    //            this.root = _this;
    //            this.all = new Dictionary<int, T>();
    //            this.all.Add(this.ID, _this);
    //            if (this.Name == "~")
    //                this.FullPath = "~/";
    //            else
    //                this.FullPath = "/" + this.Name;
    //        }
    //        else
    //        {
    //            this.root = parent.root;
    //            //this.all = root.all;
    //            //parent.childs.Add(_this);
    //            StringBuilder s = new StringBuilder(parent.FullPath);
    //            if (!parent.FullPath.EndsWith("/"))
    //                s.Append('/');
    //            s.Append(this.Name);
    //            this.FullPath = s.ToString();
    //        }
    //        //root.all.Add(_this.ID, _this);
    //    }

    //    public virtual IEnumerable<T> Childs
    //    {
    //        get
    //        {
    //            lock (root.all)
    //                for (int n1 = 0, n2 = this.childs.Count; (n1 < n2); n1++)
    //                    yield return this.childs[n1];
    //        }
    //    }

    //    public bool HasChilds => this.childs.Count > 0;

    //    public virtual IEnumerable<T> All
    //    {
    //        get
    //        {
    //            lock (root.all)
    //                foreach (T n in root.all.Values)
    //                    yield return n;
    //        }
    //    }

    //    public IEnumerable<T> GetParents(bool self = true, bool root = true)
    //    {
    //        for (T n = self ? (T)this : this.Parent; n != null; n = n.Parent)
    //        {
    //            if (n.IsRoot && (root == false))
    //                continue;
    //            yield return n;
    //        }
    //    }

    //    public delegate T CreateChildHandler(T parent, string name);

    //    protected virtual T CreateChild(string name) => _ctor.Create<T>((T)this, name);

    //    private static T _CreateChild(T parent, string name) => parent.CreateChild(name);

    //    public T GetChild(string path, CreateChildHandler create)
    //    {
    //        T _this = (T)this;
    //        if (path == null) return null;
    //        if (path == "") return _this;
    //        string name, next;// = parse_path(path, out next);
    //        path.Split("/\\?", out name, out next);

    //        lock (root.all)
    //        {
    //            T node;
    //            if (name == "~")
    //                node = root;
    //            else if (name == "")
    //                node = _this;
    //            else
    //            {
    //                node = null;
    //                for (int n1 = 0, n2 = _this.childs.Count; node == null && n1 < n2; n1++)
    //                {
    //                    var tmp = _this.childs[n1];
    //                    if (string.Compare(tmp.Name, name, true) == 0)
    //                        node = tmp;
    //                }
    //                if (node == null)
    //                {
    //                    if (create == null)
    //                        return null;
    //                    try
    //                    {
    //                        node = create(_this, name);
    //                        _this.childs.Add(node);
    //                        root.all.Add(node.ID, node);
    //                    }
    //                    catch { return null; }
    //                }
    //            }
    //            return node.GetChild(next, create);
    //        }
    //    }

    //    public T GetChild(string path, bool create = false) => GetChild(path, create ? _CreateChild : (CreateChildHandler)null);

    //    public bool GetChild(string path, out T result, CreateChildHandler create) => null != (result = GetChild(path, create));

    //    public bool GetChild(string path, out T result, bool create = false) => null != (result = GetChild(path, create));

    //    public void Sort(Comparison<T> comparison, bool sort_childs = true)
    //    {
    //        lock (root.all)
    //        {
    //            //this.childs.Sort((x, y) => comparison(all[x], all[y]));
    //            this.childs.Sort(comparison);
    //            if (sort_childs)
    //                foreach (T n in this.childs)
    //                    n.Sort(comparison, sort_childs);
    //        }
    //    }

    //    public static bool operator ==(TreeNode2<T> a, TreeNode2<T> b)
    //    {
    //        if (object.ReferenceEquals(a, b)) return true;
    //        if (object.ReferenceEquals(a, null)) return false;
    //        if (object.ReferenceEquals(b, null)) return false;
    //        if (string.Compare(a.Name, b.Name, true) == 0)
    //            return a.Parent == b.Parent;
    //        return false;
    //    }
    //    public static bool operator !=(TreeNode2<T> a, TreeNode2<T> b) => !(a == b);

    //    public override bool Equals(object obj) => base.Equals(obj);
    //    public override int GetHashCode() => base.GetHashCode();

    //    public void Remove(string path) => this.GetChild(path, false)?.Remove();

    //    public void Remove()
    //    {
    //        lock (root.all)
    //        {
    //            T _this = (T)this;
    //            T parent = this.Parent;
    //            if (parent != null)
    //                parent.childs.RemoveAll(_this);
    //            root.all.RemoveValue(_this);
    //            while (this.childs.Count > 0)
    //            {
    //                T n = this.childs[0];
    //                using (n as IDisposable)
    //                    continue;
    //            }
    //        }
    //    }

    //    void IDisposable.Dispose() => this.Remove();
    //}
}