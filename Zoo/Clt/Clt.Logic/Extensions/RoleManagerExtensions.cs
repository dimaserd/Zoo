using Clt.Model.Entities.Default;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Clt.Logic.Extensions
{
    internal static class RoleManagerExtensions
    {
        public static async Task<BaseApiResponse> CreateRolesAsync(this RoleManager<ApplicationRole> roleManager, string[] roleNames)
        {
            foreach (var role in roleNames)
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