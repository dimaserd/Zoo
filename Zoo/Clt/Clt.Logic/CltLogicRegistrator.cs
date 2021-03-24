using Clt.Contract.Services;
using Croco.Core.Application;

namespace Clt.Logic.RegistrationModule
{
    public static class CltLogicRegistrator
    {
        public static void Register(CrocoApplicationBuilder applicationBuilder)
        {
            applicationBuilder.RegisterService<IClientService, ClientService>();
        }
    }
}