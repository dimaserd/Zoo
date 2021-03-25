using Ecc.Contract.Models.Messaging;
using Ecc.Model.Entities.Email;
using Ecc.Model.Entities.Interactions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Croco.Core.Logic.Workers;
using Ecc.Model.Contexts;
using Croco.Core.Contract;

namespace Ecc.Logic.Workers.Messaging
{
    public class MessageDistributionQueryWorker : LightCrocoWorker<EccDbContext>
    {
        public MessageDistributionQueryWorker(ICrocoAmbientContextAccessor context) : base(context)
        {
        }

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