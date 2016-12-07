using System;
using Newtonsoft.Json;

namespace MarioWebService.Mappers
{
    public class CustomJsonConverter : Newtonsoft.Json.Converters.StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is System.Action)
            {
                writer.WriteValue(Enum.GetName(typeof(System.Action), (System.Action)value));
                return;
            }

            base.WriteJson(writer, value, serializer);
        }
    }
}