using Croco.Core.Documentation.Models;
using Croco.Core.Utils;
using System;
using System.Collections.Generic;
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
        /// Префикс для построения модели
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Описание типа данных
        /// </summary>
        public CrocoTypeDescriptionResult TypeDescription { get; set; }

        /// <summary>
        /// Блоки для свойств
        /// </summary>
        public List<UserInterfaceBlock> Blocks { get; set; }

        /// <summary>
        /// Данные для заполнения объекта
        /// </summary>
        public string ValueJson { get; set; }

        /// <summary>
        /// Сериализованные кастомные данные
        /// </summary>
        public string CustomDataJson { get; set; }

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
        /// <param name="modelPrefix"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromObject(object model, string modelPrefix)
        {
            var prov = Tool.JsonConverter.Serialize(model);

            return CreateFromType(model.GetType(), modelPrefix, prov, null);
        }

        /// <summary>
        /// Создать из типа
        /// </summary>
        /// <param name="type"></param>
        /// <param name="modelPrefix"></param>
        /// <param name="valueJson"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromType(Type type, string modelPrefix, string valueJson, GenericInterfaceOptions opts = null)
        {
            var desc = CrocoTypeDescription.GetDescription(type);

            if(opts == null)
            {
                opts = GenericInterfaceOptions.Default();
            }

            return new GenerateGenericUserInterfaceModel
            {
                TypeDescription = desc,
                Blocks = GenericUserInterfaceModelBuilderExtensions.GetBlocks("", desc.GetMainTypeDescription(), desc, opts),
                Prefix = modelPrefix,
                ValueJson = valueJson
            };
        }
    }
}