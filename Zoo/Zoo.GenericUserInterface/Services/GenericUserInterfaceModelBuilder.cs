using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Services
{
    public class GenericUserInterfaceModelBuilder<T> : GenericUserInterfaceModelBuilder where T : class
    {
        public GenericUserInterfaceModelBuilder(string modelPrefix) : base(typeof(T), modelPrefix)
        {

        }

        public GenericUserInterfaceModelBuilder(string modelPrefix, GenericUserInterfaceValueProvider valueProvider) : base(typeof(T), modelPrefix, valueProvider)
        {

        }

        public GenericUserInterfaceModelBuilder(T model, string modelPrefix) : base(typeof(T), modelPrefix, GenericUserInterfaceValueProvider.Create(model))
        {

        }

        public GenericUserInterfaceModelBuilder<T> HiddenFor(Expression<Func<T, object>> expression)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            return HiddenFor(memberName) as GenericUserInterfaceModelBuilder<T>;
        }

        public GenericUserInterfaceModelBuilder<T> TextAreaFor(Expression<Func<T, string>> expression)
        {
            var memberName = (expression.Body as MemberExpression).Member.Name;

            return TextAreaFor(memberName) as GenericUserInterfaceModelBuilder<T>;
        }
    }

    public class GenericUserInterfaceModelBuilder
    {
        public GenericUserInterfaceModelBuilder(object model, string modelPrefix)
        {
            Result = GenerateGenericUserInterfaceModel.CreateFromObject(model, modelPrefix);
        }

        public GenericUserInterfaceModelBuilder(GenerateGenericUserInterfaceModel result)
        {
            Result = result ?? throw new NullReferenceException(nameof(result));
        }

        public GenericUserInterfaceModelBuilder(Type type, string modelPrefix, GenericUserInterfaceValueProvider valueProvider)
        {
            Result = GenerateGenericUserInterfaceModel.CreateFromType(type, modelPrefix, valueProvider);
        }

        public GenericUserInterfaceModelBuilder(Type type, string modelPrefix)
        {
            Result = GenerateGenericUserInterfaceModel.CreateFromType(type, modelPrefix, null);
        }

        public GenerateGenericUserInterfaceModel Result { get; }
        
        public GenericUserInterfaceModelBuilder LeftOnlyPropertyNames(string propNames)
        {
            var propertyNames = propNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var t = propertyNames.Join(Result.Blocks, x => x, y => y.PropertyName, (x, y) => new KeyValuePair<string, UserInterfaceBlock>(x, y));

            Result.Blocks = t.Select(x => x.Value).ToList();

            return this;
        }

        private UserInterfaceBlock GetBlockByPropertyName(string propertyName)
        {
            var block = Result.Blocks.FirstOrDefault(x => x.PropertyName == propertyName);

            if (block == null)
            {
                throw new Exception($"Свойство {propertyName} не найдено");
            }

            return block;
        }

        /// <summary>
        /// Установить скрытый тип инпута, который нельзя редактировать
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected GenericUserInterfaceModelBuilder HiddenFor(string propertyName)
        {
            var block = GetBlockByPropertyName(propertyName);

            block.InterfaceType = UserInterfaceType.Hidden;

            return this;
        }

        public GenericUserInterfaceModelBuilder DropDownListFor(string propertyName, List<MySelectListItem> selectListItems)
        {
            var block = GetBlockByPropertyName(propertyName);

            block.InterfaceType = UserInterfaceType.DropDownList;

            block.SelectList = selectListItems;

            return this;
        }

        
        /// <summary>
        /// Установить большой текстовый инпут для имени свойства
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected GenericUserInterfaceModelBuilder TextAreaFor(string propertyName)
        {
            var block = GetBlockByPropertyName(propertyName);

            if (block.InterfaceType != UserInterfaceType.TextBox)
            {
                throw new Exception($"Только элементы с типом {nameof(UserInterfaceType.TextBox)} можно переключать на {nameof(UserInterfaceType.TextArea)}");
            }

            block.InterfaceType = UserInterfaceType.TextArea;

            return this;
        }
    }
}
