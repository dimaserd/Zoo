using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Services;
using Zoo.GenericUserInterface.Services.Definition;

namespace Zoo.GenericUserInterface.Tests.AutoComplete
{
    public class AutoCompleteForSingleTests
    {
        public class SomeType
        {
            public int Name { get; set; }
        }

        public class SomeTypeUserInterfaceOverrider : UserInterfaceDefinition<SomeType>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeType> overrider)
            {
                overrider.GetBlockBuilder(x => x.Name)
                    .SetAutoCompleteFor<SomeDataProvider>();

                return Task.CompletedTask;
            }
        }

        private GenericUserInterfaceBag GetAndBuildBag()
        {
            var serviceCollection = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(serviceCollection)
                .AddDataProviderForAutoCompletion<SomeDataProvider>()
                .AddDefaultDefinition<SomeTypeUserInterfaceOverrider>()
                .Build();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider.GetRequiredService<GenericUserInterfaceBag>();
        }

        [Test]
        public async Task Test()
        {
            var bag = GetAndBuildBag();

            var userInterface = await bag.GetDefaultInterface<SomeType>();

            Assert.AreEqual(1, userInterface.Interface.Blocks.Count);

            var fBlock = userInterface.Interface.Blocks.First();

            Assert.AreEqual(UserInterfaceType.AutoCompleteForSingle, fBlock.InterfaceType);

            var providerTypeFullName = typeof(SomeDataProvider).FullName;
            Assert.AreEqual(providerTypeFullName, fBlock.AutoCompleteData.DataProviderTypeFullName);

            var data = await bag.CallAutoCompleteDataProvider("", providerTypeFullName);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0);
        }
    }
}