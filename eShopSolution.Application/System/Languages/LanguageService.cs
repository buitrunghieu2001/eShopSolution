using eShopSolution.Application.System.Roles;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _configuration;
        private readonly EShopDbContext _context;
        public LanguageService(IConfiguration configuration, EShopDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        async Task<ApiResult<List<LanguageVM>>> ILanguageService.GetAll()
        {
            var languages = await _context.Languages.Select(x => new LanguageVM()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return new ApiSuccessResult<List<LanguageVM>>(languages);
        }
    }
}
