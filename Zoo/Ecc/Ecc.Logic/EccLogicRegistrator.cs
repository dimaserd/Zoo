using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.Implementations;
using Croco.Core.Logic.Files;
using Croco.Core.Logic.Files.Abstractions;
using Croco.Core.Logic.Files.Events;
using Ecc.Contract.Commands;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Handlers;
using Ecc.Logic.Settings;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Ecc.Logic
{
    public static class EccLogicRegistrator
    {
        public static void RegisterLogic(CrocoApplicationBuilder appBuilder, EccLinkFunctionInvokerSettings functionInvokerSettings)
        {
            Check(appBuilder);
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

            appBuilder.EventSourceOptions
                .AddMessageHandler<FilesUploadedEvent, FilesUploadedEventHandler>();
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

        private static void Check(CrocoApplicationBuilder appBuilder)
        {
            new EFCrocoApplicationRegistrator(appBuilder).CheckForEFDataCoonection<EccDbContext>();
            DbFileManagerServiceCollectionExtensions.CheckForDbFileManager(appBuilder.Services);
        }

        private static void CheckForEFDataCoonection<TDbContext>() where TDbContext : DbContext
        {
            if (!CrocoAppData.GetRegisteredDataConnctions().Any(x => x.ImplementationType == typeof(EntityFrameworkDataConnection<TDbContext>)))
            {
                throw new InvalidOperationException($"Необходимо зарегистрировать EF соединение {nameof(EntityFrameworkDataConnection<TDbContext>)}. " +
                    $"Воспользуйтесь методом {nameof(EFCrocoApplicationRegistrator.AddEntityFrameworkDataConnection)} класса {nameof(EFCrocoApplicationRegistrator)} для регистрации соединения");
            }
        }
    }
}