using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory.Pages.Users
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }



        //public static IHtmlContent TreeRoot(Microsoft.AspNetCore.Mvc.RazorPages.Page page, IHtmlHelper Html, IJsonHelper Json, DataService _data, IUser _User, bool include_root = true)
        //{
        //    return Html.Raw(Json.Serialize(TreeRoot(_data, _User, include_root)));
        //}

        //public static List<object> TreeRoot(DataService _data, IUser _User, bool include_root = true)
        //{
        //    List<object> tt = new List<object>();
        //    var user = _data.Users.GetUser(_User.Id);
        //    if (user.CorpId.IsRoot)
        //    {
        //        foreach (var c in _data.Corps.All)
        //        {
        //            if (c.Id.IsRoot && include_root == false)
        //                continue;
        //            tt.Add(new { id = c.Id, value = c.DisplayName });
        //        }
        //    }
        //    else if (user is Data.Admin)
        //    {
        //        var parent = _data.Agents.Get(user.ParentId);
        //        tt.Add(new { id = parent.Id, value = parent.DisplayName });
        //    }
        //    else if (user is Data.Agent)
        //    {
        //        tt.Add(new { id = user.Id, value = user.DisplayName });
        //    }
        //    return tt;
        //}
    }
}