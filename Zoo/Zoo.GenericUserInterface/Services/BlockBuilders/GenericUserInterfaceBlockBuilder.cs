using Croco.Core.Documentation.Models;
using Croco.Core.Documentation.Services;
using System;
using System.Collections.Generic;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;

namespace Zoo.GenericUserInterface.Services.BlockBuilders
{
    /// <summary>
    /// Построитель для конкретного блока в интерейсе
    /// </summary>
    /// <typeparam name="TProp"></typeparam>
    public class GenericUserInterfaceBlockBuilder<TProp> : IGenericInterfaceBuilder
    {
        IGenericInterfaceBuilder InterfaceBuilder { get; }

        /// <summary>
        /// построитель документации
        /// </summary>
        protected CrocoTypeDescriptionBuilder Builder { get; }
        
        /// <summary>
        /// Редактируемый блок
        /// </summary>
        protected UserInterfaceBlock Block { get; }
        
        /// <summary>
        /// Описанный тип данных
        /// </summary>
        protected CrocoTypeDescriptionResult DescribedType { get; set; }

        /// <summary>
        /// Результат - модель для построения пользовательского интерфейса
        /// </summary>
        public GenerateGenericUserInterfaceModel Result => InterfaceBuilder.Result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceBuilder"></param>
        /// <param name="builder"></param>
        /// <param name="block"></param>
        internal GenericUserInterfaceBlockBuilder(IGenericInterfaceBuilder interfaceBuilder, CrocoTypeDescriptionBuilder builder, UserInterfaceBlock block)
        {
            InterfaceBuilder = interfaceBuilder;
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
                throw new Exception($"Только блоки типа {nameof(UserInterfaceType.TextBox)} можно переключать на {nameof(UserInterfaceType.TextArea)}");
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
        public GenericUserInterfaceBlockBuilder<TProp> SetDropDownList(List<SelectListItemData<TProp>> selectListItems)
        {
            if (selectListItems is null)
            {
                throw new ArgumentNullException(nameof(selectListItems));
            }

            var mainDoc = Builder.GetTypeDescriptionResult(typeof(TProp)).GetMainTypeDescription();
            if (mainDoc.IsEnumeration)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.CantImplementSetDropListNameToEnumPropertyFormat, Block.PropertyName));
            }

            if (!mainDoc.IsPrimitive)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.CantSetDropListForPropertyThatIsNotPrimitiveFormat, Block.PropertyName));
            }

            Block.InterfaceType = UserInterfaceType.DropDownList;
            Block.DropDownData = new DropDownListData
            {
                SelectList = GenericUserInterfaceModelBuilderExtensions.ToSelectListItems(selectListItems),
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