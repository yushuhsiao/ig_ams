using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InnateGlory.Pages.Users
{
    public class AgentDetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public UserId Id { get; set; }

        public void OnGet()
        {
        }
    }
}