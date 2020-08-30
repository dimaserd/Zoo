using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class ModelWithLabel
    {
        [Display(Name = LabelTests.SomeValue)]
        public string Property { get; set; }
    }

    [TestFixture]
    public class LabelTests
    {
        public const string SomeValue = "SomeValue";

        [Test]
        public void Test()
        {
            var descr = new GenericUserInterfaceModelBuilder<ModelWithLabel>().Result;

            Assert.AreEqual(SomeValue, descr.Interface.Blocks.First().LabelText);
        }
    }
}