﻿using System;
using System.Collections.Generic;
using System.Linq;
using Croco.Core.Abstractions.Models;
using Jint;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.OpenApi;
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
        public Dictionary<string, IJsWorker> JsWorkers { get; }

        /// <summary>
        /// Внешние компоненты
        /// </summary>
        public Dictionary<string, ExternalJsComponent> ExternalComponents { get; }

        /// <summary>
        /// Обработчик Js вызовов
        /// </summary>
        public HandleJsCallWorker JsCallHandler { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        public JsExecutor(JsExecutorProperties properties)
        {
            _engineProps = properties.EngineAction;
            JsWorkers = properties.JsWorkers;
            ExternalComponents = properties.ExternalComponents;
            JsCallHandler = new HandleJsCallWorker(JsWorkers.Values.ToList(), ExternalComponents.Values.ToList());
        }
        
        private Engine _engine;

        #region Свойства
        
        private readonly List<JsLogggedVariables> Logs = new List<JsLogggedVariables>();

        /// <summary>
        /// Движок JInt
        /// </summary>
        public Engine Engine
        {
            get
            {
                if (_engine != null)
                {
                    return _engine;
                }

                _engine = new Engine();
                _engine.SetValue(JsConsts.ApiObjectName, JsCallHandler);

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
            return JsWorkers.Values.Select(x => new JsOpenApiWorkerDocumentation(x.JsWorkerDocs())).ToList();
        }

        /// <summary>
        /// Асинхронно вызвать несколько обработчиков
        /// </summary>
        /// <param name="jsScripts"></param>
        /// <returns></returns>
        public List<BaseApiResponse<object>> CallManySimpleApis(List<string> jsScripts)
        {
            return jsScripts.Select(script => CallSimpleApi(script)).ToList();
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

                return new BaseApiResponse<JsScriptExecutedResult>(true, "Скрипт выполнен успешно", new JsScriptExecutedResult
                {
                    StartedOnUtc = startDate,
                    FinishOnUtc = DateTime.UtcNow,
                    Logs = Logs,
                });
            }

            catch(Exception ex)
            {
                return new BaseApiResponse<JsScriptExecutedResult>(false, "Ошибка при выполнении скрипта. " + ex.Message, new JsScriptExecutedResult
                {
                    StartedOnUtc = startDate,
                    FinishOnUtc = DateTime.UtcNow,
                    ExceptionStackTrace = ex.ToString(),
                    Logs = Logs                    
                });
            }
        }
        #endregion

        private void Log(params object[] objs)
        {
            Logs.Add(new JsLogggedVariables 
            {
                LoggedOnUtc = DateTime.UtcNow,
                SerializedVariables = objs.Select(x => new JsSerializedVariable 
                {
                    DataJson = ZooSerializer.Serialize(x),
                    TypeFullName = x.GetType().FullName
                }).ToList()
            });
        }
    }
}