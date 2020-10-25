using System;

namespace Zoo.ServerJs.Models.Task
{
    /// <summary>
    /// Добавить Js задание
    /// </summary>
    public class AddJsTask
    {
        /// <summary>
        /// Скрипт
        /// </summary>
        public string Script { get; set; }
        
        /// <summary>
        /// Скрипт нуэно выполнить сейчас или в определенную дату
        /// </summary>
        public bool ExecuteNowOrOnDate { get; set; }

        /// <summary>
        /// В какую дату необходимо выполнить скрипт
        /// </summary>
        public DateTime ExecuteOnDate { get; set; }
    }
}