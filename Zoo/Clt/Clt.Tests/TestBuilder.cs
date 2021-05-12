using Clt.Logic;
using Clt.Logic.Services.Account;
using Clt.Model;
using Clt.Tests.Extensions;
using Croco.Core.Application;
using Croco.Core.Contract;
using Croco.Core.Contract.Application.Common;
using Croco.Core.Contract.Files;
using Croco.Core.Extensions;
using Croco.Core.Logic.DbContexts;
using Croco.Core.Logic.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Clt.Tests
{
    public class TestBuilder
    {
        public static async Task<IServiceProvider> BuildCltAppAndGetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddScoped(srv => MySqLiteFileDatabaseExtensions.Create<CrocoInternalDbContext>(opts => new CrocoInternalDbContext(opts), "croco"));
            services.AddScoped(srv => MySqLiteFileDatabaseExtensions.Create<CltDbContext>(opts => new CltDbContext(opts), "clt"));
            var appBuilder = new CrocoApplicationBuilder(services);

            appBuilder.RegisterVirtualPathMapper(Directory.GetCurrentDirectory());
            appBuilder.RegisterFileStorage(new CrocoFileOptions
            {
                SourceDirectory = Directory.GetCurrentDirectory(),
                ImgFileResizeSettings = new Dictionary<string, ImgFileResizeSetting>()
            });

            DbFileManagerServiceCollectionExtensions.RegisterDbFileManager(appBuilder);

            CltLogicRegistrator.Register(appBuilder);

            appBuilder.CheckAndRegisterApplication<CrocoApplication>();
            
            var srvProvider = services.BuildServiceProvider();
            appBuilder.SetAppAndActivator(srvProvider);

            var scope = srvProvider.CreateScope();

            InitializeDatabasesAsEmpty(scope);

            scope.ServiceProvider
                .GetRequiredService<ICrocoRequestContextAccessor>()
                .SetRequestContext(SystemCrocoExtensions.GetRequestContext());

            var accountInitiator = scope.ServiceProvider
                .GetRequiredService<AccountInitiator>();

            await accountInitiator.InitAsync();

            return srvProvider;
        }

        private static void InitializeDatabasesAsEmpty(IServiceScope scope)
        {
            InitializeDatabaseAsEmpty<CltDbContext>(scope);
            InitializeDatabaseAsEmpty<CrocoInternalDbContext>(scope);
        }

        private static void InitializeDatabaseAsEmpty<TDbContext>(IServiceScope scope) where TDbContext : DbContext
        {
            var dataBase = scope.ServiceProvider
                .GetRequiredService<TDbContext>()
                .Database;

            dataBase.EnsureDeleted();
            dataBase.EnsureCreated();
        }
    }
}