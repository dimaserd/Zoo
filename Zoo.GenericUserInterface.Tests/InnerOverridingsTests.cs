using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Definition;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class InnerOverridingsTests
    {
        public class SomeType
        {
            public string Prop1 { get; set; }

            public SomeInnerType Prop2 { get; set; }
        }

        public class SomeInnerType
        {
            public string Prop1 { get; set; }
        }

        public class SomeInnerTypeInterfaceOverrider : UserInterfaceDefinition<SomeInnerType>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeInnerType> overrider)
            {
                overrider.GetBlockBuilder(x => x.Prop1)
                    .SetDropDownList(new List<SelectListItemData<string>>
                    {
                        new SelectListItemData<string>
                        {
                            Selected = true,
                            Text = "Text",
                            Value = "Value"
                        }
                    });

                return Task.CompletedTask;
            }
        }

        [Test]
        public async Task Test()
        {
            var serviceCollection = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(serviceCollection)
                .AddDefaultDefinition<SomeInnerTypeInterfaceOverrider>()
                .Build();
                
            var bag = serviceCollection.BuildServiceProvider()
                .GetRequiredService<GenericUserInterfaceBag>();

            var interfaceModel = await bag.GetDefaultInterface<SomeType>();

            var blocks = interfaceModel.Interface.Blocks;

            Assert.AreEqual(2, blocks.Count);

            var fBlock = blocks.First();

            Assert.AreEqual(nameof(SomeType.Prop1), fBlock.PropertyName);

            var lastBlock = blocks.Last();

            Assert.AreEqual(nameof(SomeType.Prop2), lastBlock.PropertyName);

            var innerBlock = lastBlock.InnerGenericInterface.Blocks.First();

            Assert.AreEqual(nameof(SomeInnerType.Prop1), innerBlock.PropertyName);
            Assert.AreEqual(UserInterfaceType.DropDownList, innerBlock.InterfaceType);
            Assert.AreEqual(1, innerBlock.DropDownData.SelectList.Count);
        }
    }
}