using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Ecc.Contract.Commands;
using Ecc.Logic.Services.Users;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    /// <summary>
    /// Обработчик события обновления пользовательских данных
    /// </summary>
    public class UpdateUserCommandHandler : CrocoMessageHandler<UpdateUserCommand>
    {
        UserService UserService { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        public UpdateUserCommandHandler(ICrocoApplication application,
            ILogger<UpdateUserCommandHandler> logger,
            UserService userService) : base(application, logger)
        {
            UserService = userService;
        }


        /// <summary>
        /// Обработчик
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Task HandleMessage(UpdateUserCommand model)
        {
            return UserService.UpdateUser(model);
        }
    }
}