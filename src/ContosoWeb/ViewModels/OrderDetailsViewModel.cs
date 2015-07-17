using Contoso.Models;

namespace ContosoWeb.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderCostSummary OrderCostSummary { get; set; }
        public Order Order { get; set; }
    }
}