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
        public DateTime LoggedOnUtc { get; } = DateTime.UtcNow;

        /// <summary>
        /// Идентификатор события
        /// </summary>
        public string EventIdName { get; set; }

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