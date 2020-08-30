using NUnit.Framework;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class Type1
    {
        public bool? Prop1 { get; set; }
    }

    public class Type2
    {
        public bool Prop1 { get; set; }
    }

    [TestFixture]
    public class NullableBoolTests
    {
        [Test]
        public void NullableBoolTest()
        {
            var builder = new GenericUserInterfaceModelBuilder<Type1>();

            var result = builder.Result.Interface;

            Assert.AreEqual(1, result.Blocks.Count);
            
            var block = result.Blocks.First();

            Assert.AreEqual(UserInterfaceType.DropDownList, block.InterfaceType);

            var selectList = block.SelectList;
            Assert.IsTrue(selectList.Count == 3);

            var firstSelectItem = selectList.First();

            Assert.IsTrue(firstSelectItem.Selected);
            Assert.IsNull(firstSelectItem.Value);
        }

        [Test]
        public void OrdinaryBoolTest()
        {
            var builder = new GenericUserInterfaceModelBuilder<Type2>();

            var result = builder.Result.Interface;

            Assert.AreEqual(1, result.Blocks.Count);

            var block = result.Blocks.First();

            Assert.AreEqual(UserInterfaceType.DropDownList, block.InterfaceType);


            var selectList = block.SelectList;

            //Так как жто не Nullable null быть не должно
            Assert.IsFalse(selectList.Any(x => x.Value == null));

            Assert.IsTrue(selectList.Count == 2);

            var firstSelectItem = selectList.First();
            Assert.IsTrue(firstSelectItem.Selected);
            Assert.IsFalse(bool.Parse(firstSelectItem.Value));
        }
    }
}
