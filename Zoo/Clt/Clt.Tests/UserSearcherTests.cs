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
        [Test]
        public async Task SearchUser()
        {
            var srvProvider = await TestBuilder.BuildCltAppAndGetServiceProvider();

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
