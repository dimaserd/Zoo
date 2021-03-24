using System.Threading.Tasks;
using Clt.Logic.Abstractions;
using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using Microsoft.AspNetCore.Identity;

namespace Clt.Logic.Implementations
{
    public class ClientDataRefresher : IClientDataRefresher
    {
        SignInManager<ApplicationUser> SignInManager { get; }

        public ClientDataRefresher(SignInManager<ApplicationUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public Task UpdateUserDataAsync(ApplicationUser user, Client client)
        {
            return SignInManager.SignInAsync(user, true);
        }
    }
}