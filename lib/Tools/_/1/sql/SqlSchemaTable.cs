using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace System.Data
{

    [DebuggerStepThrough]
    public class SqlSchemaTable : Dictionary<string, Type>
    {
        static Dictionary<string, SqlSchemaTable> cache = new Dictionary<string, SqlSchemaTable>();

        public static SqlSchemaTable GetSchema(SqlCmd sqlcmd, string tableName)
        {
            return SqlSchemaTable.GetSchemaFromCommandText(sqlcmd, string.Format("select top(0) * from {0} nolock", tableName));
        }

        public static SqlSchemaTable GetSchemaFromCommandText(SqlCmd sqlcmd, string sqlstr)
        {
            lock (cache)
            {
                SqlSchemaTable result;
                if (!cache.TryGetValue(sqlstr, out result))
                {
                    cache[sqlstr] = result = new SqlSchemaTable();
                    using (SqlDataReader r = sqlcmd.ExecuteReader(sqlstr))
                        for (int i = 0; i < r.FieldCount; i++)
                            result[r.GetName(i)] = r.GetFieldType(i);
                }
                return result;
            }
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
    }
}
