using Ecc.Contract.Models.Messaging;
using Ecc.Model.Entities.Email;
using Ecc.Model.Entities.Interactions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Croco.Core.Contract;
using Ecc.Logic.Services.Base;
using Croco.Core.Contract.Application;

namespace Ecc.Logic.Services.Messaging
{
    /// <summary>
    /// Предоставляет методы для работы
    /// </summary>
    public class MessageDistributionQueryWorker : BaseEccService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="app"></param>
        public MessageDistributionQueryWorker(ICrocoAmbientContextAccessor context, ICrocoApplication app) : base(context, app)
        {
        }

        /// <summary>
        /// Получить список рассылок
        /// </summary>
        /// <returns></returns>
        public async Task<List<MessageDistributionCountModel>> GetDistributions()
        {
            var q = await (from mes in Query<MessageDistribution>()
                    join inter in Query<Interaction>() on mes.Id equals inter.MessageDistributionId
                    select new
                    {
                        mes.Id,
                        mes.Type,
                        mes.Data,
                        InteractionId = inter.Id
                    }).ToListAsync();

            return q.GroupBy(x => x.Id).Select(x => {

                var f = x.First();

                return new MessageDistributionCountModel
                {
                    Id = x.Key,
                    Data = f.Data,
                    Type = f.Type,
                    InteractionsCount = x.Count()
                };
            }).ToList();
        }
    }
}