using InnateGlory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webix
{
    public struct tree_node
    {
        public UserId id { get; set; }
        public string value { get; set; }
        public bool webix_kids { get; set; }
    }
    public struct tree_childs
    {
        public UserId parent { get; set; }
        public IEnumerable<tree_node> data { get; set; }
    }
}
