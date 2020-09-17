﻿using System;
using System.Collections.Generic;
using System.Linq;
using Croco.Core.Abstractions.Models;
using Jint;
using Zoo.ServerJs.Consts;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Models.OpenApi;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class JsExecutor
    {
        private readonly List<JsOpenApiWorkerDocumentation> _openApiDocs;

        /// <summary>
        /// Javascript обработчики
        /// </summary>
        Dictionary<string, JsWorkerDocumentation> JsWorkers { get; }

        /// <summary>
        /// Внешние компоненты
        /// </summary>
        Dictionary<string, ExternalJsComponent> ExternalComponents { get; }

        /// <summary>
        /// Обработчик Js вызовов
        /// </summary>
        public HandleJsCallWorker JsCallHandler { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="properties"></param>
        public JsExecutor(IServiceProvider serviceProvider, JsExecutorProperties properties)
        {
            JsWorkers = properties.JsWorkers;
            ExternalComponents = properties.ExternalComponents;

            _openApiDocs = JsWorkers.Select(x => x.Value).Select(x => new JsOpenApiWorkerDocumentation(x)).ToList();

            JsCallHandler = new HandleJsCallWorker(serviceProvider, JsWorkers, ExternalComponents);
            
            var engine = new Engine()
                .SetValue(JsConsts.ApiObjectName, JsCallHandler)
                .SetValue("console", new
                {
                    log = new Action<object[]>(Log)
                });

            properties.EngineAction?.Invoke(engine);

            Engine = engine;
        }

        
        #region Свойства
        
        private readonly List<JsLogggedVariables> Logs = new List<JsLogggedVariables>();

        /// <summary>
        /// Движок JInt
        /// </summary>
        public Engine Engine { get; }

        #endregion

        #region Методы

        /// <summary>
        /// Получить документацию
        /// </summary>
        /// <returns></returns>
        public List<JsOpenApiWorkerDocumentation> GetDocumentation() => _openApiDocs;

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