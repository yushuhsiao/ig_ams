using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using ams.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web;
using ams.Controllers;

#region //
namespace ams.Controllers
{
    //[Route("~/users_/{action}")]
    //public class UserApiController : _ApiController
    //{
    //    public jqxGridResponse<object> list(jqxGridRequest opt)
    //    {
    //        jqxGridResponse<object> objs = new jqxGridResponse<object>();
    //        for (int i = opt.recordstartindex; i < opt.recordendindex; i++)
    //            objs.records.Add(new { ID = i, ACNT = RandomValue.GetRandomString(10) });
    //        objs.totalrecords = 10000;
    //        return objs;
    //    }

    //    public void add()
    //    {
    //    }

    //    public void get()
    //    {
    //    }

    //    public void set()
    //    {
    //    }
    //}
}
#endregion
