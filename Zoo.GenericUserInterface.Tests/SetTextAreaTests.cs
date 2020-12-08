using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class SetTextAreaTests
    {
        public class SomeType
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
        }


        public class GoodOverrider : UserInterfaceDefinition<SomeType>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeType> overrider)
            {
                overrider.GetBlockBuilder(x => x.Prop2).SetTextArea();

                return Task.CompletedTask;
            }
        }

        [Test]
        public async Task SetOnStringPropShouldSucceded()
        {
            var services = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(services)
                .AddDefaultDefinition<GoodOverrider>()
                .Build();

            var bag = services.BuildServiceProvider().GetRequiredService<GenericUserInterfaceBag>();

            var interfaceModel = await bag.GetDefaultInterface<SomeType>();

            var blocks = interfaceModel.Interface.Blocks;

            Assert.AreEqual(2, blocks.Count);

            var lastBlock = blocks.Last();

            Assert.AreEqual(lastBlock.PropertyName, nameof(SomeType.Prop2));
            Assert.AreEqual(UserInterfaceType.TextArea, lastBlock.InterfaceType);
        }
    }
}
