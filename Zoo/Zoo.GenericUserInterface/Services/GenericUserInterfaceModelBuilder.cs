using Croco.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Services
{
    /// <summary>
    /// Построитель обобщенного пользовательского интерфейса
    /// </summary>
    public class GenericUserInterfaceModelBuilder
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        public GenericUserInterfaceModelBuilder(object model)
        {
            Result = GenerateGenericUserInterfaceModel.CreateFromObject(model);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="result"></param>
        public GenericUserInterfaceModelBuilder(GenerateGenericUserInterfaceModel result)
        {
            Result = result ?? throw new NullReferenceException(nameof(result));
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public GenericUserInterfaceModelBuilder(Type type) : this(type, null, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(Type type, GenericInterfaceOptions opts): this(type, null, opts)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueJson"></param>
        /// <param name="opts"></param>
        public GenericUserInterfaceModelBuilder(Type type, string valueJson, GenericInterfaceOptions opts)
        {
            Result = GenerateGenericUserInterfaceModel.CreateFromType(type, valueJson, opts);
        }

        /// <summary>
        /// Результат - модель для построения пользовательского интерфейса
        /// </summary>
        public GenerateGenericUserInterfaceModel Result { get; }

        /// <summary>
        /// Добавить кастомный блок данных
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder AddCustomData(object data)
        {
            Result.CustomDataJson = Tool.JsonConverter.Serialize(data);

            return this;
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

        /// <summary>
        /// Установить свойство в конец списка
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder ShiftPropertyToEndFor(string propertyName)
        {
            var block = GetBlockByPropertyName(propertyName);

            Result.Interface.Blocks.Remove(block);

            Result.Interface.Blocks.Add(block);

            return this;
        }

        /// <summary>
        /// Установить свойство в начало списка
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder ShiftPropertyToStartFor(string propertyName)
        {
            var block = GetBlockByPropertyName(propertyName);

            Result.Interface.Blocks.Remove(block);

            Result.Interface.Blocks.Insert(0, block);

            return this;
        }

        /// <summary>
        /// Установить скрытый тип инпута, который нельзя редактировать
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder SetHiddenFor(string propertyName)
        {
            var block = GetBlockByPropertyName(propertyName);

            block.InterfaceType = UserInterfaceType.Hidden;

            return this;
        }

        /// <summary>
        /// Установить кастомный тип инпута
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="customType"></param>
        /// <param name="customDataJson"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder SetCustomFor(string propertyName, string customType, string customDataJson)
        {
            var block = GetBlockByPropertyName(propertyName);

            block.InterfaceType = UserInterfaceType.CustomInput;
            block.CustomUserInterfaceType = customType;
            block.CustomDataJson = customDataJson;

            return this;
        }

        /// <summary>
        /// Установить выпадающий список со множественным выбором для свойства объекта
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder SetMultipleDropDownListFor(string propertyName, List<SelectListItem> selectListItems)
        {
            var block = GetBlockByPropertyName(propertyName);

            block.InterfaceType = UserInterfaceType.MultipleDropDownList;

            block.SelectList = selectListItems;

            return this;
        }

        /// <summary>
        /// Установить выпадающий список с единственным выбором для свойства объекта
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder SetDropDownListFor(string propertyName, List<SelectListItem> selectListItems)
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
        public GenericUserInterfaceModelBuilder SetTextAreaFor(string propertyName)
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