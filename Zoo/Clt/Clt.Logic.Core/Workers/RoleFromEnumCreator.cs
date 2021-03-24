using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clt.Logic.Core.Workers
{
    public static class RoleFromEnumCreator
    {
        public static async Task<BaseApiResponse> CreateRolesAsync<TEnum, TRole>(RoleManager<TRole> roleManager) where TEnum : Enum where TRole : IdentityRole, new()
        {
            var list = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

            var roles = list.Select(x => x.ToString()).ToArray();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new TRole { Name = role, ConcurrencyStamp = Guid.NewGuid().ToString() });
                }
            }

            return new BaseApiResponse(true, "Роли созданы");
        }
    }
}