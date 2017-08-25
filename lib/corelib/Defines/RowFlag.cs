using Newtonsoft.Json;
using System;

namespace casino
{
    [Flags]
	[JsonConverter(typeof(RowFlagJsonConverter))]
	public enum RowFlag
	{
		none,
		select = 0x01,
		insert = 0x02,
		update = 0x04,
		delete = 0x08,
	}

	class RowFlagJsonConverter : JsonConverter
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
