using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.WebApp.Models;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eShopSolution.WebApp.Controllers
{
    public class HomeController : Controller
    {
        //đối tượng logger được sử dụng để ghi lại các thông tin, cảnh báo, lỗi và thông tin khác trong ứng dụng
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public HomeController(ILogger<HomeController> logger,
            ISlideApiClient slideApiClient,
            IProductApiClient productApiClient,
            ICategoryApiClient categoryApiClient)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }


        public async Task<IActionResult> Index()
        {
            // get value use key in file .resx
            // var msg = _loc.GetLocalizedString("Vietnamese");
            var culture = CultureInfo.CurrentCulture.Name;
            var viewModel = new HomeViewModel
            {
                Slides = await _slideApiClient.GetSlideActive(),
                FeaturedProducts = await _productApiClient.GetFeaturedProducts(culture, SystemConstants.ProductSettings.NumberOfFeaturedProducts),
                LatestProducts = await _productApiClient.GetLatestProducts(culture, SystemConstants.ProductSettings.NumberOfLatestProducts),
                DigitalCameraProducts = await _productApiClient.GetLimitedProductByCategory(culture, 1, 6),
                LensesProducts = await _productApiClient.GetLimitedProductByCategory(culture, 2, 6),
                FirmCameraProducts = await _productApiClient.GetLimitedProductByCategory(culture, 1001, 6),
                CameraAccessoriesProducts = await _productApiClient.GetLimitedProductByCategory(culture, 1002, 6),
            };
            // tempdate tồn tại giữa các action hoặc controller
            var myData = TempData["Token"] as string;
            ViewBag.MyData = myData;
            return View(viewModel);
        }

        public IActionResult VeChungToi()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult LienHe()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //cookie để lưu trữ thông tin về ngôn ngữ(culture) được chọn bởi người dùng
        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}