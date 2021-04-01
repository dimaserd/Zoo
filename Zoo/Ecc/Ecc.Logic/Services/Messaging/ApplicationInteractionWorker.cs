using Ecc.Logic.Models;
using Ecc.Contract.Models.Interactions;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.Interactions;
using Ecc.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;

namespace Ecc.Logic.Services.Messaging
{
    /// <summary>
    /// Сервис для работы со взаимодействиями с клиентами
    /// </summary>
    public class ApplicationInteractionWorker : BaseEccService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        public ApplicationInteractionWorker(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }


        internal static IQueryable<ApplicationInteractionWithStatus<TInteraction>> GetQueryWithStatus<TInteraction>(IQueryable<TInteraction> query)
            where TInteraction : Interaction, new()
        {
            return query.Select(x => new ApplicationInteractionWithStatus<TInteraction>
            {
                Interaction = x,
                Status = x.Statuses.OrderByDescending(t => t.StartedOn).FirstOrDefault().Status
            });
        }

        /// <summary>
        /// Установить статус для взаимодействий
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        public Task SetStatusForInteractions(IEnumerable<string> ids, InteractionStatus status, string statusDescription)
        {
            var now = Application.DateTimeProvider.Now;

            var list = ids.Select(x => new InteractionStatusLog
            {
                Id = Guid.NewGuid().ToString(),
                Status = status,
                StartedOn = now,
                InteractionId = x,
                StatusDescription = statusDescription
            });

            GetRepository<InteractionStatusLog>().CreateHandled(list);
            return SaveChangesAsync();
        }

        /// <summary>
        /// Обновить статусы взаимодействиям
        /// </summary>
        /// <param name="statuses"></param>
        /// <returns></returns>
        public Task UpdateInteractionStatusesAsync(List<UpdateInteractionStatus> statuses)
        {
            var now = Application.DateTimeProvider.Now;

            var list = statuses.Select(x => new InteractionStatusLog
            {
                Id = Guid.NewGuid().ToString(),
                Status = x.Status,
                StartedOn = now,
                InteractionId = x.Id,
                StatusDescription = x.StatusDescription
            });

            GetRepository<InteractionStatusLog>().CreateHandled(list);
            return SaveChangesAsync();
        }

        /// <summary>
        /// Получить базовый запрос для взаимодействий, которые необходимо отправить
        /// </summary>
        /// <typeparam name="TInteraction"></typeparam>
        /// <returns></returns>
        public IQueryable<TInteraction> GetInitQueryToSend<TInteraction>() where TInteraction : Interaction, new()
        {
            var dateNow = Application.DateTimeProvider.Now;

            return Query<TInteraction>().Where(x => !x.SendNow && x.SendOn >= dateNow || x.SendNow);
        }
    }
}