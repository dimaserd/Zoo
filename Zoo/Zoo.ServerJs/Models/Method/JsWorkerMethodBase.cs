using System;

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
        public Func<JsWorkerMethodCallParameters, IServiceProvider, JsWorkerMethodResult> FunctionLink { get; set; }

        /// <summary>
        /// Обработать вызов
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public JsWorkerMethodResult HandleCall(JsWorkerMethodCallParameters parameters, IServiceProvider serviceProvider)
        {
            return FunctionLink(parameters, serviceProvider);
        }
    }
}