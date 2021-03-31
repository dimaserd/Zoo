using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Contract.Models.EmailRedirects;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.LinkCatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecc.Logic.Services.EmailRedirects
{
    /// <summary>
    /// Сервис для работы с методами поиска редиректов в Email
    /// </summary>
    public class EmailRedirectsQueryWorker : BaseEccService
    {
        static readonly Expression<Func<EmailLinkCatch, EmailLinkCatchRedirectsCountModel>> SelectExpression = x => new EmailLinkCatchRedirectsCountModel
        {
            Id = x.Id,
            Url = x.Url,
            Count = x.Redirects.Count
        };

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public EmailRedirectsQueryWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        /// <summary>
        /// Получить список пойманных переходов со счетчиками
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<EmailLinkCatchRedirectsCountModel>> GetCatchesByEmailId(string id)
        {
            return Query<EmailLinkCatch>().Where(x => x.MailMessageId == id)
                .OrderByDescending(x => x.CreatedOnUtc).Select(SelectExpression)
                .ToListAsync();
        }

        /// <summary>
        /// Искать ловителей переходов
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<EmailLinkCatchRedirectsCountModel>> Query(GetListSearchModel model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailLinkCatch>().OrderByDescending(x => x.CreatedOnUtc), SelectExpression);
        }

        /// <summary>
        /// Получить список всех переходов по данному ловителю переходов
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EmailLinkCatchDetailedModel> GetById(string id)
        {
            return Query<EmailLinkCatch>().Select(x => new EmailLinkCatchDetailedModel
            {
                Id = x.Id,
                RedirectsOn = x.Redirects.Select(t => t.RedirectedOnUtc).ToList(),
                Url = x.Url
            }).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}