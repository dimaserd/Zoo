﻿using Croco.Core.Documentation.Models;
using Croco.Core.Documentation.Services;
using Croco.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Resources;

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
            Result = CreateFromObject(model);
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
            Result = CreateFromType(type, valueJson, opts);
        }


        /// <summary>
        /// Создать из объекта используя провайдер значений
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromObject(object model)
        {
            var dataJson = Tool.JsonConverter.Serialize(model);

            return CreateFromType(model.GetType(), dataJson, null);
        }

        /// <summary>
        /// Создать из типа
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueJson"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromType(Type type, string valueJson = null, GenericInterfaceOptions opts = null)
        {
            var desc = CrocoTypeDescription.GetDescription(type);

            var main = desc.GetMainTypeDescription();

            if (!main.IsClass)
            {
                throw new InvalidOperationException(ExceptionTexts.NonComplexTypesAreNotSupported);
            }

            var isRecursive = new CrocoClassDescriptionChecker().IsRecursiveType(main, desc.Types);

            if (isRecursive)
            {
                throw new InvalidOperationException(ExceptionTexts.RecursiveTypesAreNotSupported);
            }

            if (opts == null)
            {
                opts = GenericInterfaceOptions.Default();
            }

            var blocks = GenericUserInterfaceModelBuilderExtensions.GetBlocks("", desc.GetMainTypeDescription(), desc, opts);

            return new GenerateGenericUserInterfaceModel
            {
                TypeDescription = desc,
                Interface = new GenericInterfaceModel
                {
                    Prefix = "",
                    Blocks = blocks
                },
                ValueJson = valueJson
            };
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