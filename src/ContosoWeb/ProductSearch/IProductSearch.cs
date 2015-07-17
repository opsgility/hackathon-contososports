using System.Collections.Generic;
using System.Threading.Tasks;
using Contoso.Models;

namespace ContosoWeb.ProductSearch
{
    public interface IProductSearch
    {
        Task<IEnumerable<Product>> Search(string query);
    }
}
