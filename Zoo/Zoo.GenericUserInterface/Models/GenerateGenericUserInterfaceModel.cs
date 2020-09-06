using Croco.Core.Documentation.Models;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Модель для создания обощенного интерфейса
    /// </summary>
    public class GenerateGenericUserInterfaceModel
    {
        /// <summary>
        /// Интерфейс
        /// </summary>
        public GenericInterfaceModel Interface { get; set; }

        /// <summary>
        /// Сериализованные кастомные данные
        /// </summary>
        public string CustomDataJson { get; set; }

        /// <summary>
        /// Данные для заполнения объекта
        /// </summary>
        public string ValueJson { get; set; }

        /// <summary>
        /// Описание типа данных
        /// </summary>
        public CrocoTypeDescriptionResult TypeDescription { get; set; }
    }
}