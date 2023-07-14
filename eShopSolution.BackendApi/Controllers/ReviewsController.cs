﻿using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Catalog.Reviews;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Reviews;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // khi ProductController được khởi tạo thì nó sẽ gọi Constructor
        // Constructor nó yêu cầu một đối tượng IPublicProductService, DI trong Program.cs đã được hưỡng dẫn
        // Sau đó nó sẽ gán đối tượng IPublicProductService vào publicProductService
        public ReviewsController(IReviewService reviewService, IHttpContextAccessor httpContextAccessor)
        {
            _reviewService = reviewService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("approved")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetReviewsIsApproved([FromQuery] PagingRequestBase request)
        {
            var products = await _reviewService.GetReviewsIsApproved(request);
            return Ok(products);
        }

        [HttpGet("product")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsByProductId([FromQuery] GetManageReviewPagingRequest request)
        {
            var result = await _reviewService.GetReviewsByProductId(request);
            
            return Ok(result);
        }

        [HttpGet("waitapproved")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetReviewsWaitApproved([FromQuery] PagingRequestBase request)
        {
            var result = await _reviewService.GetReviewsWaitApproved(request);

            return Ok(result);
        }

        [HttpDelete("{reviewId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var result = await _reviewService.Delete(reviewId);
            if (result.IsSuccessed == false)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReview([FromForm] ReviewCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reviewService.Create(request);

            return Ok(result);
        }

        [HttpGet("{reviewId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(int reviewId)
        {
            var products = await _reviewService.GetReviewById(reviewId);
            return Ok(products);
        }

        [HttpPatch("approved")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ReviewIsApproved(int reviewId)
        {
            var result = await _reviewService.ReviewIsApproved(reviewId);
            if (result.IsSuccessed == false)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("disapproved")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ReviewDisapproved(int reviewId)
        {
            var result = await _reviewService.ReviewDisapproved(reviewId);
            if (result.IsSuccessed == false)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
