using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Utils;

namespace Ecc.Logic.Services
{
    public class EmailListExtractor 
    {
        public EmailListExtractor(ICrocoApplication app)
        {
            App = app;
        }

        ICrocoApplication App { get; }

        string MapPath(string filePath) => App.MapPath(filePath);

        public async Task<BaseApiResponse<List<string>>> ExtractEmailsListFromFile(string filePath)
        {
            filePath = MapPath(filePath);

            if (!File.Exists(filePath))
            {
                return new BaseApiResponse<List<string>>(false, "Файл не существует по указанному пути");
            }

            var res = Tool.JsonConverter.Deserialize<List<string>>(await File.ReadAllTextAsync(filePath));

            return new BaseApiResponse<List<string>>(true, "Ok", res);
        }
    }
}