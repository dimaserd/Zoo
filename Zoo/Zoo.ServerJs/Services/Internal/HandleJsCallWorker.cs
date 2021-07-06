using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services.Internal
{
    /// <summary>
    /// Обработчик js вызовов
    /// </summary>
    internal class HandleJsCallWorker
    {

        private RemoteJsCaller RemoteCaller { get; }
        private JsExecutorComponents Components { get; }
        private IServiceProvider ServiceProvider { get; }
        private JsExecutionContext ExecutionContext { get; }
        public ILogger Logger { get; }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="components"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="executionContext"></param>
        /// <param name="logger"></param>
        internal HandleJsCallWorker(JsExecutorComponents components, IServiceProvider serviceProvider, 
            JsExecutionContext executionContext, ILogger logger)
        {
            Components = components;
            ServiceProvider = serviceProvider;
            ExecutionContext = executionContext;
            Logger = logger;
            RemoteCaller = new RemoteJsCaller(Components.RemoteApiDocs, Components.HttpClient, ExecutionContext);
        }

        /// <summary>
        /// Вызвать внутренний метод сервис
        /// </summary>
        /// <param name="workerName">название класса рабочего который нужно вызвать</param>
        /// <param name="method">метод который нужно вызвать у данного рабочего</param>
        /// <param name="methodParams">Параметры метода</param>
        public string Call(string workerName, string method, params dynamic[] methodParams)
        {
            var worker = Components.GetJsWorker(workerName, ExecutionContext);

            var res = worker.HandleCall(method, ServiceProvider, new JsWorkerMethodCallParameters(methodParams), ExecutionContext, Logger).Result;

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
                var result = CallExternalUnSafe(componentName, methodName, methodPayLoad).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception ex)
            {
                var mes = $"Произошла ошибка при вызове внешнего компонента. Название внешнего компонента = '{componentName}'.\n "
                    + $"Название метода = '{methodName}'.\n";

                if (methodPayLoad != null)
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
        public string CallRemoteWorkerMethod(string remoteName, string workerName, string methodName, params dynamic[] methodParams)
        {
            return RemoteCaller.CallRemoteWorkerMethod(remoteName, workerName, methodName, methodParams)
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="methodName"></param>
        /// <param name="methodPayLoad"></param>
        /// <returns></returns>
        private async Task<string> CallExternalUnSafe(string componentName, string methodName, object methodPayLoad)
        {
            var component = await Components.GetExternalComponent(componentName, ExecutionContext);

            var uid = $"n{Guid.NewGuid()}".Replace("-", "_");

            var payloadJson = ZooSerializer.Serialize(methodPayLoad);

            var variableScript = $"{uid} = {payloadJson};";

            var engine = ExecutionContext.CreateEngine();

            var variable = engine.Execute(variableScript).GetValue(uid);

            var methodExpr = engine.Execute(component.Script).GetValue(methodName);

            if (methodExpr.Type == Jint.Runtime.Types.Undefined)
            {
                throw new Exception($"Метод '{methodName}' не найден в компоненте '{componentName}'");
            }

            var resultJson = ZooSerializer.Serialize(methodExpr.Invoke(variable).ToObject());

            ExecutionContext.ExecutionLogs.Add(new JsExecutionLog
            {
                EventIdName = EventIds.JsExecutorComponents.CallExternalComponentOnResult,
                Message = "Завершен вызов внешнего компонента",
                DataJson = JsonConvert.SerializeObject(new
                {
                    ComponentName = componentName, 
                    MethodName = methodName,
                    PayloadJson = payloadJson,
                    ResultJson = resultJson
                })
            });

            return resultJson;
        }
    }
}