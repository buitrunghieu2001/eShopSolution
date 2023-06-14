using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Categories
{
    public class CategoryUpdateRequest
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int SortOrder { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public int Status { get; set; }
    }
}
