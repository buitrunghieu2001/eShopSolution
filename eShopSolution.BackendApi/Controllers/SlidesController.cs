using eShopSolution.Application.Utilities.Slides;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SlidesController : ControllerBase
    {
        private readonly ISlideService _slideService;

        public SlidesController(ISlideService slideService)
        {
            _slideService = slideService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _slideService.GetAll();
            return Ok(slides);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateSlide([FromForm] SlideCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var slideId = await _slideService.CreateSlide(request);
            if (slideId == 0) return BadRequest();

            //var slide = await _slideService.GetById(slideId);

            return Ok();
        }

        [HttpPut("{slideId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateSlide([FromRoute] int slideId, [FromForm] SlideUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _slideService.UpdateSlide(slideId, request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok(affectedResult);
        }

        [HttpDelete("{slideId}")]
        public async Task<IActionResult> DeleteProduct(int slideId)
        {
            var affectedResult = await _slideService.DeleteSlide(slideId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok(affectedResult);
        }

        // /product/1
        [HttpGet("{slideId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int slideId)
        {
            var slide = await _slideService.GetById(slideId);
            if (slide == null)
                return BadRequest("Cannot find product");
            return Ok(slide);
        }
    }
}
