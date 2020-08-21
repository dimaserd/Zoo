using Croco.Core.Documentation.Models;
using Croco.Core.Utils;
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
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(string modelPrefix, GenericInterfaceOptions opts) : base(typeof(TModel), modelPrefix, null, opts)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelPrefix"></param>
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(TModel model, string modelPrefix, GenericInterfaceOptions opts) : base(typeof(TModel), modelPrefix, Tool.JsonConverter.Serialize(model), opts)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        public GenericUserInterfaceModelBuilder(GenerateGenericUserInterfaceModel model): base(model)
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
        /// Установить скрытое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="customType"></param>
        /// <param name="customDataJson"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> CustomFor<TProp>(Expression<Func<TModel, TProp>> expression, string customType, string customDataJson)
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
        public GenericUserInterfaceModelBuilder<TModel> DropDownListFor<TProp>(Expression<Func<TModel, TProp>> expression, List<SelectListItem> selectListItems)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            var mainDoc = CrocoTypeDescription.GetDescription(typeof(TProp)).GetMainTypeDescription();
            if(mainDoc.IsEnumeration)
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
        public GenericUserInterfaceModelBuilder<TModel> MultipleDropDownListFor<TProp>(Expression<Func<TModel, TProp>> expression, List<SelectListItem> selectListItems)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            var crocDescr = CrocoTypeDescription.GetDescription(typeof(TProp));
            var main = crocDescr.GetMainTypeDescription();

            if(!main.IsEnumerable)
            {
                throw new ApplicationException(ExceptionTexts.CantImplementMultipleDropDownForToNotEnumerableProperty);
            }

            var enumerated = crocDescr.GetTypeDescription(main.EnumeratedDiplayFullTypeName);
            
            if (enumerated.IsEnumeration)
            {
                throw new ApplicationException(ExceptionTexts.CantImplementMultipleDropDownForToEnumerableOfEnumerationProperty);
            }

            return SetMultipleDropDownListFor(memberName, selectListItems) as GenericUserInterfaceModelBuilder<TModel>;
        }
    }
}