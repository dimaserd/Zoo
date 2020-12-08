using System;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models.Definition;
using Zoo.GenericUserInterface.Models.Providers;

namespace Zoo.GenericUserInterface.Tests.AutoComplete
{
    public class SomeDataProvider : DataProviderForAutoCompletion<int>
    {
        static readonly Random _random = new Random();

        public static AutoCompleteSuggestionData<int>[] GetDataStatic()
        {
            return Enumerable.Range(0, 5)
                .Select(x => GetNum())
                .Select(x => new AutoCompleteSuggestionData<int>
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
}