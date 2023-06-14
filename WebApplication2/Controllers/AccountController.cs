using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Identity;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace eShopSolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public AccountController(IUserApiClient userApiClient,
            IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.Authenticate(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", "Login failure");
                return View();
            }

            var userPrincipal = this.ValidateToken(result.ResultObj);
            // thiết lập thời gian tồn tại của cookie
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            TempData["Token"] = result.ResultObj;

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(registerRequest);
            }

            var result = await _userApiClient.RegisterUser(registerRequest);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        public IActionResult Info()
        {
            return View();
        }



        private string GetRoleFromJWT(string jwtToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(jwtToken);

            // Lấy danh sách các Claim trong token
            var claims = token.Claims;

            // Tìm và lấy thông tin vai trò
            var roleClaim = claims.FirstOrDefault(c => c.Type == "user" || c.Type == "user");
            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                return role;
            }

            // Nếu không tìm thấy vai trò trong token
            return null;
        }
    }
}
