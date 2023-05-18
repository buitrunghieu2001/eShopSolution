using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Category(int id)
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}
