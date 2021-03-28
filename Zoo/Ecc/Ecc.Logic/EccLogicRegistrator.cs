using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.Logic.Files;
using Croco.Core.Logic.Files.Events;
using Ecc.Contract.Abstractions;
using Ecc.Contract.Commands;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Handlers;
using Ecc.Logic.Services;
using Ecc.Logic.Settings;
using Ecc.Logic.Services.Base;
using Ecc.Logic.Services.Emails.Senders;
using Ecc.Model.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Ecc.Logic
{
    /// <summary>
    /// Регистратор сервисов для работы логики Ecc
    /// </summary>
    public static class EccLogicRegistrator
    {
        /// <summary>
        /// Зарегистрировать логику
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="settings"></param>
        public static void RegisterLogic<TUserStorage>(CrocoApplicationBuilder appBuilder, EccSettings settings)
            where TUserStorage : class, IUserMasterStorage
        {
            Check(appBuilder);

            var services = appBuilder.Services;
            RegisterServices<TUserStorage>(services, settings);

            RegisterEccWorkerTypes(services);
            AddMessageHandlers(appBuilder);
        }

        private static void RegisterServices<TUserStorage>(IServiceCollection services, EccSettings settings)
            where TUserStorage : class, IUserMasterStorage
        {
            services.AddScoped<IUserMasterStorage, TUserStorage>();
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
                .AddMessageHandler<CreateUserCommand, CreateUserCommandHandler>()
                .AddMessageHandler<UpdateUserCommand, UpdateUserCommandHandler>()
                .AddMessageHandler<AppendEmailsFromFileToGroup, AppendEmailsFromFileToGroupMessageHandler>()
                .AddMessageHandler<SendMailsForEmailGroup, SendMailsForEmailGroupMessageHandler>()
                .AddMessageHandler<FilesUploadedEvent, FilesUploadedEventHandler>(true);
        }

        private static void RegisterEccWorkerTypes(IServiceCollection services)
        {
            var baseType = typeof(BaseEccService);

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