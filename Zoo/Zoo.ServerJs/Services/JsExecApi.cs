using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// обертка для Апи
    /// </summary>
    public class JsExecApi
    {
        private readonly HandleJsCallWorker _callHandler;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="callHandler"></param>
        public JsExecApi(HandleJsCallWorker callHandler)
        {
            _callHandler = callHandler;
        }

        /// <summary>
        /// Вызов Js рабочего
        /// </summary>
        /// <param name="workerName"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string Call(string workerName, string methodName, params object[] parameters)
        {
            var res = _callHandler.Call(workerName, methodName, parameters);

            return ZooSerializer.Serialize(res);
        }
    }
}