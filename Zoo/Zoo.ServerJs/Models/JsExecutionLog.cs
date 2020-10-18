using Microsoft.Extensions.Logging;
using System;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Системные логи выполнения
    /// </summary>
    public class JsExecutionLog
    {
        /// <summary>
        /// Дата логгирования
        /// </summary>
        public DateTime LoggedOnUtc { get; set; }

        /// <summary>
        /// Идентификатор события
        /// </summary>
        public EventId EventId { get; set; }

        /// <summary>
        /// Сообщение в человекопонятном виде
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Данные описанные в логе
        /// </summary>
        public string DataJson { get; set; }
    }
}