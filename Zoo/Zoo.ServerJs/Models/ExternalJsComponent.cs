using System.ComponentModel.DataAnnotations;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Внешний js компонент
    /// </summary>
    public class ExternalJsComponent
    {
        /// <summary>
        /// Название компонента
        /// </summary>
        [Required, Display(Name = "Название компонента")]
        public string ComponentName { get; set; }

        /// <summary>
        /// Скрипт компонента
        /// </summary>
        [Required, Display(Name = "Скрипт")]
        public string Script { get; set; }
    }
}