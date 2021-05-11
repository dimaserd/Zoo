using Croco.Core.Model.Models;
using System;

namespace Clt.Model.Entities
{
    /// <summary>
    /// Сущность описывающая клиента
    /// </summary>
    public class Client : AuditableEntityBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Пол (Null - не указано, 0 - женский, 1 - мужской)
        /// </summary>
        public bool? Sex { get; set; }

        /// <summary>
        /// Является ли пользователь деактивированным
        /// </summary>
        public bool DeActivated { get; set; }

        /// <summary>
        /// Дополнительные данные в формате json
        /// </summary>
        public string ObjectJson { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Идентификатор аватарки с файлом
        /// </summary>
        public int? AvatarFileId { get; set; }
    }
}