using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<ProductVM> GetById(int productId, string languageId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addedQuantity);

        // Kiểu trả về void
        Task AddViewCount(int productId);
        Task<PagedResult<ProductVM>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<List<ProductImageViewModel>> GetListImages(int productId);
        Task<PagedResult<ProductVM>> GetAllByCategoryId(GetPublicProductPagingRequest request);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<List<ProductVM>> GetFeaturedProducts(string languageId, int take);
        Task<List<ProductVM>> GetBrandProducts(int brandId, string languageId, int take);
        Task<List<ProductVM>> GetLatestProducts(string languageId, int take);
        Task<List<ProductVM>> GetAllProductByCategory(string languageId, int categoryId);
        Task<List<ProductVM>> GetLimitedProductByCategory(string languageId, int categoryId, int take);
        Task<ApiResult<int>> UpdateViewCount(int id);
    }
}
