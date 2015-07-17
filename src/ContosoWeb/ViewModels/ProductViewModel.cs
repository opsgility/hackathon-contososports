using Contoso.Models;

namespace ContosoWeb.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public bool ShowRecommendations { get; set; }
    }
}