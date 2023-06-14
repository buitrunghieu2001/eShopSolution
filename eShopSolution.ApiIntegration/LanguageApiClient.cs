using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.ApiIntegration
{
    public class LanguageApiClient : BaseApiClient, ILanguageApiClient
    {
        public LanguageApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<List<LanguageVM>>> GetAll()
        {
            return await GetAsync<ApiResult<List<LanguageVM>>>("/api/languages");
        }
    }
}
