using Croco.Core.Documentation.Services;
using Croco.Core.Utils;
using System;
using System.Linq;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Resources;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Построитель обобщенного пользовательского интерфейса
    /// </summary>
    public class GenericUserInterfaceModelBuilder
    {
        /// <summary>
        /// Результат - модель для построения пользовательского интерфейса
        /// </summary>
        public GenerateGenericUserInterfaceModel Result { get; }

        /// <summary>
        /// Построитель
        /// </summary>
        protected CrocoTypeDescriptionBuilder Builder = new CrocoTypeDescriptionBuilder();

        #region Конструкторы

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        public GenericUserInterfaceModelBuilder(object model) : this(model.GetType(), Tool.JsonConverter.Serialize(model), null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public GenericUserInterfaceModelBuilder(Type type) : this(type, null, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(Type type, GenericInterfaceOptions opts): this(type, null, opts)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueJson"></param>
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(Type type, string valueJson, GenericInterfaceOptions opts)
        {
            Result = CreateFromType(type, valueJson, opts);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="result"></param>
        public GenericUserInterfaceModelBuilder(GenerateGenericUserInterfaceModel result)
        {
            Result = result ?? throw new NullReferenceException(nameof(result));
        }

        #endregion

        /// <summary>
        /// Создать из типа
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueJson"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        private GenerateGenericUserInterfaceModel CreateFromType(Type type, string valueJson = null, GenericInterfaceOptions opts = null)
        {
            var desc = Builder.GetTypeDescriptionResult(type);

            var main = desc.GetMainTypeDescription();

            if (!main.IsClass)
            {
                throw new InvalidOperationException(ExceptionTexts.NonComplexTypesAreNotSupported);
            }

            var isRecursive = new CrocoClassDescriptionChecker().IsRecursiveType(main, desc.Types);

            if (isRecursive)
            {
                throw new InvalidOperationException(ExceptionTexts.RecursiveTypesAreNotSupported);
            }

            if (opts == null)
            {
                opts = GenericInterfaceOptions.Default();
            }

            var prefix = string.Empty;

            var blocks = GenericUserInterfaceModelBuilderExtensions.GetBlocks(prefix, desc.GetMainTypeDescription(), desc, opts);

            return new GenerateGenericUserInterfaceModel
            {
                TypeDescription = desc,
                Interface = new GenericInterfaceModel
                {
                    Prefix = prefix,
                    Blocks = blocks
                },
                ValueJson = valueJson
            };
        }
    }
}