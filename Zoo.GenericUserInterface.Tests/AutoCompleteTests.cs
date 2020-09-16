using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class SomeModelForAutoComplete
    {
        public int[] Names { get; set; }
    }

    public class SomeDataProvider : DataProviderForAutoCompletion<int>
    {
        static readonly Random _random = new Random();

        public static AutoCompleteSuggestionData<int>[] GetDataStatic()
        {
            return new int[]
            {
                GetNum(), GetNum(), GetNum(), GetNum(), GetNum()
            }.Select(x => new AutoCompleteSuggestionData<int>
            {
                Text = x.ToString(),
                Value = x
            }).ToArray();
        }

        public override Task<AutoCompleteSuggestionData<int>[]> GetData(string typedText)
        {
            return Task.FromResult(GetDataStatic());
        }

        private static int GetNum()
        {
            return _random.Next(0, 100);
        }
    }

    public class SomeModelForAutoCompleteOverrider : GenericInterfaceOverrider<SomeModelForAutoComplete>
    {   
        public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeModelForAutoComplete> overrider)
        {
            overrider.GetBlockBuilderForCollection(x => x.Names).SetAutoCompleteFor<SomeDataProvider>();

            return Task.CompletedTask;
        }
    }

    public class AutoCompleteTests
    {
        private GenericUserInterfaceBag GetAndBuildBag()
        {
            var serviceCollection = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(serviceCollection)
                .AddDataProvider<SomeDataProvider, int>()
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
