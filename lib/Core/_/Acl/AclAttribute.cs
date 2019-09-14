using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace InnateGlory
{
    public class AclAttribute : Attribute, IFilterMetadata
    {
        public string Path { get; set; }

        public AclAttribute() { }
        public AclAttribute(string path)
        {
            this.Path = path;
        }
    }
}
