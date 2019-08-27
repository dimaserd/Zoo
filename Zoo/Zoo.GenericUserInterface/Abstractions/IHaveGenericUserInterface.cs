using System.Collections.Generic;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Abstractions
{
    public interface IHaveGenericUserInterface
    {
        List<UserInterfaceBlock> GetUserInterfaceBuildModel();
    }
}
