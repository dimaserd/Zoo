using Croco.Core.Documentation.Models;
using Croco.Core.Utils;
using System;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Extensions;

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

        /// <summary>
        /// Создать из объекта используя провайдер значений
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromObject(object model)
        {
            var dataJson = Tool.JsonConverter.Serialize(model);

            return CreateFromType(model.GetType(), dataJson, null);
        }

        /// <summary>
        /// Создать из типа
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueJson"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromType(Type type, string valueJson, GenericInterfaceOptions opts = null)
        {
            var desc = CrocoTypeDescription.GetDescription(type);

            if(opts == null)
            {
                opts = GenericInterfaceOptions.Default();
            }

            var blocks = GenericUserInterfaceModelBuilderExtensions.GetBlocks("", desc.GetMainTypeDescription(), desc, opts);

            return new GenerateGenericUserInterfaceModel
            {
                TypeDescription = desc,
                Interface = new GenericInterfaceModel
                {
                    Prefix = "",
                    Blocks = blocks
                },
                ValueJson = valueJson
            };
        }
    }
}