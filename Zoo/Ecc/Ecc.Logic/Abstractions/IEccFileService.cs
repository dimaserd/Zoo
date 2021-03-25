using Ecc.Contract.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecc.Logic.Abstractions
{
    public interface IEccFileService
    {
        public Task<List<EccFileData>> GetFileDatas(int[] fileIds);
    }
}