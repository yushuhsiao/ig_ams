using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameControl.Models
{
    public class Category
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class Location
    {
        public int SelectedCode { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}