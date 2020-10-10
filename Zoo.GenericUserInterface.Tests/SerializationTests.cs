﻿using NUnit.Framework;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Tests
{
    public class SerializationTests
    {
        [Test]
        public void SelectListItems()
        {
            var tInt = new SelectListItemData<int>
            {
                Text = "SomeText",
                Value = 2
            };

            var mappedAsInt = GenericUserInterfaceModelBuilderExtensions.ToSelectListItem(tInt);

            Assert.AreEqual(tInt.Value.ToString(), mappedAsInt.Value);

            var tStr = new SelectListItemData<string>
            {
                Text = "dsafa",
                Value = "dasa"
            };

            var mappedAsStr = GenericUserInterfaceModelBuilderExtensions.ToSelectListItem(tStr);

            Assert.AreEqual(tStr.Value.ToString(), mappedAsStr.Value);
        }

        [Test]
        public void AutoCompleteSuggestion()
        {
            var tInt = new AutoCompleteSuggestionData<int>
            {
                Text = "SomeText",
                Value = 2
            };

            var mappedAsInt = tInt.ToAutoCompleteSuggestion();

            Assert.AreEqual(tInt.Value.ToString(), mappedAsInt.Value);

            var tStr = new AutoCompleteSuggestionData<string>
            {
                Text = "dsafa",
                Value = "dasa"
            };

            var mappedAsStr = tStr.ToAutoCompleteSuggestion();

            Assert.AreEqual(tStr.Value.ToString(), mappedAsStr.Value);
        }
    }
}