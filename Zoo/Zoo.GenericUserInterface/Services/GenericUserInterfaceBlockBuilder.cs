using Croco.Core.Documentation.Models;
using Croco.Core.Documentation.Services;
using System;
using System.Collections.Generic;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Resources;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Построитель для конкретного блока в интерейсе
    /// </summary>
    /// <typeparam name="TProp"></typeparam>
    public class GenericUserInterfaceBlockBuilder<TProp>
    {
        CrocoTypeDescriptionBuilder Builder { get; }
        UserInterfaceBlock Block { get; }
        CrocoTypeDescriptionResult DescribedType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="block"></param>
        internal GenericUserInterfaceBlockBuilder(CrocoTypeDescriptionBuilder builder, UserInterfaceBlock block)
        {
            Builder = builder;
            Block = block;
            DescribedType = Builder.GetTypeDescriptionResult(typeof(TProp));
        }

        /// <summary>
        /// Установить текст лейбла
        /// </summary>
        /// <param name="labelText"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetLabel(string labelText)
        {
            Block.LabelText = labelText;
            return this;
        }

        /// <summary>
        /// Установить тип блока, как большое текстовое поле
        /// </summary>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetTextArea()
        {
            if (Block.InterfaceType != UserInterfaceType.TextBox)
            {
                throw new Exception($"Только блоки типа  {nameof(UserInterfaceType.TextBox)} можно переключать на {nameof(UserInterfaceType.TextArea)}");
            }

            Block.InterfaceType = UserInterfaceType.TextArea;

            return this;
        }

        /// <summary>
        /// Установить кастомный тип блока
        /// </summary>
        /// <param name="customType"></param>
        /// <param name="customDataJson"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetCustom(string customType, string customDataJson)
        {
            Block.InterfaceType = UserInterfaceType.CustomInput;
            Block.CustomUserInterfaceType = customType;
            Block.CustomDataJson = customDataJson;

            return this;
        }

        /// <summary>
        /// Установить блок, как выпадающий список
        /// </summary>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetDropDownList(List<SelectListItem> selectListItems)
        {
            var mainDoc = Builder.GetTypeDescriptionResult(typeof(TProp)).GetMainTypeDescription();
            if (mainDoc.IsEnumeration)
            {
                throw new ApplicationException(string.Format(ExceptionTexts.CantImplementMethodNameToEnumPropertyFormat, nameof(SetDropDownList)));
            }

            Block.InterfaceType = UserInterfaceType.DropDownList;
            Block.DropDownData = new DropDownListData
            {
                SelectList = selectListItems,
                CanAddNewItem = false
            };
            return this;
        }

        /// <summary>
        /// Установить выпадающий список со множественным выбором для свойства объекта
        /// </summary>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetMultipleDropDownList(List<SelectListItem> selectListItems)
        {
            var main = DescribedType.GetMainTypeDescription();

            if (!main.IsEnumerable)
            {
                throw new ApplicationException(ExceptionTexts.CantImplementMultipleDropDownForToNotEnumerableProperty);
            }

            var enumerated = DescribedType.GetTypeDescription(main.EnumeratedDiplayFullTypeName);

            if (enumerated.IsEnumeration)
            {
                throw new ApplicationException(ExceptionTexts.CantImplementMultipleDropDownForToEnumerableOfEnumerationProperty);
            }

            Block.InterfaceType = UserInterfaceType.MultipleDropDownList;

            Block.DropDownData = new DropDownListData
            {
                SelectList = selectListItems,
                CanAddNewItem = false
            };

            return this;
        }

        /// <summary>
        /// Устновить скрытый тип элемента ввода
        /// </summary>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetHidden()
        {
            Block.InterfaceType = UserInterfaceType.Hidden;
            return this;
        }
    }
}