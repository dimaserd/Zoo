using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models.Bag;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Models.Providers;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    

    public class DropDownDataProviderTests
    {
        public class SomeModel
        {
            public int Prop1 { get; set; }
        }

        public class SomeSelectListDataProvider : DataProviderForSelectList<int>
        {

            public static SelectListItemData<int>[] Items = new SelectListItemData<int>[]
            {
                new SelectListItemData<int>
                {
                    Text = "SomeText",
                    Value = 2,
                    Selected = true
                }
            };

            public override Task<SelectListItemData<int>[]> GetData()
            {
                return Task.FromResult(Items);
            }
        }

        public class SomeModelInterfaceOverrider : GenericInterfaceOverrider<SomeModel>
        {
            public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeModel> overrider)
            {
                overrider.GetBlockBuilder(x => x.Prop1)
                    .SetDropDownList<SomeSelectListDataProvider>();

                return Task.CompletedTask;
            }
        }

        [Test]
        public async Task Test()
        {
            var srvCollection = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(srvCollection)
                .AddDataProviderForSelectList<SomeSelectListDataProvider>()
                .AddOverrider<SomeModelInterfaceOverrider>()
                .Build();

            var bag = srvCollection.BuildServiceProvider()
                .GetRequiredService<GenericUserInterfaceBag>();

            var interfaceModel = await bag.GetInterface<SomeModel>();

            Assert.AreEqual(1, interfaceModel.Interface.Blocks.Count);

            var fBlock = interfaceModel.Interface.Blocks.First();

            Assert.AreEqual(UserInterfaceType.DropDownList, fBlock.InterfaceType);

            var expectedData = SomeSelectListDataProvider.Items.Select(x => x.ToSelectListItem()).ToList();
            var expectedJson = JsonConvert.SerializeObject(expectedData);
            var resultJson = JsonConvert.SerializeObject(fBlock.DropDownData.SelectList);

            Assert.AreEqual(expectedJson, resultJson);
        }
    }
}