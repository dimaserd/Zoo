using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.Overriders
{
    public class SomeTypeToOverride
    {
        public string SomeProperty { get; set; }
    }

    public class SomeTypeOverrider : GenericInterfaceOverrider<SomeTypeToOverride>
    {
        string LabelText { get; }

        public SomeTypeOverrider(string labelText)
        {
            LabelText = labelText;
        }

        public override Task OverrideInterfaceAsync(GenericUserInterfaceModelBuilder<SomeTypeToOverride> builder)
        {
            builder.GetBlockBuilder(x => x.SomeProperty).SetLabel(LabelText).SetDropDownList(new List<SelectListItemData<string>>
            {
                new SelectListItemData<string>
                {
                    Text = "Text",
                    Selected = true,
                    Value = "SomeValue"
                }
            });

            return Task.CompletedTask;
        }
    }

    [TestFixture]
    public class OverriderTests
    {
        [Test]
        public void MultipleOverridesForTypes_ShouldThrowException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                new GenericUserInterfaceCollection()
                    .AddOverrider(new SomeTypeOverrider(""))
                    .AddOverrider(new SomeTypeOverrider(""));
            });

            var expectedMes = string.Format(ExceptionTexts.OverridingForTypeIsAlreadySetFormat, typeof(SomeTypeToOverride).FullName);

            Assert.AreEqual(expectedMes, ex.Message);
        }

        [TestCase("someLabel1")]
        [TestCase("someLabel2")]
        [TestCase("someLabel3")]
        public async Task Test(string labelText)
        {
            var interfaceCollection = new GenericUserInterfaceCollection().AddOverrider(new SomeTypeOverrider(labelText));

            var descr = await interfaceCollection.GetInterface<SomeTypeToOverride>();

            var descrFBlock = descr.Interface.Blocks.First(x => x.PropertyName == nameof(SomeTypeToOverride.SomeProperty));

            //Свойство изменилось на то, которое указано в переопределении
            Assert.IsTrue(descrFBlock.InterfaceType == Enumerations.UserInterfaceType.DropDownList);
            Assert.AreEqual(labelText, descrFBlock.LabelText);
        }
    }
}