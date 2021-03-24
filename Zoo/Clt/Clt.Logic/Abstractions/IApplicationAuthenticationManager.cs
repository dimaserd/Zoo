using System.Threading.Tasks;

namespace Clt.Logic.Abstractions
{
    public interface IApplicationAuthenticationManager
    {
        Task SignOutAsync();
    }
}