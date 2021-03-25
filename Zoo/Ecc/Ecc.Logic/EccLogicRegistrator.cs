using Croco.Core.Application;
using Ecc.Contract.Commands;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Handlers;
using Ecc.Logic.Settings;
using Ecc.Logic.Workers.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Ecc.Logic
{
    public static class EccLogicRegistrator
    {
        public static void RegisterLogic(CrocoApplicationBuilder appBuilder, EccLinkFunctionInvokerSettings functionInvokerSettings)
        {
            appBuilder.Services.AddSingleton(functionInvokerSettings);
            RegisterEccWorkerTypes(appBuilder.Services);
            AddMessageHandlers(appBuilder);
        }

        private static void AddMessageHandlers(CrocoApplicationBuilder appBuilder)
        {
            appBuilder.EventSourceOptions
                .AddMessageHandler<CreateUserCommand, CreateUserCommandHandler>();
            appBuilder.EventSourceOptions
                .AddMessageHandler<UpdateUserCommand, UpdateUserCommandHandler>();

            appBuilder.EventSourceOptions
                .AddMessageHandler<AppendEmailsFromFileToGroup, AppendEmailsFromFileToGroupMessageHandler>();
            appBuilder.EventSourceOptions
                .AddMessageHandler<SendMailsForEmailGroup, SendMailsForEmailGroupMessageHandler>();
        }

        private static void RegisterEccWorkerTypes(IServiceCollection services)
        {
            var baseType = typeof(BaseEccWorker);

            var typesToRegister = baseType
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
                .ToList();

            foreach (var typeToRegister in typesToRegister)
            {
                services.AddScoped(typeToRegister);
            }
        }
    }
}