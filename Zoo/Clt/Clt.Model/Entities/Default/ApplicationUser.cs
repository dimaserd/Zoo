using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Croco.Core.Contract.Data.Entities.HaveId;
using Croco.Core.Model.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Clt.Model.Entities.Default
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class ApplicationUser : IdentityUser, IAuditableEntity, IHaveStringId
    {
        /// <summary>
        /// Роли пользователя
        /// </summary>
        public ICollection<ApplicationUserRole> Roles { get; set; }

        /// <inheritdoc />
        public string CreatedBy { get; set; }
        
        /// <inheritdoc />
        public DateTime CreatedOn { get; set; }

        /// <inheritdoc />
        public DateTime? LastModifiedOn { get; set; }

        /// <inheritdoc />
        public string LastModifiedBy { get; set; }

        /// <inheritdoc />
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}