using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Reviews
{
    public class GetManageReviewPagingRequest : PagingRequestBase
    {
        public int ProductId { get; set; }
    }
}
