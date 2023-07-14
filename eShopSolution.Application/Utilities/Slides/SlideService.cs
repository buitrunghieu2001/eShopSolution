using Azure.Core;
using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.utilities.Exceptions;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public SlideService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> CreateSlide(SlideCreateRequest request)
        {
            var slide = new Slide()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Url = request.Url,
                SortOrder = request.SortOrder,
                Status = (Status)request.Status,
        };
            if (request.Image != null)
            {
                slide.Image = await this.SaveFile(request.Image);
            }
                _context.Slides.Add(slide);
            await _context.SaveChangesAsync();
            return slide.Id;
        }

        public async Task<int> DeleteSlide(int slideId)
        {
            var slide = await _context.Slides.FindAsync(slideId);
            if (slide == null) throw new EShopException($"Cannot find a slide: {slideId}");

            _context.Slides.Remove(slide);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<SlideVM>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
                .Select(x => new SlideVM()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Status = (Status)x.Status,
                    Url = x.Url,
                    Image = x.Image,
                    SortOrder=x.SortOrder,
                }).ToListAsync();

            return slides;
        }

        public async Task<List<SlideVM>> GetSlideActive()
        {
            var slides = await _context.Slides.Where(x => x.Status == (Status)1).OrderBy(x => x.SortOrder)
                .Select(x => new SlideVM()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Status = (Status)x.Status,
                    Url = x.Url,
                    Image = x.Image,
                    SortOrder = x.SortOrder,
                }).ToListAsync();

            return slides;
        }

        public async Task<SlideVM> GetById(int slideId)
        {
            var slide = await _context.Slides.FindAsync(slideId);
            if (slide == null) throw new EShopException($"Cannot find a slide with id: {slideId}");
            var slideVM = new SlideVM()
            {
                Id = slide.Id,
                Name = slide.Name,
                Description = slide.Description,
                Price = slide.Price,
                Url = slide.Url,
                Image = slide.Image,
                SortOrder = slide.SortOrder,
                Status = (Status)slide.Status,
            };
            return slideVM;
        }

        public async Task<int> UpdateSlide(int slideId, SlideUpdateRequest request)
        {
            var slide = await _context.Slides.FindAsync(slideId);
            if (slide == null) throw new EShopException($"Cannot find a slide with id: {slideId}");

            slide.Name = request.Name;
            slide.Description = request.Description;
            slide.Price = request.Price;
            slide.Url = request.Url;
            slide.SortOrder = request.SortOrder;
            slide.Status = (Status)request.Status;


            // update image
            if (request.Image != null)
            {
                slide.Image = await this.SaveFile(request.Image);
            }
            _context.Slides.Update(slide);
            await _context.SaveChangesAsync();
            return slide.Id;

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


    }
}
