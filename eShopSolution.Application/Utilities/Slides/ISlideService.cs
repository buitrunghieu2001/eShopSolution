using eShopSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Utilities.Slides
{
    public interface ISlideService
    {
        Task<List<SlideVM>> GetAll();
        Task<int> CreateSlide(SlideCreateRequest request);
        Task<int> UpdateSlide(int slideId, SlideUpdateRequest request);
        Task<int> DeleteSlide(int slideId);
        Task<SlideVM> GetById(int slideId);
    }
}
