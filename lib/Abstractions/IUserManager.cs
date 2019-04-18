using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InnateGlory
{
    internal interface IUserManager
    {
        string SchemeName { get; }
        IUser CurrentUser { get; }
        IUser GetCurrentUser(HttpContext context);

        bool InternalApiServer { get; }
        bool AllowAgentLogin { get; }
        bool AllowAdminLogin { get; }
        bool AllowMemberLogin { get; }
    }
}
