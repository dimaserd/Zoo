using System;
using System.Collections.Generic;

namespace Zoo.GenericUserInterface.Models.Bag
{
    /// <summary>
    /// Опции для создания портфеля из интерфейсов
    /// </summary>
    public class GenericUserInterfaceBagOptions
    {
        /// <summary>
        /// Хост урл для приложения
        /// </summary>
        public string ApplicationHostUrl { get; set; }

        /// <summary>
        /// Провайдеры данных для выпадающего списка
        /// </summary>
        public Dictionary<string, Type> SelectListDataProviders { get; set; }

        /// <summary>
        /// Провайдеры данных для автокомплита
        /// </summary>
        public Dictionary<string, Type> AutoCompletionDataProviders { get; set; }

        /// <summary>
        /// Переопределители интерфейсов
        /// </summary>
        public Dictionary<Type, Type> DefaultInterfaceOverriders { get; set; }
    }
}