using Croco.Core.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Tests
{
    public class ProductInProductGroupIdModel
    {
        public string ProductGroupId { get; set; }

        public int ProductId { get; set; }
    }

    public class ProductGroupJsWorker : IJsWorker
    {
        public const string GetArrayName = "GetArray";
        public const string WorkerName = nameof(ProductGroupJsWorker);

        public static BaseApiResponse AddProductToGroup(ProductInProductGroupIdModel model)
        {
            return new BaseApiResponse(true, "ok");
        }

        public static int[] GetArray()
        {
            return new int[] { 1, 2, 3, 4 };
        }

        public JsWorkerDocumentation JsWorkerDocs(JsWorkerBuilder builder)
        {
            return builder.SetWorkerName(WorkerName)
                .SetDescription("")
                .AddMethod<ProductInProductGroupIdModel, BaseApiResponse>(AddProductToGroup, new JsWorkerMethodDocsOptions
                {
                    MethodName = "AddProductToGroup",
                    Description = "Добавить товар в группу товаров",
                })
                .AddMethod(GetArray, new JsWorkerMethodDocsOptions
                {
                    MethodName = GetArrayName,
                    Description = "Получить массив"
                }).Build();
        }
    }

    public class ApiCallTests
    {
        [Test]
        public void Test1()
        {
            var serviceCollcetion = new ServiceCollection();

            var script = $"var t = JSON.parse( api.Call(\"{ProductGroupJsWorker.WorkerName}\"," + " \"AddProductToGroup\", { ProductGroupId: \"d8c8cf9b-1d9b-4199-a85e-615edd64b4d7\", ProductId: 1 }) );";

            script += "\n console.log('Result', t)";

            new JsExecutorBuilder(serviceCollcetion)
                .AddJsWorker(builder => new ProductGroupJsWorker().JsWorkerDocs(builder))
                .Build();

            var srvProvider = serviceCollcetion.BuildServiceProvider();

            var result = srvProvider.GetRequiredService<JsExecutor>().RunScriptDetaiiled(script);
            Assert.IsTrue(result.IsSucceeded);
            Assert.IsTrue(result.ResponseObject.Logs.Count == 1);

            var log = result.ResponseObject.Logs.First();

            var json = ZooSerializer.Serialize(ProductGroupJsWorker.AddProductToGroup(null));

            Assert.AreEqual(log.SerializedVariables.Last().DataJson, json);
        }

        [Test]
        public void Test2()
        {
            var script = $"var t = JSON.parse( api.Call(\"{ProductGroupJsWorker.WorkerName}\", \"{ProductGroupJsWorker.GetArrayName}\") );\n";

            script += "console.log(t);\n";
            script += "console.log(t.length);\n";
            script += "for(var i = 0; i < t.length; i++) {";
            
            script += "\n console.log('Result', i, t[i]); \n}";

            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection)
                .AddJsWorker(builder => new ProductGroupJsWorker().JsWorkerDocs(builder))
                .Build();

            var executor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            var result = executor.RunScriptDetaiiled(script);

            Assert.AreEqual(ProductGroupJsWorker.GetArray().Length + 2, result.ResponseObject.Logs.Count);
        }
    }
}