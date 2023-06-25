using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Brands;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Brands
{
    public interface IBrandService
    {
        Task<List<BrandVM>> GetAll();
        //Task<ApiResult<bool>> Create(BrandCreateRequest request);
    }
}
