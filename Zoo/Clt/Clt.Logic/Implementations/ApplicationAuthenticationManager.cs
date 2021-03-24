using System.Threading.Tasks;
using Clt.Logic.Abstractions;
using Clt.Model.Entities.Default;
using Microsoft.AspNetCore.Identity;

namespace Clt.Logic.Implementations
{
    public class ApplicationAuthenticationManager : IApplicationAuthenticationManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationAuthenticationManager(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }
    }
}