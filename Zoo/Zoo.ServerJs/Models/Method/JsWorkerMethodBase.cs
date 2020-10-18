using System;
using Zoo.ServerJs.Abstractions;

namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Базовое описание метода для серверного javascript обработчика
    /// </summary>
    public class JsWorkerMethodBase
    {
        /// <summary>
        /// Ссылка на функцию
        /// </summary>
        public Func<IJsWorkerMethodCallParameters, IServiceProvider, JsWorkerMethodResult> FunctionLink { get; set; }

        /// <summary>
        /// Обработать вызов
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public JsWorkerMethodResult HandleCall(IJsWorkerMethodCallParameters parameters, IServiceProvider serviceProvider)
        {
            return FunctionLink(parameters, serviceProvider);
        }
    }
}