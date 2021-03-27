using Clt.Contract.Settings;
using Clt.Logic.Extensions;
using Clt.Logic.Services.Account;
using Clt.Logic.Services.Users;
using Croco.Core.Contract;
using Croco.Core.Contract.Health;
using System.Threading.Tasks;

namespace Clt.Logic.HealthChecks
{
    internal class RootHealthCheck : ICrocoHealthChecker
    {
        ISettingsFactory SettingsFactory { get; }
        UserSearcher UserSearcher { get; }

        public RootHealthCheck(ISettingsFactory settingsFactory, UserSearcher userSearcher)
        {
            SettingsFactory = settingsFactory;
            UserSearcher = userSearcher;
        }

        public async Task<CrocoHealthCheckResult> CheckHealth()
        {
            var rootSettings = await SettingsFactory.GetSettingAsync<RootSettings>();

            var result = new CrocoHealthCheckResult
            {
                HealthCheckName = this.GetType().FullName
            };

            if (string.IsNullOrWhiteSpace(rootSettings.RootEmail))
            {
                result.State = CrocoHealthState.UnHealthy;
                result.Message = $"Проверьте настройку {typeof(RootSettings).FullName} ее свойство {nameof(RootSettings.RootEmail)}";
                return result;
            }

            var rootUser = await UserSearcher.GetUserByEmailAsync(rootSettings.RootEmail);

            if(rootUser == null)
            {
                result.State = CrocoHealthState.UnHealthy;
                result.Message = $"Рут пользователь не создан." +
                    $" Воспользуйтесь методом {nameof(AccountInitiator.InitAsync)} класса {nameof(AccountInitiator)} чтобы исправить данную ситуацию.";
                
                return result;
            }

            if (rootUser.IsInRole(rootSettings.RootEmail))
            {
                result.State = CrocoHealthState.UnHealthy;
                result.Message = $"Текущий рут пользователь не имеет рут прав." +
                    $" Воспользуйтесь методом {nameof(AccountInitiator.InitAsync)} класса {nameof(AccountInitiator)} чтобы исправить данную ситуацию.";
                
                return result;
            }

            result.State = CrocoHealthState.Healthy;
            result.Message = "С пользователем рут все в порядке";

            return result;
        }
    }
}
