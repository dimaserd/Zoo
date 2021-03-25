using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Principal;
using Tms.Logic.Abstractions;
using Tms.Logic.Services;
using Tms.Model;

namespace Tms.Logic
{
    public static class TmsRegistrator
    {
        public static void Register<TUsersStorage>(IServiceCollection services, Func<IPrincipal, bool> isAdminChecker)
            where TUsersStorage : class, IUsersStorage
        {
            RegisterInner<TUsersStorage>(services, isAdminChecker, () => services.AddScoped<IUsersStorage, TUsersStorage>());
        }

        public static void Register<TUsersStorage>(IServiceCollection services, Func<IServiceProvider, TUsersStorage> regiteringStorage, Func<IPrincipal, bool> isAdminChecker)
            where TUsersStorage : class, IUsersStorage
        {
            RegisterInner<TUsersStorage>(services, isAdminChecker, () => services.AddScoped<IUsersStorage, TUsersStorage>(regiteringStorage));
        }

        private static void RegisterInner<TClientStorage>(IServiceCollection services, Func<IPrincipal, bool> isAdminChecker, Action registerClientStorageFunc)
            where TClientStorage : class, IUsersStorage
        {
            if (!CrocoAppData.GetRegisteredDataConnctions().Any(x => x.ImplementationType == typeof(EntityFrameworkDataConnection<TmsDbContext>)))
            {
                throw new InvalidOperationException($"Необходимо зарегистрировать соединение {nameof(EntityFrameworkDataConnection<TmsDbContext>)}. " +
                    $"Воспользуйтесь методом {nameof(EFCrocoApplicationRegistrator.AddEntityFrameworkDataConnection)} класса {nameof(EFCrocoApplicationRegistrator)} для регистрации соединения");
            }

            services.AddSingleton(new PrincipalCheker(isAdminChecker));
            registerClientStorageFunc();
            services.AddScoped<DayTasksService>();
            services.AddScoped<DayTaskCommentService>();
        }
    }
}