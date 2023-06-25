using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.Data.Enums;

namespace eShopSolution.ApiIntegration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SlideApiClient(IHttpClientFactory httpClientFactory,
                    IConfiguration configuration,
                   IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CreateSlide(SlideCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            //var client = new HttpClient();
            // _configuration["BaseAddress"]: lấy từ appsetting dev
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            if (request.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Image", request.Image.FileName);
            }
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.Url.ToString()), "url");
            requestContent.Add(new StringContent(request.SortOrder.ToString()), "sortOrder");
            requestContent.Add(new StringContent(request.Status.ToString()), "status");

            var reponse = await client.PostAsync($"/api/slides/", requestContent);
            return reponse.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSlide(int slideId)
        {
            return await Delete($"/api/slides/" + slideId);
        }

        public async Task<List<SlideVM>> GetAll()
        {
            return await GetListAsync<SlideVM>("/api/slides");
        }

        public async Task<List<SlideVM>> GetSlideActive()
        {
            return await GetListAsync<SlideVM>("/api/slides/active");
        }

        public async Task<SlideVM> GetById(int slideId)
        {
            var data = await GetAsync<SlideVM>($"api/slides/{slideId}");
            return data;
        }

        public async Task<bool> UpdateSlide(int slideId, SlideUpdateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "image", request.Image.FileName);
            }

            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.Url.ToString()), "url");
            requestContent.Add(new StringContent(request.SortOrder.ToString()), "sortOrder");
            requestContent.Add(new StringContent(request.Status.ToString()), "status");

            var response = await client.PutAsync($"/api/slides/" + slideId, requestContent);
            return response.IsSuccessStatusCode;
        }
    }
}
