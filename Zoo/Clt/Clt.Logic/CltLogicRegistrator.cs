using Clt.Logic.Abstractions;
using Clt.Logic.Core.Workers;
using Clt.Logic.Implementations;
using Clt.Logic.Workers;
using Clt.Model;
using Clt.Model.Entities.Default;
using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Clt.Logic.RegistrationModule
{
    public static class CltLogicRegistrator
    {
        public static void Register(CrocoApplicationBuilder applicationBuilder, Action<IdentityOptions> setupAction = null)
        {
            var services = applicationBuilder.Services;

            if (!CrocoAppData.GetRegisteredDataConnctions().Any(x => x.ImplementationType == typeof(EntityFrameworkDataConnection<CltDbContext>)))
            {
                throw new InvalidOperationException($"Необходимо зарегистрировать соединение {nameof(EntityFrameworkDataConnection<CltDbContext>)}. " +
                    $"Воспользуйтесь методом {nameof(EFCrocoApplicationRegistrator.AddEntityFrameworkDataConnection)} класса {nameof(EFCrocoApplicationRegistrator)} для регистрации соединения");
            }

            services.AddScoped<IApplicationAuthenticationManager, ApplicationAuthenticationManager>();
            services.AddScoped<IClientDataRefresher, ClientDataRefresher>();
            services.AddTransient<ApplicationUserManager>();
            services.AddTransient<ApplicationSignInManager>();

            setupAction ??= config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
                config.Password.RequiredLength = 8;
            };

            services.AddIdentity<ApplicationUser, ApplicationRole>(setupAction)
                .AddEntityFrameworkStores<CltDbContext>()
                .AddDefaultTokenProviders();

            RegisterCoreTypes(services);
            RegisterCltWorkerTypes(services);
        }

        private static void RegisterCoreTypes(IServiceCollection services)
        {
            services.AddScoped<CoreCltAccountLoginWorker<ApplicationUser, CltDbContext>>();
            services.AddScoped<PasswordChanger<ApplicationUser, CltDbContext>>();
            services.AddScoped<PasswordForgotWorker<ApplicationUser, CltDbContext>>();
            services.AddScoped<PasswordHashValidator<ApplicationUser, CltDbContext>>();
        }

        private static void RegisterCltWorkerTypes(IServiceCollection services)
        {
            var baseType = typeof(BaseCltWorker);

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