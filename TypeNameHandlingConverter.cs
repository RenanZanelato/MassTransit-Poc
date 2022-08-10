using System;
using MassTransit;
using Newtonsoft.Json;

namespace Playground
{
    public class TypeNameHandlingConverter : JsonConverter
    {
        private readonly TypeNameHandling _typeNameHandling;
        private readonly JsonSerializer _serializer;

        public TypeNameHandlingConverter(TypeNameHandling typeNameHandling)
        {
            _typeNameHandling = typeNameHandling;
            _serializer = new JsonSerializer { TypeNameHandling = _typeNameHandling };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            _serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var message = _serializer.Deserialize(reader, objectType);
            return message;
        }

        public override bool CanConvert(Type objectType)
        {
            return !IsMassTransitOrSystemType(objectType);
        }

        private static bool IsMassTransitOrSystemType(Type objectType)
        {
            return objectType.Assembly == typeof(IConsumer).Assembly ||
                          objectType.Assembly == typeof(MassTransitBus).Assembly ||
                          objectType.Assembly.IsDynamic ||
                          objectType.Assembly == typeof(object).Assembly;
        }
    }
}