using System;
using System.Collections.Generic;
using System.Linq;

namespace Clt.Contract.Settings
{
    /// <summary>
    /// Модель описывающая настройки названий системных ролей
    /// </summary>
    public class CltRolesSetting
    {
        /// <summary>
        /// Название роли администратора
        /// </summary>
        public string AdminRoleName { get; set; }

        /// <summary>
        /// Название роли Рута
        /// </summary>
        public string RootRoleName { get; set; }

        /// <summary>
        /// Названия остальных ролей в приложении
        /// </summary>
        public string[] OtherRoleNames { get; set; }


        /// <summary>
        /// Получить все возможные роли
        /// </summary>
        /// <returns></returns>
        public string[] GetAllRoleNames()
        {
            return new List<string>(OtherRoleNames)
            {
                AdminRoleName,
                RootRoleName
            }.Distinct().ToArray();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CltRolesSetting()
        {
            AdminRoleName = "Admin";
            RootRoleName = "Root";
            OtherRoleNames = Array.Empty<string>();
        }
    }
}