using NUnit.Framework;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models.Definition;

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

            var mappedAsInt = GenericUserInterfaceModelBuilderMappings.ToSelectListItem(tInt);

            Assert.AreEqual(tInt.Value.ToString(), mappedAsInt.Value);

            var tStr = new SelectListItemData<string>
            {
                Text = "dsafa",
                Value = "dasa"
            };

            var mappedAsStr = GenericUserInterfaceModelBuilderMappings.ToSelectListItem(tStr);

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