﻿using Azure.Core;
using eShopSolution.ApiIntegration;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.WebApp.Models;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        //đối tượng logger được sử dụng để ghi lại các thông tin, cảnh báo, lỗi và thông tin khác trong ứng dụng
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(
            IProductApiClient productApiClient,
            ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public IActionResult Category(string category)
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            return View();
        }

        public IActionResult Search(GetManageProductPagingRequest request)
        {
            return View();
        }
    }
}
