using eShopSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAll(string languageId);
        Task<int> Create(CategoryCreateRequest request);
        Task<CategoryVM> GetById(int categoryTranslationId);
        Task<int> Update(int categoryTranslationId, CategoryUpdateRequest request);
        Task<CategoryVM> GetBySeoAlias(string seoAlias);
    }
}
