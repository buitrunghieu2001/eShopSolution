using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShopSolution.WebApp.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;

        public PagesController(ILogger<PagesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SanPham()
        {
            return View();
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
    }
}