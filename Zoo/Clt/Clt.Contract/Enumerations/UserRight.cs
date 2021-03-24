using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Enumerations
{
    /// <summary>
    /// Перечисление описывающее право пользователя
    /// </summary>
    public enum UserRight
    {
        [Display(Name = "Root")]
        Root,

        /// <summary>
        /// Супер-администратор
        /// </summary>
        [Display(Name = "Супер-Админ")]
        SuperAdmin,

        /// <summary>
        /// Администратор
        /// </summary>
        [Display(Name = "Админ")]
        Admin,

        /// <summary>
        /// Пользователь группы Продавец может создавать продавцов
        /// </summary>
        [Display(Name = "Продавец")]
        Seller,

        [Display(Name = "Клиент")]
        Customer,

        [Display(Name = "Разработчик")]
        Developer
    }
}