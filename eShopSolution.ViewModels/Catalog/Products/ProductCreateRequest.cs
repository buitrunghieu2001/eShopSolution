using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập giá bán")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập giá gốc")]
        public decimal OriginalPrice { get; set; }

        [Required(ErrorMessage = "Bạn phải số lượng")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập tên sản phẩm")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Bạn phải nhập mô tả")]
        public string? Description { set; get; }

        [Required(ErrorMessage = "Bạn phải chi tiết")]
        public string? Details { set; get; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string? SeoDescription { set; get; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string? SeoTitle { set; get; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string? SeoAlias { get; set; }
        public string? LanguageId { set; get; }
        public bool? IsFeatured { get; set; }
        
        [Required(ErrorMessage = "Bạn phải nhập danh mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string Origin { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string Warranty { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public IFormFile ThumbnailImage { get; set; }
    }
}
