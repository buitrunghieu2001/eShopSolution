using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace eShopSolution.AdminApp.Services
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected BaseApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            //var client = new HttpClient();
            // _configuration["BaseAddress"]: lấy từ appsetting dev
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"{url}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                TResponse myDeserializedObject = (TResponse)JsonConvert.DeserializeObject(body, typeof(TResponse));
                return myDeserializedObject;
            }

            return JsonConvert.DeserializeObject<TResponse>(body);
        }
    }
}
