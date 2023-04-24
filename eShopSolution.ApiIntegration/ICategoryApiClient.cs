using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;

namespace eShopSolution.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVM>> GetAll(string languageId);
    }
}
