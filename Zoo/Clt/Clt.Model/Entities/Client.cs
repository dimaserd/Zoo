using Croco.Core.Model.Abstractions.Entity;
using Croco.Core.Model.Models;
using System;

namespace Clt.Model.Entities
{
    public class Client : AuditableEntityBase, ICrocoUser
    {
        public string Id { get; set; }
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
        /// Баланс пользователя
        /// </summary>
        public decimal Balance { get; set; }

        public bool DeActivated { get; set; }

        public string ObjectJson { get; set; }

        public string PhoneNumber { get; set; }

        public int? AvatarFileId { get; set; }
    }
}