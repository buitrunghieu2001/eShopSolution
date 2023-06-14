using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Utilities.Slides;
using System.Collections.Generic;

namespace eShopSolution.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideVM> Slides { get; set; }
        public List<ProductVM> FeaturedProducts { get; set; }
        public List<ProductVM> LatestProducts { get; set; }
        public List<ProductVM> DigitalCameraProducts { get; set; }
        public List<ProductVM> LensesProducts { get; set; }
        public List<ProductVM> FirmCameraProducts { get; set; }
        public List<ProductVM> CameraAccessoriesProducts { get; set; }
    }
}
