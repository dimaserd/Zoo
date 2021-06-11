using Croco.Core.Documentation.Models.Methods;

namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Модель описывающая js метод или функцию
    /// </summary>
    public class JsWorkerMethodDocs
    {
        /// <summary>
        /// Тип возвращаемого значения 
        /// </summary>
        public MethodDescription Description { get; set; }

        /// <summary>
        /// Ссылка на метод
        /// </summary>
        public JsWorkerMethodBase Method { get; set; }
    }
}