using Croco.Core.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
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
                .AddMethodViaFunction<ProductInProductGroupIdModel, BaseApiResponse>(AddProductToGroup, new JsWorkerMethodDocsOptions
                {
                    MethodName = "AddProductToGroup",
                    Description = "Добавить товар в группу товаров",
                })
                .AddMethodViaFunction(GetArray, new JsWorkerMethodDocsOptions
                {
                    MethodName = GetArrayName,
                    Description = "Получить массив"
                }).Build();
        }
    }

    public class ApiCallTests
    {
        [Test]
        public async Task Test1()
        {
            var serviceCollcetion = new ServiceCollection();

            var script = $"var t = api.Call(\"{ProductGroupJsWorker.WorkerName}\"," + " \"AddProductToGroup\", { ProductGroupId: \"d8c8cf9b-1d9b-4199-a85e-615edd64b4d7\", ProductId: 1 });";

            script += "\n console.log('Result', t)";

            new JsExecutorBuilder(serviceCollcetion)
                .AddJsWorker(new ProductGroupJsWorker())
                .Build();

            var srvProvider = serviceCollcetion.BuildServiceProvider();

            var executor = srvProvider.GetRequiredService<JsExecutor>();

            var result = await executor.RunScriptDetaiiled(script);
            Assert.IsTrue(result.IsSucceeded);
            Assert.AreEqual(1, result.ConsoleLogs.Count);

            var log = result.ConsoleLogs.First();

            var json = ZooSerializer.Serialize(ProductGroupJsWorker.AddProductToGroup(null));

            Assert.AreEqual(log.SerializedVariables.Last().DataJson, json);
        }

        [Test]
        public async Task Test2()
        {
            var script = $"var t = api.Call(\"{ProductGroupJsWorker.WorkerName}\", \"{ProductGroupJsWorker.GetArrayName}\");";

            script += "console.log(t);\n";
            script += "console.log(t.length);\n";
            script += "for(var i = 0; i < t.length; i++) {";
            
            script += "\n console.log('Result', i, t[i]); \n}";

            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection)
                .AddJsWorker(new ProductGroupJsWorker())
                .Build();

            var executor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            var result = await executor.RunScriptDetaiiled(script);
            Assert.IsTrue(result.IsSucceeded);
            Assert.AreEqual(ProductGroupJsWorker.GetArray().Length + 2, result.ConsoleLogs.Count);
        }
    }
}