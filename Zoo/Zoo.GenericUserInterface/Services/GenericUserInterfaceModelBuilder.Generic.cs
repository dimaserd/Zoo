using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Resources;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Вспомогательный класс для построения обобщенного интерфейса
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class GenericUserInterfaceModelBuilder<TModel> : GenericUserInterfaceModelBuilder where TModel : class
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="modelPrefix"></param>
        public GenericUserInterfaceModelBuilder(string modelPrefix) : base(typeof(TModel), modelPrefix)
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="modelPrefix"></param>
        /// <param name="valueProvider"></param>
        public GenericUserInterfaceModelBuilder(string modelPrefix, GenericUserInterfaceValueProvider valueProvider) : base(typeof(TModel), modelPrefix, valueProvider)
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelPrefix"></param>
        public GenericUserInterfaceModelBuilder(TModel model, string modelPrefix) : base(typeof(TModel), modelPrefix, GenericUserInterfaceValueProvider.Create(model))
        {

        }

        /// <summary>
        /// Переместить свойство на начальную позицию
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> ShiftToStartFor(Expression<Func<TModel, object>> expression)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            return ShiftPropertyToStartFor(memberName) as GenericUserInterfaceModelBuilder<TModel>;
        }

        /// <summary>
        /// Переместить свойство в конец списка
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> ShiftToEndFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            return ShiftPropertyToEndFor(memberName) as GenericUserInterfaceModelBuilder<TModel>;
        }

        /// <summary>
        /// Установить скрытое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> HiddenFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            return SetHiddenFor(memberName) as GenericUserInterfaceModelBuilder<TModel>;
        }

        /// <summary>
        /// Установить большой текстовое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> TextAreaFor(Expression<Func<TModel, string>> expression)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            return SetTextAreaFor(memberName) as GenericUserInterfaceModelBuilder<TModel>;
        }

        
        /// <summary>
        /// Установить выпадающий список с единственным выбором для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> DropDownListFor<TProp>(Expression<Func<TModel, TProp>> expression, List<MySelectListItem> selectListItems)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            var crocDescr = CrocoTypeDescription.GetDescription(typeof(TProp));

            if(crocDescr.IsEnumeration)
            {
                throw new ApplicationException(string.Format(ExceptionTexts.CantImplementMethodNameToEnumPropertyFormat, nameof(DropDownListFor)));
            }

            return SetDropDownListFor(memberName, selectListItems) as GenericUserInterfaceModelBuilder<TModel>;
        }


        /// <summary>
        /// Установить выпадающий список со множественным выбором для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> MultipleDropDownListFor<TProp>(Expression<Func<TModel, TProp>> expression, List<MySelectListItem> selectListItems)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            var crocDescr = CrocoTypeDescription.GetDescription(typeof(TProp));

            if(!crocDescr.IsEnumerable)
            {
                throw new ApplicationException(ExceptionTexts.CantImplementMultipleDropDownForToNotEnumerableProperty);
            }

            if (crocDescr.EnumeratedType.IsEnumeration)
            {
                throw new ApplicationException(ExceptionTexts.CantImplementMultipleDropDownForToEnumerableOfEnumerationProperty);
            }

            return SetMultipleDropDownListFor(memberName, selectListItems) as GenericUserInterfaceModelBuilder<TModel>;
        }
    }
}