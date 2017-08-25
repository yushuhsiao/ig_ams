using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	class SR
	{
		public static string GetString(params object [] args)
		{
			return null;
		}

		public static object Error_parsing_sql_partition_resolver_string { get; set; }

		public static object Login_failed_sql_session_database { get; set; }

		public static object[] Need_v2_SQL_Server { get; set; }

		public static object[] Need_v2_SQL_Server_partition_resolver { get; set; }

		public static object[] No_database_allowed_in_sql_partition_resolver_string { get; set; }

		public static object[] No_database_allowed_in_sqlConnectionString { get; set; }

		public static object[] Bad_partition_resolver_connection_string { get; set; }

		public static object[] Cant_connect_sql_session_database { get; set; }

		public static object[] Cant_connect_sql_session_database_partition_resolver { get; set; }

		public static object[] Error_parsing_session_sqlConnectionString { get; set; }
	}
}
