using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Text;

namespace eShopSolution.AdminApp.Services
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            :base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public Task<PagedResult<ProductVM>> GetPagings(GetManageProductPagingRequest request)
        {
            var data = GetAsync<PagedResult<ProductVM>>($"api/products/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.KeyWord}&languageId={request.LanguageId}");
            return data;
        }
    }
}
