using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : Controller
    {

        private readonly ICategoryService _categoryService;
        // khi ProductController được khởi tạo thì nó sẽ gọi Constructor
        // Constructor nó yêu cầu một đối tượng IPublicProductService, DI trong Program.cs đã được hưỡng dẫn
        // Sau đó nó sẽ gán đối tượng IPublicProductService vào publicProductService
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var product = await _categoryService.GetAll(languageId);
            return Ok(product);
        }
    }
}
