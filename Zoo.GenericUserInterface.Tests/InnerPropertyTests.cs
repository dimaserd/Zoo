using NUnit.Framework;
using System;
using System.Linq;
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
            var builder = new GenericUserInterfaceModelBuilder<SomeType>(TestsHelper.CreateDefaultBag());

            var result = builder.Result.Interface;

            var block0 = result.Blocks[0];

            Assert.AreEqual(block0.PropertyName, nameof(SomeType.Property1));
            Assert.AreEqual(block0.InterfaceType, UserInterfaceType.TextBox);

            var block1 = result.Blocks[1];

            var innerInterface = block1.InnerGenericInterface;

            Assert.AreEqual(nameof(SomeType.CreatedOn), innerInterface.Prefix);
            Assert.AreEqual(UserInterfaceType.GenericInterfaceForClass, block1.InterfaceType);
            Assert.AreEqual(block1.PropertyName, nameof(SomeType.CreatedOn));

            var innerInterfaceBlock1 = innerInterface.Blocks.First();

            Assert.AreEqual(innerInterfaceBlock1.PropertyName, nameof(SomeType.CreatedOn.Min));
            Assert.AreEqual(innerInterfaceBlock1.InterfaceType, UserInterfaceType.DatePicker);

            var innerInterfaceBlock2 = innerInterface.Blocks.Last();

            Assert.AreEqual(innerInterfaceBlock2.PropertyName, nameof(SomeType.CreatedOn.Max));
            Assert.AreEqual(innerInterfaceBlock2.InterfaceType, UserInterfaceType.DatePicker);
        }
    }
}