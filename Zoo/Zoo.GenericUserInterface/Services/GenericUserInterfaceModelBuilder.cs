using Croco.Core.Documentation.Services;
using System;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Options;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Utils;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Построитель обобщенного пользовательского интерфейса
    /// </summary>
    public class GenericUserInterfaceModelBuilder
    {
        /// <inheritdoc />
        public GenerateGenericUserInterfaceModel Result { get; }

        /// <summary>
        /// Построитель
        /// </summary>
        public CrocoTypeDescriptionBuilder TypeDescriptionBuilder = new CrocoTypeDescriptionBuilder();

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
            var desc = TypeDescriptionBuilder.GetTypeDescriptionResult(type);

            var main = desc.GetMainTypeDescription();

            if (!main.IsClass)
            {
                throw new InvalidOperationException(ExceptionTexts.NonComplexTypesAreNotSupported);
            }

            var typeChecker = new CrocoClassDescriptionChecker();

            if (typeChecker.IsRecursiveType(desc))
            {
                throw new InvalidOperationException(ExceptionTexts.RecursiveTypesAreNotSupported);
            }

            if (typeChecker.HasMultiDimensionalArrays(desc))
            {
                throw new InvalidOperationException(ExceptionTexts.ClassesWithMultiDimensionalArrayPropertiesAreNotSupported);
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