using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Croco.Core.Contract.Data.Entities.HaveId;
using Croco.Core.Model.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Clt.Model.Entities.Default
{
    public class ApplicationUser : IdentityUser, IAuditableEntity, IHaveStringId
    {
        public ICollection<ApplicationUserRole> Roles { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}