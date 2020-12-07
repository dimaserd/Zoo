using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Bag;
using Zoo.GenericUserInterface.Services;
using Zoo.GenericUserInterface.Utils;

namespace Zoo.GenericUserInterface.Tests
{
    public static class TestsHelper
    {
        /// <summary>
        /// Создать дефолтный пустой портфель интерфейсов
        /// </summary>
        /// <returns></returns>
        public static GenericUserInterfaceBag CreateDefaultBag()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            return new GenericUserInterfaceBag(serviceProvider, new GenericUserInterfaceBagOptions
            {
                AutoCompletionDataProviders = new Dictionary<string, Type>(),
                SelectListDataProviders = new Dictionary<string, Type>(),
                DefaultInterfaceOverriders = new Dictionary<Type, Type>()
            }, GenericInterfaceOptions.Default());
        }

        public static void AssertAreEqualViaJson<T>(T data1, T data2)
        {
            var json1 = Tool.JsonConverter.Serialize(data1);
            var json2 = Tool.JsonConverter.Serialize(data2);

            Assert.AreEqual(json1, json2);
        }
    }
}