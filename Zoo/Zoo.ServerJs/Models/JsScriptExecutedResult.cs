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
        /// Конструктор
        /// </summary>
        public JsScriptExecutedResult()
        {
        }

        internal JsScriptExecutedResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Текст скрипта
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Выполнено успешно
        /// </summary>
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime? StartedOnUtc { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime? FinishedOnUtc { get; set; }

        /// <summary>
        /// Список логов консиоли
        /// </summary>
        public List<JsLogggedVariables> ConsoleLogs { get; set; }

        /// <summary>
        /// Системные логи времени выполнения
        /// </summary>
        public List<JsExecutionLog> ExecutionLogs { get; set; }

        /// <summary>
        /// Исключение
        /// </summary>
        public ExcepionData ExceptionData { get; set; }
    }
}