using Azure.Core;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using System.Text;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // khi ProductController được khởi tạo thì nó sẽ gọi Constructor
        // Constructor nó yêu cầu một đối tượng IPublicProductService, DI trong Program.cs đã được hưỡng dẫn
        // Sau đó nó sẽ gán đối tượng IPublicProductService vào publicProductService
        public ProductsController(IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productService.GetAllPaging(request);
            return Ok(products);
        }

        // /product/1
        [HttpGet("{languageId}/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productService.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpGet("featured/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedProducts(int take, string languageId)
        {
            var products = await _productService.GetFeaturedProducts(languageId, take);
            return Ok(products);
        }

        [HttpGet("latest/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestProducts(int take, string languageId)
        {
            var products = await _productService.GetLatestProducts(languageId, take);
            return Ok(products);
        }

        [HttpGet("category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _productService.GetAllByCategoryId(request);
            return Ok(products);
        }

        [HttpGet("getallbycategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductByCategory(string languageId, int categoryId)
        {
            var products = await _productService.GetAllProductByCategory(languageId, categoryId);
            return Ok(products);
        }

        [HttpGet("category/{languageId}/{categoryId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLimitedProductsByCategory(int categoryId, string languageId, int take)
        {
            var products = await _productService.GetLimitedProductByCategory(languageId, categoryId, take);
            return Ok(products);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _productService.Create(request);
            if (productId == 0) return BadRequest();

            var product = await _productService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var affectedResult = await _productService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var affectedResult = await _productService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPatch("{productId}/{newPrice}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _productService.UpdatePrice(productId, newPrice);
            if (isSuccessful) return Ok();

            return BadRequest();
        }

        //Images
        [HttpPost("{productId}/images")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _productService.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _productService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _productService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find product");
            return Ok(image);
        }

        // /products/id/roles
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPatch]
        [Route("{id}/viewCount")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateViewCount(int id)
        {
            string sessionKey = $"LastViewIncreaseTime_{id}";

            // kiểm tra lastViewIncreaseTime có tồn tại trong session ko
            if (_httpContextAccessor.HttpContext.Session.TryGetValue(sessionKey, out byte[] value) &&
                DateTime.TryParse(Encoding.UTF8.GetString(value), out DateTime lastViewIncreaseTime))
            {
                // Kiểm tra và cập nhật giá trị lastViewIncreaseTime
                TimeSpan timeSinceLastIncrease = DateTime.Now - lastViewIncreaseTime;
                if (timeSinceLastIncrease.TotalMinutes >= 15)
                {
                    // Thực hiện tăng viewcount và cập nhật lastViewIncreaseTime
                    var result = await _productService.UpdateViewCount(id);
                    if (result.IsSuccessed)
                    {
                        lastViewIncreaseTime = DateTime.Now;
                        _httpContextAccessor.HttpContext.Session.Set(sessionKey, Encoding.UTF8.GetBytes(lastViewIncreaseTime.ToString()));
                        return Ok(result);
                    }
                }

                TimeSpan timeLeft = lastViewIncreaseTime.AddMinutes(15).Subtract(DateTime.Now);
                return BadRequest("Phát hiện hành vi bất thường!");
            }
            else
            {
                // Thực hiện tăng viewcount và lưu lastViewIncreaseTime lần đầu
                var result = await _productService.UpdateViewCount(id);
                if (result.IsSuccessed)
                {
                    lastViewIncreaseTime = DateTime.Now;
                    _httpContextAccessor.HttpContext.Session.Set(sessionKey, Encoding.UTF8.GetBytes(lastViewIncreaseTime.ToString()));
                    return Ok(result);
                }
                return BadRequest(result);
            }
        }
    }
}
