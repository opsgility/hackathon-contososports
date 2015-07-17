using System.Collections.Generic;
using Contoso.Models;

namespace ContosoWeb.ViewModels
{
    public class HomeViewModel
    {
        public Match NextMatch { get; set; }
        public Match CurrentMatch { get; set; }
        public List<Match> PreviousMatches { get; set; }

        public List<Product> NewProducts { get; set; }
        public List<Product> TopSellingProducts { get; set; }
    }
}