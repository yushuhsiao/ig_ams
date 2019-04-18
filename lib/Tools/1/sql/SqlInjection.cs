using System;
using System.Collections.Generic;
using System.Text;

namespace System.Data
{
    public static class SQLinjection
    {
        static string[] _escape = { "'", "\"\"", ")", "(", ";", "-", "|" };
        public static string escape(string input)
        {
            foreach (string r in _escape)
                if (input.Contains(r))
                    input = input.Replace(r, "''");
            return input;
        }

        static string[] known_bad = { "select", "insert", "update", "delete", "drop", "--", "'" };
        public static bool validate_string(string input)
        {
            foreach (string bad in known_bad)
                if (input.IndexOf(bad, StringComparison.OrdinalIgnoreCase) >= 0)
                    return false;
            return true;
        }

        public static string Validate_SQLinjection(this string input ) 
        {
            if (input == null)
                return input;
            if (validate_string(input))
                return escape(input);
            else
                return "";
        }
    }
}