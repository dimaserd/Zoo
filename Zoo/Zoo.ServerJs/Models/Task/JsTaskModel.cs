using System;

namespace Zoo.ServerJs.Models.Task
{
    /// <summary>
    /// Модель описывающая Js задание
    /// </summary>
    public class JsTaskModel
    {
        /// <summary>
        /// Идентификатор скрипта
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Является ли скрипт выполненым
        /// </summary>
        public bool IsExecuted { get; set; }

        /// <summary>
        /// Скрипт нужно исполнить сейчас или в определенную дату
        /// </summary>
        public bool ExecuteNowOrOnDate { get; set; }

        /// <summary>
        /// В какую дату нужно исполнить скрипт
        /// </summary>
        public DateTime ExecuteOnDate { get; set; }

        /// <summary>
        /// Результат выполнения скрипта
        /// </summary>
        public JsScriptExecutedResult Result { get; set; }
    }
}
