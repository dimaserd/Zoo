using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Croco.Core.Contract.Models;
using Ecc.Contract.Models.Emails;

namespace Ecc.Logic.Abstractions
{
    public interface IEmailSender
    {
        Task<List<(TModel, BaseApiResponse)>> SendEmails<TModel>(IEnumerable<TModel> messages, Func<TModel, SendEmailModel> mapper);
    }
}