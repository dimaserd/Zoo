namespace Zoo.ServerJs.Models.OpenApi
{
    /// <summary>
    /// Результат выполнения 
    /// </summary>
    public class CallOpenApiWorkerMethodResponse
    {
        /// <summary>
        /// Определяет успешность вызова
        /// </summary>
        public bool IsSucceeded { get; set; }
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public ExcepionData ExcepionData { get; set; }
        
        /// <summary>
        /// Сериализованный ответ
        /// </summary>
        public string ResponseJson { get; set; }
    }
}