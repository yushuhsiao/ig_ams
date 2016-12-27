using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _DebuggerStepThroughAttribute = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough, DebuggerDisplay("({ID}) {FullPath}")]
    public abstract class TreeNode<T> : IDisposable where T : TreeNode<T>, new()
    {
        static int _id_alloc = 1;
        public readonly int ID;
        public virtual string Name { get; private set; }
        public readonly T Parent;
        public readonly string FullPath;
        private readonly List<T> childs;
        private readonly Dictionary<int, T> all;


        readonly T root;

        public bool IsRoot
        {
            get { return object.ReferenceEquals(this, root); }
        }

        static Dictionary<Thread, string> ctor_name = new Dictionary<Thread, string>();
        static Dictionary<Thread, T> ctor_parent = new Dictionary<Thread, T>();

        protected static T CreateItem(string name, T parent)
        {
            Thread t = Thread.CurrentThread;
            lock (ctor_name) ctor_name[t] = name;
            lock (ctor_parent) ctor_parent[t] = parent;
            return new T();
        }

        public TreeNode()
        {
            Thread t = Thread.CurrentThread;
            string name;
            lock (ctor_name)
                name = ctor_name.GetValue(t, true) ?? "~";
            T parent;
            lock(ctor_parent)
                parent = ctor_parent.GetValue(t, true);

            T _this = (T)this;
            this.ID = Interlocked.Increment(ref _id_alloc);
            this.Name = name;
            this.Parent = parent;
            this.childs = new List<T>();
            if (object.ReferenceEquals(parent, null))
            {
                this.root = _this;
                this.all = new Dictionary<int, T>();
                if (this.Name == "~")
                    this.FullPath = "~/";
                else
                    this.FullPath = "/" + this.Name;
            }
            else
            {
                this.root = parent.root;
                this.all = root.all;
                parent.childs.Add(_this);
                StringBuilder s = new StringBuilder(parent.FullPath);
                if (!parent.FullPath.EndsWith("/"))
                    s.Append('/');
                s.Append(this.Name);
                this.FullPath = s.ToString();
            }
            root.all.Add(_this.ID, _this);
        }

        void IDisposable.Dispose()
        {
            lock (root.all)
            {
                T _this = (T)this;
                T parent = this.Parent;
                if (parent != null)
                    parent.childs.RemoveAll(_this);
                root.all.RemoveValue(_this);
                while (this.childs.Count > 0)
                    using (T n = this.childs[0])
                        continue;
            }
        }

        protected virtual T CreateChild(string name) { return CreateItem(name, (T)this); }
        //protected virtual T CreateChild(string name)
        //{
        //    return (T)ctor.Invoke(new object[] { name, (T)this });
        //}
        //static readonly ConstructorInfo ctor;
        //static TreeNode()
        //{
        //    ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new[] { typeof(string), typeof(T) }, null);
        //    if (ctor == null) throw new Exception(string.Format("new {0}(string name, {0} parent) not found!", (typeof(T).Name)));
        //}

        public virtual IEnumerable<T> Childs
        {
            get
            {
                lock (root.all)
                    for (int n1 = 0, n2 = this.childs.Count; (n1 < n2); n1++)
                        yield return this.childs[n1];
            }
        }

        public bool HasChilds
        {
            get { return this.childs.Count > 0; }
        }

        public virtual IEnumerable<T> All
        {
            get
            {
                lock (root.all)
                    foreach (T n in root.all.Values)
                        yield return n;
            }
        }

        public IEnumerable<T> GetParents(bool self = true, bool root = true)
        {
            for (T n = self ? (T)this : this.Parent; n != null; n = n.Parent)
            {
                if (n.IsRoot && (root == false))
                    continue;
                yield return n;
            }
        }

        public virtual T GetChild(string path, bool create = false)
        {
            T _this = (T)this;
            if (path == null) return null;
            if (path == "") return _this;
            string name, next;// = parse_path(path, out next);
            path.Split("/\\?", out name, out next);

            lock (root.all)
            {
                T node;
                if (name == "~")
                    node = root;
                else
                {
                    node = null;
                    for (int n1 = 0, n2 = _this.childs.Count; (node == null) && (n1 < n2); n1++)
                    {
                        node = _this.childs[n1];
                        if (string.Compare(node.Name, name, true) != 0)
                            node = null;
                    }
                }
                if (node == null)
                {
                    if (create) node = this.CreateChild(name);
                    else return null;
                }
                return node.GetChild(next, create);
            }
        }
        public bool GetChild(string path, bool create, out T result)
        {
            return null != (result = GetChild(path, create));
        }

        public void Sort(Comparison<T> comparison, bool sort_childs = true)
        {
            lock (root.all)
            {
                //this.childs.Sort((x, y) => comparison(all[x], all[y]));
                this.childs.Sort(comparison);
                if (sort_childs)
                    foreach (T n in this.childs)
                        n.Sort(comparison, sort_childs);
            }
        }

        public static bool operator ==(TreeNode<T> a, TreeNode<T> b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;
            if (string.Compare(a.Name, b.Name, true) == 0)
                return a.Parent == b.Parent;
            return false;
        }
        public static bool operator !=(TreeNode<T> a, TreeNode<T> b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}