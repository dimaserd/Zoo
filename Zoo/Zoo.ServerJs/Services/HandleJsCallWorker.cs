using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Statics;

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
        /// <param name="externalComponents"></param>
        public HandleJsCallWorker(List<IJsWorker> workers, List<ExternalJsComponent> externalComponents)
        {
            Workers = workers.Select(x => x.JsWorkerDocs()).ToList();
            ExternalComponents = externalComponents ?? new List<ExternalJsComponent>();
        }

        private List<ExternalJsComponent> ExternalComponents { get; }
        private List<JsWorkerDocumentation> Workers { get; }
        
        /// <summary>
        /// Вызвать внутренний сервис, написанный на Js
        /// </summary>
        /// <param name="workerName">название класса рабочего который нужно вызвать</param>
        /// <param name="method">метод который нужно вызвать у данного рабочего</param>
        /// <param name="methodParams">Параметры метода</param>
        public string Call(string workerName, string method, params dynamic[] methodParams)
        {
            var worker = Workers.FirstOrDefault(x => x.WorkerName == workerName);

            if (worker == null)
            {
                throw new ArgumentNullException($"В системе нет зарегистрированного рабочего класса с именем '{workerName}'");
            }
            
            var res = worker.HandleCall(method, new JsWorkerMethodCallParameters(methodParams)).Result;

            return ZooSerializer.Serialize(res);
        }

        /// <summary>
        /// Вызвать внешний сервис, определенный через Js
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="methodName"></param>
        /// <param name="methodPayLoad"></param>
        /// <returns></returns>
        public string CallExternal(string componentName, string methodName, object methodPayLoad)
        {
            var component = ExternalComponents.FirstOrDefault(x => x.ComponentName == componentName);

            if(component == null)
            {
                throw new Exception($"Компонент не найден по указанному названию '{methodName}'");
            }

            var uid = $"n{Guid.NewGuid()}".Replace("-", "_");

            var variableScript = $"{uid} = {ZooSerializer.Serialize(methodPayLoad)};";

            var engine = new Engine();
            engine.SetValue(JsConsts.ApiObjectName, this);

            var variable = engine.Execute(variableScript).GetValue(uid);

            var methodExpr = engine.Execute(component.Script).GetValue(methodName);

            if (methodExpr.Type == Jint.Runtime.Types.Undefined)
            {
                throw new Exception($"Метод '{methodName}' не найден в компоненте '{componentName}'");
            }

            return ZooSerializer.Serialize(methodExpr.Invoke(variable).ToObject());
        }
    }
}