using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using eShopSolution.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace eShopSolution.AdminApp.Services
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<List<RoleVM>>> GetAll()
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            //var client = new HttpClient();
            // _configuration["BaseAddress"]: lấy từ appsetting dev
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/roles");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<RoleVM> myDeserializedObject = (List<RoleVM>)JsonConvert.DeserializeObject(body, typeof(List<RoleVM>));
                return new ApiSuccessResult<List<RoleVM>>(myDeserializedObject);
            }

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<RoleVM>>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<List<RoleVM>>>(body);
        }
    }
}
