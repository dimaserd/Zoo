using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.Tests
{
    public class ApiCallComponentTests
    {
        [Test]
        public void Test()
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

            var arg1 = 5;

            var arg2 = 6;

            var script = $"var res = api.CallExternal('Test', 'Calculator', {{ 'Arg1': {arg1}, 'Arg2': {arg2} }}); \n";

            script += "console.log(res)";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);
            var logValue = resp.Logs.First().SerializedVariables.First();

            Assert.AreEqual($"\"{arg1 + arg2}.0\"", logValue.DataJson);
        }

        [Test]
        public void CallAfterCall()
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
                        "var s = JSON.parse( api.CallExternal('Test2', 'CalculatorNew', { 'Arg1': t, 'Arg2': 4 }) );\n" +
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

            var script = "var res = api.CallExternal('Test', 'Calculator', { 'Arg1': 6, 'Arg2': 6 });\n";

            script += "res = JSON.parse(res);\n";
            script += "console.log(res);\n";

            var result = jsExecutor.RunScriptDetaiiled(script);

            Assert.IsTrue(result.IsSucceeded);

            var resp = result.ResponseObject;

            Assert.IsTrue(resp.Logs.Count == 1);

            var logVar = resp.Logs.First().SerializedVariables.First();

            //(6 + 6) / 4
            Assert.AreEqual(logVar.DataJson, "3.0");
        }
    }
}