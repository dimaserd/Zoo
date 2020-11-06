namespace Zoo.GenericUserInterface.Utils
{
    /// <summary>
    /// Сериализатор JSON
    /// </summary>
    public interface IJsonConverter
    {
        /// <summary>
        /// Десериализовать объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        T Deserialize<T>(string json);

        /// <summary>
        /// Сериализовать объект
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string Serialize(object obj);
    }

    /// <summary>
    /// Вспомогательный класс, собирающий в себе большинство базовых методов
    /// </summary>
    public class Tool
    {
        /// <summary>
        /// Установить тулзы
        /// </summary>
        /// <param name="jsonConverter"></param>
        internal static void SetTools(IJsonConverter jsonConverter)
        {
            JsonConverter = jsonConverter;
        }

        /// <summary>
        /// Json сериализатор
        /// </summary>
        public static IJsonConverter JsonConverter { get; private set; } = new NewtonsoftJsonConverter();
    }
}
