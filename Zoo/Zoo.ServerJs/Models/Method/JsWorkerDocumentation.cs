using System;
using System.Collections.Generic;
using Zoo.ServerJs.Resources;

namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Документация серверного js обработчика
    /// </summary>
    public class JsWorkerDocumentation
    {
        /// <summary>
        /// Название обработчика
        /// </summary>
        public string WorkerName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Методы обработчика
        /// </summary>
        public Dictionary<string, JsWorkerMethodDocs> Methods { get; set; } = new Dictionary<string, JsWorkerMethodDocs>();

        /// <summary>
        /// Обработка вызова метода
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal JsWorkerMethodResult HandleCall(string methodName, IServiceProvider serviceProvider, JsWorkerMethodCallParameters parameters)
        {
            if (!Methods.ContainsKey(methodName))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.MethodWithNameAlreadyExistsInWorkerFormat, methodName, WorkerName));
            }

            return Methods[methodName].Method.HandleCall(parameters, serviceProvider);
        }

        internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(WorkerName))
            {
                throw new InvalidOperationException(ExceptionTexts.WorkerNameIsRequired);
            }

            if (Methods.Count == 0)
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.NoMethodsInWorkerFormat, WorkerName));
            }
        }
    }
}