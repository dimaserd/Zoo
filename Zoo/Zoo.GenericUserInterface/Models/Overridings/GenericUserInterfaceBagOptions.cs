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
        /// Переопределители интерфейсов
        /// </summary>
        public Dictionary<Type, Type> InterfaceOverriders { get; set; }
    }
}