using Ecc.Model.Entities.Chats;
using Ecc.Model.Entities.Ecc.Messaging;
using Ecc.Model.Entities.Email;
using Ecc.Model.Entities.External;
using Ecc.Model.Entities.IntegratedApps;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Entities.LinkCatch;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Ecc.Model.Contexts
{
    /// <summary>
    /// Контекст для рассылок
    /// </summary>
    public class EccDbContext : DbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
        public EccDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Приложения с интеграциями
        /// </summary>
        public DbSet<IntegratedApp> IntegratedApps { get; set; }

        /// <summary>
        /// Настройки для интеграций
        /// </summary>
        public DbSet<IntegratedAppUserSetting> IntegratedAppUserSettings { get; set; }

        /// <summary>
        /// Группы эмейлов
        /// </summary>
        public DbSet<EmailGroup> EmailGroups { get; set; }

        /// <summary>
        /// Эмейлы в группах эмейлов
        /// </summary>
        public DbSet<EmailInEmailGroupRelation> EmailInEmailGroupRelations { get; set; }

        /// <summary>
        /// Шаблоны Email сообщений
        /// </summary>
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        #region Сообщения и чаты
        /// <summary>
        /// Чаты
        /// </summary>
        public DbSet<EccChat> Chats { get; set; }

        /// <summary>
        /// Сообщения
        /// </summary>
        public DbSet<EccChatMessage> ChatMessages { get; set; }

        /// <summary>
        /// Пользователи в чатах
        /// </summary>
        public DbSet<EccChatUserRelation> ChatUserRelations { get; set; }

        /// <summary>
        /// Вложения к сообщениям
        /// </summary>
        public DbSet<EccChatMessageAttachment> ChatMessageAttachments { get; set; }
        #endregion

        /// <summary>
        /// Перехваты переходов по ссылкам в эмейле
        /// </summary>
        public DbSet<EmailLinkCatch> EmailLinkCatches { get; set; }

        /// <summary>
        /// Редиректы на перехваты
        /// </summary>
        public DbSet<EmailLinkCatchRedirect> EmailLinkCatchRedirects { get; set; }

        /// <summary>
        /// Рассылки сообщений
        /// </summary>
        public DbSet<MessageDistribution> MessageDistributions { get; set; }

        /// <summary>
        /// Переорпеделение создания модели для контекста
        /// </summary>
        /// <param name="modelBuilder"></param>
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
            MailDistribution.OnModelCreating(modelBuilder);
        }
    }
}