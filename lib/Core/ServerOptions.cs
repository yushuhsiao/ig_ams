using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading;

namespace InnateGlory
{
    public class ServerOptions
    {
        private const string Default_CoreDB_Key = "CoreDB_R";
        public string CoreDB_Key { get; set; } = Default_CoreDB_Key;
    }
}