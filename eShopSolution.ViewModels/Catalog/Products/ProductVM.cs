using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.System.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductVM
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int Rating { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }
        public string Origin { get; set; }
        public string Warranty { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string LanguageId { set; get; }
        public string SeoAlias { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public bool? IsFeatured { get; set; }
        public string ThumbnailImage { get; set; }
        public BrandVM Brand { get; set; }
        public List<string> Categories { get; set; }
    }
}
