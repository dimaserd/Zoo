using NUnit.Framework;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Services;
using Zoo.GenericUserInterface.Utils;

namespace Zoo.GenericUserInterface.Tests
{
    public class TypeWithInteger
    {
        public int Prop1 { get; set; }
    }


    [TestFixture]
    public class TextBoxDataTests
    {
        [Test]
        public void Test()
        {
            var interfaceModel = new GenericUserInterfaceModelBuilder<TypeWithInteger>(TestsHelper.CreateDefaultBag()).Result;

            var prop1Block = interfaceModel.Interface.Blocks.First();

            Assert.AreEqual(UserInterfaceType.NumberBox, prop1Block.InterfaceType);

            var textBoxData = prop1Block.NumberBoxData;

            Assert.IsNotNull(textBoxData);
            Assert.IsTrue(textBoxData.IsInteger);
            Assert.AreEqual(Tool.JsonConverter.Serialize(int.MinValue), textBoxData.MinValue);
            Assert.AreEqual(Tool.JsonConverter.Serialize(int.MaxValue), textBoxData.MaxValue);
        }
    }
}