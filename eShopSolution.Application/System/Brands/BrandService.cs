using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Brands;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.System.Brands
{
    public class BrandService : IBrandService
    {
        // _context: đối tượng của lớp EShopDbContext
        private readonly EShopDbContext _context;
        // constructor
        public BrandService(EShopDbContext context)
        {
            _context = context;
        }

        //public async Task<ApiResult<bool>> Create(BrandCreateRequest request)
        //{
        //    var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Name == request.Name);
        //    if (brand != null)
        //    {
        //        return new ApiErrorResult<bool>("Thương hiệu đã tồn tại");
        //    }

        //    var br = new Brands()
        //    {
        //        Name = request.Name
        //    };

        //    _context.Brands.Add(br);
        //    var result = await _context.SaveChangesAsync();

        //    if (result > 0)
        //    {
        //        return new ApiSuccessResult<bool>();
        //    }

        //    return new ApiErrorResult<bool>("Thêm thương hiệu mới không thành công");
        //}


        public async Task<List<BrandVM>> GetAll()
        {
            var brands = await _context.Brands.Select(b => new BrandVM
            {
                Id = b.Id,
                Name = b.Name,
            }).ToListAsync();
            return brands;
        }
    }
}
