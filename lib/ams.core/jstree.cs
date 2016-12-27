using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ams
{
    public class jstree
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class node
        {
            [JsonProperty]
            public string id;
            [JsonProperty]
            public string label;
            [JsonProperty]
            public string icon;
            [JsonProperty]
            public state state;
            [JsonProperty]
            public List<node> items;
            [JsonProperty]
            public string value;
            public VirtualPath path;

            public node() { }
            public node(VirtualPath path, string name, bool children = false)
            {
                this.path = path;
                this.value = path.FullPath;
                this.id = Guid.NewGuid().ToString("N");
                this.label = name ?? path.Name;
                if (children)
                    this.items = new List<node>();
            }
        }
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class state
        {
            [JsonProperty]
            public bool? opened;
            [JsonProperty]
            public bool? disabled;
            [JsonProperty]
            public bool? selected;
        }
    }
}