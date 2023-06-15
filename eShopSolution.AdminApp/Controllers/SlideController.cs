using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;
using eShopSolution.Data.Enums;
using System;

namespace eShopSolution.AdminApp.Controllers
{
    public class SlideController : Controller
    {
        private readonly ISlideApiClient _slideApiClient;
        public SlideController(ISlideApiClient slideApiClient)
        {
            _slideApiClient = slideApiClient;
        }
        public async Task<IActionResult> Index()
        {
            var slides = await _slideApiClient.GetAll();
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(slides);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Status model = new Status();
            ViewBag.Status = Enum.GetValues(typeof(Status)).Cast<Status>();
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] SlideCreateRequest request)
        {
            // kiểm tra validate
            if (!ModelState.IsValid)
                return View();

            var result = await _slideApiClient.CreateSlide(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Status model = new Status();
            ViewBag.Status = Enum.GetValues(typeof(Status)).Cast<Status>();
            var slide = await _slideApiClient.GetById(id);
            var editVm = new SlideUpdateRequest()
            {
                Id = slide.Id,
                Name = slide.Name,
                Description = slide.Description,
                Price = slide.Price,
                Url = slide.Url,
                SortOrder = slide.SortOrder,
                Status = slide.Status,
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(int Id, [FromForm] SlideUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _slideApiClient.UpdateSlide(Id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật slide thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật slide thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _slideApiClient.GetById(Id);
            return View(new SlideDeleteRequest()
            {
                Name = result.Name,
                Id = Id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SlideDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _slideApiClient.DeleteSlide(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa slide thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa slide không thành công");
            return View(request);
        }
    }
}
