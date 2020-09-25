using Croco.Core.Utils;
using System;
using System.Linq;
using System.Linq.Expressions;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Bag;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Вспомогательный класс для построения обобщенного интерфейса
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class GenericUserInterfaceModelBuilder<TModel> : GenericUserInterfaceModelBuilder, IGenericInterfaceBuilder where TModel : class
    {
        /// <inheritdoc />
        public GenericUserInterfaceBag Bag { get; }
        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        public GenericUserInterfaceModelBuilder(GenericUserInterfaceBag bag) : base(typeof(TModel))
        {
            Bag = bag;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal GenericUserInterfaceModelBuilder(GenerateGenericUserInterfaceModel model, GenericUserInterfaceBag bag) : base(model)
        {
            Bag = bag;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="bag"></param>
        public GenericUserInterfaceModelBuilder(GenericInterfaceOptions opts, GenericUserInterfaceBag bag) : base(typeof(TModel), null, opts)
        {
            Bag = bag;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bag"></param>
        public GenericUserInterfaceModelBuilder(TModel model, GenericUserInterfaceBag bag) : base(typeof(TModel), Tool.JsonConverter.Serialize(model), bag.Options)
        {
            Bag = bag;
        }
        #endregion

        /// <summary>
        /// Переместить свойство на начальную позицию
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> ShiftToStartFor(Expression<Func<TModel, object>> expression)
        {
            var block = GetBlockByExpression(expression);

            Result.Interface.Blocks.Remove(block);
            Result.Interface.Blocks.Insert(0, block);
            return this;
        }

        /// <summary>
        /// Переместить свойство в конец списка
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> ShiftToEndFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            var block = GetBlockByExpression(expression);

            Result.Interface.Blocks.Remove(block);
            Result.Interface.Blocks.Add(block);

            return this;
        }

        /// <summary>
        /// Установить скрытое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> HiddenFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            this.GetBlockBuilder(expression).SetHidden();
            return this;
        }

        /// <summary>
        /// Установить скрытое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="customType"></param>
        /// <param name="customDataJson"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> CustomFor<TProp>(Expression<Func<TModel, TProp>> expression, string customType, string customDataJson)
        {
            this.GetBlockBuilder(expression).SetCustom(customType, customDataJson);
            return this;
        }

        /// <summary>
        /// Установить большой текстовое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> TextAreaFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            this.GetBlockBuilder(expression).SetTextArea();
            return this;
        }

        internal UserInterfaceBlock GetBlockByExpression<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            return GetBlockByPropertyName(MyExpressionExtensions.GetMemberName(expression));
        }

        private UserInterfaceBlock GetBlockByPropertyName(string propertyName)
        {
            var block = Result.Interface.Blocks.FirstOrDefault(x => x.PropertyName == propertyName);

            if (block == null)
            {
                throw new Exception($"Свойство {propertyName} не найдено");
            }

            return block;
        }
    }
}