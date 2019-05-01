using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InnateGlory.Pages
{
    public class TestModel : PageModel
    {
        public TestModel()
        {
        }
        public void OnGet()
        {
        }
    }

    public abstract class aaa : Page
    {
        public aaa()
        {
        }
    }
}