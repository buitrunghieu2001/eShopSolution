using Azure.Core;
using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.utilities.Exceptions;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        // _context: đối tượng của lớp EShopDbContext
        private readonly EShopDbContext _context;
        // constructor
        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var languages = _context.Languages;
            var translations = new List<CategoryTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    translations.Add(new CategoryTranslation()
                    {
                        Name = request.Name,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId,
                    });
                }
                else
                {
                    translations.Add(new CategoryTranslation()
                    {
                        Name = SystemConstants.ProductConstants.NA,
                        SeoDescription = SystemConstants.ProductConstants.NA,
                        SeoAlias = SystemConstants.ProductConstants.NA,
                        SeoTitle = SystemConstants.ProductConstants.NA,
                        LanguageId = language.Id,
                    });
                }
            }

            var sort = await _context.Categories.MaxAsync(x => x.SortOrder);

            var category = new Category ()
            {
                SortOrder = sort + 1,
                ParentId = request.ParentId,
                IsShowOnHome = true,
                Status = (Status)1,
                CategoryTranslations = translations,
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<CategoryVM> GetById(int categoryTranslationId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.Id == categoryTranslationId
                        select new { c, ct };
            var result = await query.FirstOrDefaultAsync();

            var category = new CategoryVM
            {
                Id = result.c.Id,
                Name = result.ct.Name,
                Status = result.c.Status,
                SortOrder = result.c.SortOrder,
                ParentId = result.c.ParentId,
                SeoTitle = result.ct.SeoTitle,
                SeoDescription = result.ct.SeoDescription,
                SeoAlias = result.ct.SeoAlias,
            };

            return category;
        }

        public async Task<List<CategoryVM>> GetAll(string languageId)
        {
            // step 1: select join
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            
            return await query.Select(x => new CategoryVM()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                Status = x.c.Status,
                SortOrder = x.c.SortOrder,
                ParentId = x.c.ParentId,
                SeoTitle = x.ct.SeoTitle,
                SeoDescription = x.ct.SeoDescription,
                SeoAlias = x.ct.SeoAlias,
            }).ToListAsync();
        }

        public async Task<int> Update(int categoryTranslationId, CategoryUpdateRequest request)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.Id == categoryTranslationId
                        select new { c, ct };
            var result = await query.FirstOrDefaultAsync();

            if (result == null) throw new EShopException($"Cannot find a category with id: {categoryTranslationId}");

            result.c.SortOrder = request.SortOrder;
            result.c.ParentId = request.ParentId;
            result.c.Status = (Status)request.Status;
            result.ct.SeoDescription = request.SeoDescription;
            result.ct.SeoTitle = request.SeoTitle;
            result.ct.SeoAlias = request.SeoAlias;
            result.ct.Name = request.Name;

            _context.Categories.Update(result.c);
            _context.CategoryTranslations.Update(result.ct);
            return await _context.SaveChangesAsync();
        }
    }
}
