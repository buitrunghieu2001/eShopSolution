using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleVM>> GetAll();
        Task<ApiResult<bool>> Create(RoleCreateRequest request);
    }
}
