using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo.ServerJs.Models
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
        public List<JsWorkerMethodDocs> Methods { get; set; }

        /// <summary>
        /// Получить модель для построения документации
        /// </summary>
        /// <returns></returns>
        public JsOpenApiWorkerDocumentation GetOpenApiDocumentation()
        {
            return new JsOpenApiWorkerDocumentation(this);
        }

        /// <summary>
        /// Обработка вызова метода
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public JsWorkerMethodResult HandleCall(string methodName, JsWorkerMethodCallParameters parameters)
        {
            var method = Methods.FirstOrDefault(x => x.MethodName == methodName);

            if(method == null)
            {
                throw new ApplicationException($"Метод с названием '{methodName}' не существует в группе методов класса '{WorkerName}'");
            }

            return method.Method.HandleCall(parameters);
        }
    }
}