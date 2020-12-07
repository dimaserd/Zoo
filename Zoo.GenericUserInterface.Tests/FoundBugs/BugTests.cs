using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Models.Providers;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.FoundBugs
{
    public class TestClass
    {
        [Display(Name = "Идентификатор")]
        public string Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "ТестАреа")]
        public string SomeTextArea { get; set; }

        [Display(Name = "Какая-то дата")]
        public DateTime SomeDateTime { get; set; }

        [Display(Name = "Описания")]
        public string[] Descriptions { get; set; }

        [Display(Name = "Свойство для автокомплита мульти")]
        public int[] AutoCompleteProp1 { get; set; }

        [Display(Name = "Свойство для автокомплита сингл")]
        public int AutoCompleteProp2 { get; set; }

        [Display(Name = "Список типов")]
        public List<TestClassSubType> Types { get; set; }

        [Display(Name = "Вложенный тип")]
        public TestClassSubType MainType { get; set; }
    }

    public class TestClassSubType
    {
        [Display(Name = "Свойство 1")]
        public string Property1 { get; set; }

        [Display(Name = "Свойство 2")]
        public string Property2 { get; set; }

        [Display(Name = "Еще вложенное свойство")]
        public TestClassSubTypeSubType Property3 { get; set; }
    }

    public class TestClassSubTypeSubType
    {
        [Display(Name = "Свойство 1")]
        public string Property1 { get; set; }

        [Display(Name = "Описания")]
        public string[] Descriptions { get; set; }
    }

    public class TestClassInterfaceOverrider : UserInterfaceOverrider<TestClass>
    {
        public override Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<TestClass> overrider)
        {
            overrider.GetBlockBuilderForCollection(x => x.Descriptions)
                .SetMultipleDropDownList(new List<SelectListItemData<string>>
            {
                new SelectListItemData<string>
                {
                    Selected = true,
                    Text = "Описание 1",
                    Value = "Value 1"
                },
                new SelectListItemData<string>
                {
                    Selected = false,
                    Text = "Описание 2",
                    Value = "Value 2"
                },
                new SelectListItemData<string>
                {
                    Selected = true,
                    Text = "Описание 3",
                    Value = "Value 3"
                },
                new SelectListItemData<string>
                {
                    Selected = false,
                    Text = "Описание 4",
                    Value = "Value 4"
                },
                new SelectListItemData<string>
                {
                    Selected = false,
                    Text = "Описание 5",
                    Value = "Value 5"
                }
            });
            overrider.GetBlockBuilder(x => x.Name).SetDropDownList(new List<SelectListItemData<string>>
            {
                new SelectListItemData<string>
                {
                    Value = "someValue1",
                    Selected = true,
                    Text = "someText1",
                },
                new SelectListItemData<string>
                {
                    Value = "someValue2",
                    Selected = false,
                    Text = "someText2"
                }
            });

            overrider.GetBlockBuilder(x => x.SomeTextArea).SetTextArea();

            overrider
                .GetBlockBuilderForCollection(x => x.AutoCompleteProp1)
                .SetAutoCompleteFor<TestDataProvider>();

            overrider.GetBlockBuilder(x => x.AutoCompleteProp2)
                .SetAutoCompleteFor<TestDataProvider>();

            return Task.CompletedTask;
        }
    }

    public class TestDataProvider : DataProviderForAutoCompletion<int>
    {
        public override Task<AutoCompleteSuggestionData<int>[]> GetData(string typedText)
        {
            var result = Enumerable.Range(1, 5).Select(x => new AutoCompleteSuggestionData<int>
            {
                Text = $"{typedText}{x}",
                Value = x
            }).ToArray();

            return Task.FromResult(result);
        }
    }

    public class BugTests
    {
        [Test]
        public async Task Test()
        {
            var services = new ServiceCollection();

            new GenericUserInterfaceBagBuilder(services)
                .AddDataProviderForAutoCompletion<TestDataProvider>()
                .AddDefaultOverrider<TestClassInterfaceOverrider>()
                .Build();

            var bag = services.BuildServiceProvider().GetRequiredService<GenericUserInterfaceBag>();

            await bag.GetDefaultInterface<TestClass>();
        }
    }
}