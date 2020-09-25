using Croco.Core.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.Overriders
{
    public class SetDropListTests
    {
        [Test]
        public void Test()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>(TestsHelper.CreateDefaultBag());

            var list = new List<SelectListItemData<bool?>>
            {
                new SelectListItemData<bool?>
                {
                    Selected = false,
                    Text = "Text",
                }
            };

            builder.GetBlockBuilder(x => x.Sex).SetDropDownList(list);

            var lastProp = builder.Result.Interface.Blocks.Last();

            Assert.IsTrue(lastProp.InterfaceType == UserInterfaceType.DropDownList);
            AsserAreEqualViaJson(GenericUserInterfaceModelBuilderExtensions.ToSelectListItems(list), lastProp.DropDownData.SelectList);
        }

        private void AsserAreEqualViaJson<T>(T data1, T data2)
        {
            var json1 = Tool.JsonConverter.Serialize(data1);
            var json2 = Tool.JsonConverter.Serialize(data2);

            Assert.AreEqual(json1, json2);
        }

        [Test]
        public void ToEnumProperty_ShouldThrowException()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>(TestsHelper.CreateDefaultBag());

            var ex = Assert.Throws<InvalidOperationException>(() => builder.GetBlockBuilder(x => x.EnumProp).SetDropDownList(new List<SelectListItemData<SomeEnumType>>()));

            var expectedMessage = string.Format(ExceptionTexts.CantImplementSetDropListNameToEnumPropertyFormat, nameof(SomeClass.EnumProp));
            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}