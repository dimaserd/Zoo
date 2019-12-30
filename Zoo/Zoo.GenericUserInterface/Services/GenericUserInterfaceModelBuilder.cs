﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Services
{
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

        /// <summary>
        /// Результат - модель для построения пользовательского интерфейса
        /// </summary>
        public GenerateGenericUserInterfaceModel Result { get; }
        
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
        /// Установить свойство в конец списка
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder ShiftPropertyToEndFor(string propertyName)
        {
            var block = GetBlockByPropertyName(propertyName);

            Result.Blocks.Remove(block);

            Result.Blocks.Add(block);

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

            Result.Blocks.Remove(block);

            Result.Blocks.Insert(0, block);

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
        /// Установить выпадающий список со множественным выбором для свойства объекта
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="selectListItems"></param>
        /// <returns></returns>
        public GenericUserInterfaceModelBuilder SetMultipleDropDownListFor(string propertyName, List<MySelectListItem> selectListItems)
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
        public GenericUserInterfaceModelBuilder SetDropDownListFor(string propertyName, List<MySelectListItem> selectListItems)
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