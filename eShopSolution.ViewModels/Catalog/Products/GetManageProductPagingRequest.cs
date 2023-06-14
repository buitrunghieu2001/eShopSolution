using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string? KeyWord { get; set; }
        public string LanguageId { get; set; }
        public int? CategoryId { get; set; }
        public string? OrderBy { get; set; }
        public int? Rating { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}
