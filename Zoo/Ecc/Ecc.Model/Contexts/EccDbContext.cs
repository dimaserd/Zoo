using Ecc.Model.Entities.Chats;
using Ecc.Model.Entities.Ecc.Messaging;
using Ecc.Model.Entities.Email;
using Ecc.Model.Entities.External;
using Ecc.Model.Entities.IntegratedApps;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Entities.LinkCatch;
using Microsoft.EntityFrameworkCore;

namespace Ecc.Model.Contexts
{
    /// <summary>
    /// Контекст для рассылок
    /// </summary>
    public class EccDbContext : DbContext
    {
        /// <summary>
        /// Приложения с интеграциями
        /// </summary>
        public DbSet<IntegratedApp> IntegratedApps { get; set; }

        public DbSet<IntegratedAppUserSetting> IntegratedAppUserSettings { get; set; }

        public DbSet<EmailGroup> EmailGroups { get; set; }

        public DbSet<EmailInEmailGroupRelation> EmailInEmailGroupRelations { get; set; }

        /// <summary>
        /// Шаблоны Email сообщений
        /// </summary>
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        #region Сообщения и чаты
        public DbSet<EccChat> Chats { get; set; }

        public DbSet<EccChatMessage> ChatMessages { get; set; }

        public DbSet<EccChatUserRelation> ChatUserRelations { get; set; }

        public DbSet<EccChatMessageAttachment> ChatMessageAttachments { get; set; }
        #endregion

        public DbSet<EmailLinkCatch> EmailLinkCatches { get; set; }

        public DbSet<EmailLinkCatchRedirect> EmailLinkCatchRedirects { get; set; }

        public DbSet<MessageDistribution> MessageDistributions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            EmailLinkCatch.OnModelCreating(modelBuilder);
            Interaction.OnModelCreating(modelBuilder);
            InteractionAttachment.OnModelCreating(modelBuilder);

            EccFile.OnModelCreating(modelBuilder);
            EccUserGroup.OnModelCreating(modelBuilder);
            EccUserInUserGroupRelation.OnModelCreating(modelBuilder);

            EmailInEmailGroupRelation.OnModelCreating(modelBuilder);
            EccChatUserRelation.OnModelCreating(modelBuilder);
            IntegratedApp.OnModelCreating(modelBuilder);
            
            EmailGroup.OnModelCreating(modelBuilder);
            MessageDistribution.OnModelCreating(modelBuilder);
            MailDistributionUserGroupRelation.OnModelCreating(modelBuilder);
        }
    }
}