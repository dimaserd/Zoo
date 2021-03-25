using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Ecc.Contract.Commands;
using Ecc.Logic.Workers.Users;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    public class CreateUserCommandHandler : CrocoMessageHandler<CreateUserCommand>
    {
        UserService UserService { get; }

        public CreateUserCommandHandler(ICrocoApplication application, 
            ILogger<CreateUserCommandHandler> logger,
            UserService userService) : base(application, logger)
        {
            UserService = userService;
        }

        
        public override Task HandleMessage(CreateUserCommand model)
        {
            return UserService.CreateUser(model);
        }
    }
}