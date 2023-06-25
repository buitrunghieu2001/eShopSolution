using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Reviews;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Reviews
{
    public interface IReviewService
    {
        Task<PagedResult<ProductReview>> GetReviewsIsApproved(PagingRequestBase request);
        Task<PagedResult<ProductReview>> GetReviewsByProductId(GetManageReviewPagingRequest request);
        Task<List<ReviewVM>> GetReviewByUserId(int userId);
        Task<PagedResult<ReviewVM>> GetReviewsWaitApproved(PagingRequestBase request);   
        Task<List<ReviewVM>> GetReviewUser(int userId, int productId);
        Task<ApiResult<string>> Create(ReviewCreateRequest request);
        Task<ApiResult<string>> Delete(int reviewId);
        Task<ApiResult<string>> ReviewIsApproved(int reviewId);
        Task<ApiResult<string>> ReviewDisapproved(int reviewId);
        Task<ReviewVM> GetReviewById(int reviewId);
    }
}
