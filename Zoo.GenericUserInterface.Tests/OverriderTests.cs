using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class SomeTypeToOverride
    {
        public string SomeProperty { get; set; }
    }

    public class SomeTypeOverrider : GenericInterfaceOverrider<SomeTypeToOverride>
    {
        public override Task OverrideInterfaceAsync(GenericUserInterfaceModelBuilder<SomeTypeToOverride> builder)
        {
            builder.DropDownListFor(x => x.SomeProperty, new List<SelectListItem>
            {
                new SelectListItem
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
        public async Task Test()
        {
            var descr = new GenericUserInterfaceModelBuilder<SomeTypeToOverride>().Result;

            var overridings = new GenericUserInterfaceOverridings().Add(new SomeTypeOverrider());

            await descr.OverrideAsync(overridings);

            var descrFBlock = descr.Interface.Blocks.First(x => x.PropertyName == nameof(SomeTypeToOverride.SomeProperty));

            //Свойство изменилось на то, которое указано в переопределении
            Assert.IsTrue(descrFBlock.InterfaceType == Enumerations.UserInterfaceType.DropDownList);
        }
    }
}