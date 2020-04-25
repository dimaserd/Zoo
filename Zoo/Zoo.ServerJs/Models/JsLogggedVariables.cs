using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Залоггированные переменные
    /// </summary>
    public class JsLogggedVariables
    {
        /// <summary>
        /// Дата логгирования
        /// </summary>
        public DateTime LoggedOnUtc { get; set; }

        /// <summary>
        /// Сериализованные переменные
        /// </summary>
        public List<JsSerializedVariable> SerializedVariables { get; set; }
    }
}