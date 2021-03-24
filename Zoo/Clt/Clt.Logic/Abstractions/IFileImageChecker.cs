using Croco.Core.Contract.Files;

namespace Clt.Logic.Abstractions
{
    public interface IFileImageChecker
    {
        public bool IsImage(IFileData fileData);
    }
}
