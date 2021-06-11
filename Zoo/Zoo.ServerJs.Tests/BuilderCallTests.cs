using Croco.Core.Documentation.Models.Methods;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Resources;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.Tests
{
    public class BuilderCallTests
    {
        public const string MultiplyByTwoMethodName = "MultiplyByTwo";

        public static void SomeAction()
        {

        }

        public class SomeService
        {
            public Task<int> SomeTask(int p1)
            {
                return Task.FromResult(p1*2);
            }
        }

        [Test]
        public void TestWithNoRegistrationInServiceProvider_ShouldThrowException()
        {
            var serviceCollection = new ServiceCollection();
            
            var ex = Assert.Throws<InvalidOperationException>(() => new JsExecutorBuilder(serviceCollection)
                .AddJsWorker(builder => builder
                .GetServiceMethodBuilder<SomeService>()
                .Build()));

            var mes = string.Format(ExceptionTexts.TypeOfServiceNotRegisteredInServiceCollectionFormat, typeof(SomeService).FullName);
            Assert.AreEqual(mes, ex.Message);
        }

        [Test]
        public void TestWithNoNameToWorkerProvided_ShouldThrowException()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<SomeService>();

            var ex = Assert.Throws<InvalidOperationException>(() => new JsExecutorBuilder(serviceCollection)
                .AddJsWorker(builder => builder.AddMethodViaAction(SomeAction, new MethodDocsOptions
                {
                    MethodName = "sda"
                })
                .GetServiceMethodBuilder<SomeService>()
                .Build()));

            var mes = ExceptionTexts.WorkerNameIsRequired;
            Assert.AreEqual(mes, ex.Message);
        }

        [TestCase("sadsa")]
        [TestCase("sadsa1")]
        [TestCase("sadsa2")]
        public void TestWithNoMethodsToWorkerProvided_ShouldThrowException(string workerName)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<SomeService>();

            var ex = Assert.Throws<InvalidOperationException>(() => new JsExecutorBuilder(serviceCollection)
                .AddJsWorker(builder => builder
                    .SetWorkerName(workerName)
                    .GetServiceMethodBuilder<SomeService>()
                    .Build()));

            var mes = string.Format(ExceptionTexts.NoMethodsInWorkerFormat, workerName);
            Assert.AreEqual(mes, ex.Message);
        }

        public static JsExecutor BuildAndGetExecutor(string workerName)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<SomeService>();

            new JsExecutorBuilder(serviceCollection)
                .AddJsWorker(builder => builder
                .SetWorkerName(workerName)
                .AddMethodViaAction(SomeAction, new MethodDocsOptions
                {
                    MethodName = "sda"
                })
                .GetServiceMethodBuilder<SomeService>()
                .AddMethodViaTaskWithResult<int, int>((srv, p1) => srv.SomeTask(p1), new MethodDocsOptions
                {
                    MethodName = MultiplyByTwoMethodName,
                    Description = "some"
                })
                .AddMethodViaTaskWithResult((srv) => srv.SomeTask(2), new MethodDocsOptions
                {
                    MethodName = "MultiplyTwoByTwo",
                    Description = "some"
                })
                .Build())
                .Build();

            var srvProvider = serviceCollection.BuildServiceProvider();

            return srvProvider.GetRequiredService<JsExecutor>();
        }

        [TestCase("someName")]
        [TestCase("someName1")]
        [TestCase("someName2")]
        public async Task TestDocumentation(string workerName)
        {
            var jsExecutor = BuildAndGetExecutor(workerName);
            var docs = await jsExecutor.GetDocumentation();
            Assert.AreEqual(1, docs.Workers.Count);

            var fWorker = docs.Workers.First();

            Assert.AreEqual(workerName, fWorker.WorkerName);
            Assert.AreEqual(3, fWorker.Methods.Count);
        }
    }
}