using Croco.Core.Abstractions.Models;
using Croco.Core.Abstractions.Models.Search;
using System.Threading.Tasks;

namespace Zoo.GenericUserInterface.Services
{
    public interface CrudGenericUserInterface<TListSearchModel, TModel, TCreateOrUpdateModel>
        where TListSearchModel : GetListSearchModel
        where TModel : class
        where TCreateOrUpdateModel : class
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<GetListResult<TModel>> GetList(TListSearchModel model);

        /// <summary>
        /// Создать
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseApiResponse<string>> Create(TCreateOrUpdateModel model);

        /// <summary>
        /// Обновить
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseApiResponse> Update(string id, TCreateOrUpdateModel model);
    }
}