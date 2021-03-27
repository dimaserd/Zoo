using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.External;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Files
{
    public class EccFileService : BaseEccWorker
    {
        public EccFileService(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        public async Task<BaseApiResponse> CreateFiles(int[] fileIds)
        {
            if(fileIds == null)
            {
                return new BaseApiResponse(false, "fileIds is null");
            }

            var files = fileIds.Select(x => new EccFile
            {
                Id = x
            });

            CreateHandled(files);

            return await TrySaveChangesAndReturnResultAsync("Ok");
        }
    }
}