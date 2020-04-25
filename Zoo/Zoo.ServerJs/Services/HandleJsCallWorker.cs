using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Обработчик js вызовов
    /// </summary>
    public class HandleJsCallWorker
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="workers"></param>
        public HandleJsCallWorker(List<IJsWorker> workers)
        {
            Workers = workers.Select(x => x.JsWorkerDocs()).ToList();
        }

        private List<JsWorkerDocumentation> Workers { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerName">название класса рабочего который нужно вызвать</param>
        /// <param name="method">метод который нужно вызвать у данного рабочего</param>
        /// <param name="methodParams">Параметры метода</param>
        public object Call(string workerName, string method, params dynamic[] methodParams)
        {
            var worker = Workers.FirstOrDefault(x => x.WorkerName == workerName);

            if (worker == null)
            {
                throw new ArgumentNullException($"В системе нет зарегистрированного рабочего класса с именем '{workerName}'");
            }
            
            return worker.HandleCall(method, new JsWorkerMethodCallParameters(methodParams)).Result;
        }
    }
}