using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;

namespace ams
{
    [TypeConverter(typeof(EnumWrapperTypeConverter))]
    [JsonConverter(typeof(EnumWrapperJsonConverter))]
    public struct EnumWrapper<T> where T : struct
    {
        public T Value;

    }
    class EnumWrapperTypeConverter : TypeConverter
    {
    }
    class EnumWrapperJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
