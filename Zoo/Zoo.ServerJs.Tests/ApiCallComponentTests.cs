using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Services;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Tests
{
    public class ApiCallComponentTests
    {
        [TestCase(7, 12)]
        [TestCase(3, 4)]
        public async Task Test(int arg1, int arg2)
        {
            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection).AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test",
                Script = "function Calculator(model) { \n" +
                "return model.Arg1 + model.Arg2; \n }"
            })
            .AddHttpClientFactory<DefaultHttpClientProvider>()
            .Build();

            var jsExecutor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            
            var script = $"var res = api.CallExternal('Test', 'Calculator', {{ 'Arg1': {arg1}, 'Arg2': {arg2} }}); \n";

            script += "console.log(res)";

            var result = await jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            Assert.IsTrue(result.ConsoleLogs.Count == 1);
            var logValue = result.ConsoleLogs.First().SerializedVariables.First();

            var expectedRes = (double)(arg1 + arg2);
            var directCallResult = jsExecutor.CallExternalComponent<int>("Test", "Calculator", new { Arg1 = arg1, Arg2 = arg2 });

            Assert.AreEqual(expectedRes, directCallResult);
            Assert.AreEqual(directCallResult, ZooSerializer.Deserialize<int>(logValue.DataJson));
        }

        [TestCase(6, 6, 4)]
        [TestCase(6, 1, 3)]
        public async Task CallAfterCall(double arg1, double arg2, double delimeter)
        {
            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection).AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test",
                Script = "function Calculator(model) { \n" +
                        "var t = model.Arg1 + model.Arg2;\n" +
                        "var s = api.CallExternal('Test2', 'CalculatorNew', { 'Arg1': t, 'Arg2': " + $"{delimeter}" + " });\n" +
                        "return s;\n" +
                        " }"
            })
            .AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test2",
                Script = "function CalculatorNew(model) { \n" +
                        "return model.Arg1 / model.Arg2; }"
            })
            .AddHttpClientFactory<DefaultHttpClientProvider>()
            .Build();

            var jsExecutor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            var script = "var res = api.CallExternal('Test', 'Calculator', { 'Arg1': " + $"{arg1}, 'Arg2': {arg2}" + " });\n";

            script += "console.log(res);\n";

            var result = await jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result;

            Assert.IsTrue(resp.ConsoleLogs.Count == 1);

            var logVar = resp.ConsoleLogs.First().SerializedVariables.First();

            var directCallValue = jsExecutor.CallExternalComponent<double>("Test", "Calculator", new { Arg1 = arg1, Arg2 = arg2 });
            var expectedValue = (arg1 + arg2) / delimeter;

            Assert.AreEqual(directCallValue, expectedValue);
            Assert.AreEqual(expectedValue, ZooSerializer.Deserialize<double>(logVar.DataJson));
        }

        [TestCase(12)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(14)]
        public async Task CallWithNoArgs(double n1)
        {
            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection).AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test",
                Script = "function Test() { \n" +
                $"return {n1};\n" +
                " }"
            })
            .AddHttpClientFactory<DefaultHttpClientProvider>()
            .Build();

            var jsExecutor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            var script = "var res = api.CallExternal('Test', 'Test');\n";

            script += "console.log(res);\n";

            var result = await jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            Assert.IsTrue(result.ConsoleLogs.Count == 1);

            var logVar = result.ConsoleLogs.First().SerializedVariables.First();

            Assert.AreEqual(JsonConvert.DeserializeObject<double>(logVar.DataJson), n1);
        }
    }
}