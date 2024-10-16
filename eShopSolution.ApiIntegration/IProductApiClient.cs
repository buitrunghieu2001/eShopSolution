﻿using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVM>> GetPagings(GetManageProductPagingRequest request);
        Task<bool> CreateProduct(ProductCreateRequest request);
        Task<bool> UpdateProduct(ProductUpdateRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ProductVM> GetById(int id, string languageId);
        Task<List<ProductVM>> GetFeaturedProducts(string languageId, int take);
        Task<List<ProductVM>> GetLatestProducts(string languageId, int take);
        Task<List<ProductVM>> GetLimitedProductByCategory(string languageId, int categoryId, int take);
        Task<bool> DeleteProduct(int id);
    }
}
