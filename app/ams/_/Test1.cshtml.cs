using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InnateGlory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ams.Pages
{
    [Acl("aaa")]
    public class TestModel : PageModel
    {
        public TestModel()
        {
        }

        public void OnGet()
        {

        }
    }
}