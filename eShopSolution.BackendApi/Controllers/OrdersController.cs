using eShopSolution.Application.Catalog.Cart;
using eShopSolution.Application.Catalog.Carts;
using eShopSolution.Application.Catalog.Orders;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Orders;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.System.Languages;
using eShopSolution.ViewModels.System.Users;
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
    public class OrdersController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderService _orderService;
        public OrdersController(UserManager<AppUser> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        [HttpGet("check-purchase")]
        public async Task<IActionResult> CheckPurchase([FromQuery] int productId)
        {
            var userIdentity = User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userIdentity.Name || u.UserName == userIdentity.Name);
                var purchaseStatus = await _orderService.HasUserPurchasedProductAsync(user.Id, productId);

                return Ok(purchaseStatus);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {
            var userIdentity = User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userIdentity.Name || u.UserName == userIdentity.Name);
                if (user != null)
                {
                    request.UserId = user.Id;
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("user"))
                    {
                        var result = await _orderService.Create(request);
                        return Ok(result);
                    }
                }
            }
            return BadRequest();
        }
    }
}
