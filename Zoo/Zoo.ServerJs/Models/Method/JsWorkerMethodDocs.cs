using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Модель описывающая js метод или функцию
    /// </summary>
    public class JsWorkerMethodDocs
    {
        /// <summary>
        /// Типы входных параметров
        /// </summary>
        public List<Type> Parameters { get; set; }

        /// <summary>
        /// Тип возвращаемого значения 
        /// </summary>
        public Type Response { get; set; }

        /// <summary>
        /// Ссылка на метод
        /// </summary>
        public JsWorkerMethodBase Method { get; set; }

        /// <summary>
        /// Название метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Описание метода
        /// </summary>
        public string Description { get; set; }
    }
}