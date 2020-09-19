namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Данные для исключения
    /// </summary>
    public class ExcepionData
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Стек вызовов
        /// </summary>
        public string StackTrace { get; set; }
    }
}