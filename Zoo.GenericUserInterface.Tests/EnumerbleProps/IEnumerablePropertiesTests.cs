using NUnit.Framework;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.EnumerbleProps
{
    public class IEnumerablePropertiesTests
    {
        public class ClassWithStringArrayProp : ClassWithArrayProp<string>
        {
        }

        [Test]
        public void TestClassWithStringArrayProp()
        {
            var result = new GenericUserInterfaceModelBuilder<ClassWithStringArrayProp>().Result;

            var block = result.Interface.Blocks.First();

            Assert.AreEqual(nameof(ClassWithStringArrayProp.Property), block.LabelText);
            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, block.InterfaceType);
            Assert.AreEqual(0, block.DropDownData.SelectList.Count);
        }

        public class ClassWithIntProp : ClassWithArrayProp<int>
        {
        }

        [Test]
        public void TestClassWithIntArrayProp()
        {
            var result = new GenericUserInterfaceModelBuilder<ClassWithIntProp>().Result;

            var block = result.Interface.Blocks.First();

            Assert.AreEqual(nameof(ClassWithIntProp.Property), block.LabelText);
            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, block.InterfaceType);
            Assert.AreEqual(0, block.DropDownData.SelectList.Count);
        }

        public class ClassWithByteProp : ClassWithArrayProp<byte>
        {
        }

        [Test]
        public void TestClassWithByteArrayProp()
        {
            var result = new GenericUserInterfaceModelBuilder<ClassWithByteProp>().Result;

            var block = result.Interface.Blocks.First();

            Assert.AreEqual(nameof(ClassWithByteProp.Property), block.LabelText);
            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, block.InterfaceType);
            Assert.AreEqual(0, block.DropDownData.SelectList.Count);
        }

        public class SomeComplexType
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class ClassWithComplexArrayProp : ClassWithArrayProp<SomeComplexType>
        {
        }

        [Test]
        public void TestClassOnComplexArrayProp()
        {
            var result = new GenericUserInterfaceModelBuilder<ClassWithComplexArrayProp>().Result;

            Assert.AreEqual(1, result.Interface.Blocks.Count);

            var block = result.Interface.Blocks.First();

            Assert.AreEqual(nameof(ClassWithComplexArrayProp.Property), block.PropertyName);
            Assert.AreEqual(UserInterfaceType.GenericInterfaceForArray, block.InterfaceType);

            var innerInterface = block.InnerGenericInterface;

            Assert.AreEqual(nameof(ClassWithComplexArrayProp.Property), innerInterface.Prefix);

            Assert.AreEqual(2, innerInterface.Blocks.Count);

            var firstBlock = innerInterface.Blocks.First();

            Assert.AreEqual(nameof(SomeComplexType.Name), firstBlock.PropertyName);

            var lastBlock = innerInterface.Blocks.Last();

            Assert.AreEqual(nameof(SomeComplexType.Description), lastBlock.PropertyName);
        }
    }
}