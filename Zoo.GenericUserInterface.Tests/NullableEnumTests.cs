using NUnit.Framework;
using System.Linq;
using Zoo.GenericUserInterface.Models;
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
            var result = new GenericUserInterfaceModelBuilder<Type1WithNullableEnum>("someprefix").Result;

            var fProp = result.Blocks.First();

            Assert.IsTrue(fProp.InterfaceType == Enumerations.UserInterfaceType.DropDownList);

            var selectList = fProp.SelectList;

            Assert.IsTrue(selectList.Count == 3);

            var fItem = selectList.First();

            Assert.IsTrue(fItem.Selected);

            Assert.IsNull(fItem.Value);
        }
    }
}