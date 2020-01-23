using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Services
{
    public class HandleJsCallWorker
    {
        public HandleJsCallWorker(List<IJsWorker> workers)
        {
            Workers = workers;
        }

        private List<IJsWorker> Workers { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerName">название класса рабочего который нужно вызвать</param>
        /// <param name="method">метод который нужно вызвать у данного рабочего</param>
        /// <param name="methodParams">Параметры метода</param>
        public object Call(string workerName, string method, params object[] methodParams)
        {
            var worker = Workers.FirstOrDefault(x => x.JsWorkerDocs().WorkerName == workerName);

            if (worker == null)
            {
                throw new ArgumentNullException($"В системе нет зарегистрированного рабочего класса с именем '{workerName}'");
            }
            
            return worker.JsWorkerDocs().HandleCall(method, new JsWorkerMethodCallParameters(methodParams)).Result;
        }
    }
}