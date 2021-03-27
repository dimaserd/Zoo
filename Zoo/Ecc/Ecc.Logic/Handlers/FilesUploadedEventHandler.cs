using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Croco.Core.Logic.Files.Events;
using Ecc.Logic.Workers.Files;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    public class FilesUploadedEventHandler : CrocoMessageHandler<FilesUploadedEvent>
    {
        EccFileService EccFileService { get; }

        public FilesUploadedEventHandler(ICrocoApplication application, 
            ILogger<FilesUploadedEventHandler> logger,
            EccFileService eccFileService
            ) : base(application, logger)
        {
            EccFileService = eccFileService;
        }

        public override Task HandleMessage(FilesUploadedEvent model)
        {
            return EccFileService.CreateFiles(model.FileIds);
        }
    }
}