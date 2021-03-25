using Croco.Core.Contract.Files;

namespace Ecc.Contract.Models
{
    public class EccFileData : IFileData
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] FileData { get; set; }
    }
}