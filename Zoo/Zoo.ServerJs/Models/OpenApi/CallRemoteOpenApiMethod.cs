namespace Zoo.ServerJs.Models.OpenApi
{
    /// <summary>
    /// Вызвать удаленный метод (передается сериализованным)
    /// </summary>
    public class CallRemoteOpenApiMethod
    {
        /// <summary>
        /// Имя рабочего класса
        /// </summary>
        public string WorkerName { get; set; }
        /// <summary>
        /// Название метода
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Сериализованные параметры
        /// </summary>
        public string[] SerializedParameters { get; set; }
    }
}