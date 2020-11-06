using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Zoo.GenericUserInterface.Utils
{
    /// <summary>
    /// Сериализатор JSON
    /// </summary>
    public class NewtonsoftJsonConverter : IJsonConverter
    {
        private JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public NewtonsoftJsonConverter() : this(GetSettings())
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settings"></param>
        public NewtonsoftJsonConverter(JsonSerializerSettings settings)
        {
            Settings = settings;
        }

        private static JsonSerializerSettings GetSettings()
        {
            var result = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            result.Converters.Add(new StringEnumConverter());

            return result;
        }

        /// <summary>
        /// Десериализовать объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        /// <summary>
        /// Сериализовать объект
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }
    }
}
