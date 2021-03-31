using Croco.Core.Contract.Application;
using Croco.Core.EventSourcing.Implementations;
using Croco.Core.Logic.Files.Events;
using Ecc.Logic.Services.Files;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    /// <summary>
    /// Обработчик события загруженных файлов
    /// </summary>
    public class FilesUploadedEventHandler : CrocoMessageHandler<FilesUploadedEvent>
    {
        EccFileService EccFileService { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        /// <param name="eccFileService"></param>
        public FilesUploadedEventHandler(ICrocoApplication application, 
            ILogger<FilesUploadedEventHandler> logger,
            EccFileService eccFileService
            ) : base(application, logger)
        {
            EccFileService = eccFileService;
        }

        /// <summary>
        /// Обработать сообщение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Task HandleMessage(FilesUploadedEvent model)
        {
            return EccFileService.CreateFiles(model.FileIds);
        }
    }
}