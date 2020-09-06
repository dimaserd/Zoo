using Croco.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Вспомогательный класс для построения обобщенного интерфейса
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class GenericUserInterfaceModelBuilder<TModel> : GenericUserInterfaceModelBuilder where TModel : class
    {
        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        public GenericUserInterfaceModelBuilder() : base(typeof(TModel))
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal GenericUserInterfaceModelBuilder(GenerateGenericUserInterfaceModel model) : base(model)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(GenericInterfaceOptions opts) : base(typeof(TModel), null, opts)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(TModel model, GenericInterfaceOptions opts) : base(typeof(TModel), Tool.JsonConverter.Serialize(model), opts)
        {
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
            GetBlockBuilder(expression).SetHidden();
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
            GetBlockBuilder(expression).SetCustom(customType, customDataJson);
            return this;
        }

        /// <summary>
        /// Установить большой текстовое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> TextAreaFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            GetBlockBuilder(expression).SetTextArea();
            return this;
        }
        
        /// <summary>
        /// Установить выпадающий список с единственным выбором для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> DropDownListFor<TProp>(Expression<Func<TModel, TProp>> expression, List<SelectListItem> selectListItems)
        {
            GetBlockBuilder(expression).SetDropDownList(selectListItems);
            return this;
        }

        /// <summary>
        /// Установить выпадающий список со множественным выбором для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> MultipleDropDownListFor<TProp>(Expression<Func<TModel, TProp>> expression, List<SelectListItem> selectListItems)
        {
            GetBlockBuilder(expression).SetMultipleDropDownList(selectListItems);
            return this;
        }


        /// <summary>
        /// Получить конфигуратор для блока
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> GetBlockBuilder<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            return new GenericUserInterfaceBlockBuilder<TProp>(Builder, GetBlockByExpression(expression));
        } 

        private UserInterfaceBlock GetBlockByExpression<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            return GetBlockByPropertyName(GetMemberName(expression));
        }

        private string GetMemberName<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            return (expression.Body as MemberExpression).Member.Name;
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