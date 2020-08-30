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
        public GenericUserInterfaceModelBuilder() : base(typeof(TModel))
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
            var memberName = GetMemberName(expression);

            return ShiftPropertyToStartFor(memberName) as GenericUserInterfaceModelBuilder<TModel>;
        }

        /// <summary>
        /// Переместить свойство в конец списка
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> ShiftToEndFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            var memberName = GetMemberName(expression);

            return ShiftPropertyToEndFor(memberName) as GenericUserInterfaceModelBuilder<TModel>;
        }

        /// <summary>
        /// Установить скрытое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> HiddenFor<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            var memberName = GetMemberName(expression);

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
            var memberName = GetMemberName(expression);

            return SetCustomFor(memberName, customType, customDataJson) as GenericUserInterfaceModelBuilder<TModel>;
        }

        /// <summary>
        /// Установить большой текстовое поле ввода для свойства объекта
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder<TModel> TextAreaFor(Expression<Func<TModel, string>> expression)
        {
            var memberName = GetMemberName(expression);

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
            var memberName = GetMemberName(expression);

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
            var memberName = GetMemberName(expression);

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

        private string GetMemberName<TProp>(Expression<Func<TModel, TProp>> expression)
        {
            return (expression.Body as MemberExpression).Member.Name;
        }
    }
}