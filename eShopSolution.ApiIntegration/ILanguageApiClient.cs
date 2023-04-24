using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;

namespace eShopSolution.ApiIntegration
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageVM>>> GetAll();
    }
}
