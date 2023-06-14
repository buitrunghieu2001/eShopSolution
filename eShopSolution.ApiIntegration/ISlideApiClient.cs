using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface ISlideApiClient
    {
        Task<List<SlideVM>> GetAll();
        Task<bool> CreateSlide(SlideCreateRequest request);
        Task<bool> UpdateSlide(int slideId, SlideUpdateRequest request);
        Task<bool> DeleteSlide(int slideId);
        Task<SlideVM> GetById(int slideId);
    }
}
