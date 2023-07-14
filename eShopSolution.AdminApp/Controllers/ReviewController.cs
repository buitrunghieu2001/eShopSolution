using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Approve()
        {
            return View();
        }
    }
}
