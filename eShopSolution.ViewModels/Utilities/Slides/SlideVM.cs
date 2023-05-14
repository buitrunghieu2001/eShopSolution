using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Utilities.Slides
{
    public class SlideVM
    {
        public int Id { set; get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }
}
