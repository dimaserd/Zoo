namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Удаленное Апи
    /// </summary>
    public class RemoteJsOpenApi
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Базовый урл на котором крутится удаленные Апи 
        /// </summary>
        public string HostUrl { get; set; }
    }
}