using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Ecc.Contract.Commands;
using Ecc.Logic.Services.Users;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    public class UpdateUserCommandHandler : CrocoMessageHandler<UpdateUserCommand>
    {
        UserService UserService { get; }

        public UpdateUserCommandHandler(ICrocoApplication application,
            ILogger<UpdateUserCommandHandler> logger,
            UserService userService) : base(application, logger)
        {
            UserService = userService;
        }


        public override Task HandleMessage(UpdateUserCommand model)
        {
            return UserService.UpdateUser(model);
        }
    }
}