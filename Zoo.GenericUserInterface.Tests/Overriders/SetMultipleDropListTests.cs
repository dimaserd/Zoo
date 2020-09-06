using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.Overriders
{
    public class SetMultipleDropListTests
    {
        internal class SomeType
        {
            public string Prop1 { get; set; }
            public List<int[]> ComplexCollectionType { get; set; }
            public List<string> Prop2 { get; set; }
            public int[] Prop3 { get; set; }
            public IEnumerable<string> Prop4 { get; set; }
        }

        internal class SomeType2
        {
            public string Prop1 { get; set; }
        }

        [Test]
        public void TestImplementingMultipleDropDownForToNotEnumerableProperty()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeType>();

            var ex = Assert.Throws<InvalidOperationException>(() => builder.GetBlockBuilderForCollection(x => x.Prop1));

            Assert.AreEqual(ex.Message, ExceptionTexts.DontUseGetBlockBuilderForCollectionOnCollectionsOfChars);
        }

        [Test]
        public void CollectionOfComplexObjects_ShouldFail()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => 
            {
                new GenericUserInterfaceModelBuilder<SomeType>()
                    .GetBlockBuilderForCollection(x => x.ComplexCollectionType)
                    .SetMultipleDropDownList(new List<SelectListItemData<int[]>>());
            });

            var mes = string.Format(ExceptionTexts.CantSetMultipleDropListNotOnPrimitivesFormat, nameof(SomeType.ComplexCollectionType), typeof(int[]).FullName);

            Assert.AreEqual(mes, ex.Message);
        }

        [Test]
        public void SetOnListOfPrimitives_ShouldBeOk()
        {
            var propName = nameof(SomeType.Prop2);
            string defaultValue = "someValue";

            var interfaceModel = new GenericUserInterfaceModelBuilder<SomeType>()
                    .GetBlockBuilderForCollection(x => x.Prop2)
                    .SetMultipleDropDownList(new List<SelectListItemData<string>>
                    {
                        new SelectListItemData<string>
                        {
                            Selected = true,
                            Text = "SomeText",
                            Value = defaultValue
                        }
                    }).Result;

            var block = interfaceModel.Interface.Blocks.First(x => x.PropertyName == propName);

            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, block.InterfaceType);
            Assert.AreEqual(1, block.DropDownData.SelectList.Count);
        }

        [Test]
        public void SetOnArrayOfPrimitives_ShouldBeOk()
        {
            var propName = nameof(SomeType.Prop3);
            int defaultValue = 0;

            var interfaceModel = new GenericUserInterfaceModelBuilder<SomeType>()
                    .GetBlockBuilderForCollection(x => x.Prop3)
                    .SetMultipleDropDownList(new List<SelectListItemData<int>>
                    {
                        new SelectListItemData<int>
                        {
                            Selected = true,
                            Text = "SomeText",
                            Value = defaultValue
                        }
                    }).Result;

            var block = interfaceModel.Interface.Blocks.First(x => x.PropertyName == propName);

            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, block.InterfaceType);
            Assert.AreEqual(1, block.DropDownData.SelectList.Count);
        }

        [Test]
        public void SetOnEnumerableOfPrimitives_ShouldBeOk()
        {
            var propName = nameof(SomeType.Prop4);
            var defaultValue = "";

            var interfaceModel = new GenericUserInterfaceModelBuilder<SomeType>()
                    .GetBlockBuilderForCollection(x => x.Prop4)
                    .SetMultipleDropDownList(new List<SelectListItemData<string>>
                    {
                        new SelectListItemData<string>
                        {
                            Selected = true,
                            Text = "SomeText",
                            Value = defaultValue
                        }
                    }).Result;

            var block = interfaceModel.Interface.Blocks.First(x => x.PropertyName == propName);

            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, block.InterfaceType);
            Assert.AreEqual(1, block.DropDownData.SelectList.Count);
        }

    }
}