using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Ecc.Contract.Commands;
using Ecc.Logic.Services.Users;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    /// <summary>
    /// Обработчик для команды создания пользователя
    /// </summary>
    public class CreateUserCommandHandler : CrocoMessageHandler<CreateUserCommand>
    {
        UserService UserService { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        public CreateUserCommandHandler(ICrocoApplication application, 
            ILogger<CreateUserCommandHandler> logger,
            UserService userService) : base(application, logger)
        {
            UserService = userService;
        }

        /// <summary>
        /// Обработать сообщение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Task HandleMessage(CreateUserCommand model)
        {
            return UserService.CreateUser(model);
        }
    }
}