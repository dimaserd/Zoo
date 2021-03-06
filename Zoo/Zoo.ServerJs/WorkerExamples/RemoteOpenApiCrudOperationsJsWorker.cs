﻿using Croco.Core.Contract.Models;
using Croco.Core.Documentation.Models.Methods;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.WorkerExamples
{
    /// <summary>
    /// Js рабочий с методами для работы с удаленными хостами
    /// </summary>
    public class RemoteOpenApiCrudOperationsJsWorker : IJsWorker
    {
        string WorkerName { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="workerName"></param>
        public RemoteOpenApiCrudOperationsJsWorker(string workerName = "RemoteOpenApiCrud")
        {
            WorkerName = workerName;
        }

        /// <inheritdoc />
        public JsWorkerDocumentation JsWorkerDocs(JsWorkerBuilder builder)
        {
            return builder.SetWorkerName(WorkerName)
                .SetDescription("Crud операции над удаленными хостами")
                .GetServiceMethodBuilder<JsExecutor>()
                .AddMethodViaTaskWithResult<RemoteJsOpenApi, BaseApiResponse>((srv, p1) => srv.AddRemoteApi(p1), new MethodDocsOptions
                {
                    MethodName = "Add",
                    Description = "Добавить новый удаленный апи хост",
                    ParameterDescriptions = new[]
                    {
                        "Модель описывающая удаленный хост"
                    },
                    ResultDescription = "Результат операции"
                })
                .AddMethodViaTaskWithResult<RemoteJsOpenApi, BaseApiResponse>((srv, p1) => srv.EditRemoteApi(p1), new MethodDocsOptions
                {
                    MethodName = "Edit",
                    Description = "Редактировать удаленный апи хост",
                    ParameterDescriptions = new[]
                    {
                        "Модель описывающая удаленный хост"
                    },
                    ResultDescription = "Результат операции"
                })
                .AddMethodViaTaskWithResult<string, BaseApiResponse>((srv, p1) => srv.DeleteRemoteApi(p1), new MethodDocsOptions
                {
                    MethodName = "Delete",
                    Description = "Удалить удаленный апи хост",
                    ParameterDescriptions = new[]
                    {
                        "Название хоста"
                    },
                    ResultDescription = "Результат операции"
                })
                .Build();
        }
    }
}