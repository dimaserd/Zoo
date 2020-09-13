using NUnit.Framework;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Services;

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
            var interfaceModel = new GenericUserInterfaceModelBuilder<TypeWithInteger>(GenericUserInterfaceBag.CreateDefault()).Result;

            var prop1Block = interfaceModel.Interface.Blocks.First();

            Assert.AreEqual(UserInterfaceType.TextBox, prop1Block.InterfaceType);

            var textBoxData = prop1Block.TextBoxData;

            Assert.IsNotNull(textBoxData);
            Assert.IsTrue(textBoxData.IsInteger);
            Assert.AreEqual(1, textBoxData.IntStep);
        }
    }
}