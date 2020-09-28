using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Модель описывающая js метод или функцию
    /// </summary>
    public class JsWorkerMethodDocs
    {
        internal JsWorkerMethodDocs(JsWorkerMethodDocsOptions opts)
        {
            MethodName = opts.MethodName;
            Description = opts.Description;
            ResultDescription = opts.ResultDescription;
            ParameterDescriptions = opts.ParameterDescriptions;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public JsWorkerMethodDocs()
        {
        }

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

        /// <summary>
        /// Описание результата
        /// </summary>
        public string ResultDescription { get; set; }

        /// <summary>
        /// Описание параметров
        /// </summary>
        public string[] ParameterDescriptions { get; set; }
    }
}