using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Brands;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class BrandApiClient : BaseApiClient, IBrandApiClient
    {
        public BrandApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<List<BrandVM>> GetAll()
        {
            return await GetAsync<List<BrandVM>>("/api/Brands");
        }
    }
}
