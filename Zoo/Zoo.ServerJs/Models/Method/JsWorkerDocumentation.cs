using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Zoo.ServerJs.Abstractions;
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
        /// <param name="executionContext"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal JsWorkerMethodResult HandleCall(string methodName, IServiceProvider serviceProvider, IJsWorkerMethodCallParameters parameters, JsExecutionContext executionContext, ILogger logger)
        {
            if (!Methods.ContainsKey(methodName))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.MethodWithNameDoesNotExistInWorkerFormat, methodName, WorkerName));
            }

            var method = Methods[methodName];

            var countOfMethodParams = method.Description.Parameters?.Count ?? 0;

            var paramsInBag = parameters.GetParamsLength();

            if (paramsInBag < countOfMethodParams)
            {
                var mes = string.Format(ExceptionTexts.MethodWasCalledWithLessParamsFormat,
                    methodName, WorkerName, countOfMethodParams, paramsInBag);

                throw new InvalidOperationException(mes);
            }

            return method.Method.HandleCall(parameters, serviceProvider, logger);
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