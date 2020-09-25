using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Bag;

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
                InterfaceOverriders = new Dictionary<Type, Type>()
            }, GenericInterfaceOptions.Default());
        }
    }
}