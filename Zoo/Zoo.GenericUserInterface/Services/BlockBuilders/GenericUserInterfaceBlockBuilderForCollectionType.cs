using Croco.Core.Documentation.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;

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

            var main = DescribedType.GetMainTypeDescription();

            if (!main.IsEnumerable)
            {
                throw new InvalidOperationException(ExceptionTexts.CantImplementMultipleDropDownForToNotEnumerableProperty);
            }

            var enumerated = DescribedType.GetTypeDescription(main.EnumeratedDiplayFullTypeName);

            if (!enumerated.IsPrimitive)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.CantSetMultipleDropListNotOnPrimitivesFormat, Block.PropertyName, typeof(TItem).FullName));
            }

            Block.InterfaceType = UserInterfaceType.MultipleDropDownList;
            Block.DropDownData = new DropDownListData
            {
                SelectList = GenericUserInterfaceModelBuilderExtensions.ToSelectListItems(selectListItems),
                CanAddNewItem = false
            };

            return this;
        }

        /// <summary>
        /// Установить список с автоподсказами для свойства данного объекта
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <returns></returns>
        public GenericUserInterfaceBlockBuilderForCollectionType<TItem> SetAutoCompleteFor<TDataProvider>(DataProviderForAutoCompletion<TItem> dataProvider) where TDataProvider : DataProviderForAutoCompletion<TItem>
        {
            var mainDesc = DescribedType.GetMainTypeDescription();
            if (mainDesc.IsEnumerable && !DescribedType.GetTypeDescription(mainDesc.EnumeratedDiplayFullTypeName).IsPrimitive)
            {
                throw new InvalidOperationException(
                    $"Вы не можете установить для свойства {Block.PropertyName} тип блока 'Автозаполнение'. " +
                    "Так как у данное свойство является массивом из непримитивов. " +
                    "Необходим тип, который будет являтся примитивом или массивом из примитивов.");
            }



            //TODO Сделать логику
            

            return this;
        }
    }
}