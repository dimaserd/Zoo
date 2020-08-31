using Croco.Core.Documentation.Models;
using Croco.Core.Documentation.Services;
using Croco.Core.Utils;
using System;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Resources;

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

        /// <summary>
        /// Кастомно переопределить
        /// </summary>
        /// <param name="overridings"></param>
        /// <returns></returns>
        public Task OverrideAsync(GenericUserInterfaceOverridings overridings)
        {
            var overriding = overridings.GetOverriding(TypeDescription.GetMainTypeDescription().FullTypeName);
            
            if(overriding == null)
            {
                return Task.CompletedTask;
            }

            return overriding(this);
        }
    }
}