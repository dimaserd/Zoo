using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Bag;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.AutoComplete
{
    public class SettingAutoCompletionDataProviderTests
    {
        public class SomeDataProviderWithComplexData
        {

        }

        [Test]
        public void Test()
        {

        }
    }

    public class AutoCompleteForMultipleTests
    {
        public class SomeModelForAutoComplete
        {
            public int[] Names { get; set; }
        }

        public class SomeModelForAutoCompleteOverrider : UserInterfaceOverrider<SomeModelForAutoComplete>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeModelForAutoComplete> overrider)
            {
                overrider.GetBlockBuilderForCollection(x => x.Names).SetAutoCompleteFor<SomeDataProvider>();

                return Task.CompletedTask;
            }
        }

        private GenericUserInterfaceBag GetAndBuildBag()
        {
            var serviceCollection = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(serviceCollection)
                .AddDataProviderForAutoCompletion<SomeDataProvider>()
                .AddOverrider<SomeModelForAutoCompleteOverrider>()
                .Build();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider.GetRequiredService<GenericUserInterfaceBag>();
        }


        [Test]
        public async Task Test()
        {
            var bag = GetAndBuildBag();

            var userInterface = await bag.GetInterface<SomeModelForAutoComplete>();

            Assert.AreEqual(1, userInterface.Interface.Blocks.Count);

            var fBlock = userInterface.Interface.Blocks.First();

            Assert.AreEqual(UserInterfaceType.AutoCompleteForMultiple, fBlock.InterfaceType);

            var providerTypeFullName = typeof(SomeDataProvider).FullName;
            Assert.AreEqual(providerTypeFullName, fBlock.AutoCompleteData.DataProviderTypeFullName);

            var data = await bag.CallAutoCompleteDataProvider("", providerTypeFullName);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0);
        }
    }
}