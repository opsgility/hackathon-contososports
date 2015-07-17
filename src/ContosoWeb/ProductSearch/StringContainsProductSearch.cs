using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Models;

namespace ContosoWeb.ProductSearch
{
    public class StringContainsProductSearch : IProductSearch
    {
        private readonly IContosoWebContext _context;

        public StringContainsProductSearch(IContosoWebContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> Search(string query)
        {
            var lowercase_query = query.ToLower();

            var q = _context.Products
                .Where(p => p.Title.ToLower().Contains(lowercase_query));

            return await q.ToListAsync();
        }
    }
}
