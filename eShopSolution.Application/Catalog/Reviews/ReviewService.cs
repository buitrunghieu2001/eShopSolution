using Azure;
using Azure.Core;
using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.utilities.Exceptions;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Reviews;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Reviews
{
    public class ReviewService : IReviewService
    {
        // _context: đối tượng của lớp EShopDbContext
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        // constructor
        public ReviewService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<string>> Create(ReviewCreateRequest request)
        {
            var review = await _context.ProductReviews.FirstOrDefaultAsync(r =>  r.ProductId == request.ProductId && r.PhoneNumber == request.PhoneNumber);
            if (review != null)
            {
                return new ApiErrorResult<string>("Bạn đã đánh giả sản phẩm này.");
            }
            var productReview = new ProductReview()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Content = request.Content,
                Rating = request.Rating,
                IsApproved = 0,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateCreated = DateTime.Now,
            };

            var reviewImage = _context.ReviewImages.FirstOrDefault(i => i.ReviewId == request.ProductId);

            //Save image
            if (request.ImagePath != null)
            {
                productReview.ReviewImage = new List<ReviewImage>()
                {
                    new ReviewImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ImagePath.Length,
                        ImagePath = await this.SaveFile(request.ImagePath),
                        IsDefault = true,
                        SortOrder = reviewImage != null ? reviewImage.SortOrder + 1 : 1
                    }
                };
            }
            _context.ProductReviews.Add(productReview);
            if (await _context.SaveChangesAsync() != 0)
            {
                return new ApiSuccessResult<string>("Đánh giá đã được cho vào danh sách duyệt.");
            }
            return new ApiErrorResult<string>("Vui lòng thử lại sau.");
        }

        public async Task<ApiResult<string>> Delete(int reviewId)
        {
            var review = await _context.ProductReviews.FindAsync(reviewId);
            if (review == null) throw new EShopException($"Không tìm thấy đánh giá: {reviewId}");

            var images = _context.ReviewImages.Where(i => i.ReviewId == reviewId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.ProductReviews.Remove(review);
            if (await _context.SaveChangesAsync() != 0)
            {
                return new ApiSuccessResult<string>("Xóa thành công đánh giá.");
            }
            return new ApiErrorResult<string>("Xóa đánh giá thất bại.");
        }

        public async Task<PagedResult<ProductReview>> GetReviewsIsApproved(PagingRequestBase request)
        {
            var query = from r in _context.ProductReviews
                        join ri in _context.ReviewImages on r.Id equals ri.ReviewId into pri
                        from ri in pri.DefaultIfEmpty()
                        where r.IsApproved == 1
                        select new { r, ri };

            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.r.DateCreated).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductReview()
                {
                    Id = x.r.Id,
                    ProductId = x.r.ProductId,
                    UserId = x.r.UserId,
                    Content = x.r.Content,
                    Rating = x.r.Rating,
                    IsApproved = x.r.IsApproved,
                    Name = x.r.Name,
                    Email = x.r.Email,
                    PhoneNumber = x.r.PhoneNumber,
                    DateCreated = x.r.DateCreated,
                    DateUpdate = x.r.DateUpdate,
                    ReviewImage = x.ri != null ? new List<ReviewImage>() { x.ri } : null
                }).ToListAsync();

            var result = new PagedResult<ProductReview>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return result;
        }

        public Task<List<ReviewVM>> GetReviewByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ReviewVM>> GetReviewsWaitApproved(PagingRequestBase request)
        {
            var query = from r in _context.ProductReviews
                        join pt in _context.ProductTranslations on r.ProductId equals pt.ProductId
                        join ri in _context.ReviewImages on r.Id equals ri.ReviewId into pri
                        from ri in pri.DefaultIfEmpty()
                        where r.IsApproved == 0 && pt.LanguageId == "vi-VN"
                        select new { r, ri, pt };

            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(x => x.r.DateCreated).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ReviewVM()
                {
                    Id = x.r.Id,
                    Product = x.pt.Name,
                    UserId = (Guid)(x.r.UserId ?? Guid.Empty),
                    Content = x.r.Content,
                    Rating = x.r.Rating,
                    IsApproved = x.r.IsApproved,
                    Name = x.r.Name,
                    Email = x.r.Email,
                    PhoneNumber = x.r.PhoneNumber,
                    DateCreated = x.r.DateCreated,
                    DateUpdated = x.r.DateUpdate,
                    ReviewImages = x.ri != null ? new List<ReviewImagesRequest>()
                    {
                        new ReviewImagesRequest()
                        {
                            Id = x.ri.Id,
                            ReviewId = x.ri.ReviewId,
                            ImagePath = x.ri.ImagePath,
                            Caption = x.ri.Caption,
                            IsDefault = x.ri.IsDefault,
                            DateCreated = x.ri.DateCreated,
                            SortOrder = x.ri.SortOrder,
                            FileSize = x.ri.FileSize
                        }
                    } : null
                }).ToListAsync();

            
            var result = new PagedResult<ReviewVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return result;
        }

        public async Task<PagedResult<ProductReview>> GetReviewsByProductId(GetManageReviewPagingRequest request)
        {
            var query = from r in _context.ProductReviews
                        join ri in _context.ReviewImages on r.Id equals ri.ReviewId into pri
                        from ri in pri.DefaultIfEmpty()
                        where r.IsApproved == 1 && r.ProductId == request.ProductId
                        select new { r, ri };

            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.r.DateCreated).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductReview()
                {
                    Id = x.r.Id,
                    ProductId = x.r.ProductId,
                    UserId = x.r.UserId,
                    Content = x.r.Content,
                    Rating = x.r.Rating,
                    IsApproved = x.r.IsApproved,
                    Name = x.r.Name,
                    Email = x.r.Email,
                    PhoneNumber = x.r.PhoneNumber,
                    DateCreated = x.r.DateCreated,
                    DateUpdate = x.r.DateUpdate,
                    ReviewImage = x.ri != null ? new List<ReviewImage>() { x.ri } : null
                }).ToListAsync();
            var result = new PagedResult<ProductReview>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return result;
        }

        public Task<List<ReviewVM>> GetReviewUser(int userId, int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> ReviewIsApproved(int reviewId)
        {
            var review = await _context.ProductReviews.FindAsync(reviewId);
            if (review == null)
            {
                return new ApiErrorResult<string>($"Không tìm thấy đánh giá {reviewId}.");
            }
            review.IsApproved = 1;
            if (await _context.SaveChangesAsync() == 0)
            {
                return new ApiErrorResult<string>($"Cập nhật thất bại.");
            }
            return new ApiSuccessResult<string>($"Duyệt đánh đánh giá thành công.");
        }

        public async Task<ApiResult<string>> ReviewDisapproved(int reviewId)
        {
            var review = await _context.ProductReviews.FindAsync(reviewId);
            if (review == null)
            {
                return new ApiErrorResult<string>($"Không tìm thấy đánh giá {reviewId}.");
            }
            review.IsApproved = -1;
            if (await _context.SaveChangesAsync() == 0)
            {
                return new ApiErrorResult<string>($"Cập nhật thất bại.");
            }
            return new ApiSuccessResult<string>($"Đánh giá đã cho vào danh sách hạn chế.");
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            // create a new name for the image to avoid the same name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            // save image to user-content
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<ReviewVM> GetReviewById(int reviewId)
        {
            var query = from r in _context.ProductReviews
                        join pt in _context.ProductTranslations on r.ProductId equals pt.ProductId
                        join ri in _context.ReviewImages on r.Id equals ri.ReviewId into pri
                        from ri in pri.DefaultIfEmpty()
                        where r.Id == reviewId && pt.LanguageId == "vi-VN"
                        select new { r, ri, pt };

            int totalRow = await query.CountAsync();

            var data = await query.Select(x => new ReviewVM()
                {
                    Id = x.r.Id,
                    Product = x.pt.Name,
                    UserId = (Guid)(x.r.UserId ?? Guid.Empty),
                    Content = x.r.Content,
                    Rating = x.r.Rating,
                    IsApproved = x.r.IsApproved,
                    Name = x.r.Name,
                    Email = x.r.Email,
                    PhoneNumber = x.r.PhoneNumber,
                    DateCreated = x.r.DateCreated,
                    DateUpdated = x.r.DateUpdate,
                    ReviewImages = x.ri != null ? new List<ReviewImagesRequest>()
                    {
                        new ReviewImagesRequest()
                        {
                            Id = x.ri.Id,
                            ReviewId = x.ri.ReviewId,
                            ImagePath = x.ri.ImagePath,
                            Caption = x.ri.Caption,
                            IsDefault = x.ri.IsDefault,
                            DateCreated = x.ri.DateCreated,
                            SortOrder = x.ri.SortOrder,
                            FileSize = x.ri.FileSize
                        }
                    } : null
                }).FirstOrDefaultAsync();

            return data;
        }
    }
}
