using Jint;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services.Properties;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Обработчик js вызовов
    /// </summary>
    public class HandleJsCallWorker
    {
        private RemoteJsCaller RemoteCaller { get; }
        private JsExecutorComponents Components { get; }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="executionContext"></param>
        internal HandleJsCallWorker(JsExecutionContextProperties properties, JsExecutionContext executionContext)
        {
            Components = properties.Components;
            RemoteCaller = new RemoteJsCaller(Components.RemoteApiDocs, Components.HttpClientProvider, executionContext);
        }
        
        /// <summary>
        /// Вызвать внутренний метод сервис
        /// </summary>
        /// <param name="workerName">название класса рабочего который нужно вызвать</param>
        /// <param name="method">метод который нужно вызвать у данного рабочего</param>
        /// <param name="methodParams">Параметры метода</param>
        public string Call(string workerName, string method, params dynamic[] methodParams)
        {
            var worker = Components.GetJsWorker(workerName);

            var res = worker.HandleCall(method, Components.ServiceProvider, new JsWorkerMethodCallParameters(methodParams)).Result;

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
            try
            {
                return CallExternalUnSafe(componentName, methodName, methodPayLoad);
            } 
            catch(Exception ex)
            {
                var mes = $"Произошла ошибка при вызове внешнего компонента. Название внешнего компонента = '{componentName}'.\n "
                    + $"Название метода = '{methodName}'.\n";

                if(methodPayLoad != null)
                {
                    mes += $"Параметр метода = {ZooSerializer.Serialize(methodPayLoad)}.\n ";
                }
                else
                {
                    mes += $"Параметр метода = [Метод был вызван без параметра].\n ";
                }

                throw new Exception(mes + ex.Message);
            }
        }

        /// <summary>
        /// Вызвать удаленное апи
        /// </summary>
        /// <param name="remoteName"></param>
        /// <param name="workerName"></param>
        /// <param name="methodName"></param>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        public Task<string> CallRemoteApi(string remoteName, string workerName, string methodName, params dynamic[] methodParams)
        {
            return RemoteCaller.CallRemoteApi(remoteName, workerName, methodName, methodParams);
        }

        private string CallExternalUnSafe(string componentName, string methodName, object methodPayLoad)
        {
            if (!Components.ExternalComponents.ContainsKey(componentName))
            {
                throw new InvalidOperationException($"Компонент не найден по указанному названию '{componentName}'");
            }

            var component = Components.ExternalComponents[componentName];

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