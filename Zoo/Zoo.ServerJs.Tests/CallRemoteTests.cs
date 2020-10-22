using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Zoo.ServerJs.Services;
using Zoo.ServerJs.WorkerExamples;

namespace Zoo.ServerJs.Tests
{
    public class CallRemoteTests
    {
        [Test]
        public async Task Test()
        {
            var mockHttpClient = NSubstitute.Substitute.For<HttpClient>();

            var services = new ServiceCollection();
            new JsExecutorBuilder(services)
                .AddJsWorker(new CalculatorJsWorker())
                .Build();

            var srvProvider = services.BuildServiceProvider();

            var jsExecutor = srvProvider.GetRequiredService<JsExecutor>();

        }

        [TestCase(10, 2, 5)]
        [TestCase(18, 3, 6)]
        [TestCase(18, 2, 9)]
        public void TestCall(int param1, int param2, int expectedResult)
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
            Assert.AreEqual(expectedResult.ToString(), result.ResponseJson);
        }
    }
}