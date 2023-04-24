using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVM>> GetPagings(GetManageProductPagingRequest request);
        Task<bool> CreateProduct(ProductCreateRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ProductVM> GetById(int id, string languageId);
    }
}
