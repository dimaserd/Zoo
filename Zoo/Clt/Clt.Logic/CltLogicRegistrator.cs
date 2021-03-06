﻿using Clt.Contract.Events;
using Clt.Logic.Abstractions;
using Clt.Logic.HealthChecks;
using Clt.Logic.Implementations;
using Clt.Logic.Services;
using Clt.Model;
using Clt.Model.Entities.Default;
using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.IntegrationMessagesDescriptor.Enumerations;
using Croco.Core.Logic.Files;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Clt.Logic
{
    /// <summary>
    /// Регистратор клиентской логики
    /// </summary>
    public static class CltLogicRegistrator
    {
        /// <summary>
        /// Зарегистрировать клиентскую логику
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="setupAction"></param>
        public static void Register(CrocoApplicationBuilder applicationBuilder, Action<IdentityOptions> setupAction = null)
        {
            Check(applicationBuilder);

            var services = applicationBuilder.Services;

            services.AddScoped<IApplicationAuthenticationManager, ApplicationAuthenticationManager>();
            services.AddScoped<IClientDataRefresher, ClientDataRefresher>();
            
            setupAction ??= config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
                config.Password.RequiredLength = 8;
            };

            services.AddIdentity<ApplicationUser, ApplicationRole>(setupAction)
                .AddEntityFrameworkStores<CltDbContext>()
                .AddDefaultTokenProviders();

            RegisterCltWorkerTypes(services);

            applicationBuilder
                .RegisterHealthCheck<CltSettingsHealthCheck>()
                .RegisterHealthCheck<RootHealthCheck>();

            applicationBuilder.IntegrationMessagesDescriptorBuilder
                    .AddMessageDescription<ClientStartedRestoringPasswordEvent>("Выбрасывается в систему, когда клиент начинает процедуру сброса пароля", IntegrationMessageType.Event)
                    .AddMessageDescription<ClientChangedPassword>("Выбрасывыется в систему, когда клиент закончил процедуру изменения пароля", IntegrationMessageType.Event)
                    .AddMessageDescription<ClientDataUpdatedEvent>("Выбрасывается в систему, когда данные клиента обновляются либо администратором либо самим клиентом", IntegrationMessageType.Event)
                    .AddMessageDescription<ClientRegisteredEvent>("Выбрасывается в систему, когда клиент прошёл регистрацию", IntegrationMessageType.Event);
        }

        private static void Check(CrocoApplicationBuilder applicationBuilder)
        {
            new EFCrocoApplicationRegistrator(applicationBuilder).RegiterIfNeedEFDataCoonection<CltDbContext>();

            DbFileManagerServiceCollectionExtensions.CheckForDbFileManager(applicationBuilder.Services);
        }

        private static void RegisterCltWorkerTypes(IServiceCollection services)
        {
            var baseType = typeof(BaseCltService);

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