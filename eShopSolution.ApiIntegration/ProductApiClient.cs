﻿using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace eShopSolution.ApiIntegration
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            :base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/products/{request.Id}/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<bool> CreateProduct(ProductCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            //var client = new HttpClient();
            // _configuration["BaseAddress"]: lấy từ appsetting dev
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Details.ToString()), "details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(languageId), "languageId");

            var reponse = await client.PostAsync($"/api/products/", requestContent);
            return reponse.IsSuccessStatusCode;
        }

        public async Task<ProductVM> GetById(int id, string languageId)
        {
            var data = await GetAsync<ProductVM>($"api/products/{id}/{languageId}");
            return data;
        }

        public async Task<List<ProductVM>> GetFeaturedProducts(string languageId, int take)
        {
            var data = await GetListAsync<ProductVM>($"/api/products/featured/{languageId}/{take}");
            return data;
        }

        public async Task<List<ProductVM>> GetLatestProducts(string languageId, int take)
        {
            var data = await GetListAsync<ProductVM>($"/api/products/latest/{languageId}/{take}");
            return data;
        }

        public async Task<PagedResult<ProductVM>> GetPagings(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<ProductVM>>($"api/products/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.KeyWord}&languageId={request.LanguageId}&categoryId={request.CategoryId}");
            return data;
        }
    }
}
