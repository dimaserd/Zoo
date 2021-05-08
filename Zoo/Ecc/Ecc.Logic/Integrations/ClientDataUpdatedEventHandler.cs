using Clt.Contract.Events;
using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Ecc.Logic.Services.Users;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecc.Logic.Integrations
{
    /// <summary>
    /// Обработчик события зарегистрированного пользователя
    /// </summary>
    public class ClientDataUpdatedEventHandler : CrocoMessageHandler<ClientDataUpdatedEvent>
    {
        UserService UserService { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        public ClientDataUpdatedEventHandler(ICrocoApplication application,
            ILogger<ClientRegisteredEventHandler> logger,
            UserService userService) : base(application, logger)
        {
            UserService = userService;
        }

        /// <inheritdoc/>
        public override Task HandleMessage(ClientDataUpdatedEvent model)
        {
            return UserService.UpdateUser(model.Id);
        }
    }
}