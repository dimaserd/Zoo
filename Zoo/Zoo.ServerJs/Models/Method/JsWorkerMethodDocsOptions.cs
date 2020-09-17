namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Опции для построения документации по методу
    /// </summary>
    public class JsWorkerMethodDocsOptions
    {
        /// <summary>
        /// Название метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Описание метода
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Описание результата
        /// </summary>
        public string ResultDescription { get; set; }

        /// <summary>
        /// Описание параметров
        /// </summary>
        public string[] ParameterDescriptions { get; set; }
    }
}