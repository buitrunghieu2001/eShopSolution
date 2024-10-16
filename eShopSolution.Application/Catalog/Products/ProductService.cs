﻿using Azure.Core;
using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.utilities.Exceptions;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Brands;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        // _context: đối tượng của lớp EShopDbContext
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        // constructor
        public ProductService(EShopDbContext context, IStorageService storageService) 
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                // the SaveFile method is defined below
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        // trả về kiểu void
        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var languages = _context.Languages;
            var translations = new List<ProductTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    translations.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    translations.Add(new ProductTranslation()
                    {
                        Name = SystemConstants.ProductConstants.NA,
                        Description = SystemConstants.ProductConstants.NA,
                        Details = SystemConstants.ProductConstants.NA,
                        SeoDescription = SystemConstants.ProductConstants.NA,
                        SeoAlias = SystemConstants.ProductConstants.NA,
                        SeoTitle = SystemConstants.ProductConstants.NA,
                        LanguageId = language.Id
                    });
                }
            }

            var productInCategory = new List<ProductInCategory>()
            {
                new ProductInCategory ()
                {
                    CategoryId = request.CategoryId,
                }
            };

            var product = new Product()
            {
                Price = request.Price ?? 0m,
                OriginalPrice = request.OriginalPrice ?? 0m,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                BrandId = request.BrandId,
                Origin = request.Origin,
                Warranty = request.Warranty,
                ProductTranslations = translations,
                ProductInCategories = productInCategory
            };
            //Save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach(var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }
        

        public async Task<PagedResult<ProductVM>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // step 1: select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join b in _context.Brands on p.BrandId equals b.Id into bp
                        from b in bp.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into piccct
                        from ct in piccct.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId && ct.LanguageId == request.LanguageId && pi.IsDefault == true
                        select new { p, pt, pic, pi, ct, b };

            // step 2: filter
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                query = query.Where(x => x.pt.Name.Contains(request.KeyWord) || x.b.Name.Contains(request.KeyWord));
            }

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            if (request.Rating != null)
            {
                query = query.Where(p => p.p.Rating >= request.Rating);
            }

            if (request.PriceFrom != null && request.PriceFrom > 0)
            {
                query = query.Where(p => p.p.Price >= request.PriceFrom);
            }

            if (request.PriceTo != null && request.PriceTo > 0)
            {
                query = query.Where(p => p.p.Price <= request.PriceTo);
            }

            switch (request.OrderBy)
            {
                case "latest":
                    query = query.OrderByDescending(x => x.p.DateCreated);
                    break;
                case "topsales":
                    query = query.OrderByDescending(x => x.p.ViewCount);
                    break;
                case "price-lh":
                    query = query.OrderBy(x => x.p.Price);
                    break;
                case "price-hl":
                    query = query.OrderByDescending(x => x.p.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.p.Rating);
                    break;
            }

            // step 3: paging
            // number of records
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Rating = x.p.Rating,
                    Origin = x.p.Origin,
                    Warranty = x.p.Warranty,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath,
                    Categories = new List<string>() { x.ct.Name },
                    Brand = x.b != null ? new BrandVM()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                    } : null
                }).ToListAsync();
            //ToListAsync(): chuyển thành một List<Product>

            // step 4: select and projection
            var pagedResult = new PagedResult<ProductVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ProductVM> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }
            var brand = await _context.Brands.FindAsync(product.BrandId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            var categories = (from c in _context.Categories
                              join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                              join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                              where pic.ProductId == productId && ct.LanguageId == languageId
                              select ct.Name).ToList();
            var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == productId && x.IsDefault == true);
            var productViewModel = new ProductVM()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                Brand = brand != null ? new BrandVM()
                {
                    Id = brand.Id,
                    Name = brand.Name,
                } : null,
                Origin = product.Origin,
                Warranty = product.Warranty,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                Rating = product.Rating,
                ViewCount = product.ViewCount,
                Categories = categories,
                ThumbnailImage = thumbnailImage != null ? thumbnailImage.ImagePath : null
            };
            return productViewModel;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new EShopException($"Cannot find an image with id {imageId}");
            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(i => i.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync() ;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {request.Id}");

            product.Price = request.Price.HasValue ? request.Price.Value : 0m;
            product.OriginalPrice = request.OriginalPrice.HasValue ? request.OriginalPrice.Value : 0m;
            product.BrandId = request.BrandId;
            product.Origin = request.Origin;
            product.Warranty = request.Warranty;
            productTranslations.Name = request.Name;
            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;

            // update image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = _context.ProductImages.FirstOrDefault(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                } 
            }
            _context.Products.Update(product);
            return await _context.SaveChangesAsync();

        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id {imageId}");
            
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;

            // Phương thức này để thay đổi database và trả về một số nguyên là số bản ghi được ảnh hưởng bởi thay đổi
            return await _context.SaveChangesAsync() > 0;
        }

        // save image 
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            // create a new name for the image to avoid the same name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            // save image to user-content
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<PagedResult<ProductVM>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            // step 1: select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        join b in _context.Brands on p.BrandId equals b.Id 
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into piccct
                        from ct in piccct.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId && pic.CategoryId == request.CategoryId
                        select new { p, pt, ct, pi, pic, b };
            // step 2: filter

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            // step 3: paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    Rating = x.p.Rating,
                    Origin = x.p.Origin,
                    Warranty = x.p.Warranty,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    IsFeatured = x.p.IsFeatured,
                    ThumbnailImage = x.pi.ImagePath,
                    Categories = new List<string>() { x.ct.Name },
                    Brand = x.b != null ? new BrandVM()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                    } : null
                }).ToListAsync();

            // step 4: select and projection
            var pagedResult = new PagedResult<ProductVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm với id {id} không tồn tại");
            }

            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
                    && x.ProductId == id);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductVM>> GetFeaturedProducts(string languageId, int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join b in _context.Brands on p.BrandId equals b.Id into pb
                        from b in pb.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into piccct
                        from ct in piccct.DefaultIfEmpty()
                        where pt.LanguageId == languageId && ct.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        && p.IsFeatured == true
                        select new { p, pt, pic, pi, ct, b };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    Warranty = x.p.Warranty,
                    Origin = x.p.Origin,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Rating = x.p.Rating,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    IsFeatured = x.p.IsFeatured,
                    ThumbnailImage = x.pi.ImagePath,
                    Categories = new List<string>() { x.ct.Name },
                    Brand = x.b != null ? new BrandVM()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                    } : null
                }).ToListAsync();

            return data;
        }

        public async Task<List<ProductVM>> GetLatestProducts(string languageId, int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join b in _context.Brands on p.BrandId equals b.Id into pb
                        from b in pb.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into piccct
                        from ct in piccct.DefaultIfEmpty()
                        where pt.LanguageId == languageId && ct.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        select new { p, pt, pic, pi, ct, b };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Rating = x.p.Rating,
                    Warranty = x.p.Warranty,
                    Origin = x.p.Origin,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath,
                    Categories = new List<string>() { x.ct.Name },
                    Brand = x.b != null ? new BrandVM()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                    } : null
                }).ToListAsync();

            return data;
        }

        public async Task<List<ProductVM>> GetAllProductByCategory(string languageId, int categoryId)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join b in _context.Brands on p.BrandId equals b.Id into pb
                        from b in pb.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into piccct
                        from ct in piccct.DefaultIfEmpty()
                        where pt.LanguageId == languageId && ct.LanguageId == languageId && (pi == null || pi.IsDefault == true) && c.Id == categoryId
                        select new { p, pt, pic, pi, ct, b };

            var data = await query.OrderByDescending(x => x.p.ViewCount)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Rating = x.p.Rating,
                    Origin = x.p.Origin,
                    Warranty = x.p.Warranty,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath,
                    Categories = new List<string>() { x.ct.Name },
                    Brand = x.b != null ? new BrandVM()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                    } : null
                }).ToListAsync();

            return data;
        }

        public async Task<List<ProductVM>> GetLimitedProductByCategory(string languageId, int categoryId, int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join b in _context.Brands on p.BrandId equals b.Id into pb
                        from b in pb.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into piccct
                        from ct in piccct.DefaultIfEmpty()
                        where pt.LanguageId == languageId && ct.LanguageId == languageId && (pi == null || pi.IsDefault == true) && c.Id == categoryId
                        select new { p, pt, pic, pi, ct, b };

            var data = await query.OrderByDescending(x => x.p.ViewCount).Take(take)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Rating = x.p.Rating,
                    Origin = x.p.Origin,
                    Warranty = x.p.Warranty,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath,
                    Categories = new List<string>() { x.ct.Name },
                    Brand = x.b != null ? new BrandVM()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                    } : null
                }).ToListAsync();

            return data;
        }

        public async Task<ApiResult<int>> UpdateViewCount(int id)
        {
            // Tìm sản phẩm theo id
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return new ApiErrorResult<int>($"Sản phẩm với id {id} không tồn tại");
            }

            // Tăng view count lên 1
            product.ViewCount++;
            // Cập nhật sản phẩm trong cơ sở dữ liệu
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int>(product.ViewCount);
        }

        public async Task<List<ProductVM>> GetBrandProducts(int brandId, string languageId, int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join b in _context.Brands on p.BrandId equals b.Id into pb
                        from b in pb.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into piccct
                        from ct in piccct.DefaultIfEmpty()
                        where pt.LanguageId == languageId && ct.LanguageId == languageId && (pi == null || pi.IsDefault == true) && b.Id == brandId
                        select new { p, pt, pic, pi, ct, b };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVM()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    Warranty = x.p.Warranty,
                    Origin = x.p.Origin,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Rating = x.p.Rating,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    IsFeatured = x.p.IsFeatured,
                    ThumbnailImage = x.pi.ImagePath,
                    Categories = new List<string>() { x.ct.Name },
                    Brand = x.b != null ? new BrandVM()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                    } : null
                }).ToListAsync();

            return data;
        }
    }
}
