using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Zoo.ServerJs.Services;
using Zoo.ServerJs.Statics;
using Zoo.ServerJs.WorkerExamples;

namespace Zoo.ServerJs.Tests
{
    public class CallRemoteTests
    {
        [TestCase(10, 2, 5)]
        [TestCase(18, 3, 6)]
        [TestCase(18, 2, 9)]
        public void TestCall(int param1, int param2, double expectedResult)
        {
            var services = new ServiceCollection();
            new JsExecutorBuilder(services)
                .AddJsWorker(new CalculatorJsWorker())
                .Build();

            var srvProvider = services.BuildServiceProvider();

            var jsExecutor = srvProvider.GetRequiredService<JsExecutor>();

            var result = jsExecutor.CallWorkerMethod(new Models.OpenApi.CallOpenApiWorkerMethod
            {
                WorkerName = "Calculator",
                MethodName = "Divide",
                SerializedParameters = new string[] { param1.ToString(), param2.ToString() }
            });

            Assert.IsTrue(result.IsSucceeded);
            var expetedDataResult = ZooSerializer.Serialize(expectedResult);
            Assert.AreEqual(expetedDataResult, result.ResponseJson);
        }
    }
}