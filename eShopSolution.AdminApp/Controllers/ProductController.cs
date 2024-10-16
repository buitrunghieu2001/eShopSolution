﻿using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IBrandApiClient _brandApiClient;
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration, ICategoryApiClient categoryApiClient, IBrandApiClient brandApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _brandApiClient = brandApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? keyWord, int? categoryId, int pageIndex = 1, int pageSize = 7)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetManageProductPagingRequest()
            {
                KeyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize, 
                LanguageId = languageId,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyWord;

            var categories = await _categoryApiClient.GetAll(languageId);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var result = await _productApiClient.GetById(id, languageId);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var categories = await _categoryApiClient.GetAll(languageId);
            var brands = await _brandApiClient.GetAll();
            ViewBag.Brands = brands;
            ViewBag.Categories = categories;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            // Kiểm tra nếu ModelState không hợp lệ
            if (!ModelState.IsValid)
            {
                var categories = await _categoryApiClient.GetAll(languageId);
                var brands = await _brandApiClient.GetAll();
                ViewBag.Brands = brands;
                ViewBag.Categories = categories;
                return View();
            }

            // Xử lý các trường decimal? (nếu cần giá trị mặc định)
            request.Price = request.Price == null ? 0m : request.Price;  // Gán 0m nếu Price là null
            request.OriginalPrice = request.OriginalPrice == null ? 0m : request.OriginalPrice;  // Gán 0m nếu OriginalPrice là null

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }


        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(int id, CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CategoryAssign(id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var categoryAssignRequest = await GetCategoryAssignRequest(request.Id);

            return View(categoryAssignRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var categories = await _categoryApiClient.GetAll(languageId);
            var brands = await _brandApiClient.GetAll();
            ViewBag.Brands = brands;
            ViewBag.Categories = categories;
            var product = await _productApiClient.GetById(id, languageId);
            var editVm = new ProductUpdateRequest()
            {
                Id = product.Id,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                BrandId = product.Brand.Id,
                Origin = product.Origin,
                Warranty = product.Warranty,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle
            };

            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            // kiểm tra validate
            if (!ModelState.IsValid)
            {
                var categories = await _categoryApiClient.GetAll(languageId);
                var brands = await _brandApiClient.GetAll();
                ViewBag.Brands = brands;
                ViewBag.Categories = categories;
                return View(request);
            }
            // Xử lý các trường decimal? (nếu cần giá trị mặc định)
            request.Price = request.Price == null ? 0m : request.Price;  // Gán 0m nếu Price là null
            request.OriginalPrice = request.OriginalPrice == null ? 0m : request.OriginalPrice;  // Gán 0m nếu OriginalPrice là null

            var result = await _productApiClient.UpdateProduct(request);
            if (result)
            {
                TempData["result"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var productObj = await _productApiClient.GetById(id, languageId);
            var categories = await _categoryApiClient.GetAll(languageId);
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var role in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = productObj.Categories.Contains(role.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var productObj = await _productApiClient.GetById(id, languageId);
            return View(new ProductDeleteRequest()
            {
                Name = productObj.Name,
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.DeleteProduct(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }
    }
}
