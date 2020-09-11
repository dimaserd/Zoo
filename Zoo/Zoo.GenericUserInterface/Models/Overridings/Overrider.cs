using System;
using System.Threading.Tasks;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    public class Overrider
    {
        public Func<GenericUserInterfaceBag, GenerateGenericUserInterfaceModel, Task> OverrideFunction { get; set; }
        public InterfaceOverriderType Type { get; set; }
    }
}