using System;
using System.Collections.Generic;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Опции для создания портфеля из интерфейсов
    /// </summary>
    public class GenericUserInterfaceBagOptions
    {
        /// <summary>
        /// Провайдеры данных
        /// </summary>
        public Dictionary<string, Type> DataProviders { get; set; }

        /// <summary>
        /// Переопределители интерфейсов
        /// </summary>
        public Dictionary<Type, Type> InterfaceOverriders { get; set; }
    }
}