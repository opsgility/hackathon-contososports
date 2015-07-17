using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Contoso.Models;
using ContosoWeb.Recommendations;
using ContosoWeb.Utils;

namespace ContosoWeb.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly IContosoWebContext db;
        private readonly IRecommendationEngine recommendation;

        public RecommendationsController(IContosoWebContext context, IRecommendationEngine recommendationEngine)
        {
            db = context;
            recommendation = recommendationEngine;
        }

        public async Task<ActionResult> GetRecommendations(string productId)
        {
            if (!ConfigurationHelpers.GetBool("ShowRecommendations"))
            {
                return new EmptyResult();
            }

            var recommendedProductIds = await recommendation.GetRecommendationsAsync(productId);

            var recommendedProducts = await db.Products.Where(x => recommendedProductIds.Contains(x.ProductId.ToString())).ToListAsync();

            return PartialView("_Recommendations", recommendedProducts);
        }
    }
}
