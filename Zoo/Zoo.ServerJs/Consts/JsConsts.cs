namespace Zoo.ServerJs.Consts
{
    /// <summary>
    /// Константы для вызова js обработчиков
    /// </summary>
    public static class JsConsts
    {
        /// <summary>
        /// Название объекта
        /// </summary>
        public const string ApiObjectName = "api";

        /// <summary>
        /// Название функции, которое используется для вызова js обработчиков
        /// </summary>
        public const string CallFunctionName = "Call";

        /// <summary>
        /// Название функции, которое используется для вызова внешнего js компонента
        /// </summary>
        public const string CallExternalFunctionName = "CallExternal";
    }
}