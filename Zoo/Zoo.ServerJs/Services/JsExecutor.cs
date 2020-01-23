using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions.Application;
using Croco.Core.Application;
using Croco.Core.Extensions;
using Croco.Core.Models;
using Jint;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Services
{
    public class JsExecutor<TApplication> where TApplication : class, ICrocoApplication
    {
        private readonly Action<Engine> _engineProps;
        
        public List<IJsWorker> JsWorkers { get; }

        protected TApplication Application { get; }

        public JsExecutor(JsExecutorProperties properties)
        {
            _engineProps = properties.EngineProperties;
            JsWorkers = properties.JsWorkers;
            _callHandler = new HandleJsCallWorker(JsWorkers);
            Application = CrocoApp.Application.As<TApplication>();
        }
        
        private Engine _engine;

        private readonly HandleJsCallWorker _callHandler;

        private List<List<object>> _logs;
        

        #region Свойства
        
        private List<List<object>> Logs => _logs ?? (_logs = new List<List<object>>());

        private Engine Engine
        {
            get
            {
                if (_engine != null)
                {
                    return _engine;
                }

                _engine = new Engine();

                _engine.SetValue(JsConsts.ApiObjectName, new
                {
                    //Данное название функции должно быть неизменным относительно
                    Call = new Func<string, string, object[], object>(Call)
                });

                _engine.SetValue("console", new 
                {
                    log = new Action<object[]>(Log)
                });

                _engineProps(_engine);
                
                return _engine;
            }
        }
        
        #endregion
        
        #region Методы

        public List<JsOpenApiWorkerDocumentation> GetDocumentation()
        {
            return JsWorkers.Select(x => new JsOpenApiWorkerDocumentation(x.JsWorkerDocs())).ToList();
        }

        public async Task<List<BaseApiResponse<object>>> CallManySimpleApis(List<string> jsScripts)
        {
            var tasks = jsScripts.Select(script => Task.FromResult(CallSimpleApi(script)));

            return (await Task.WhenAll(tasks)).ToList();
        }

        public BaseApiResponse<object> CallSimpleApi(string jsScript)
        {
            try
            {
                Engine.Execute($"var result = {jsScript}");
                var result = Engine.GetValue("result").ToObject();

                return new BaseApiResponse<object>(true, "Ok", result);
            }
            catch(Exception ex)
            {
                return new BaseApiResponse<object>(ex);
            }
        }

        public BaseApiResponse<JsScriptExecutedResult> RunScriptDetaiiled(string jsScript)
        {
            var startDate = Application.DateTimeProvider.Now;

            try
            {
                Engine.Execute(jsScript);

                var finishDate = Application.DateTimeProvider.Now;

                return new BaseApiResponse<JsScriptExecutedResult>(true, "Скрипт выполнен успешно", new JsScriptExecutedResult
                {
                    StartDate = startDate,
                    FinishDate = finishDate,
                    ExecutionMSecs = (finishDate - startDate).TotalMilliseconds,
                    Logs = Logs,
                } );
            }

            catch(Exception ex)
            {
                var finishDate = Application.DateTimeProvider.Now;

                return new BaseApiResponse<JsScriptExecutedResult>(false, "Ошибка при выполнении скрипта. " + ex.Message, new JsScriptExecutedResult
                {
                    StartDate = startDate,
                    FinishDate = finishDate,
                    ExecutionMSecs = (finishDate - startDate).TotalMilliseconds,
                    ExceptionStackTrace = ex.ToString()
                });
            }
        }
        #endregion

        #region Подключенные методы
        
        protected object Call(string workerName, string methodName, params object[] parameters)
        {
            return _callHandler.Call(workerName, methodName, parameters);
        }

        protected void Log(params object[] objs)
        {
            Logs.Add(objs.ToList());
        }

        #endregion
    }
}