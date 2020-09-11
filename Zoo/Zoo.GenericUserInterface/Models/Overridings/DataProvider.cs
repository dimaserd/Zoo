using System;
using System.Threading.Tasks;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    public class DataProvider
    {
        public Func<string, Task<AutoCompleteSuggestion[]>> DataFunction { get; set; }
    }
}