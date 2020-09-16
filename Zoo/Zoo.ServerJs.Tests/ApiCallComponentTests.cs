using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.Tests
{
    public class ApiCallComponentTests
    {
        [TestCase(7, 12)]
        [TestCase(3, 4)]
        public void Test(int arg1, int arg2)
        {
            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection).AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test",
                Script = "function Calculator(model) { \n" +
                "return model.Arg1 + model.Arg2; \n }"
            }).Build();

            var jsExecutor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            var script = $"var res = api.CallExternal('Test', 'Calculator', {{ 'Arg1': {arg1}, 'Arg2': {arg2} }}); \n";

            script += "console.log(res)";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);
            var logValue = resp.Logs.First().SerializedVariables.First();

            Assert.AreEqual((double)(arg1 + arg2), JsonConvert.DeserializeObject<double>(logValue.DataJson));
        }

        [TestCase(6, 6, 4)]
        [TestCase(6, 1, 3)]
        public void CallAfterCall(double arg1, double arg2, double delimeter)
        {
            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection).AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test",
                Script = "function Calculator(model) { \n" +
                        "var t = model.Arg1 + model.Arg2;\n" +
                        "var s = JSON.parse( api.CallExternal('Test2', 'CalculatorNew', { 'Arg1': t, 'Arg2': " + $"{delimeter}" + " }) );\n" +
                        "return s;\n" +
                        " }"
            })
            .AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test2",
                Script = "function CalculatorNew(model) { \n" +
                        "return model.Arg1 / model.Arg2; }"
            }).Build();

            var jsExecutor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            var script = "var res = api.CallExternal('Test', 'Calculator', { 'Arg1': " + $"{arg1}, 'Arg2': {arg2}" + " });\n";

            script += "res = JSON.parse(res);\n";
            script += "console.log(res);\n";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);

            var logVar = resp.Logs.First().SerializedVariables.First();


            var exprexctedValue = (arg1 + arg2) / delimeter;
            
            Assert.AreEqual(exprexctedValue, JsonConvert.DeserializeObject<double>(logVar.DataJson));
        }

        [TestCase(12)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(14)]
        public void CallWithNoArgs(double n1)
        {
            var serviceCollection = new ServiceCollection();

            new JsExecutorBuilder(serviceCollection).AddExternalComponent(new ExternalJsComponent
            {
                ComponentName = "Test",
                Script = "function Test() { \n" +
                $"return {n1};\n" +
                " }"
            }).Build();

            var jsExecutor = serviceCollection.BuildServiceProvider().GetRequiredService<JsExecutor>();

            var script = "var res = api.CallExternal('Test', 'Test');\n";

            script += "res = JSON.parse(res);\n";
            script += "console.log(res);\n";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);

            var logVar = resp.Logs.First().SerializedVariables.First();

            Assert.AreEqual(JsonConvert.DeserializeObject<double>(logVar.DataJson), n1);
        }
    }
}