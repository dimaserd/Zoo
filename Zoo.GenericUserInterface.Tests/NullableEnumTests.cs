using NUnit.Framework;
using System.Linq;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public enum SomeEnum
    {
        Value1,
        Value2
    }

    public class Type1WithNullableEnum
    {
        public SomeEnum? Prop1 { get; set; }
    }

    [TestFixture]
    public class NullableEnumTests
    {
        [Test]
        public void NullableEnumTest()
        {
            var result = new GenericUserInterfaceModelBuilder<Type1WithNullableEnum>().Result;

            var fProp = result.Interface.Blocks.First();

            Assert.AreEqual(Enumerations.UserInterfaceType.DropDownList, fProp.InterfaceType);

            var selectList = fProp.DropDownData.SelectList;

            Assert.AreEqual(3, selectList.Count);

            var fItem = selectList.First();

            Assert.IsTrue(fItem.Selected);

            Assert.IsNull(fItem.Value);
        }
    }
}