﻿using Croco.Core.Application;
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
using Ecc.Contract.Events.Chat;
using Croco.Core.IntegrationMessagesDescriptor.Enumerations;
using Ecc.Logic.Integrations;
using Clt.Contract.Events;
using Ecc.Logic.Clients;

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
        public static void RegisterLogic(CrocoApplicationBuilder appBuilder, EccSettings settings)
        {
            Check(appBuilder);

            var services = appBuilder.Services;
            services.AddHttpClient<OneSignalClient>();
            RegisterServices<CltUserMasterStorage>(services, settings);

            RegisterEccWorkerTypes(services);
            AddMessageHandlers(appBuilder);


            var descriptoBuilder = appBuilder.IntegrationMessagesDescriptorBuilder;
            descriptoBuilder
                .AddMessageDescription<CreateUserCommand>("Команда для создания пользователя", IntegrationMessageType.Command)
                .AddMessageDescription<UpdateUserCommand>("Команда для обновления данных пользователя", IntegrationMessageType.Command)
                .AddMessageDescription<AppendEmailsFromFileToGroup>("Команда для добавления эмейлов в групу из файла", IntegrationMessageType.Command)
                .AddMessageDescription<SendMailsForEmailGroup>("Команда для отправки эмейлов для групы", IntegrationMessageType.Command);

            descriptoBuilder
                .AddMessageDescription<ChatCreatedEvent>("Событие о том, что чат создался", IntegrationMessageType.Event)
                .AddMessageDescription<ChatRelationUpdatedEvent>("Событие о том, что пользователь посетил чат или зашёл в него", IntegrationMessageType.Event);
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
            services.AddSingleton<EmailListExtractor>();
        }

        private static void AddMessageHandlers(CrocoApplicationBuilder appBuilder)
        {
            var eventSourceOptions = appBuilder.EventSourceOptions;

            eventSourceOptions
                .AddMessageHandler<CreateUserCommand, CreateUserCommandHandler>()
                .AddMessageHandler<UpdateUserCommand, UpdateUserCommandHandler>()
                .AddMessageHandler<AppendEmailsFromFileToGroup, AppendEmailsFromFileToGroupMessageHandler>()
                .AddMessageHandler<SendMailsForEmailGroup, SendMailsForEmailGroupMessageHandler>()
                .AddMessageHandler<ClientRegisteredEvent, ClientRegisteredEventHandler>(true)
                .AddMessageHandler<ClientDataUpdatedEvent, ClientDataUpdatedEventHandler>(true)
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
            new EFCrocoApplicationRegistrator(appBuilder).RegiterIfNeedEFDataCoonection<EccDbContext>();
            DbFileManagerServiceCollectionExtensions.CheckForDbFileManager(appBuilder.Services);
        }
    }
}