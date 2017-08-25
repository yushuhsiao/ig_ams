using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ams
{
    [JsonConverter(typeof(EncodingPath._JsonConverter))]
    public class EncodingPath
    {
        class _JsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return true;
            }
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new EncodingPath() { Value2 = serializer.Deserialize<string>(reader) };
            }
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                EncodingPath p = value as EncodingPath;
                if (p == null)
                    serializer.Serialize(writer, value);
                else
                    serializer.Serialize(writer, p.Value2);
            }
        }

        public static implicit operator EncodingPath(string n)
        {
            return new EncodingPath() { Value = n };
        }
        public static implicit operator string(EncodingPath n)
        {
            return n.Value;
        }

        public string Value;
        public string Value2
        {
            get { return Encoding.UTF8.ToBase64String(this.Value); }
            set { this.Value = Encoding.UTF8.FromBase64String(value); }
        }
    }
    public static class EncodingPathExtension
    {
        public static T GetTreeNode<T>(this EncodingPath p, TreeNode<T> t, bool create = false) where T : TreeNode<T>, new()
        {
            if (p == null) return null;
            if (string.IsNullOrEmpty(p.Value)) return null;
            return t.GetChild(p.Value, create);
        }
        public static bool HasValue(this EncodingPath p)
        {
            if (p == null) return false;
            return !string.IsNullOrEmpty(p.Value);
        }
    }
}
