using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Models.Providers;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.Overriders
{
    public class SetMultipleDropListViaDataProviderTest
    {
        public class SomeType
        {
            public int[] Prop1 { get; set; }
        }

        public class SomeDataProviderForSelectList : DataProviderForSelectList<int>
        {
            public static readonly SelectListItemData<int>[] Data = new[]
            {
                new SelectListItemData<int>
                {
                    Text = "1",
                    Value = 1,
                    Selected = false
                },
                new SelectListItemData<int>
                {
                    Text = "2",
                    Value = 12,
                    Selected = true
                }
            };

            public override Task<SelectListItemData<int>[]> GetData()
            {
                return Task.FromResult(Data);
            }
        }

        public class SomeTypeInterfaceOverrider : UserInterfaceDefinition<SomeType>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeType> overrider)
            {
                overrider.GetBlockBuilderForCollection(x => x.Prop1)
                    .SetMultipleDropDownList<SomeDataProviderForSelectList>();

                return Task.CompletedTask;
            }
        }

        [Test]
        public async Task Test()
        {
            var serviceCollection = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(serviceCollection)
                .AddDataProviderForSelectList<SomeDataProviderForSelectList>()
                .AddDefaultDefinition<SomeTypeInterfaceOverrider>()
                .Build();

            var bag = serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<GenericUserInterfaceBag>();

            var interfaceModel = await bag.GetDefaultInterface<SomeType>();

            var fBlock = interfaceModel.Interface.Blocks.First();

            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, fBlock.InterfaceType);
            Assert.AreEqual(typeof(SomeDataProviderForSelectList).FullName, fBlock.DropDownData.DataProviderTypeFullName);

            var expectedData = SomeDataProviderForSelectList.Data
                .Select(GenericUserInterfaceModelBuilderExtensions.ToSelectListItem)
                .ToList();

            TestsHelper.AssertAreEqualViaJson(expectedData, fBlock.DropDownData.SelectList);
        }
    }
}
