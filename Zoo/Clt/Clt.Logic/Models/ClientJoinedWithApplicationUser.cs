using Clt.Model.Entities;
using Clt.Model.Entities.Default;

namespace Clt.Logic.Models
{
    public class ClientJoinedWithApplicationUser
    {
        public Client Client { get; set; }

        public ApplicationUser User { get; set; }
    }
}