using System;

namespace Clt.Contract.Models.Users
{
    /// <summary>
    /// Модель описывающая клиента
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// Имя клиента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public bool? Sex { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Идентификатор файла с аватаром
        /// </summary>
        public int? AvatarFileId { get; set; }
    }
}