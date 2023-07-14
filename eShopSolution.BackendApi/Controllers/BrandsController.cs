using eShopSolution.Application.Catalog.Cart;
using eShopSolution.Application.Catalog.Carts;
using eShopSolution.Application.System.Brands;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Brands;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BrandsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBrandService _brandService;
        public BrandsController(UserManager<AppUser> userManager, IBrandService brandService)
        {
            _userManager = userManager;
            _brandService = brandService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAll();
            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(BrandCreateRequest request)
        //{
        //    var result = await _brandService.Create(request);
        //    return Ok(result);
        //}
    }
}
