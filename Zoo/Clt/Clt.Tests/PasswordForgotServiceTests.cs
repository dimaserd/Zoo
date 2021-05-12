using Clt.Contract.Models.Account;
using Clt.Logic.Services.Account;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Extensions;
using Croco.Testing.Impelementations;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clt.Tests
{
    public class PasswordForgotServiceTests
    {
        [TestCase("ds@mail.com")]
        public async Task Test(string email)
        {
            var srvProvider = await TestBuilder.BuildCltAppAndGetServiceProvider();

            srvProvider = srvProvider.GetRequiredService<ICrocoApplication>()
                .GetTransactionHandler()
                .CrocoTransaction.ServiceProvider;

            var testBasePrincipal = new TestPrincipalBase(new TestCrocoIdentity());
            var testCrocoPrincipal = new TestCrocoPrincipal(testBasePrincipal, Guid.NewGuid().ToString());
            testCrocoPrincipal.SetAuthenticatation(false);
            var testRequestContext = new TestCrocoRequestContext(testCrocoPrincipal);

            srvProvider.GetRequiredService<ICrocoRequestContextAccessor>().SetRequestContext(testRequestContext);

            var regResp = await srvProvider.GetRequiredService<AccountRegistrationService>().RegisterAsync(new RegisterModel
            {
                Email = email,
                Name = "Name",
                Password = "SomePass@1234",
                Patronymic = "Ssafsd",
                PhoneNumber = "89166044960",
                Surname = "Asdhfds"
            }, false);

            //Убираем из очереди сообщение о регистрации клиента
            await EventSourcerExtensions.GetTaskForHandlingOneIntegrationMessage(srvProvider);

            Assert.IsTrue(regResp.IsSucceeded);

            var resp = await srvProvider.GetRequiredService<PasswordForgotService>().UserForgotPassword(new ForgotPasswordModel
            {
                Email = email
            });

            Assert.IsTrue(resp.IsSucceeded);

            //Получаем сообщение 
            var integrationMessage = await EventSourcerExtensions.GetTaskForHandlingOneIntegrationMessage(srvProvider);

            Assert.IsNotNull(integrationMessage);
        }
    }
}
