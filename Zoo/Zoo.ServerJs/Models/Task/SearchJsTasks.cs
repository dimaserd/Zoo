using Croco.Core.Abstractions.Models.Search;
using System;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Модель для поиска резельтатов выполнения скрипта
    /// </summary>
    public class SearchJsTasks : GetListSearchModel
    {
        /// <summary>
        /// Скрипт исполнен успешно
        /// </summary>
        public bool IsSucceeded { get; set; }
        
        /// <summary>
        /// Начало выполнения скрипта
        /// </summary>
        public GenericRange<DateTime> StartedOnUtc { get; set; }
        
        /// <summary>
        /// Окончание выполнения скрипта
        /// </summary>
        public GenericRange<DateTime> FinishedOnUtc { get; set; }
    }
}