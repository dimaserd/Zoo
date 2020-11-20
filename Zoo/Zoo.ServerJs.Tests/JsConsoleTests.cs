using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.Tests
{
    public class JsConsoleTests
    {
        [Test]
        public async Task ConsoleLogNull()
        {
            var services = new ServiceCollection();
            new JsExecutorBuilder(services)
                .Build();

            var srvProvider = services.BuildServiceProvider();

            var jsExecutor = srvProvider.GetRequiredService<JsExecutor>();

            var result = await jsExecutor.RunScriptDetaiiled("console.log(null)");

            AssertResult(result);
        }

        [Test]
        public async Task ConsoleLogNullFromService()
        {
            var services = new ServiceCollection();
            new JsExecutorBuilder(services)
                .AddJsWorker(builder => 
                {
                    builder.SetWorkerName("Test")
                        .SetDescription("Test")
                        .AddMethodViaFunction(FuncWithNull, new JsWorkerMethodDocsOptions
                        {
                            MethodName = "NullFunc",
                            Description = "Some description"
                        });

                    return builder.Build();
                })
                .Build();

            var srvProvider = services.BuildServiceProvider();

            var jsExecutor = srvProvider.GetRequiredService<JsExecutor>();

            var script = "var res = api.Call('Test', 'NullFunc');\n";
            script += "console.log(res);";

            var result = await jsExecutor.RunScriptDetaiiled(script);

            AssertResult(result);
        }

        private static void AssertResult(JsScriptExecutedResult result)
        {
            Assert.IsTrue(result.IsSucceeded);
            Assert.AreEqual(1, result.ConsoleLogs.Count);

            var log = result.ConsoleLogs.First();
            Assert.AreEqual(1, log.SerializedVariables.Count);

            var variable = log.SerializedVariables.First();
            Assert.AreEqual("null", variable.DataJson);
            Assert.IsNull(variable.TypeFullName);
        }

        private static object FuncWithNull()
        {
            return null;
        }
    }
}