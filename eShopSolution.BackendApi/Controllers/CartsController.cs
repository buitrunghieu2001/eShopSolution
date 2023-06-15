using Azure.Core;
using eShopSolution.Application.Catalog.Cart;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ICartService _cartService;
        public CartsController(UserManager<AppUser> userManager, ICartService cartService)
        {
            _userManager = userManager;
            _cartService = cartService;
        }

        
        [HttpPost("{productId}")]
        public async Task<IActionResult> addToCart(int productId)
        {
            var userIdentity = User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userIdentity.Name || u.UserName == userIdentity.Name);
                var result = await _cartService.addToCart(productId, user.Id);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> getCartByUser(string languageId)
        {
            var userIdentity = User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userIdentity.Name || u.UserName == userIdentity.Name);
                var result = await _cartService.getCartByUser(user.Id, languageId);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> removeFromCart(int cartId)
        {
            var userIdentity = User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userIdentity.Name || u.UserName == userIdentity.Name);
                var result = await _cartService.removeFromCart(cartId, user.Id);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> updateQuantity(int cartId, int quantity)
        {
            var userIdentity = User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userIdentity.Name || u.UserName == userIdentity.Name);
                var result = await _cartService.updateQuantity(cartId, quantity);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
