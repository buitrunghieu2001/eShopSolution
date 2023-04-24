using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.ApiIntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<List<CategoryVM>> GetAll(string languageId)
        {
            return await GetListAsync<CategoryVM>($"/api/categories?languageId={languageId}");
        }
    }
}
