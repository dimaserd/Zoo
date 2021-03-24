using Clt.Contract.Models.Account;
using Clt.Contract.Models.Users;
using Clt.Contract.Services;
using Clt.Logic.Workers.Account;
using Croco.Core.Contract.Models;
using System.Threading.Tasks;

namespace Clt.Logic
{
    public class ClientService : IClientService
    {
        AccountRegistrationWorker AccountRegistrationWorker { get; }

        public ClientService(AccountRegistrationWorker accountRegistrationWorker)
        {
            AccountRegistrationWorker = accountRegistrationWorker;
        }

        
        public Task<BaseApiResponse<RegisteredUser>> RegisterAndSignInAsync(RegisterModel model, bool createRandomPassword)
        {
            return AccountRegistrationWorker
                .RegisterAndSignInAsync(model, createRandomPassword);
        }
    }
}