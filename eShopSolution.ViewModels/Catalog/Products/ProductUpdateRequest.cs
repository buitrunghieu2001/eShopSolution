using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Brands;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public decimal OriginalPrice { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string? Origin { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string? Warranty { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Bạn phải nhập danh mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string Details { set; get; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string SeoDescription { set; get; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string SeoTitle { set; get; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string SeoAlias { get; set; }

        public string? LanguageId { set; get; }
        public bool? IsFeatured { get; set; }
        public IFormFile? ThumbnailImage { get; set; }

    }
}
