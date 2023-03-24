using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Users
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }

        // gán giá trị mặc định
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
