using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Модель описывающая js метод или функцию
    /// </summary>
    public class JsWorkerMethodDocs
    {
        internal JsWorkerMethodDocs(Type response, List<Type> parameters, JsWorkerMethodDocsOptions opts)
        {
            Response = response;
            Parameters = parameters;
            MethodName = opts.MethodName;
            Description = opts.Description;
            ResultDescription = opts.ResultDescription;
            ParameterDescriptions = opts.ParameterDescriptions;
            SetDescriptions(Response != null, Parameters?.Count ?? 0);
        }

        internal void SetDescriptions(bool hasResult, int paramsLength)
        {
            if (hasResult && string.IsNullOrEmpty(ResultDescription))
            {
                ResultDescription = "";
            }

            var list = new List<string>();

            if(ParameterDescriptions != null)
            {
                foreach(var p in ParameterDescriptions)
                {
                    list.Add(p);
                }
            }

            for (int i = list.Count; i <= paramsLength; i++)
            {
                list.Add("");
            }

            ParameterDescriptions = list.ToArray();
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