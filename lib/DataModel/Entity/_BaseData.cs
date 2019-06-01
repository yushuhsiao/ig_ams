using System;
using System.Data;
using System.Diagnostics;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class BaseData
    {
        //internal IServiceProvider ServiceProvider { get; set; }

        //public BaseData() { }
        //public BaseData(IServiceProvider services) { this._services = services; }

        //[DebuggerStepThrough]
        //public IServiceProvider GetServiceProvider() => _services;

        //[DebuggerStepThrough]
        //public void SetServiceProvider(IServiceProvider value) => _services = value;

        public DateTime CreateTime { get; set; }
        public UserId CreateUser { get; set; }
        public DateTime ModifyTime { get; set; }
        public UserId ModifyUser { get; set; }
    }
}
