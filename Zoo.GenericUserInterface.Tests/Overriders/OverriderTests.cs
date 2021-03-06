﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Definition;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;
using Zoo.GenericUserInterface.Services.Definition;

namespace Zoo.GenericUserInterface.Tests.Overriders
{
    public class SomeTypeToOverride
    {
        public string SomeProperty { get; set; }
    }

    public class SomeTypeOverrider : UserInterfaceDefinition<SomeTypeToOverride>
    {
        string LabelText { get; }

        public SomeTypeOverrider(string labelText)
        {
            LabelText = labelText;
        }

        public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<SomeTypeToOverride> builder)
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
                new GenericUserInterfaceBagBuilder(new ServiceCollection())
                    .AddDefaultDefinition<SomeTypeOverrider>()
                    .AddDefaultDefinition<SomeTypeOverrider>();
            });

            var expectedMes = string.Format(ExceptionTexts.OverridingForTypeIsAlreadySetFormat, typeof(SomeTypeToOverride).FullName);

            Assert.AreEqual(expectedMes, ex.Message);
        }

        [TestCase("someLabel1")]
        [TestCase("someLabel2")]
        [TestCase("someLabel3")]
        public async Task Test(string labelText)
        {
            var srv = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(srv)
                .AddDefaultDefinition(srv => new SomeTypeOverrider(labelText))
                .Build();

            var provider = srv.BuildServiceProvider();

            var interfaceCollection = provider.GetRequiredService<GenericUserInterfaceBag>();

            var descr = await interfaceCollection.GetDefaultInterface<SomeTypeToOverride>();

            var descrFBlock = descr.Interface.Blocks.First(x => x.PropertyName == nameof(SomeTypeToOverride.SomeProperty));

            //Свойство изменилось на то, которое указано в переопределении
            Assert.IsTrue(descrFBlock.InterfaceType == Enumerations.UserInterfaceType.DropDownList);
            Assert.AreEqual(labelText, descrFBlock.LabelText);
        }
    }
}