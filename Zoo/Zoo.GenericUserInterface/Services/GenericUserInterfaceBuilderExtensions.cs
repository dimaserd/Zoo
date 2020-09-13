using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        /// Получить конфигуратор для блока, который является колекцией
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="builder"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static GenericUserInterfaceBlockBuilderForCollectionType<TItem> GetBlockBuilderForCollection<TModel, TItem>(this GenericUserInterfaceModelBuilder<TModel> builder, Expression<Func<TModel, TItem[]>> expression) where TModel : class
        {
            CheckItem<TItem>();
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
            CheckItem<TItem>();
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
            CheckItem<TItem>();
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

        private static void CheckItem<TItem>()
        {
            if (typeof(TItem) == typeof(char))
            {
                throw new InvalidOperationException(ExceptionTexts.DontUseGetBlockBuilderForCollectionOnCollectionsOfChars);
            }
        }
    }
}