using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace InnateGlory
{
    public class SqlSettingAttribute : Attribute, IAppSettingAttribute
    {
        bool IAppSettingAttribute.GetValue(MemberInfo m, out string result, params object[] index)
        {
            return _null.noop(false, out result);
        }
    }
}