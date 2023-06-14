using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.System.Roles;
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var product = await _categoryService.GetAll(languageId);
            return Ok(product);
        }

        [HttpGet("{categoryTranslationId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute]int categoryTranslationId)
        {
            var product = await _categoryService.GetById(categoryTranslationId);
            return Ok(product);
        }


        [HttpPut("{categoryTranslationId}")]
        public async Task<IActionResult> Update([FromRoute] int categoryTranslationId,[FromForm] CategoryUpdateRequest request)
        {
            var product = await _categoryService.Update(categoryTranslationId, request);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryService.Create(request);
            if (result == 0) return BadRequest(result);
            return Ok(result);
        }
    }
}
