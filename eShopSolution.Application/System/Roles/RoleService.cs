using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager) 
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResult<bool>> Create(RoleCreateRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.Name);
            if (role != null)
            {
                return new ApiErrorResult<bool>("Vai trò đã tồn tại");
            }

            role = new AppRole()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Thêm vai trò không thành công");
        }

        public async Task<List<RoleVM>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return roles;
        }
    }
}
