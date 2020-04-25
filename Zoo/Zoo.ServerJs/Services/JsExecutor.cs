﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions.Models;
using Croco.Core.Utils;
using Jint;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class JsExecutor
    {
        private readonly Action<Engine> _engineProps;
        
        /// <summary>
        /// Javascript обработчики
        /// </summary>
        public List<IJsWorker> JsWorkers { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        public JsExecutor(JsExecutorProperties properties)
        {
            _engineProps = properties.EngineAction;
            JsWorkers = properties.JsWorkers;
            _callHandler = new HandleJsCallWorker(JsWorkers);
        }
        
        private Engine _engine;

        private readonly HandleJsCallWorker _callHandler;

        private List<List<object>> _logs;
        

        #region Свойства
        
        private List<List<object>> Logs => _logs ?? (_logs = new List<List<object>>());

        /// <summary>
        /// Движок JInt
        /// </summary>
        protected Engine Engine
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
                    Call = new Func<string, string, object[], object>(Call),
                    CallJson = new Func<string, string, object[], string>(CallJson)
                });

                _engine.SetValue("console", new 
                {
                    log = new Action<object[]>(Log)
                });

                _engineProps?.Invoke(_engine);
                
                return _engine;
            }
        }
        
        #endregion
        
        #region Методы

        /// <summary>
        /// Получить документацию
        /// </summary>
        /// <returns></returns>
        public List<JsOpenApiWorkerDocumentation> GetDocumentation()
        {
            return JsWorkers.Select(x => new JsOpenApiWorkerDocumentation(x.JsWorkerDocs())).ToList();
        }

        /// <summary>
        /// Асинхронно вызвать несколько обработчиков
        /// </summary>
        /// <param name="jsScripts"></param>
        /// <returns></returns>
        public async Task<List<BaseApiResponse<object>>> CallManySimpleApis(List<string> jsScripts)
        {
            var tasks = jsScripts.Select(script => Task.FromResult(CallSimpleApi(script)));

            return (await Task.WhenAll(tasks)).ToList();
        }

        /// <summary>
        /// Вызвать обработчик
        /// </summary>
        /// <param name="jsScript"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Вызвать скрипт
        /// </summary>
        /// <param name="jsScript"></param>
        /// <returns></returns>
        public BaseApiResponse<JsScriptExecutedResult> RunScriptDetaiiled(string jsScript)
        {
            var startDate = DateTime.UtcNow;

            try
            {
                Engine.Execute(jsScript);

                var finishDate = DateTime.UtcNow;

                return new BaseApiResponse<JsScriptExecutedResult>(true, "Скрипт выполнен успешно", new JsScriptExecutedResult
                {
                    StartDate = startDate,
                    FinishDate = finishDate,
                    Logs = Logs,
                });
            }

            catch(Exception ex)
            {
                var finishDate = DateTime.UtcNow;

                return new BaseApiResponse<JsScriptExecutedResult>(false, "Ошибка при выполнении скрипта. " + ex.Message, new JsScriptExecutedResult
                {
                    StartDate = startDate,
                    FinishDate = finishDate,
                    ExceptionStackTrace = ex.ToString()
                });
            }
        }
        #endregion

        #region Подключенные методы
        
        private object Call(string workerName, string methodName, params object[] parameters)
        {
            return _callHandler.Call(workerName, methodName, parameters);
        }

        private string CallJson(string workerName, string methodName, params object[] parameters)
        {
            return ZooSerializer.Serialize(_callHandler.Call(workerName, methodName, parameters));
        }

        private void Log(params object[] objs)
        {
            Logs.Add(objs.ToList());
        }
        #endregion
    }
}