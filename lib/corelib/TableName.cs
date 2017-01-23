using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http.ModelBinding;
using System.Diagnostics;

namespace ams
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TableNameAttribute : Attribute
    {
        public string TableName { get; private set; }
        public string SortField { get; set; }
        public Type ClassType { get; private set; }
        public SortOrder SortOrder { get; set; }
        //public SqlBuilder.str sqlTableName { get; private set; }
        private List<SortableAttribute> sortables = new List<SortableAttribute>();
        private List<FilterableAttribute> filterables = new List<FilterableAttribute>();
        public TableNameAttribute(string tableName)
        {
            this.TableName = tableName;
            //this.sqlTableName = tableName;
            this.SortField = "CreateTime";
            this.SortOrder = SortOrder.desc;
        }

        void add(FieldInfo field, PropertyInfo property, IEnumerable<Attribute> aa)
        {
            foreach (Attribute a in aa)
            {
                SortableAttribute s = a as SortableAttribute;
                if (s != null)
                {
                    s.Field = field;
                    s.Property = property;
                    sortables.Add(s);
                }
                FilterableAttribute f = a as FilterableAttribute;
                if (f != null)
                {
                    f.Field = field;
                    f.Property = property;
                    filterables.Add(f);
                }
            }
        }

        static readonly Dictionary<Type, TableNameAttribute> _instances = new Dictionary<Type, TableNameAttribute>();

        public static TableNameAttribute GetInstance(Type classType)
        {
            lock (_instances)
            {
                TableNameAttribute result;
                if (!_instances.TryGetValue(classType, out result))
                {
                    result = _instances[classType] = classType.GetCustomAttribute<TableNameAttribute>() ?? new TableNameAttribute("");
                    result.ClassType = classType;
                    foreach (FieldInfo f in classType.GetFields(_TypeExtensions.BindingFlags0))
                        result.add(f, null, f.GetCustomAttributes());
                    foreach (PropertyInfo p in classType.GetProperties(_TypeExtensions.BindingFlags0))
                        result.add(null, p, p.GetCustomAttributes());

                    if (!string.IsNullOrEmpty(result.SortField))
                    {
                        SortableAttribute attr = result.GetSortable(result.SortField);
                        if (attr == null)
                        {
                            FieldInfo field = classType.GetField(result.SortField, _TypeExtensions.BindingFlags0);
                            if (field != null)
                                attr = new SortableAttribute(true) { Field = field };
                            else
                            {
                                PropertyInfo property = classType.GetProperty(result.SortField, _TypeExtensions.BindingFlags0); ;
                                if (property != null)
                                    attr = new SortableAttribute(true) { Property = property };
                            }
                            if (attr != null)
                                result.sortables.Add(attr);
                        }
                        else attr.Sortable = true;
                    }
                }
                return result;
            }
        }
        public static TableNameAttribute GetInstance<T>() => GetInstance(typeof(T));
        public static TableNameAttribute GetInstance(object obj) => GetInstance(obj.GetType());
        public static string GetTableName(Type classType) => GetInstance(classType).TableName;
        public static string GetTableName(object obj) => GetInstance(obj).TableName;

        public SortableAttribute GetSortable(string name)
        {
            lock (this) return sortables.Find((n) => string.Compare(n.Name, name, true) == 0);
        }
        public FilterableAttribute GetFilterable(string name)
        {
            lock (this) return filterables.Find((n) => string.Compare(n.Name, name, true) == 0);
        }
        public FilterableAttribute GetFilterable2(string name)
        {
            FieldInfo f = ClassType.GetField(name, _TypeExtensions.BindingFlags0);
            PropertyInfo p = ClassType.GetProperty(name, _TypeExtensions.BindingFlags0);
            return f?.GetCustomAttribute<FilterableAttribute>() ?? p?.GetCustomAttribute<FilterableAttribute>();
        }
        public bool IsSortable(string name)
        {
            SortableAttribute attr = GetSortable(name);
            if (attr == null) return false;
            return attr.Sortable;
        }
        public bool IsFilterable(string name, out FilterableAttribute attr)
        {
            attr = GetFilterable(name);
            if (attr == null) return false;
            return attr.Filterable;
        }
        public bool IsFilterable(string name)
        {
            FilterableAttribute attr;
            return this.IsFilterable(name, out attr);
        }
    }

    [DebuggerStepThrough]
    public static class TableName<T>
    {
        public static TableNameAttribute _
        {
            get { return TableNameAttribute.GetInstance<T>(); }
        }

        public static string Value
        {
            get { return TableNameAttribute.GetInstance(typeof(T)).TableName; }
        }
    }

    public enum SortOrder { asc, desc }

    [DebuggerDisplay("sortable:{Sortable}"), AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SortableAttribute : Attribute
    {
        public bool Sortable { get; set; }
        public SortableAttribute(bool sortable = true) { this.Sortable = sortable; }
        //internal MemberInfo Member;
        public FieldInfo Field;
        public PropertyInfo Property;
        public string Name
        {
            get { return Field?.Name ?? Property?.Name; }
        }
    }

    [DebuggerDisplay("filterable:{Filterable}"), AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FilterableAttribute : Attribute
    {
        public bool Filterable { get; set; }
        public FilterableAttribute(bool filterable = true) { this.Filterable = filterable; }
        //internal MemberInfo Member;
        public FieldInfo Field;
        public PropertyInfo Property;
        public string Name
        {
            get { return Field?.Name ?? Property?.Name; }
        }
        public Type ValueType
        {
            get { return Field?.FieldType ?? Property?.PropertyType; }
        }
        internal dynamic Value;
    }
}
