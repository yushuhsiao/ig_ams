using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using Dapper;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Data
{
    [_DebuggerStepThrough]
    public class SqlSchemaTable : Dictionary<string, Type>
    {
        static Dictionary<string, SqlSchemaTable> cache = new Dictionary<string, SqlSchemaTable>();

        private SqlSchemaTable(SqlCmd sqlcmd, string commandText, string tag)
        {
            using (SqlDataReader r = sqlcmd.ExecuteReader(commandText))
                for (int i = 0; i < r.FieldCount; i++)
                    this[r.GetName(i)] = r.GetFieldType(i);
        }
        private SqlSchemaTable(IDbConnection conn, string commandText, string tag)
        {
            using(IDataReader r = conn.ExecuteReader(commandText))
                for (int i = 0; i < r.FieldCount; i++)
                    this[r.GetName(i)] = r.GetFieldType(i);
        }

        public static SqlSchemaTable GetSchema(SqlCmd sqlcmd, string tableName, string id = null)
        {
            return SqlSchemaTable.GetSchemaFromCommandText(sqlcmd, string.Format("select top(0) * from {0}", tableName), id ?? tableName);
        }
        public static SqlSchemaTable GetSchema(IDbConnection conn, string tableName, string id = null)
        {
            return SqlSchemaTable.GetSchemaFromCommandText(conn, string.Format("select top(0) * from {0}", tableName), id ?? tableName);
        }

        public static SqlSchemaTable GetSchemaFromCommandText(SqlCmd sqlcmd, string commandText, string id = null)
        {
            lock (cache)
            {
                string _id = id ?? commandText;
                if (cache.ContainsKey(_id))
                    return cache[_id];
                else
                    return cache[_id] = new SqlSchemaTable(sqlcmd, commandText, id);
            }
        }
        public static SqlSchemaTable GetSchemaFromCommandText(IDbConnection conn, string commandText, string id = null)
        {
            lock (cache)
            {
                string _id = id ?? commandText;
                if (cache.ContainsKey(_id))
                    return cache[_id];
                else
                    return cache[_id] = new SqlSchemaTable(conn, commandText, id);
            }
        }

        public bool HasField(string name)
        {
            foreach (string s in this.Keys)
                if (string.Compare(s, name, true) == 0)
                    return true;
            return false;
        }

        public string GetFieldName(string name)
        {
            foreach (string s in this.Keys)
                if (string.Compare(s, name, true) == 0)
                    return s;
            return null;
        }

        public Type GetFieldType(string name)
        {
            foreach (KeyValuePair<string, Type> s in this)
                if (string.Compare(s.Key, name, true) == 0)
                    return s.Value;
            return null;
        }

        public IEnumerable<MemberInfo> GetValueMembers(object obj)
        {
            foreach (FieldInfo f in obj.GetType().GetFields(_TypeExtensions._BindingFlags))
                if (this.HasField(f.Name))
                    yield return f;
            foreach (PropertyInfo p in obj.GetType().GetProperties(_TypeExtensions._BindingFlags))
                if (this.HasField(p.Name))
                    yield return p;
        }

        public IEnumerable<FieldInfo> GetFields(object obj)
        {
            foreach (FieldInfo f in obj.GetType().GetFields(_TypeExtensions._BindingFlags))
                if (this.HasField(f.Name))
                    yield return f;
        }
        public IEnumerable<PropertyInfo> GetProperties(object obj)
        {
            foreach (PropertyInfo p in obj.GetType().GetProperties(_TypeExtensions._BindingFlags))
                if (this.HasField(p.Name))
                    yield return p;
        }
    }
}
