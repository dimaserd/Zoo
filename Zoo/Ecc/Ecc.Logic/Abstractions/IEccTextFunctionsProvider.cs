using Ecc.Logic.Services;
using System.Collections.Generic;

namespace Ecc.Logic.Abstractions
{
    public interface IEccTextFunctionsProvider
    {
        Dictionary<string, IEccTextFunctionInvoker> GetFunctions();
    }
}