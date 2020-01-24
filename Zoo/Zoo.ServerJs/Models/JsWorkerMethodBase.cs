using System;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Базовое описание метода для серверного javascript обработчика
    /// </summary>
    public class JsWorkerMethodBase
    {
        /// <summary>
        /// Ссылка на функцию
        /// </summary>
        public Func<JsWorkerMethodCallParameters, JsWorkerMethodResult> FunctionLink { get; set; }

        /// <summary>
        /// Обработать вызов
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public JsWorkerMethodResult HandleCall(JsWorkerMethodCallParameters parameters)
        {
            return FunctionLink(parameters);
        }
    }
}