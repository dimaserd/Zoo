using Croco.Core.Utils;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    public class AutoCompleteSuggestionData<TItem>
    {
        public string Text { get; set; }
        public string DataJson { get; set; }
        public TItem Value { get; set; }

        internal AutoCompleteSuggestion ToAutoCompleteSuggestion()
        {
            return new AutoCompleteSuggestion
            {
                DataJson = DataJson,
                Text = Text,
                Value = Tool.JsonConverter.Serialize(Value)
            };
        }
    }
}