using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Services
{
    public class GenericUserInterfaceOverrider
    {
        public Task OverrideAsync(GenerateGenericUserInterfaceModel model, Dictionary<string, Func<GenerateGenericUserInterfaceModel, Task>> dictionary)
        {
            var key = model.TypeDescription.FullTypeName;

            if (!dictionary.ContainsKey(key))
            {
                return Task.CompletedTask;
            }

            var task = dictionary[key];

            return task(model);
        }
    }
}