using Clt.Contract.Settings;
using Croco.Core.Contract;
using Croco.Core.Contract.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clt.Logic.HealthChecks
{
    internal class CltSettingsHealthCheck : ICrocoHealthChecker
    {
        ISettingsFactory SettingsFactory { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settingsFactory"></param>
        public CltSettingsHealthCheck(ISettingsFactory settingsFactory)
        {
            SettingsFactory = settingsFactory;
        }

        /// <inheritdoc />
        public async Task<CrocoHealthCheckResult> CheckHealth()
        {
            var setting = await SettingsFactory.GetSettingAsync<CltRolesSetting>();

            var result = new CrocoHealthCheckResult
            {
                HealthCheckName = this.GetType().FullName
            };

            if (string.IsNullOrWhiteSpace(setting.AdminRoleName))
            {
                result.State = CrocoHealthState.Warning;
                result.Message = $"Настройка {typeof(CltRolesSetting).FullName} неправильно заполнена." +
                    $" Проверьте свойство {nameof(CltRolesSetting.AdminRoleName)}.";

                return result;
            }

            if (string.IsNullOrWhiteSpace(setting.RootRoleName))
            {
                result.State = CrocoHealthState.Warning;
                result.Message = $"Настройка {typeof(CltRolesSetting).FullName} неправильно заполнена." +
                    $" Проверьте свойство {nameof(CltRolesSetting.RootRoleName)}.";

                return result;
            }

            if (setting.RootRoleName == setting.AdminRoleName)
            {
                result.State = CrocoHealthState.Warning;
                result.Message = $"Настройка {typeof(CltRolesSetting).FullName} неправильно заполнена." +
                    $" Свойства {nameof(CltRolesSetting.RootRoleName)} и {nameof(CltRolesSetting.AdminRoleName)} не должны быть одинаковыми.";

                return result;
            }

            var roleNames = new List<string>(setting.OtherRoleNames)
            {
                setting.AdminRoleName,
                setting.RootRoleName
            };

            var hasDuplicates = roleNames
                .GroupBy(x => x)
                .Any(x => x.Count() > 1);

            if (hasDuplicates)
            {
                result.State = CrocoHealthState.Warning;
                result.Message = $"Настройка {typeof(CltRolesSetting).FullName} неправильно заполнена." +
                    $" Не должно быть дубликатов в названиях ролей, а также пересечений c ролями в свойствах {nameof(CltRolesSetting.AdminRoleName)} и {nameof(CltRolesSetting.RootRoleName)}";
            }

            result.State = CrocoHealthState.Healthy;
            result.Message = $"Настройка {typeof(CltRolesSetting).FullName} правильно заполнена.";
            return result;
            throw new NotImplementedException();
        }
    }
}
