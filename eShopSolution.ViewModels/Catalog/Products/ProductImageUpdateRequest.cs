using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductImageUpdateRequest
    {
        public int Id { get; set; }

        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        public int SortOrder { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
