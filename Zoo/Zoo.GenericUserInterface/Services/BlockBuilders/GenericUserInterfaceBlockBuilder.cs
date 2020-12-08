using Croco.Core.Documentation.Models;
using Croco.Core.Documentation.Services;
using System;
using System.Collections.Generic;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Models.Providers;
using Zoo.GenericUserInterface.Resources;

namespace Zoo.GenericUserInterface.Services.BlockBuilders
{
    /// <summary>
    /// Построитель для конкретного блока в интерейсе
    /// </summary>
    /// <typeparam name="TProp"></typeparam>
    public class GenericUserInterfaceBlockBuilder<TProp> : IGenericInterfaceBlockBuilder
    {
        /// <summary>
        /// Построитель интерфейса
        /// </summary>
        protected IGenericInterfaceBuilder InterfaceBuilder { get; }

        /// <summary>
        /// построитель документации
        /// </summary>
        protected CrocoTypeDescriptionBuilder DocsBuilder { get; }
        
        /// <summary>
        /// Редактируемый блок
        /// </summary>
        public UserInterfaceBlock Block { get; }
        
        /// <summary>
        /// Описанный тип данных
        /// </summary>
        protected CrocoTypeDescriptionResult DescribedType { get; set; }

        /// <inheritdoc />
        public GenerateGenericUserInterfaceModel Result => InterfaceBuilder.Result;

        /// <inheritdoc />
        public GenericUserInterfaceBag Bag => InterfaceBuilder.Bag;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceBuilder"></param>
        /// <param name="builder"></param>
        /// <param name="block"></param>
        internal GenericUserInterfaceBlockBuilder(IGenericInterfaceBuilder interfaceBuilder, CrocoTypeDescriptionBuilder builder, UserInterfaceBlock block)
        {
            InterfaceBuilder = interfaceBuilder;
            DocsBuilder = builder;
            Block = block;
            DescribedType = DocsBuilder.GetTypeDescriptionResult(typeof(TProp));
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
        /// Установить блок, как выпадающий список со статическими данными
        /// </summary>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetDropDownList(List<SelectListItemData<TProp>> selectListItems)
        {
            if (selectListItems is null)
            {
                throw new ArgumentNullException(nameof(selectListItems));
            }

            DropDownListChecks();

            Block.InterfaceType = UserInterfaceType.DropDownList;
            Block.DropDownData = new DropDownListData
            {
                SelectList = GenericUserInterfaceModelBuilderExtensions.ToSelectListItems(selectListItems),
                CanAddNewItem = false
            };
            return this;
        }

        /// <summary>
        /// Установить блок, как выпадающий список с динамическими данными
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetDropDownList<TDataProvider>() where TDataProvider : DataProviderForSelectList<TProp>
        {
            var key = typeof(TDataProvider).FullName;

            if (!Bag.SelectListDataProviders.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.DataProviderWithTypeNotRegisteredFormat, key));
            }

            DropDownListChecks();

            Block.InterfaceType = UserInterfaceType.DropDownList;
            Block.DropDownData = new DropDownListData
            {
                DataProviderTypeFullName = typeof(TDataProvider).FullName
            };
            return this;
        }

        /// <summary>
        /// Установить список с автоподсказами для свойства данного объекта
        /// </summary>
        /// <typeparam name="TDataProvider"></typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilder<TProp> SetAutoCompleteFor<TDataProvider>()
           where TDataProvider : DataProviderForAutoCompletion<TProp>
        {
            var mainDesc = DescribedType.GetMainTypeDescription();
            if (!mainDesc.IsPrimitive)
            {
                throw new InvalidOperationException(
                    $"Вы не можете установить для свойства {Block.PropertyName} тип блока 'Автозаполнение'. " +
                    "Так как данное свойство не является примитивным. " +
                    "Необходим тип, который будет являтся примитивом.");
            }

            Block.InterfaceType = UserInterfaceType.AutoCompleteForSingle;
            Block.AutoCompleteData = new AutoCompleteData
            {
                DataProviderTypeFullName = typeof(TDataProvider).FullName
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

        private void DropDownListChecks()
        {
            var mainDoc = DocsBuilder.GetTypeDescriptionResult(typeof(TProp)).GetMainTypeDescription();
            
            if(mainDoc.ArrayDescription.IsArray)
            {
                throw new InvalidOperationException(ExceptionTexts.ArrayTypesAreNotSupportedSetMultipleDropDownList);
            }
            
            if (mainDoc.IsEnumeration)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.CantImplementSetDropListNameToEnumPropertyFormat, Block.PropertyName));
            }

            if (!mainDoc.IsPrimitive)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.CantSetDropListForPropertyThatIsNotPrimitiveFormat, Block.PropertyName));
            }
        }
    }
}