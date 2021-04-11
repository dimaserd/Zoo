using Croco.Core.Contract.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.Tests.TestWorkers
{
    public class FoundBugTests
    {
        [Test]
        public async Task Bug1()
        {
            var services = new ServiceCollection();

            services.AddTransient<FilesCopyService>();

            new JsExecutorBuilder(services).AddJsWorker(new TestJsWorker()).Build();

            var serviceProvider = services.BuildServiceProvider();

            var executor = serviceProvider.GetRequiredService<JsExecutor>();

            var script = "api.Call(\"Test\", \"CopyFiles\", 10);";

            var result = await executor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);
        }

        public class TestJsWorker : IJsWorker
        {
            public JsWorkerDocumentation JsWorkerDocs(JsWorkerBuilder builder)
            {
                builder
                    .SetWorkerName("Test")
                    .SetDescription("Предоставляет тестовые методы")
                .GetServiceMethodBuilder<FilesCopyService>()
                .AddMethodViaTask<int>((srv, count) => srv.CopyFiles(count), new JsWorkerMethodDocsOptions
                {
                    MethodName = "CopyFiles",
                    Description = "Копирует несколько файлов",
                    ParameterDescriptions = new string[]
                    {
                        "Количество файлов"
                    }
                });

                return builder.Build();
            }
        }

        public class FilesCopyService
        {
            public Task<BaseApiResponse> CopyFiles(int count)
            {
                return Task.FromResult(new BaseApiResponse(true, "ok"));
            }
        }
    }
}
