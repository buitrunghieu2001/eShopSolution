using eShopSolution.ViewModels.System.Languages;

namespace eShopSolution.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageVM> Languages { get; set; }

        public string CurrentLanguageId { get; set; }
    }
}
