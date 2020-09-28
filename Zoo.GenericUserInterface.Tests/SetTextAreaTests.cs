using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Bag;
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

        public class FailingOverrider : UserInterfaceOverrider<SomeType>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeType> overrider)
            {
                overrider.GetBlockBuilder(x => x.Prop1).SetTextArea();

                return Task.CompletedTask;
            }
        }

        public class GoodOverrider : UserInterfaceOverrider<SomeType>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeType> overrider)
            {
                overrider.GetBlockBuilder(x => x.Prop2).SetTextArea();

                return Task.CompletedTask;
            }
        }

        [Test]
        public void SetOnNonStringPropShouldFail()
        {
            var services = new ServiceCollection();
            
            new GenericUserInterfaceBagBuilder(services)
                .AddOverrider<FailingOverrider>()
                .Build();

            var bag = services.BuildServiceProvider().GetRequiredService<GenericUserInterfaceBag>();

            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => bag.GetInterface<SomeType>());

            Assert.AreEqual(ExceptionTexts.TextAreaCanBeSetOnlyOnStrings, ex.Message);
        }

        [Test]
        public async Task SetOnStringPropShouldSucceded()
        {
            var services = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(services)
                .AddOverrider<GoodOverrider>()
                .Build();

            var bag = services.BuildServiceProvider().GetRequiredService<GenericUserInterfaceBag>();

            var interfaceModel = await bag.GetInterface<SomeType>();

            var blocks = interfaceModel.Interface.Blocks;

            Assert.AreEqual(2, blocks.Count);

            var lastBlock = blocks.Last();

            Assert.AreEqual(lastBlock.PropertyName, nameof(SomeType.Prop2));
            Assert.AreEqual(UserInterfaceType.TextArea, lastBlock.InterfaceType);
        }
    }
}
