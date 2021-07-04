using Clt.Contract.Settings;
using Clt.Logic.Services.Users;
using Croco.Core.Contract;
using Croco.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
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