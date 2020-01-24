using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Результат выполнения метода серверного javascript обработчика
    /// </summary>
    public class JsScriptExecutedResult
    {
        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Список логов
        /// </summary>
        public List<List<object>> Logs { get; set; }

        /// <summary>
        /// Исключение
        /// </summary>
        public string ExceptionStackTrace { get; set; }
    }
}