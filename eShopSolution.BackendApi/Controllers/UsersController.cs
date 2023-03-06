﻿using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        //AllowAnonymous: chưa đăng nhập vẫn có thể gọi được
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);      
            
            var resultToken = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(resultToken))
                return BadRequest("Username or Password is incorrect");

            return Ok(resultToken);
        }

        // /users/register
        [HttpPost("register")]
        //AllowAnonymous: chưa đăng nhập vẫn có thể gọi được
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result)
                return BadRequest("Register unsuccessful");
                
            return Ok();
        }

        // /users?pageIndex=1&pageSize=10&keyword=admin
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPaging([FromQuery]GetUserPagingRequest request)
        {
            var products = await _userService.GetUsersPaging(request);
            return Ok(products);
        }
    }
}
