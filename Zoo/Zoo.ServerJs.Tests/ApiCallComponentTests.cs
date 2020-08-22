using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
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
            var jsExecutor = new JsExecutor(new JsExecutorProperties
            {
                ExternalComponents = new List<ExternalJsComponent>
                {
                    new ExternalJsComponent
                    {
                        ComponentName = "Test",
                        Script = "function Calculator(model) { \n" +
                        "return model.Arg1 + model.Arg2; \n }"
                    }
                }
            });

            var script = $"var res = api.CallExternal('Test', 'Calculator', {{ 'Arg1': {arg1}, 'Arg2': {arg2} }}); \n";

            script += "console.log(res)";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);
            var logValue = resp.Logs.First().SerializedVariables.First();

            Assert.AreEqual($"\"{arg1 + arg2}.0\"", logValue.DataJson);
        }

        [TestCase(6, 6)]
        public void CallAfterCall(double arg1, double arg2, double delimeter)
        {
            var jsExecutor = new JsExecutor(new JsExecutorProperties
            {
                ExternalComponents = new List<ExternalJsComponent>
                {
                    new ExternalJsComponent
                    {
                        ComponentName = "Test",
                        Script = "function Calculator(model) { \n" +
                        "var t = model.Arg1 + model.Arg2;\n" +
                        "var s = JSON.parse( api.CallExternal('Test2', 'CalculatorNew', { 'Arg1': t, 'Arg2': " + $"{delimeter}" + " }) );\n" +
                        "return s;\n" +
                        " }"
                    },

                    new ExternalJsComponent
                    {
                        ComponentName = "Test2",
                        Script = "function CalculatorNew(model) { \n" +
                        "return model.Arg1 / model.Arg2; }"
                    }
                }
            });

            var script = "var res = api.CallExternal('Test', 'Calculator', { 'Arg1': " + $"{arg1}, 'Arg2': {arg2}" + " });\n";

            script += "res = JSON.parse(res);\n";
            script += "console.log(res);\n";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);

            var logVar = resp.Logs.First().SerializedVariables.First();


            var exprexctedValue = (arg1 + arg2) / delimeter;
            
            //(6 + 6) / 4
            Assert.AreEqual(exprexctedValue, JsonConvert.DeserializeObject<double>(logVar.DataJson));
        }

        [Test]
        public void CallWithNoArgs()
        {
            var jsExecutor = new JsExecutor(new JsExecutorProperties
            {
                ExternalComponents = new List<ExternalJsComponent>
                {
                    new ExternalJsComponent
                    {
                        ComponentName = "Test",
                        Script = "function Test() { \n" +
                        "return 5;\n" +
                        " }"
                    }
                }
            });

            var script = "var res = api.CallExternal('Test', 'Test');\n";

            script += "res = JSON.parse(res);\n";
            script += "console.log(res);\n";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);

            var logVar = resp.Logs.First().SerializedVariables.First();

            //(6 + 6) / 4
            Assert.AreEqual(logVar.DataJson, "5.0");
        }
    }
}