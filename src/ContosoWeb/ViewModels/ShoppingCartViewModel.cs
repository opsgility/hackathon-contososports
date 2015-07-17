using System.Collections.Generic;
using Contoso.Models;

namespace ContosoWeb.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public int CartCount { get; set; }
        public OrderCostSummary OrderCostSummary { get; set; }
    }
}
