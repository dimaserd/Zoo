using NUnit.Framework;
using System;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class GenericRange<TStruct> where TStruct : struct
    {
        public TStruct? Min { get; set; }

        public TStruct? Max { get; set; }
    }

    public class SomeType
    {
        public string Property1 { get; set; }

        public GenericRange<DateTime> CreatedOn { get; set; }
    }

    [TestFixture]
    public class InnerPropertyTests
    {
        [Test]
        public void Test()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeType>("prefix");

            var result = builder.Result;

            var block0 = result.Blocks[0];

            Assert.AreEqual(block0.PropertyName, nameof(SomeType.Property1));
            Assert.AreEqual(block0.InterfaceType, UserInterfaceType.TextBox);

            var block1 = result.Blocks[1];

            Assert.AreEqual(block1.PropertyName, $"{nameof(SomeType.CreatedOn)}.{nameof(SomeType.CreatedOn.Min)}");
            Assert.AreEqual(block1.InterfaceType, UserInterfaceType.DatePicker);

            var block2 = result.Blocks[2];

            Assert.AreEqual(block2.PropertyName, $"{nameof(SomeType.CreatedOn)}.{nameof(SomeType.CreatedOn.Max)}");
            Assert.AreEqual(block2.InterfaceType, UserInterfaceType.DatePicker);
        }
    }
}
