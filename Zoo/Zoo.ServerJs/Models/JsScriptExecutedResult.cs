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
        public DateTime StartedOnUtc { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime FinishOnUtc { get; set; }

        /// <summary>
        /// Список логов
        /// </summary>
        public List<JsLogggedVariables> Logs { get; set; }

        /// <summary>
        /// Исключение
        /// </summary>
        public string ExceptionStackTrace { get; set; }
    }
}