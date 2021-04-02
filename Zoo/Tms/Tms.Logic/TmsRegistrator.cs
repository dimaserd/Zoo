using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.Logic.Files;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Principal;
using Tms.Logic.Abstractions;
using Tms.Logic.Services;
using Tms.Model;

namespace Tms.Logic
{
    /// <summary>
    /// Регистратор для контекста Tms
    /// </summary>
    public static class TmsRegistrator
    {
        /// <summary>
        /// Зарегистрировать контекст
        /// </summary>
        /// <typeparam name="TUsersStorage"></typeparam>
        /// <param name="appBuilder"></param>
        /// <param name="isAdminChecker"></param>
        public static void Register<TUsersStorage>(CrocoApplicationBuilder appBuilder, Func<IPrincipal, bool> isAdminChecker)
            where TUsersStorage : class, IUsersStorage
        {
            RegisterInner<TUsersStorage>(appBuilder, isAdminChecker, () => appBuilder.Services.AddScoped<IUsersStorage, TUsersStorage>());
        }

        /// <summary>
        /// Зарегистрировать контекст
        /// </summary>
        /// <typeparam name="TUsersStorage"></typeparam>
        /// <param name="appBuilder"></param>
        /// <param name="regiteringStorage"></param>
        /// <param name="isAdminChecker"></param>
        public static void Register<TUsersStorage>(CrocoApplicationBuilder appBuilder, Func<IServiceProvider, TUsersStorage> regiteringStorage, Func<IPrincipal, bool> isAdminChecker)
            where TUsersStorage : class, IUsersStorage
        {
            RegisterInner<TUsersStorage>(appBuilder, isAdminChecker, () => appBuilder.Services.AddScoped<IUsersStorage, TUsersStorage>(regiteringStorage));
        }

        private static void RegisterInner<TClientStorage>(CrocoApplicationBuilder appBuilder, Func<IPrincipal, bool> isAdminChecker, Action registerClientStorageFunc)
            where TClientStorage : class, IUsersStorage
        {
            Check(appBuilder);

            var services = appBuilder.Services;

            services.AddSingleton(new PrincipalCheker(isAdminChecker));
            registerClientStorageFunc();
            services.AddScoped<DayTasksService>();
            services.AddScoped<DayTaskCommentService>();
        }

        private static void Check(CrocoApplicationBuilder applicationBuilder)
        {
            new EFCrocoApplicationRegistrator(applicationBuilder).RegiterIfNeedEFDataCoonection<TmsDbContext>();
            DbFileManagerServiceCollectionExtensions.CheckForDbFileManager(applicationBuilder.Services);
        }
    }
}