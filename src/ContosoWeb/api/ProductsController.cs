using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Contoso.Models;

namespace ContosoWeb.Api
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IContosoWebContext _context;

        public ProductsController(IContosoWebContext context)
        {
            _context = context;
        }

        [HttpGet, Route]
        public IEnumerable<Product> Get(bool sale = false)
        {
            if (!sale)
            {
                return _context.Products;
            }

            return _context.Products.Where(p => p.Price != p.SalePrice);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return Content(HttpStatusCode.OK, product);
        }
    }
}
