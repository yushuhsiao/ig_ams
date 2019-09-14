using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace System.Collections.Generic
{
    [JsonConverter(typeof(_JsonConverter))]
    public sealed class VirtualPath : PathList<VirtualPath>
    {
        class _JsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return true;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                string s = serializer.Deserialize<string>(reader);
                return VirtualPath.GetPath(s);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is VirtualPath)
                    serializer.Serialize(writer, ((VirtualPath)value).FullPath);
            }
        }

        public static readonly VirtualPath root = new VirtualPath();

        public static VirtualPath GetPath(string path, bool create = true) => root.GetChild(path, create)?.Value;
        public static bool GetPath(string path, out VirtualPath result, bool create = true)
        {
            if (root.GetChild(path, out var tmp, true))
                result = tmp?.Value;
            else
                result = null;
            return result != null;
        }
    }
}