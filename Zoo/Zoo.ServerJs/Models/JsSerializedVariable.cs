namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// сериализованная переменная
    /// </summary>
    public class JsSerializedVariable
    {
        /// <summary>
        /// Полное название типа данных
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        /// Сериализованные данные
        /// </summary>
        public string DataJson { get; set; }
    }
}