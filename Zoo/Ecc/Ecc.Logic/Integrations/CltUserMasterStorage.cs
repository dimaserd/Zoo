using Clt.Model;
using Clt.Model.Entities;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Contract.Abstractions;
using Ecc.Contract.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecc.Logic.Integrations
{
    /// <summary>
    /// Имплементация клиенсткого хранилища
    /// </summary>
    public class CltUserMasterStorage : IUserMasterStorage
    {
        CltDbContext CltDbContext { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cltDbContext"></param>
        public CltUserMasterStorage(CltDbContext cltDbContext)
        {
            CltDbContext = cltDbContext;
        }

        /// <inheritdoc />
        public Task<EccUserModel> GetUserById(string userId)
        {
            return CltDbContext.Clients.Select(SelectExpression).FirstOrDefaultAsync(x => x.Id == userId);
        }


        /// <inheritdoc />
        public Task<GetListResult<EccUserModel>> GetUsers(GetListSearchModel model)
        {
            var query = CltDbContext.Clients
                .AsNoTracking()
                .OrderBy(x => x.CreatedOn);

            return EFCoreExtensions.GetAsync(model, query, SelectExpression);
        }

        private static readonly Expression<Func<Client, EccUserModel>> SelectExpression = x => new EccUserModel
        {
            Id = x.Id,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber
        };
    }
}