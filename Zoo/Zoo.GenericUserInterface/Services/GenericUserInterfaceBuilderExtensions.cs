using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services.BlockBuilders;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Расширения
    /// </summary>
    public static class GenericUserInterfaceBuilderExtensions
    {
        /// <summary>
        /// Установить видимость для блока
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="blockBuilder"></param>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public static TBuilder SetVisibility<TBuilder>(this TBuilder blockBuilder, bool isVisible) where TBuilder : IGenericInterfaceBlockBuilder
        {
            blockBuilder.Block.IsVisible = isVisible;
            return blockBuilder;
        }

        /// <summary>
        /// Установить текст лейбла для блока
        /// </summary>
        /// <param name="blockBuilder"></param>
        /// <param name="labelText"></param>
        /// <returns></returns>
        public static TBuilder SetLabel<TBuilder>(this TBuilder blockBuilder, string labelText) where TBuilder : IGenericInterfaceBlockBuilder
        {
            blockBuilder.Block.LabelText = labelText;
            return blockBuilder;
        }

        /// <summary>
        /// Установить блок в виде текстарии
        /// </summary>
        /// <param name="blockBuilder"></param>
        /// <returns></returns>
        public static GenericUserInterfaceBlockBuilder<string> SetTextArea(this GenericUserInterfaceBlockBuilder<string> blockBuilder)
        {
            blockBuilder.Block.InterfaceType = Enumerations.UserInterfaceType.TextArea;
            return blockBuilder;
        }

        /// <summary>
        /// Установить блок в виде инпута для пароля
        /// </summary>
        /// <param name="blockBuilder"></param>
        /// <returns></returns>
        public static GenericUserInterfaceBlockBuilder<string> SetPassword(this GenericUserInterfaceBlockBuilder<string> blockBuilder)
        {
            blockBuilder.Block.InterfaceType = Enumerations.UserInterfaceType.Password;
            return blockBuilder;
        }

        /// <summary>
        /// Получить конфигуратор для блока, который является колекцией
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="builder"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static GenericUserInterfaceBlockBuilderForCollectionType<TItem> GetBlockBuilderForCollection<TModel, TItem>(this GenericUserInterfaceModelBuilder<TModel> builder, Expression<Func<TModel, TItem[]>> expression) where TModel : class
        {
            CheckCollectionItem<TItem>();
            return new GenericUserInterfaceBlockBuilderForCollectionType<TItem>(builder, builder.TypeDescriptionBuilder, builder.GetBlockByExpression(expression));
        }

        /// <summary>
        /// Получить конфигуратор для блока, который является колекцией
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="builder"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static GenericUserInterfaceBlockBuilderForCollectionType<TItem> GetBlockBuilderForCollection<TModel, TItem>(this GenericUserInterfaceModelBuilder<TModel> builder, Expression<Func<TModel, IEnumerable<TItem>>> expression) where TModel : class
        {
            CheckCollectionItem<TItem>();
            return new GenericUserInterfaceBlockBuilderForCollectionType<TItem>(builder, builder.TypeDescriptionBuilder, builder.GetBlockByExpression(expression));
        }

        /// <summary>
        /// Получить конфигуратор для блока, который является колекцией
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="builder"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static GenericUserInterfaceBlockBuilderForCollectionType<TItem> GetBlockBuilderForCollection<TModel, TItem>(this GenericUserInterfaceModelBuilder<TModel> builder, Expression<Func<TModel, List<TItem>>> expression) where TModel : class
        {
            CheckCollectionItem<TItem>();
            return new GenericUserInterfaceBlockBuilderForCollectionType<TItem>(builder, builder.TypeDescriptionBuilder, builder.GetBlockByExpression(expression));
        }

        /// <summary>
        /// Получить конфигуратор для блока
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="builder"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static GenericUserInterfaceBlockBuilder<TProp> GetBlockBuilder<TModel, TProp>(this GenericUserInterfaceModelBuilder<TModel> builder, Expression<Func<TModel, TProp>> expression) where TModel : class
        {
            return new GenericUserInterfaceBlockBuilder<TProp>(builder, builder.TypeDescriptionBuilder, builder.GetBlockByExpression(expression));
        }

        private static void CheckCollectionItem<TItem>()
        {
            if (typeof(TItem) == typeof(char))
            {
                throw new InvalidOperationException(ExceptionTexts.DontUseGetBlockBuilderForCollectionOnCollectionsOfChars);
            }
        }
    }
}