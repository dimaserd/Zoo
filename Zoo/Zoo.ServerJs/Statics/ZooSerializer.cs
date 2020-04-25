using Croco.Core.Common.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace Zoo.ServerJs.Statics
{
    internal class NewtonsoftSerializer : IJsonConverter
    {
        internal class StrictIntConverter : JsonConverter
        {
            readonly JsonSerializer defaultSerializer = new JsonSerializer();

            public override bool CanConvert(Type objectType)
            {
                return IsIntegerType(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Integer:
                    case JsonToken.Float: // Accepts numbers like 4.00
                    case JsonToken.Null:
                        return defaultSerializer.Deserialize(reader, objectType);
                    default:
                        throw new JsonSerializationException(string.Format("Token \"{0}\" of type {1} was not a JSON integer", reader.Value, reader.TokenType));
                }
            }

            public override bool CanWrite => false;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public static bool IsIntegerType(Type type)
            {
                type = Nullable.GetUnderlyingType(type) ?? type;
                if (type == typeof(long)
                    || type == typeof(ulong)
                    || type == typeof(int)
                    || type == typeof(uint)
                    || type == typeof(short)
                    || type == typeof(ushort)
                    || type == typeof(byte)
                    || type == typeof(sbyte)
                    || type == typeof(System.Numerics.BigInteger))
                    return true;
                return false;
            }
        }

        JsonSerializerSettings Settings { get; }

        public NewtonsoftSerializer()
        {
            Settings = GetSettings();
        }

        private static JsonSerializerSettings GetSettings()
        {
            var result = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            result.Converters.Add(new StringEnumConverter());
            result.Converters.Add(new StrictIntConverter());

            return result;
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }
    }

    public static class ZooSerializer
    {
        static readonly IJsonConverter Converter = new NewtonsoftSerializer();

        

        public static T Deserialize<T>(string json)
        {
            return Converter.Deserialize<T>(json);
        }

        public static string Serialize(object obj)
        {
            return Converter.Serialize(obj);
        }
    }
}