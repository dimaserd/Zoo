using Clt.Contract.Models.Users;
using Clt.Contract.Settings;
using Clt.Logic;
using Clt.Logic.Services.Account;
using Clt.Logic.Services.Users;
using Clt.Model;
using Clt.Tests.Extensions;
using Croco.Core.Application;
using Croco.Core.Contract;
using Croco.Core.Contract.Application.Common;
using Croco.Core.Contract.Files;
using Croco.Core.Extensions;
using Croco.Core.Logic.DbContexts;
using Croco.Core.Logic.Files;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Clt.Tests
{
    public class UserSearcherTests
    {
        public async Task<IServiceProvider> BuildCltAppAndGetServiceProvider()
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
                ImgFileResizeSettings = new System.Collections.Generic.Dictionary<string, ImgFileResizeSetting>()
            });

            DbFileManagerServiceCollectionExtensions.RegisterDbFileManager(appBuilder);

            CltLogicRegistrator.Register(appBuilder);

            appBuilder.CheckAndRegisterApplication<CrocoApplication>();

            var srvProvider = services.BuildServiceProvider();

            var scope = srvProvider.CreateScope();

            scope.ServiceProvider
                .GetRequiredService<CltDbContext>()
                .Database.EnsureCreated();

            scope.ServiceProvider
                .GetRequiredService<CrocoInternalDbContext>()
                .Database.EnsureCreated();

            scope.ServiceProvider
                .GetRequiredService<ICrocoRequestContextAccessor>()
                .SetRequestContext(SystemCrocoExtensions.GetRequestContext());

            var accountInitiator = scope.ServiceProvider
                .GetRequiredService<AccountInitiator>();

            await accountInitiator.InitAsync();

            return srvProvider;
        }


        [Test]
        public async Task SearchUser()
        {
            var srvProvider = await BuildCltAppAndGetServiceProvider();

            var scope = srvProvider.CreateScope();
            scope.ServiceProvider
                .GetRequiredService<ICrocoRequestContextAccessor>()
                .SetRequestContext(SystemCrocoExtensions.GetRequestContext());

            var userSearcher = scope.ServiceProvider.GetRequiredService<UserSearcher>();

            var rootSettings = scope.ServiceProvider.GetRequiredService<ISettingsFactory>()
                .GetSetting<RootSettings>();

            var user = await userSearcher.GetUserByEmailAsync(rootSettings.RootEmail);

            Assert.IsNotNull(user);
        }
    }
}
