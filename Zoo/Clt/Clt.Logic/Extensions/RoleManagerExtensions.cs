using Clt.Model.Entities.Default;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clt.Logic.Extensions
{
    internal static class RoleManagerExtensions
    {
        public static async Task<BaseApiResponse> CreateRolesAsync<TEnum>(this RoleManager<ApplicationRole> roleManager) 
            where TEnum : Enum
        {
            var list = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

            var roles = list.Select(x => x.ToString()).ToArray();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role, ConcurrencyStamp = Guid.NewGuid().ToString() });
                }
            }

            return new BaseApiResponse(true, "Роли созданы");
        }
    }
}