using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Clt.Model
{
    /// <summary>
    /// Контекст с данными клиента
    /// </summary>
    public class CltDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
        ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
        public CltDbContext([NotNull] DbContextOptions<CltDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Клиенты приложения
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Дополнительные свойства клиента
        /// </summary>
        public DbSet<ClientExtraProperty> ClientExtraProperties { get; set; }

        /// <summary>
        /// Переопределение
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.Roles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ClientExtraProperty>(clientProp =>
            {
                clientProp.Property(x => x.PropertyName).HasMaxLength(128)
                    .IsRequired();

                clientProp.HasKey(x => new { x.ClientId, x.PropertyName });
            });
        }
    }
}