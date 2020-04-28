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
        public string ComponentName { get; set; }

        /// <summary>
        /// Скрипт компонента
        /// </summary>
        public string Script { get; set; }
    }
}