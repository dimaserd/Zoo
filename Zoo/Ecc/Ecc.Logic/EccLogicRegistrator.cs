using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.Logic.Files;
using Croco.Core.Logic.Files.Events;
using Ecc.Contract.Commands;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Handlers;
using Ecc.Logic.Services;
using Ecc.Logic.Settings;
using Ecc.Logic.Workers.Base;
using Ecc.Logic.Workers.Emails;
using Ecc.Logic.Workers.Emails.Senders;
using Ecc.Model.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Ecc.Logic
{
    public static class EccLogicRegistrator
    {
        public static void RegisterLogic(CrocoApplicationBuilder appBuilder, EccSettings settings)
        {
            Check(appBuilder);

            var services = appBuilder.Services;
            RegisterServices(services, settings);

            RegisterEccWorkerTypes(services);
            AddMessageHandlers(appBuilder);
        }

        private static void RegisterServices(IServiceCollection services, EccSettings settings)
        {
            services.AddSingleton(settings);

            if (!services.Any(x => x.ServiceType == typeof(IEmailSender)))
            {
                services.AddScoped<IEmailSender, SmtpEmailSender>();
            }

            services.AddSingleton<EccPixelUrlProvider>();
        }

        private static void AddMessageHandlers(CrocoApplicationBuilder appBuilder)
        {
            var eventSourceOptions = appBuilder.EventSourceOptions;

            eventSourceOptions
                .AddMessageHandler<CreateUserCommand, CreateUserCommandHandler>();
            
            eventSourceOptions
                .AddMessageHandler<UpdateUserCommand, UpdateUserCommandHandler>();

            eventSourceOptions
                .AddMessageHandler<AppendEmailsFromFileToGroup, AppendEmailsFromFileToGroupMessageHandler>();
            eventSourceOptions
                .AddMessageHandler<SendMailsForEmailGroup, SendMailsForEmailGroupMessageHandler>();

            eventSourceOptions
                .AddMessageHandler<FilesUploadedEvent, FilesUploadedEventHandler>();
        }

        private static void RegisterEccWorkerTypes(IServiceCollection services)
        {
            var baseType = typeof(BaseEccWorker);

            var typesToRegister = baseType
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract);

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
    }
}