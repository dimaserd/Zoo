using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Zoo.ServerJs.Resources;

namespace Zoo.ServerJs.Tests
{
    public class CallAndParseTests
    {
        [TestCase("someName", 3)]
        [TestCase("someName1", 4)]
        [TestCase("someName2", 5)]
        public async Task TestMethodCall_ShouldReturnRightResult(string workerName, int a)
        {
            var jsExecutor = BuilderCallTests.BuildAndGetExecutor(workerName);

            var expectedRes = await new BuilderCallTests.SomeService().SomeTask(a);
            var res = jsExecutor.Call<int>(workerName, BuilderCallTests.MultiplyByTwoMethodName, a);
            Assert.AreEqual(expectedRes, res);
        }


        [TestCase("someName")]
        [TestCase("someName1")]
        [TestCase("someName2")]
        public void TestMethodCall_WithMissingParameters_ShouldThrow(string workerName)
        {
            var jsExecutor = BuilderCallTests.BuildAndGetExecutor(workerName);

            var ex = Assert.Throws<InvalidOperationException>(() => jsExecutor.Call<int>(workerName,
                    BuilderCallTests.MultiplyByTwoMethodName));

            var mes = string.Format(ExceptionTexts.MethodWasCalledWithLessParamsFormat,
                    BuilderCallTests.MultiplyByTwoMethodName, workerName, 1, 0);

            Assert.AreEqual(mes, ex.Message);
        }
    }
}