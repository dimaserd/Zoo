using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
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

            Assert.IsTrue(result.IsSucceeded);
            Assert.AreEqual(1, result.ConsoleLogs.Count);

            var log = result.ConsoleLogs.First();
            Assert.AreEqual(1, log.SerializedVariables.Count);
            
            var variable = log.SerializedVariables.First();
            Assert.AreEqual("null", variable.DataJson);
            Assert.IsNull(variable.TypeFullName);
        }
    }
}