using System.Collections.Generic;
using System.Threading.Tasks;
using Tms.Logic.Models;

namespace Tms.Logic.Abstractions
{
    public interface IUsersStorage
    {
        Task<UserFullNameEmailAndAvatarModel> GetUserById(string userId);
        Task<Dictionary<string, UserFullNameEmailAndAvatarModel>> GetUsersDictionary();
    }
}