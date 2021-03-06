﻿using Croco.Core.Documentation.Services;
using System;
using System.Collections.Generic;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Definition;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services.Providers;

namespace Zoo.GenericUserInterface.Services.BlockBuilders
{
    /// <summary>
    /// Построитель блока для обощенного интерфейса с типом коллекция
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class GenericUserInterfaceBlockBuilderForCollectionType<TItem> : GenericUserInterfaceBlockBuilder<TItem[]>
    {
        internal GenericUserInterfaceBlockBuilderForCollectionType(IGenericInterfaceBuilder interfaceBuilder, CrocoTypeDescriptionBuilder builder, UserInterfaceBlock block) 
            : base(interfaceBuilder, builder, block)
        {
        }

        /// <summary>
        /// Установить выпадающий список со множественным выбором для свойства объекта
        /// </summary>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilderForCollectionType<TItem> SetMultipleDropDownList(List<SelectListItemData<TItem>> selectListItems)
        {
            if (selectListItems is null)
            {
                throw new ArgumentNullException(nameof(selectListItems));
            }

            ValidateForMultipleDropDownList();

            Block.InterfaceType = UserInterfaceType.MultipleDropDownList;
            Block.DropDownData = new DropDownListData
            {
                SelectList = GenericUserInterfaceModelBuilderMappings.ToSelectListItems(selectListItems),
                CanAddNewItem = false
            };

            return this;
        }

        /// <summary>
        /// Установить выпадающий список со множественным выбором для свойства объекта
        /// </summary>
        /// <typeparam name="TDataProvider">Провайдер данных</typeparam>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilderForCollectionType<TItem> SetMultipleDropDownList<TDataProvider>() where TDataProvider : DataProviderForSelectList<TItem>
        {
            var key = typeof(TDataProvider).FullName;

            if (!Bag.SelectListDataProviders.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.DataProviderWithTypeNotRegisteredFormat, key));
            }

            ValidateForMultipleDropDownList();

            Block.InterfaceType = UserInterfaceType.MultipleDropDownList;
            Block.DropDownData = new DropDownListData
            {
                DataProviderTypeFullName = key,
                CanAddNewItem = false
            };

            return this;
        }

        

        /// <summary>
        /// Установить список с автоподсказами для свойства данного объекта
        /// </summary>
        /// <returns></returns>
        public new GenericUserInterfaceBlockBuilderForCollectionType<TItem> SetAutoCompleteFor<TDataProvider>()
            where TDataProvider : DataProviderForAutoCompletion<TItem>
        {
            var key = typeof(TDataProvider).FullName;

            if (!Bag.AutoCompletionDataProviders.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.DataProviderForAutoCompletionNotRegisteredFormat, key));
            }

            var mainDesc = DescribedType.GetMainTypeDescription();
            if (mainDesc.ArrayDescription.IsArray && !DescribedType.GetTypeDescription(mainDesc.ArrayDescription.ElementDiplayFullTypeName).IsPrimitive)
            {
                throw new InvalidOperationException(
                    $"Вы не можете установить для свойства {Block.PropertyName} тип блока 'Автозаполнение'. " +
                    "Так как данное свойство является массивом из непримитивов. " +
                    "Необходим тип, который будет являтся массивом из примитивов.");
            }

            Block.InterfaceType = UserInterfaceType.AutoCompleteForMultiple;
            Block.AutoCompleteData = new AutoCompleteData
            {
                DataProviderTypeFullName = typeof(TDataProvider).FullName
            };

            return this;
        }

        private void ValidateForMultipleDropDownList()
        {
            var main = DescribedType.GetMainTypeDescription();

            if (!main.ArrayDescription.IsArray)
            {
                throw new InvalidOperationException(ExceptionTexts.CantImplementMultipleDropDownForToNotEnumerableProperty);
            }

            var enumerated = DescribedType.GetTypeDescription(main.ArrayDescription.ElementDiplayFullTypeName);

            if (!enumerated.IsPrimitive)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.CantSetMultipleDropListNotOnPrimitivesFormat, Block.PropertyName, typeof(TItem).FullName));
            }
        }
    }
}