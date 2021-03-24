using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using System.Threading.Tasks;

namespace Clt.Logic.Abstractions
{
    public interface IClientDataRefresher
    {
        Task UpdateUserDataAsync(ApplicationUser user, Client client);
    }
}