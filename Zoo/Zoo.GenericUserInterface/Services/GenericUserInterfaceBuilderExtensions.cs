using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models.Overridings;
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

        //TODO Реализовать позже
        private static void SetInterfaceForClass<TModel, TProp, TOverrider>(this GenericUserInterfaceModelBuilder<TModel> builder, Expression<Func<TModel, TProp>> expression, TOverrider overrider) 
            where TModel : class 
            where TProp : class
            where TOverrider : UserInterfaceOverrider<TProp>
        {
            var propName = MyExpressionExtensions.GetMemberName(expression);
            var blockForProp = builder.Result.Interface.Blocks.First(x => x.PropertyName == propName);

            if(blockForProp.InterfaceType != UserInterfaceType.GenericInterfaceForClass)
            {
                throw new InvalidOperationException("");
            }

            //blockForProp.InnerGenericInterface = overrider.OverrideInterfaceAsync<>
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